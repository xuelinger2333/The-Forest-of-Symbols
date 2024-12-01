using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class TileOrderComputer 
{
    public static int row_num;
    private static Dictionary<string, int> tiles = new Dictionary<string, int>();
    static int tileDistance = 0;
    public static int maxOrder = 0;
    public static void setRowNumber(int v)
    {
        row_num = v;
    }
    public static void ItemRegist(string s, int order)
    {
        //越早注册，层级越高，图层越高
        tiles[s] = order;
        maxOrder = order > maxOrder ? order : maxOrder;
        tileDistance = maxOrder;
    }
    public static int TileOrder(int x, int y, string type)
    {
        int totalOrder = tileDistance * row_num;
        int result = totalOrder - y * tileDistance;
        int row_order = maxOrder - tiles[type];
        return result - row_order;
    }
}
