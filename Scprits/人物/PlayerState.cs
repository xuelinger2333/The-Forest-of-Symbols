using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class PlayerState
{
    private PlayerViewAdapter playerBase;
    protected Player _player;
    protected PlayerStateMachine stateMachine;

    private string aniBoolName;

    protected float stateTimer;
    protected bool triggerCalled;
    protected bool attackTriggerCalled;

    public PlayerState(Player _player ,PlayerStateMachine _stateMachine, string _aniBoolName)
    {
        this._player = _player;
        this.stateMachine = _stateMachine;
        this.aniBoolName = _aniBoolName;
    }
    public virtual void Enter()
    {
        _player.SetAnimationBool(aniBoolName, true);
        triggerCalled = false;
        attackTriggerCalled = false;
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
        _player.SetAnimationBool(aniBoolName, false);
        //playerBase.ani.SetBool(aniBoolName, false);
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public void AnimationAttackTrigger()
    {
        attackTriggerCalled = true;
    }
}
