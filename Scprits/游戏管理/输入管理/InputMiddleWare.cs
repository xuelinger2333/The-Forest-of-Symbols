using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Operation
{
    Move,
    MainAttack,
    ChargeAttack,
    ActiveSkill,
    UniqueSkill,
    All
}
public class InputMiddleWare : MonoBehaviour
{
    public PlayerInputControl1 inputControl1;
    public PlayerInputControl2 inputControl2;

    public Character player1Controller;
    public Character player0Controller;

    GameManager gameManager;

    private void Awake()
    { 
        gameManager = GameManager.Instance;
        inputControl1 = new PlayerInputControl1();
        inputControl2 = new PlayerInputControl2();
        if (!gameManager)
            return;
        if (gameManager.FishNet_isUsingNetwork)
            Destroy(gameObject);
        player0Controller = gameManager.player0 as Character;
        player1Controller = gameManager.player1 as Character;
        gameManager.inputControl1 = inputControl1;
        gameManager.inputControl2 = inputControl2;
    }
    public void EnableOperation(bool playerID, Operation operation)
    {
        if (operation == Operation.All)
        {
            if (playerID) inputControl1.Enable();
            else inputControl2.Enable();
        }
        switch (operation)
        {
            case Operation.Move:
                if (playerID) inputControl1.Player.Move.Enable();
                else inputControl2.Player.Move.Enable();
                break;
            case Operation.MainAttack:
                if (playerID) inputControl1.Player.MainAttack.Enable();
                else inputControl2.Player.MainAttack.Enable();
                break;
            case Operation.ChargeAttack:
                if (playerID) inputControl1.Player.Charge.Enable();
                else inputControl2.Player.Charge.Enable();
                break;
            case Operation.ActiveSkill:
                if (playerID) inputControl1.Player.ActiveSkill.Enable();
                else inputControl2.Player.ActiveSkill.Enable();
                break;
            case Operation.UniqueSkill:
                if (playerID) inputControl1.Player.UniqueSkill.Enable();
                else inputControl2.Player.UniqueSkill.Enable();
                break;
        }
    }

    public void DisableOperation(bool playerID, Operation operation)
    {
        if (operation == Operation.All)
        {
            if (playerID) inputControl1.Disable();
            else inputControl2.Disable();
        }
        switch (operation)
        {
            case Operation.Move:
                if (playerID) inputControl1.Player.Move.Disable();
                else inputControl2.Player.Move.Disable();
                break;
            case Operation.MainAttack:
                if (playerID) inputControl1.Player.MainAttack.Disable();
                else inputControl2.Player.MainAttack.Disable();
                break;
            case Operation.ChargeAttack:
                if (playerID) inputControl1.Player.Charge.Disable();
                else inputControl2.Player.Charge.Disable();
                break;
            case Operation.ActiveSkill:
                if (playerID) inputControl1.Player.ActiveSkill.Disable();
                else inputControl2.Player.ActiveSkill.Disable();
                break;
            case Operation.UniqueSkill:
                if (playerID) inputControl1.Player.UniqueSkill.Disable();
                else inputControl2.Player.UniqueSkill.Disable();
                break;
        }
    }
    public void SwitchInputControl()
    {
        //交换双方控制的角色，事件专用
        player1Controller = gameManager.player0 as Character;
        player0Controller = gameManager.player1 as Character;
    }
    protected virtual void OnEnable()
    {
        inputControl1.Enable();
        inputControl1.Player.Charge.started += OnChargeStarted;
        inputControl1.Player.Charge.canceled += OnChargeCancled;
        inputControl1.Player.Move.performed += OnMovePerformed;
        inputControl1.Player.Move.canceled += OnMoveCanceled;
        inputControl1.Player.MainAttack.started += OnMainAttackStarted;
        inputControl1.Player.ActiveSkill.performed += OnASStarted;
        inputControl1.Player.ActiveSkill.canceled += OnASCanceled;
        inputControl1.Player.UniqueSkill.started += OnUSStarted;

        inputControl2.Enable();
        inputControl2.Player.Charge.started += OnChargeStarted;
        inputControl2.Player.Charge.canceled += OnChargeCancled;
        inputControl2.Player.Move.performed += OnMovePerformed;
        inputControl2.Player.Move.canceled += OnMoveCanceled;
        inputControl2.Player.MainAttack.started += OnMainAttackStarted;
        inputControl2.Player.ActiveSkill.performed += OnASStarted;
        inputControl2.Player.ActiveSkill.canceled += OnASCanceled;
        inputControl2.Player.UniqueSkill.started += OnUSStarted;
    }

    protected virtual void OnDisable()
    {
        if (inputControl1 != null)
        {
            inputControl1.Player.Charge.started -= OnChargeStarted;
            inputControl1.Player.Charge.canceled -= OnChargeCancled;
            inputControl1.Player.Move.performed -= OnMovePerformed;
            inputControl1.Player.Move.canceled -= OnMoveCanceled;
            inputControl1.Player.MainAttack.started -= OnMainAttackStarted;
            inputControl1.Player.ActiveSkill.started -= OnASStarted;
            inputControl1.Player.ActiveSkill.canceled -= OnASCanceled;
            inputControl1.Player.UniqueSkill.started -= OnUSStarted;
            inputControl1.Disable();
        }

        if (inputControl2 != null)
        {
            inputControl2.Enable();
            inputControl2.Player.Charge.started -= OnChargeStarted;
            inputControl2.Player.Charge.canceled -= OnChargeCancled;
            inputControl2.Player.Move.performed -= OnMovePerformed;
            inputControl2.Player.Move.canceled -= OnMoveCanceled;
            inputControl2.Player.MainAttack.started -= OnMainAttackStarted;
            inputControl2.Player.ActiveSkill.started -= OnASStarted;
            inputControl2.Player.ActiveSkill.canceled -= OnASCanceled;
            inputControl2.Player.UniqueSkill.started -= OnUSStarted;
            inputControl2.Disable();
        }

    }
    private void OnASStarted(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1")){
            player1Controller.OnActiveSkill();
        }
        else
        {
            player0Controller.OnActiveSkill();
        }
    }
    private void OnASCanceled(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnActiveSkillCancel();
        }
        else
        {
            player0Controller.OnActiveSkillCancel();
        }
    }

    private void OnUSStarted(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnUniqueSkill();
        }
        else
        {
            player0Controller.OnUniqueSkill();
        }
    }
    protected void OnChargeStarted(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnCharge();
        }
        else
        {
            player0Controller.OnCharge();
        }
    }
    protected virtual void OnChargeCancled(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnChargeCancel();
        }
        else
        {
            player0Controller.OnChargeCancel();
        }
    }
    //protected void OnHidePerformed(InputAction.CallbackContext context)
    //{
    //    if (context.action.actionMap.asset.name.EndsWith("1"))
    //    {
    //        player1Controller.OnHide();
    //    }
    //    else
    //    {
    //        player0Controller.OnHide();
    //    }
    //}
    //protected void OnHideCanceled(InputAction.CallbackContext context)
    //{
    //    if (context.action.actionMap.asset.name.EndsWith("1"))
    //    {
    //        player1Controller.OnHideCancel();
    //    }
    //    else
    //    {
    //        player0Controller.OnHideCancel();
    //    }
    //}
    protected void OnMainAttackStarted(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnMainAttack();
        }
        else
        {
            player0Controller.OnMainAttack();
        }
    }
    protected virtual void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 inputDirection;
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            if (inputControl1.Player.Move.ReadValue<Vector2>().x != 0)
            {
                inputDirection = new Vector2(inputControl1.Player.Move.ReadValue<Vector2>().x, 0);
            }
            else inputDirection = new Vector2(0, inputControl1.Player.Move.ReadValue<Vector2>().y);
            player1Controller.OnMove(inputDirection);
        }
        else
        { 
            if (inputControl2.Player.Move.ReadValue<Vector2>().x != 0)
            {
                inputDirection = new Vector2(inputControl2.Player.Move.ReadValue<Vector2>().x, 0);
            }
            else inputDirection = new Vector2(0, inputControl2.Player.Move.ReadValue<Vector2>().y);
            player0Controller.OnMove(inputDirection);
        }
    }
    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (context.action.actionMap.asset.name.EndsWith("1"))
        {
            player1Controller.OnMoveCancel();
        }
        else
        {
            player0Controller.OnMoveCancel();
        }
    }
}
