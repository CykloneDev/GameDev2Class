using UnityEngine;

public class IdleState : EnemyBaseState
{
    public IdleState(EnemyContext context, EnemyState key) : base(context, key)
    {

    }

    private readonly int IdleHash = Animator.StringToHash("Idle");

    public override void EnterState()
    {
        // Used to set what should happen when we enter the Idle state
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        // For this project, I'm going to keep things simple and not use Root Motion for any movement animations.
        // This isn't super hard to setup and if the team wants, I'll add it in. 

        agent.isStopped = true;
        agent.Warp(_context.GetTransform().position);

        animator.CrossFade(IdleHash, 0.2f);
    }

    public override void UpdateState()
    {
        // Here we can put a timer to play a idle/bored animation
        // Or anything we want to keep track of each frame in the Idle state, which for now isn't anything.
    }

    public override EnemyState GetNextState()
    {
        // Here we define what are the requirements to transition to another state
        // We will use the members of the context to determine this
        return EnemyState.Idle;
    }
}
