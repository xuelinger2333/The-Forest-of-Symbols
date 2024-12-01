using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private PlayerViewAdapter playerView => GetComponent<PlayerViewAdapter>();
    private Player player;
    float chromaticFadeOutCD = 0.5f;
    float chromaticFadeOutTimer = 0.5f;
    [SerializeField]
    protected LayerMask whatisplayer;


    private void Start()
    {
        player = playerView.player;
    }
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void PauseGame()
    {
        if (!GameManager.Instance)
            return;
        GameManager.Instance.PauseGame();        
        float Target = player.myCamera.fieldOfView - 8f;
        player.myCamera.DOFieldOfView(Target, 0.2f).SetUpdate(true);

        Sequence myCameraSeq = DOTween.Sequence();
        myCameraSeq.SetUpdate(true);
        myCameraSeq.Append(player.myCamera.DORect(new Rect(0f, 0f, 1f, 1f), 0.2f)).SetEase(Ease.InCirc);

        myCameraSeq.OnComplete(() => { 
            
            GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;});
        if (player.playerID)
            player.enemy.myCamera.DORect(new Rect(1f, 0f, 0f, 1f), 0.2f).SetEase(Ease.InCirc).SetUpdate(true);
        else
            player.enemy.myCamera.DORect(new Rect(0f, 0f, 0f, 1f), 0.2f).SetEase(Ease.InCirc).SetUpdate(true);
    }
    private void UnPauseGame()
    {
        if (!GameManager.Instance)
            return;
        float Target = player.myCamera.fieldOfView + 8f;
        player.myCamera.DOFieldOfView(Target, 0.2f).SetUpdate(true);
        if (player.playerID)
        {
            Sequence myCameraSeq = DOTween.Sequence();
            myCameraSeq.SetUpdate(true);
            myCameraSeq.Append(player.myCamera.DORect(new Rect(0f, 0f, 0.5f, 1f), 0.1f)).SetEase(Ease.InCirc);
            myCameraSeq.OnComplete(() => {
                GameManager.Instance.UnPauseGame();
                GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
            });

            Sequence myCameraSeq2 = DOTween.Sequence();
            myCameraSeq2.SetUpdate(true);
            myCameraSeq2.Append(player.enemy.myCamera.DORect(new Rect(0.5f, 0f, 0.5f, 1f), 0.1f)).SetEase(Ease.InCirc);
        }
        else 
        {
            Sequence myCameraSeq = DOTween.Sequence();
            myCameraSeq.SetUpdate(true);
            myCameraSeq.Append(player.myCamera.DORect(new Rect(0.5f, 0f, 0.5f, 1f), 0.1f)).SetEase(Ease.InCirc);
            myCameraSeq.OnComplete(() => {
                GameManager.Instance.UnPauseGame();
                GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
            });

            Sequence myCameraSeq2 = DOTween.Sequence();
            myCameraSeq2.SetUpdate(true);
            myCameraSeq2.Append(player.enemy.myCamera.DORect(new Rect(0f, 0f, 0.5f, 1f), 0.1f)).SetEase(Ease.InCirc);
        }
        
    }
    private void AttackTrigger()
    {
        //修改了判定杀死对方的条件，将pos_update改为pos_x
        if (player.enemy && player.pos_x == player.enemy.pos_x && player.pos_y == player.enemy.pos_y)
        {
            Debug.Log("攻击" + player.enemy.name);
            player.enemy.Die("Attack");
            //判定杀死敌人后，不进行其他伤害判定
            return;
        }
        player.AttackTriggerCalled();
        if (player.enemy && player.enemy.characterName == "织梦魔女" && player.enemy.stats.isCoffinExist)
        {
            //判断附近1格距离内是否有织梦魔女棺椁
            Dreamweaver e = (Dreamweaver)player.enemy;
            int dx = math.abs(e.posOfCoffin.x - player.pos_x);
            int dy = math.abs(e.posOfCoffin.y - player.pos_y);
            if (dx <= 1 && dy <= 1)
            {
                //有则销毁
                e.DestroyCoffin();
                Character c = (Character)player;
                //取消擦刀时间
                player.stateMachine.ChangeState(c.idleState);
            }

        }
    }
    private void EffectTrigger()
    {
        if (player.chromatic)
            player.chromatic.intensity.value += 1f;
        if (player.myCamera)
        {
            player.myCamera.DOShakePosition(0.3f, 0.1f);
        }
        
    }
    private void EffectFadeTrigger()
    {
        chromaticFadeOutTimer = 0;
    }

    private void Update()
    {
        if (chromaticFadeOutTimer < chromaticFadeOutCD)
        {
            chromaticFadeOutTimer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(0f, 0.9f, chromaticFadeOutTimer / chromaticFadeOutCD);
            if (player.chromatic)
                player.chromatic.intensity.value = 1 - currentAlpha;
        }
       
    }
}
