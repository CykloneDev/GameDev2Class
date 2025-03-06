using UnityEngine;

public class PStateMachine : PStateManager<PStateMachine.PlayerState>
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Falling,
        Crouch,
        Slide
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
