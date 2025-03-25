using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
    private List<Vector3> _points = new List<Vector3>();

    public override void EnterState()
    {
        var animator = _context.GetAnimator();
        var agent = _context.GetAgent();
        var transform = _context.GetPlayerDetector().transform;

        _points.Clear();

        foreach (var cover in GameManager.instance.coverList)
        {
            var dist = Vector3.Distance(cover.transform.position, cover.transform.position);
            if (dist <= _searchRange)
                _points.Add(cover.transform.position);
        }

        var length = _points.Count;
        var point = _points[ Random.Range(0, _points.Count) ];

        if(length > 0)
        {
            _hideTime = Random.Range(_minHideTime, _maxHideTime);
            agent.isStopped = false;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.speed = _coverSpeed;
            agent.stoppingDistance = 0.2f;
            agent.SetDestination(point);
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
