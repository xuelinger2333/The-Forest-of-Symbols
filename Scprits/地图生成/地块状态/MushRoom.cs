using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MushRoomBuilder : GroundBuilder
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
        product.type = "毒菇";
        product.isObstacle = false;
        product.isSpecial = true;
        product.type_id = id;
    }
    public override void setState()
    {
        product.SetState(new MushRoom());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class MushRoom : GroundState
{
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        Debug.Log("毒菇对玩家生效！");
        player.stats.GROUND_ChangeMushroomState(5);
        //if (ground.hp <= 0)
        //{
        //    map.CreatGrass(x, y);
        //    map.mapData[x, y].hp = 0;
        //    int index = UnityEngine.Random.Range(0, map.posOfGrass.Count);
        //    map.CreatMushroom((int)map.posOfGrass[index].x, (int)map.posOfGrass[index].y);
        //    map.posOfGrass.Remove(new Vector2((int)map.posOfGrass[index].x, (int)map.posOfGrass[index].y));
        //    map.posOfGrass.Add(new Vector2(x, y));
        //    map.UpdateTile((int)map.posOfGrass[index].x, (int)map.posOfGrass[index].y);
        //}
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[0], groundDatas[ground.type_id].beforeTile[0]);
    }
}
