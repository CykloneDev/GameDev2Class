using UnityEngine;

public class FleeState : EnemyBaseState
{
    public FleeState(EnemyContext context, EnemyMachine.EnemyState key, 
        float fleeSpeed, float fleeRadius) : base(context, key)
    {
        _fleeSpeed = fleeSpeed;
        _fleeRadius = fleeRadius;
    }

    private float _fleeSpeed;
    private float _fleeRadius;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
    }
}
