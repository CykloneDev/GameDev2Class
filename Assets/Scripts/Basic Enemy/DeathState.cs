using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(EnemyContext context, EnemyMachine.EnemyState key) : base(context, key)
    {

    }

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;

        animator.CrossFade("Die", 0.02f);
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        return EnemyMachine.EnemyState.Death;
    }
}
