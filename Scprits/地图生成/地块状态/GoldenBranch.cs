using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoldenBranchBuilder : GroundBuilder
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
        product.type = "金枝";
        product.isObstacle = false;
        product.isSpecial = true;
        product.type_id = id;
    }
    public override void setState()
    {
        product.SetState(new GoldenBranch());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class GoldenBranch : GroundState
{
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        Debug.Log("金枝对玩家生效！");
        if (player != null)
        {
            player.CDController.setActiveSkillTimer(0);
            player.CDController.AddUniqueSkillCount(25);
        }
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[0], groundDatas[ground.type_id].beforeTile[0]);
    }
    public override void EndSick(groundData ground, int x, int y) { }
    public override void BeSick(groundData ground, int x, int y, Coroutine c) { if (c != null) MapManager.Instance.StopCoroutine(c); }
}
