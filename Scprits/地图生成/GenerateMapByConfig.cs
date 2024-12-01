using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//通过阅读scriptableObject设置，来修改一次mapGenerator中地块的种类和生命的类
//操作的类：mapGenerator
public class GenerateMapByConfig : MonoBehaviour
{
    [SerializeField] MapGenerator map;
    [SerializeField] MapConfigSO mapConfig;
    private void Start()
    {
        ChangeMapSetting();
    }
    public void ChangeMapSetting()
    {
        for (int y = 0; y < mapConfig.Height; y++)
        {
            for (int x = 0; x < mapConfig.Width; x++)
            {
                int num = mapConfig.GetSpecialGroundTypeId(x, y);
                int health = mapConfig.GetGroundHealth(x, y);
                if (num >= 0)
                {
                    map.SetSpecialGround(num, x, y);
                    map.UpdateTile(x, y);
                }
                if (health >= 0)
                {
                    map.mapData[x, y].hp = health;
                    map.UpdateTile(x, y);
                }

            }
        }
    }
}
