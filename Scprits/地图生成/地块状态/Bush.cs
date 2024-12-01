using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BushBuilder : GroundBuilder
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
        product.hp = Random.Range(2, product.maxHP);
        product.type = "灌木";
        product.isObstacle = false;
        product.isSpecial = true;
        product.type_id = id;
    }
    public override void setState()
    {
        product.SetState(new Bush());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class Bush : GroundState
{
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        Debug.Log("灌木对玩家生效！");
        player.stats.GROUND_ChangeBushState(3);
        MapManager.Instance.Bush_startSettingPlayerExposure(player, ground, x, y);
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[0], groundDatas[ground.type_id].beforeTile[0]);
    }
}
