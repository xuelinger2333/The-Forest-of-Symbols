using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassBuilder : GroundBuilder
{
    private groundData product;

    public override void newData()
    {
        product = new groundData();
    }
    public override void setData(ref List<groundData> groundDatas, int id)
    {
        product.type = groundDatas[id].type;
        product.maxHP = groundDatas[id].maxHP;
        product.hp = groundDatas[id].maxHP;
        product.type = "草地";
        product.type_id = id;
        product.isObstacle = false;
        product.isSpecial = false;
        int randomInt = Random.Range(0, 4);
        product.category_id = randomInt;
    }
    public override void setState()
    {
        product.SetState(new Grass());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class Grass : GroundState
{
    MapManager manager = MapManager.Instance;
    public bool isWired = false;
    int hasPlayerOnGrass = 0;

    private IEnumerator StayAndObserve(float time, groundData g, int x, int y)
    {
        yield return new WaitForSeconds(time);
        if (g.hp < g.maxHP)
        {
            g.hp += 1;
            MapGenerator.Instance.UpdateTile(x, y);
        }
    
        if (g.hp < g.maxHP)
            g.current_state = manager.StartCoroutine(StayAndObserve(1f, g, x, y));
    }
    private IEnumerator StopByPlayer( groundData g, int x, int y)
    {
        while (true)
        {
            yield return null;
            if (hasPlayerOnGrass == 0)
            {
                break;
            }
        }
        if (map.GrassAutoGrow)
            g.current_state = manager.StartCoroutine(StayAndObserve(4f, g, x, y));
    }
    public override void applyEffectToPlayer(ref Player player, groundData groundint ,int x, int y)
    {
        manager = MapManager.Instance;
        manager.Grass_startSettingPlayerExposure(player, groundint, x, y);
    }
    public override void playerEnter(groundData ground, int x, int y)
    {
        hasPlayerOnGrass += 1;
        if (ground.isPlagued == true) return;
        if (ground.current_state != null)
        {
            manager.StopCoroutine(ground.current_state);
        }
        ground.current_state = manager.StartCoroutine(StopByPlayer(ground, x, y));
        
    }
    public override void playerExit(groundData ground, int x, int y)
    {
        hasPlayerOnGrass -= 1;
    }
    public override void BeSick(groundData ground, int x, int y, Coroutine c)
    {
        if (ground.current_state != null)
        {
            manager.StopCoroutine(ground.current_state);
        }
        base.BeSick(ground, x, y, c);
    }
    public override void EndSick(groundData ground, int x, int y)
    {
        base.EndSick(ground, x, y);
        if (hasPlayerOnGrass != 0)
        {
            ground.current_state = manager.StartCoroutine(StopByPlayer(ground, x, y));
        }
        else
        {
            if (map.GrassAutoGrow)
                ground.current_state = manager.StartCoroutine(StayAndObserve(4.1f, ground, x, y));
        }
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        if (ground.hp <= 0)
        {
            return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[ground.category_id + 12], groundDatas[ground.type_id].beforeTile[ground.category_id + 12]);
        }
        else if (ground.hp <= 2)
        {
            return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[ground.category_id + 8], groundDatas[ground.type_id].beforeTile[ground.category_id + 8]);
        }
        else if (ground.hp <= 4)
        {
            return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[ground.category_id + 4], groundDatas[ground.type_id].beforeTile[ground.category_id + 4]);
        }
        else if (ground.hp <= 6)
        {
            return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[ground.category_id], groundDatas[ground.type_id].beforeTile[ground.category_id]);
        }       
        else return new KeyValuePair<TileBase, TileBase>(null, null);
    }

}
