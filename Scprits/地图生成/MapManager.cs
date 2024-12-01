using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MapManager : MonoBehaviour
{
    MapGenerator map;
    public static MapManager Instance { get; private set; }
    public PrefabControl prefabControl;

    private void Awake()
    {
        // 确保只有一个实例
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    private void Start()
    {
        map = MapGenerator.Instance;
        prefabControl = PrefabControl.Instance;
    }
    //象征之林效果
    public void StartImageForest(int interval)
    {
        StartCoroutine(ImageForest(interval));
    }
    public Coroutine startGroundBeSickCoroutine(float Time, groundData g, int x, int y)
    {
        return StartCoroutine(ControlWired(Time, g, x, y));
    }
    public void Grass_startSettingPlayerExposure(Player player, groundData g, int x, int y)
    {
        StartCoroutine(Grass_SetPlayerExposure(player, g, x, y));
    }
    public void Bush_startSettingPlayerExposure(Player player, groundData g, int x, int y)
    {
        StartCoroutine(Bush_SetPlayerExposure(player, g, x, y));
    }
    private IEnumerator ImageForest(int interval)
    {
        if (interval >= 1)
        {
            Player player0, player1;
            player0 = GameManager.Instance.player0;
            player1 = GameManager.Instance.player1;
            for (int i = 0; i < map.width / 2 - 1; i++)
            {
                yield return new WaitForSeconds(interval - 1.5f);

                GameObject Instance = Instantiate(prefabControl.ImageForestHintEffect);
                int in_len = map.width - (map.ImageForestEdgeValue) * 2;
                Instance.transform.GetChild(1).localScale = new Vector3(in_len + 0.3f, in_len + 0.3f, 1);
                Instance.transform.GetChild(0).localScale = new Vector3(in_len - 2.2f, in_len - 2.2f, 1);

                GameObject Instance2 = Instantiate(prefabControl.ImageForestHintEffect);

                Instance2.transform.localPosition = new Vector3(map.width + map.space, 0, 0);
                Instance2.transform.GetChild(1).localScale = new Vector3(in_len + 0.3f, in_len + 0.3f, 1);
                Instance2.transform.GetChild(0).localScale = new Vector3(in_len - 2.2f, in_len - 2.2f, 1);
                Sequence s = DOTween.Sequence();
                for (int t = 0; t < 3; t++)
                {
                    s.Append(Instance.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0f, 0.2f));
                    s.Append(Instance.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0.5f, 0.2f));
                }
                for (int t = 0; t < 4; t++)
                {
                    s.Append(Instance.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0f, 0.1f));
                    s.Append(Instance.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0.5f, 0.1f));
                }
                Sequence s2 = DOTween.Sequence();
                for (int t = 0; t < 3; t++)
                {
                    s2.Append(Instance2.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0f, 0.2f));
                    s2.Append(Instance2.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0.5f, 0.2f));
                }
                for (int t = 0; t < 4; t++)
                {
                    s2.Append(Instance2.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0f, 0.1f));
                    s2.Append(Instance2.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0.5f, 0.1f));
                }
                yield return new WaitForSeconds(1.5f);
                Destroy(Instance);
                Destroy(Instance2);
                map.AddImageForestByOne(player0, player1);
            }
        }

    }
    private IEnumerator Grass_SetPlayerExposure(Player player, groundData g, int x, int y)
    {
        int lastghp = -1;
        while (player.pos_x == x && player.pos_y == y)
        {
            yield return null;
            int exposure_level = 0;
            if (g.hp == lastghp) continue;
            else lastghp = g.hp;
            switch (g.hp)
            {
                case int n when (n >= 5 && n <= 6):
                    exposure_level = 0;
                    break;
                case int n when (n >= 3 && n <= 4):
                    exposure_level = 1;
                    break;
                case int n when (n >= 1 && n <= 2):
                    exposure_level = 2;
                    break;
                case int n when (n == 0):
                    exposure_level = 3;
                    break;
            }
            player.stats.SetExposureLevel_model(exposure_level);
        }
    }
    private IEnumerator Bush_SetPlayerExposure(Player player, groundData g, int x, int y)
    {
        while (player.pos_x == x && player.pos_y == y)
        {
            yield return null;
            int exposure_level = 0;
            if (g.hp != 0) exposure_level = 0;
            else exposure_level = 3;
            player.stats.SetExposureLevel_model(exposure_level);
        }
    }
    private IEnumerator ControlWired(float t, groundData g, int x, int y)
    {
        GameObject instance = Instantiate(prefabControl.plagueMapPrefab);
        Animator ani = instance.GetComponent<Animator>();
        Vector2 pos = UtilFunction.ComputePosRotationforViewAdapter(false, map.rotationCounts1, new Vector2(x, y), map.width, new Vector2(0.5f, 0.8f), map.space);
        instance.transform.position = new Vector3(pos.x, pos.y, 0);
        instance.transform.SetParent(prefabControl.plaguePrarent.transform, true);

        GameObject instance2 = Instantiate(prefabControl.plagueMapPrefab);
        pos = UtilFunction.ComputePosRotationforViewAdapter(true, map.rotationCounts2, new Vector2(x, y), map.width, new Vector2(0.5f, 0.8f), map.space);
        instance2.transform.position = new Vector3(pos.x, pos.y, 0);
        instance2.transform.SetParent(prefabControl.plaguePrarent.transform, true);

        map.mapData[x, y].wiredCoroutine = StartCoroutine(Withered( x, y));
        float CD = t; float timer = 0;
        while (timer < CD)
        {
            timer += Time.deltaTime;
            if (map.mapData[x, y].type == "金枝" || map.mapData[x, y].type == "障碍物" || map.mapData[x, y].type == "象征之森")
            {
                Destroy(instance);
                Destroy(instance2);
                if (map.mapData[x, y].wiredCoroutine != null) StopCoroutine(map.mapData[x, y].wiredCoroutine);
                map.mapData[x, y].wiredCoroutine = null;
                map.mapData[x, y].endSick(x, y);
                map.mapData[x, y].isPlagued = false;
                yield break;
            }
            yield return null;
        }
       // yield return new WaitForSeconds(8f);
        Destroy(instance);
        Destroy(instance2);
        if (map.mapData[x, y].wiredCoroutine != null) StopCoroutine(map.mapData[x, y].wiredCoroutine);
        map.mapData[x, y].wiredCoroutine = null;
        map.mapData[x, y].endSick(x, y);
    }
    private IEnumerator Withered(int x, int y)
    {
        Player p = null;
        while (map.mapData[x, y].hp > 0)
        {
            map.UpdateMap(x, y, p); //减血，更新地块种类，并更改地块贴图
            
            yield return new WaitForSeconds(2f);
        }
    }
}
