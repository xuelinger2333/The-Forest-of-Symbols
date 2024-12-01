using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAction : MonoBehaviour
{
    public bool chargeActionComplete = false;
    float cur_camera_add = 0;
    GameObject effect;
    float chargeTimer;
    Camera myCamera;
    float readyTimer = 0;
    private void Awake()
    {
        enabled = false;
    }
    public void Enter(in float chargeTimer, in bool playerID, in string characterName, Camera myCamera)//仅会修改myCamera
    {
        cur_camera_add = 0;
        readyTimer = 0;
        chargeActionComplete= false;
        this.chargeTimer = chargeTimer;
        this.myCamera = myCamera;

        CharacterColorInfo ColorInfo = ConfigReader.queryColorByCharacter(characterName);
        if (playerID)
            effect = PrefabControl.Instance.createPrefeb(PrefabControl.Instance.ChargeCompleteEffect, playerID, 1, new Vector3(0, 0.7f, 0));
        else
            effect = PrefabControl.Instance.createPrefeb(PrefabControl.Instance.ChargeCompleteEffect, playerID, 2, new Vector3(0, 0.7f, 0));
        effect.GetComponent<Animator>().speed = 1f * (1 / chargeTimer);
        effect.GetComponent<SpriteRenderer>().color = new Color(ColorInfo.accumulateColor.x, ColorInfo.accumulateColor.y, ColorInfo.accumulateColor.z);

        enabled = true;
    }
    public void Exit()
    {
        enabled = false;
        chargeActionComplete = false;
        if (myCamera)
            myCamera.fieldOfView += cur_camera_add;
        if (effect != null)
            PrefabControl.Instance.DestroyGameObject(effect);
    }

    void Update()
    {
        float max_camera_drag_out = 1.5f;
        
        if (readyTimer < chargeTimer) readyTimer += Time.deltaTime;
        else chargeActionComplete= true;
        if (cur_camera_add < max_camera_drag_out)
        {
            if (myCamera)
                myCamera.fieldOfView -= Time.deltaTime;
            cur_camera_add += Time.deltaTime;
        }
        
    }
}
