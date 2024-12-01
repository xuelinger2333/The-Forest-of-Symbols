using UnityEngine;
using static UnityEngine.ParticleSystem;
using DG.Tweening;

public interface PlayerAnimationObserver
{
    public void OnNotifyBool(string animationName, bool value);
    public void OnNotifyInt(string animationName, int value);
    public void OnNotifySpeed(float speed);
}
public class PlayerViewAdapter : MonoBehaviour, PlayerAnimationObserver
{
    public Vector2 ViewOffset;
    public Material commonMaterial;
    public Material flashMaterial;
    public Sprite ParticleSprite;
    public bool changeMaterial = false;
    public bool isInMap2;
    public bool playerId;
    public GameObject UIParent;
    public Character player;
    public PlayerViewAdapter enemy_in_this_map;
    private Vector3 PlayerPosition;
    private float timer = 0;
    bool isFlash = false;

    private int mapWidth = 0;
    private GameObject pointer = null;
    public Animator ani { get; private set; }
    public SpriteRenderer sr;
    ParticleSystem particle;
    ParticleSystemRenderer particleRenderer;
    int faceD = 1; //faceD = 1,表示flipx = true
    string current_exposure = "";
    string deathReason = "";
    CharacterColorInfo ColorInfo;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x,transform.position.y + 0.5f), new Vector3(0.8f, 0.8f));
    }
    protected virtual void Awake()
    {
        ViewOffset = new Vector2(0.5F, 0.55F);
        if (player == null)
        {
            if (!playerId) player = GameManager.Instance.player0 as Character;
            else player = GameManager.Instance.player1 as Character;
        }
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        }
        TileOrderComputer.ItemRegist("shadow", 2);
        TileOrderComputer.ItemRegist("player", 3);
        ColorInfo = ConfigReader.queryColorByCharacter(player.characterName);
        RuntimeAnimatorController animatorController = Resources.Load(ColorInfo.AnimatorControllerPath) as RuntimeAnimatorController;
        ani.runtimeAnimatorController = animatorController;
        ParticleSprite = Resources.Load<Sprite>(ColorInfo.ParticleSpritePath);
        player.AddAnimationObserver(this);
    }
    protected virtual void Start()
    { 
        if (player == null) Debug.Log("player null");
        if (player.map == null) Debug.Log("player map null");
        mapWidth = player.map.width;
        if ((isInMap2 && !player.playerID) || (!isInMap2 && player.playerID))
        {
            ani.SetBool("exposure_complete", true);
            current_exposure = "exposure_complete";
        }
        else
        {
            ani.SetBool("exposure_transparent", true);
            current_exposure = "exposure_transparent";
        }
        sr.material = commonMaterial;
        changeMaterial = false;
        if (particle != null)
        {
            var main = particle.main;
            Color start = new Color(ColorInfo.shadowColor[0].x, ColorInfo.shadowColor[0].y, ColorInfo.shadowColor[0].z);
            Color end = new Color(ColorInfo.shadowColor[1].x, ColorInfo.shadowColor[1].y, ColorInfo.shadowColor[1].z);
            main.startColor = new MinMaxGradient(start, end);
            var shape = particle.shape;
            shape.position = new Vector3(ColorInfo.particlePos.x, ColorInfo.particlePos.y, ColorInfo.particlePos.z);
            particle.textureSheetAnimation.SetSprite(0, ParticleSprite);
            main.startSize = ColorInfo.particleScale;
        }
    }
    public void OnNotifyBool(string aniName, bool val)
    {
        ani.SetBool(aniName, val);
    }
    public void OnNotifySpeed(float val)
    {
        ani.speed = val;
    }
    public void OnNotifyInt(string aniName, int val)
    {
        ani.SetInteger(aniName, val);
    }
    public void SetPointer(PlayerViewAdapter enemy)
    {
        if (pointer != null)
        {
            pointer.SetActive(true);
        }
        else
        {
            GameObject instance = Instantiate(PrefabControl.Instance.pointerPrefeb);
            instance.GetComponent<ExposurePointer>().main = this;
            instance.GetComponent<ExposurePointer>().target = enemy;
            instance.transform.SetParent(this.transform, false);
            pointer = instance;
        }
    }
    public void HidePointer()
    {
        if (pointer != null)
        {
            pointer.SetActive(false);
        }
    }
    private void Update()
    {
        //shader
        if (player.action.chargeAction.chargeActionComplete)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (!isFlash)
                {
                    sr.material = flashMaterial;
                    isFlash = true;
                }
                else
                {
                    sr.material = commonMaterial;
                    isFlash = false;
                }
                timer = 0.2f;
            }
            changeMaterial = true;
        }
        else if (!player.action.chargeAction.chargeActionComplete && changeMaterial)
        {
            isFlash = false;
            sr.material = commonMaterial;
        }
    }
    private void FixedUpdate()
    {
        //设定在敌方场地中角色的暴露表现
        if (!((isInMap2 && !player.playerID) || (!isInMap2 && player.playerID)))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (player.stats.exposureLevel_model != 3 && player.stats.exposureLevel_skill == 0)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            if (player.stats.exposureLevel_skill != 0)
            {
                float dis_x = Mathf.Abs(enemy_in_this_map.transform.position.x - transform.position.x);
                float dis_y = Mathf.Abs(enemy_in_this_map.transform.position.y - transform.position.y);
                if (dis_x < 3 && dis_y < 3)
                    enemy_in_this_map.HidePointer();
                else
                    enemy_in_this_map.SetPointer(this);
                ani.SetBool(current_exposure, false);
                current_exposure = "exposure_complete";
                ani.SetBool(current_exposure, true);
            }
            else
            {
                enemy_in_this_map.HidePointer();

                switch (player.stats.exposureLevel_model)
                {
                    case 0:
                        if (current_exposure != "exposure_transparent")
                        {
                            ani.SetBool(current_exposure, false);
                            current_exposure = "exposure_transparent";
                            ani.SetBool(current_exposure, true);
                        }
                        break;
                    case 1:
                        if (current_exposure != "exposure_blink")
                        {
                            ani.SetBool(current_exposure, false);
                            current_exposure = "exposure_blink";
                            ani.SetBool(current_exposure, true);
                        }
                        break;
                    case 2:
                        if (current_exposure != "exposure_black")
                        {
                            ani.SetBool(current_exposure, false);
                            current_exposure = "exposure_black";
                            ani.SetBool(current_exposure, true);
                        }
                        break;
                    case 3:
                        if (current_exposure != "exposure_complete")
                        {
                            ani.SetBool(current_exposure, false);
                            current_exposure = "exposure_complete";
                            ani.SetBool(current_exposure, true);
                        }
                        break;
                }
            }
        }
        //设定主角色的位置
        if (!isInMap2)
        {
            Vector2 RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, player.map.rotationCounts1, player.pos_update, mapWidth, ViewOffset, player.map.space);
            PlayerPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);

        }
        else
        {
            Vector2 RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, player.map.rotationCounts2, player.pos_update, mapWidth, ViewOffset, player.map.space);
            PlayerPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);
        }
        if (transform.position != PlayerPosition)
        {
            transform.position = PlayerPosition;
        }
        //设定主角色的图层顺序
        sr.sortingOrder = TileOrderComputer.TileOrder((int)PlayerPosition.x, (int)PlayerPosition.y, "player");
        //设定是否显现残影效果
        if (player.stats.currentMoveSpeed > PlayerStats.BASIC_MOVE_SPEED)
        {
            if (particle != null && particle.isStopped)
            particle.Play();
        }
        else
        {

            if (particle != null && !particle.isStopped)
                particle.Stop();
        }
        //设定主角色的转向表现
        if (CheckFilp())
        {
            sr.flipX = !sr.flipX;
            //设定残影的转向表现
            if (particle != null)
            {
                float a = particleRenderer.flip.x;
                particleRenderer.flip = new Vector3(1 - a, 0, 0);
                var shape = particle.shape;
                shape.position = new Vector3(-shape.position.x, shape.position.y, shape.position.z);
            }
        }
        //设定残影的图层顺序
        if (particle != null)
        {
            particleRenderer.sortingOrder = TileOrderComputer.TileOrder((int)PlayerPosition.x, (int)PlayerPosition.y, "shadow");
        }

    }
    bool CheckFilp()
    {
        if ((isInMap2 && !player.playerID) || (!isInMap2 && player.playerID))
        {
            if (faceD != (int)player.faceDir.x && player.faceDir.x != 0)
            {
                faceD = (int)player.faceDir.x;
                return true;
            }
        }
        else
        {
            int rotation;
            if (!isInMap2)
            {
                rotation = player.map.rotationCounts2 - player.map.rotationCounts1;
            }
            else
            {
                rotation = player.map.rotationCounts1 - player.map.rotationCounts2;
            }
            if (rotation < 0) rotation += 4;
            Vector2 directionInAnotherMap = UtilFunction.ComputeDirectionRotateforPlayer(player.faceDir, rotation);
            if (faceD != (int)directionInAnotherMap.x && (int)directionInAnotherMap.x != 0)
            {
                faceD = (int)directionInAnotherMap.x;
                return true;
            }

        }
        return false;
    }
    public void setSpriteRendererSortingOrder(int v)
    {
        sr.sortingOrder = v;
    }

    internal void dieAction(string deathReason)
    {
        if (deathReason == "ImageForest")
        {
            if (!((isInMap2 && !player.playerID) || (!isInMap2 && player.playerID)))
            {

                //player.enemy.myCamera.GetComponent<CameraFollow>().followTarget = transform;
                Vector3 CameraTargetPos = new Vector3(transform.position.x, transform.position.y + 0.7f, player.enemy.myCamera.transform.position.z);
                Debug.Log(player.enemy.playerID + " " + isInMap2 + " " + CameraTargetPos);
                player.enemy.myCamera.transform.DOMove(CameraTargetPos, 0.2f).SetUpdate(true).SetEase(Ease.InQuart);
            }
                
        }
        this.deathReason = deathReason;
        ani.enabled = false;
        sr.sprite = Resources.Load<Sprite>(ColorInfo.dieSpritePath);
        sr.color = new Color(1, 1, 1, 1);
        
        int enemy_face = enemy_in_this_map.faceD;
        if (faceD != -enemy_face)
        {
            faceD =  -enemy_face;
            sr.flipX = !sr.flipX;
        }
        float distance_fly = 0.8f;
        transform.position = transform.position + new Vector3(0.2F*-faceD, 0, 0);
        Vector3 move_pos = new Vector3(transform.position.x + distance_fly * -faceD, transform.position.y + 0.1f, 0);
        Vector3 move_pos_final = new Vector3(transform.position.x + 1*-faceD, transform.position.y + 0.2f, 0);
        Sequence deathAnimation = DOTween.Sequence();
        deathAnimation.SetUpdate(true);
        deathAnimation.AppendInterval(0.3f);
        if (!((isInMap2 && !player.playerID) || (!isInMap2 && player.playerID)))
        {
            deathAnimation.AppendCallback(AddBlood);
            sr.sortingLayerName = "AfterAllGameObject";
            enemy_in_this_map.sr.sortingLayerName = "AfterAllGameObject";
            
        }
            
        deathAnimation.Append(transform.DOMove(move_pos, 0.1f).SetEase(Ease.OutExpo));
        deathAnimation.Append(transform.DOMove(move_pos_final, 5f).SetEase(Ease.OutSine));
    }
    void AddBlood()
    {
        player.enemy.myCamera.transform.DORotate(new Vector3(0, 0, 5*-faceD), 0.1f).SetUpdate(true);
        GameObject camera_left = GameObject.FindGameObjectWithTag("DeathCameraShadow_left");
        GameObject camera_right = GameObject.FindGameObjectWithTag("DeathCameraShadow_right");
        camera_left.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.8f);
        camera_right.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.8f);
        if (playerId)
        {
            DOVirtual.Color(new Color(0, 0, 0, 0.9f), new Color(0, 0, 0, 1f), 5F, c => { camera_left.GetComponent<SpriteRenderer>().color = c; }).SetUpdate(true);
        }
        else
        {
            DOVirtual.Color(new Color(0, 0, 0, 0.9f), new Color(0, 0, 0, 1f), 5F, c => { camera_right.GetComponent<SpriteRenderer>().color = c; }).SetUpdate(true);
        }

        GameObject instance = TriggerManager.ExecuteTrigger("结算溅血", UIParent, false);
            if (deathReason == "ImageForest")
                instance.GetComponent<AnimationTrigger>().myParamName = "green";
            else
                instance.GetComponent<AnimationTrigger>().myParamName = "";

        if (isInMap2) instance.transform.localPosition = new Vector3(480, 0, 0);
        else instance.transform.localPosition = new Vector3(-480, 0, 0);
        instance.GetComponent<TriggerInfo>().StartTrigger(UIParent);
    }
}
