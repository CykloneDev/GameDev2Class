using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState
    {
        Idle,
        Movement,
        Jump,
        Falling,
        Sprint,
        Crouch,
        Sliding,
        Dash,
    }



}
