using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// 玩家总类，包括所有角色的共同属性
/// </summary>
public partial class Player : MonoBehaviour
{
    public bool NETWORK_isMainController = true;
    protected Vector3 targetPos;
    public Vector2 inputDirection;
    public Vector2 pos_update;
    public int pos_x, pos_y;
    [HideInInspector]
    public MapGenerator map;
    public float moveCD;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public PlayerAction action;
    public PlayerCDController CDController;

    public Vector2 faceDir;
    public Vector2 aimDir;
    public bool playerID;
    [SerializeField]
    public PlayerViewAdapter playerViewAdapter;
    [SerializeField]
    public PlayerViewAdapter playerViewAdapter_map2;
    List<PlayerAnimationObserver> animationObservers;
    [HideInInspector]
    public PlayerSpecialStatisticCounter specialStatisticCounter;
    public Player enemy;
    [HideInInspector] public Camera myCamera;
    [HideInInspector] public Volume myVolume;
    protected Vignette vignette;
    [HideInInspector]
    public ChromaticAberration chromatic;
    [HideInInspector]
    public LensDistortion lensDistortion;
    protected PrefabControl prefabControl;
    public string characterName;
    public float attackLen = 0.8f;
    public float attackWid = 0.8f;
    public float height = 0.5f;
    public PlayerStateMachine stateMachine { get; private set; }
    protected virtual void Awake()
    {
        pos_update = new Vector3(pos_x, pos_y, 0);
        stateMachine = new PlayerStateMachine();
        animationObservers = new List<PlayerAnimationObserver>();
        stats = GetComponent<PlayerStats>();
        action = GetComponent<PlayerAction>();
        CDController = GetComponent<PlayerCDController>();
    }
    protected virtual void Start()
    {
        map = MapGenerator.Instance;
        prefabControl = PrefabControl.Instance;
        if (myVolume != null)
        {
            myVolume.profile.TryGet(out vignette);
            myVolume.profile.TryGet(out chromatic);
            myVolume.profile.TryGet(out lensDistortion);
        }

        //特殊数据计数器
        TryGetComponent(out specialStatisticCounter);
        if (specialStatisticCounter)
        specialStatisticCounter.StartCountPlayerStep(8f, 5, true, OnPlayerStepnotEnough);

    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
        if (moveCD >= 0)
            moveCD -= Time.deltaTime;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void AttackTriggerCalled() => stateMachine.currentState.AnimationAttackTrigger();
    public void AddAnimationObserver(PlayerAnimationObserver observer)
    {
        animationObservers.Add(observer);
    }
    public void SetAnimationBool(string animation, bool val)
    {
        for (int i = 0; i < animationObservers.Count; i++)
        {
            animationObservers[i].OnNotifyBool(animation, val);
        }
    }
    public void SetAnimationSpeed(float speed)
    {
        for (int i = 0; i < animationObservers.Count; i++)
        {
            animationObservers[i].OnNotifySpeed(speed);
        }
    }
    public void SetAnimationInt(string animation, int val)
    {
        for (int i = 0; i < animationObservers.Count; i++)
        {
            animationObservers[i].OnNotifyInt(animation, val);
        }
    }
    public void OnPlayerStepnotEnough()//步数不够的回调函数
    {
        if (NETWORK_isMainController == false)
            return;
        CreatureGenerator.Instance.GenerateSnake(playerID);
    }
    public virtual void Die(string deathReason)
    {
        if (stats.isDead) return;
        else stats.isDead = true;
        Debug.Log(gameObject.name + "死了");
        if (GameManager.Instance)
            GameManager.Instance.PauseGame();
        if (playerViewAdapter)
            playerViewAdapter.dieAction(deathReason);
        if (playerViewAdapter_map2)
            playerViewAdapter_map2.dieAction(deathReason);
        if (GameManager.Instance)
        {
            GameManager.Instance.winPlayerId = enemy.playerID;
            GameManager.Instance.isGameRunning = false;
        }

    }

}
