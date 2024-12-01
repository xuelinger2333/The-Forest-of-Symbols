using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoldenTreeBuilder : GroundBuilder
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
        product.type = "黄金树";
        product.isObstacle = true;
        product.isSpecial = true;
        product.type_id = id;
    }
    public override void setState()
    {
        product.SetState(new GoldenTree());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class GoldenTree : GroundState
{
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        Debug.Log("黄金树对玩家生效！");
        if (player != null)
        {
            player.CDController.setActiveSkillTimer(0);
            player.CDController.AddUniqueSkillCount(100);
        }
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[0], groundDatas[ground.type_id].beforeTile[0]);
    }
    public override void EndSick(groundData ground, int x, int y) { }
    public override void BeSick(groundData ground, int x, int y, Coroutine c) { if (c != null) MapManager.Instance.StopCoroutine(c); }
}
