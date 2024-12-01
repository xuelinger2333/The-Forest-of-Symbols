using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImageForestBuilder : GroundBuilder
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
        product.type = "象征之森";
        product.isObstacle = true;
        product.isSpecial = true;
        product.type_id = id;
        int randomInt = Random.Range(0, 5);
        product.category_id = randomInt;
    }
    public override void setState()
    {
        product.SetState(new ImageForest());
    }
    public override groundData getResult()
    {
        return product;
    }
}
public class ImageForest : GroundState
{
    public override void applyEffectToPlayer(ref Player player, groundData ground, int x, int y)
    {
        player.Die("ImageForest");
    }
    public override KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground)
    {
        return new KeyValuePair<TileBase, TileBase>(groundDatas[ground.type_id].afterTile[ground.category_id], groundDatas[ground.type_id].beforeTile[ground.category_id]);
    }
    public override void EndSick(groundData ground, int x, int y) { }
    public override void BeSick(groundData ground, int x, int y, Coroutine c) { Debug.Log("suc");if (c != null) { MapManager.Instance.StopCoroutine(c);  } }
}
