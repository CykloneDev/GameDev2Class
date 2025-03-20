using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoverState : EnemyBaseState
{
    public CoverState(EnemyContext context, EnemyMachine.EnemyState key, float minHideTime, float maxHideTime,
        float searchRange, float coverSpeed) : base(context, key)
    {
        _minHideTime = minHideTime;
        _maxHideTime = maxHideTime;
        _searchRange = searchRange;
        _coverSpeed = coverSpeed;
    }

    private float _searchRange;
    private float _minHideTime;
    private float _maxHideTime;
    private float _hideTime;
    private float _coverSpeed;
    private bool _complete;
    private bool _reachedCover;
    private readonly int RunHash = Animator.StringToHash("Run");
    private readonly int CrouchHash = Animator.StringToHash("Crouch");

    public override void EnterState()
    {
        var animator = _context.GetAnimator();
        var agent = _context.GetAgent();
        var transform = _context.GetPlayerDetector().transform;

        var covers = Physics.OverlapSphere(transform.position, _searchRange, LayerMask.NameToLayer("Cover"));
        var orderedByProximity = covers.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

        var length = orderedByProximity.Length;

        if(length > 0)
        {
            Debug.Log("NEAREST COVER " + orderedByProximity[0].transform.position.ToString());
            _hideTime = Random.Range(_minHideTime, _maxHideTime);
            agent.isStopped = false;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.speed = _coverSpeed;
            agent.stoppingDistance = 0;
            agent.SetDestination(orderedByProximity[0].transform.position);
            animator.CrossFade(RunHash, 0.2f);
            _complete = false;
            _reachedCover = false;
            return;
        }
        else 
        { 
            _complete = true;
            _reachedCover = false;
        }            
    }

    public override void UpdateState()
    {
        var animator = _context.GetAnimator();
        var agent = _context.GetAgent();

        if(!_reachedCover && !_complete)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.CrossFade(CrouchHash, 0.02f);
                _reachedCover = true;
            }
        }

        else if(_reachedCover)
        {
            _hideTime -= Time.deltaTime;
            if(_hideTime < 0)
            {
                _complete = true;
            }
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var dead = _context.GetDead();

        if (dead) return EnemyMachine.EnemyState.Death;

        if (_complete) return EnemyMachine.EnemyState.Attack;

        return EnemyMachine.EnemyState.Cover;
    }
}
