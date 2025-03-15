using UnityEngine;

public class DamageState : EnemyBaseState
{
    public DamageState(EnemyContext context, EnemyMachine.EnemyState key, float flinchTime) : base(context, key)
    {
        _flinchTime = flinchTime;
    }

    private float _flinchTime;
    private float _currentFlinchTime;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = true;
        agent.updateRotation = false;

        animator.Play("Idle"); // Just play idle for now until I get a damage reaction animation
        _currentFlinchTime = 0;
        _context.SetDamage(false);
    }

    public override void UpdateState()
    {
        _currentFlinchTime += Time.deltaTime;
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var dead = _context.GetDead();

        if (dead) return EnemyMachine.EnemyState.Death;

        if (_currentFlinchTime > _flinchTime) return EnemyMachine.EnemyState.FocusIdle;

        return EnemyMachine.EnemyState.Damage;
    }
}
