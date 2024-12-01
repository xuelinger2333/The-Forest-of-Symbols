using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;

public class InputNetworkMiddleWare : NetworkBehaviour
{
    public PlayerInputControl1 inputControl1;
    public PlayerInputControl2 inputControl2;

    public Character player1Controller;
    public Character player0Controller;

    GameManager gameManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameManager.Instance; 
        if (!IsOwner)
        {
            gameObject.GetComponent<InputNetworkMiddleWare>().enabled = false;
        }
        else
        { 
            inputControl1 = new PlayerInputControl1();
            inputControl2 = new PlayerInputControl2();
            if (base.IsServerInitialized)
                player1Controller = gameManager.player1 as Character;
            else
                player0Controller = gameManager.player0 as Character;
            gameManager.inputControl1 = inputControl1;
            gameManager.inputControl2 = inputControl2;
            InitializeInputController();
        }

    }
    protected virtual void InitializeInputController()
    {
        if (player1Controller != null)
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
        }

        if (player0Controller != null)
        {
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

    }

    protected virtual void OnDestroy()
    {
        if (player1Controller != null)
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

        if (player0Controller != null)
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
        Server_OnASStarted(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnASStarted(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnActiveSkill();
        }
        else
        {
            ((Character)gameManager.player0).OnActiveSkill();
        }
    }
    private void OnASCanceled(InputAction.CallbackContext context)
    {
        Server_OnASCanceled(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnASCanceled(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnActiveSkillCancel();
        }
        else
        {
            ((Character)gameManager.player0).OnActiveSkillCancel();
        }
    }
    private void OnUSStarted(InputAction.CallbackContext context)
    {
        Server_OnUSStarted(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnUSStarted(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnUniqueSkill();
        }
        else
        {
            ((Character)gameManager.player0).OnUniqueSkill();
        }
    }
    protected void OnChargeStarted(InputAction.CallbackContext context)
    {
        Server_OnChargeStarted(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnChargeStarted(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnCharge();
        }
        else
        {
            ((Character)gameManager.player0).OnCharge();
        }
    }
    protected virtual void OnChargeCancled(InputAction.CallbackContext context)
    {
        Server_OnChargeCanceled(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnChargeCanceled(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnChargeCancel();
        }
        else
        {
            ((Character)gameManager.player0).OnChargeCancel();
        }
    }
    //protected void OnHidePerformed(InputAction.CallbackContext context)
    //{
    //    Server_OnHidePerformed(context.action.actionMap.asset.name);
    //}
    //[ServerRpc]
    //void Server_OnHidePerformed(string assetName)
    //{
    //    if (assetName.EndsWith("1"))
    //    {
    //        ((Character)gameManager.player1).OnHide();
    //    }
    //    else
    //    {
    //        ((Character)gameManager.player0).OnHide();
    //    }
    //}
    //protected void OnHideCanceled(InputAction.CallbackContext context)
    //{
    //    Server_OnHideCanceled(context.action.actionMap.asset.name);
    //}
    //[ServerRpc]
    //void Server_OnHideCanceled(string assetName)
    //{
    //    if (assetName.EndsWith("1"))
    //    {
    //        ((Character)gameManager.player1).OnHideCancel();
    //    }
    //    else
    //    {
    //        ((Character)gameManager.player0).OnHideCancel();
    //    }
    //}
    protected void OnMainAttackStarted(InputAction.CallbackContext context)
    {
        Server_OnMainAttackStarted(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnMainAttackStarted(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnMainAttack();
        }
        else
        {
            ((Character)gameManager.player0).OnMainAttack();
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
            Server_OnMovePerformed(context.action.actionMap.asset.name, inputDirection);
        }
        else
        {
            if (inputControl2.Player.Move.ReadValue<Vector2>().x != 0)
            {
                inputDirection = new Vector2(inputControl2.Player.Move.ReadValue<Vector2>().x, 0);
            }
            else inputDirection = new Vector2(0, inputControl2.Player.Move.ReadValue<Vector2>().y);
            Server_OnMovePerformed(context.action.actionMap.asset.name, inputDirection);
        }
    }
    [ServerRpc]
    void Server_OnMovePerformed(string assetName, Vector2 inputDirection)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnMove(inputDirection);
        }
        else
        {
            ((Character)gameManager.player0).OnMove(inputDirection);
        }
    }
    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Server_OnMoveCanceled(context.action.actionMap.asset.name);
    }
    [ServerRpc]
    void Server_OnMoveCanceled(string assetName)
    {
        if (assetName.EndsWith("1"))
        {
            ((Character)gameManager.player1).OnMoveCancel();
        }
        else
        {
            ((Character)gameManager.player0).OnMoveCancel();
        }
    }
}
