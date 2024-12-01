using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterBuilder : GroundBuilder
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
        product.type = "水洼";
        product.isObstacle = false;
        product.isSpecial = true;
        product.type_id = id;
    }
    public override void setState()
    {
        product.SetState(new Water());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class Water : GroundState
{
    MapManager manager = MapManager.Instance;
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        Debug.Log("水洼对玩家生效！");
        //player.stats.GROUND_ChangeWaterState(3);
        //模拟向前滑动
        manager.StartCoroutine(WaterMoveForward((Character) player));
    }
    IEnumerator WaterMoveForward(Character c)
    {
        GameManager.Instance.DisablePlayerInput(c.playerID);
        c.action.ChangeAction(Actions.WaterAction, 0);
        c.OnMove(c.faceDir);
        c.moveCD = 0;
        yield return new WaitForSeconds(c.stats.moveTime);
        c.OnMoveCancel();
        c.action.ChangeAction(Actions.StopWaterAction, 0);
        GameManager.Instance.EnablePlayerInput(c.playerID);
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[0], groundDatas[ground.type_id].beforeTile[0]);
    }
}
