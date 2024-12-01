using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class UtilFunction 
{
    public static double CalculatePFromCValue(double cValue)
    {
        double preProbability = 0.0;
        double averageAttackCount = 0.0;
        int maxN = (int)Math.Ceiling(1.0 / cValue);
        for (int i = 1; i <= maxN; i++)
        {
            double curProbability = Math.Min(1.0, i * cValue) * (1 - preProbability);
            preProbability += curProbability;
            averageAttackCount += curProbability * i;
        }
        return 1.0 / averageAttackCount;
    }

    public static double CalculateCValue(double p, double error = 0.000005)
    {
        double right = p; double left = 0.0;
        while (true)
        {
            double mid = (right + left) / 2.0;
            double testedCritRate = CalculatePFromCValue(mid);

            if (Math.Abs(testedCritRate - p) <= error) return mid;

            if (testedCritRate > p)
                right = mid;
            else
                left = mid;
        }
    }
    //拨乱反正，这是逆时针旋转
    public static Vector2 ComputeDirectionRotateforPlayer(Vector2 Direction, int rotationCount)
    {
        Vector2 result = new Vector2(0, 0);
        switch (rotationCount)
        {
            case 0:
                result = new Vector2(Direction.x, Direction.y);
                break;
            case 1:
            case -3:
                result = new Vector2(-Direction.y, Direction.x);
                break;
            case 2:
            case -2:
                result = new Vector2(-Direction.x, -Direction.y);
                break;
            case 3:
            case -1:
                result = new Vector2(Direction.y, -Direction.x);
                break;
        }
        return result;
    }
    public static Vector2 ComputePosRotationforViewAdapter(bool mapId, int mapRotationCount, Vector2 Position, int mapEdgeLength, Vector2 offset, int mapSpace)
    {
        Vector2 result = new Vector2(0, 0);
        //if (Position.x < 0 ) Position.x = 0;
        //else if (Position.y < 0) Position.y = 0;
        //else if(Position.x > mapEdgeLength - 1) Position.x = mapEdgeLength - 1;
        //else if (Position.y > mapEdgeLength - 1) Position.y = mapEdgeLength - 1;

        if (!mapId)
        {
            switch (mapRotationCount)
            {
                case 0:
                    result = new Vector2(Position.x + offset.x, Position.y + offset.y);
                    break;
                case 1:
                    result = new Vector2(Position.y + offset.x, mapEdgeLength - 1 - Position.x + offset.y);
                    break;
                case 2:
                    result = new Vector2(mapEdgeLength - 1 - Position.x + offset.x, mapEdgeLength - 1 - Position.y + offset.y);
                    break;
                case 3:
                    result = new Vector2(mapEdgeLength - 1 - Position.y + offset.x, Position.x + offset.y);
                    break;
            }
        }
        else
        {
            switch (mapRotationCount)
            {
                case 0:
                    result = new Vector2(Position.x + offset.x + mapEdgeLength + mapSpace, Position.y + offset.y);
                    break;
                case 1:
                    result = new Vector2(Position.y + offset.x + mapEdgeLength + mapSpace, mapEdgeLength - 1 - Position.x + offset.y);
                    break;
                case 2:
                    result = new Vector2(mapEdgeLength - 1 - Position.x + offset.x + mapEdgeLength + mapSpace, mapEdgeLength - 1 - Position.y + offset.y);
                    break;
                case 3:
                    result = new Vector2(mapEdgeLength - 1 - Position.y + offset.x + mapEdgeLength + mapSpace, Position.x + offset.y);
                    break;
            }
        }
        return result;
    }

    public static KeyValuePair<int, int> computeByTag(string type, object tag_v, string prefabMap_v)
    {
        int tag = (int)tag_v;
        int min = 0, max = 0;
        if (type == "水洼")
        {
            if (prefabMap_v == "")
            {
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 3; max = 6; break;
                    case 2: min = 9; max = 15; break;
                    case 3: min = 20; max = 30; break;
                }
            }

            else
            {
                if (prefabMap_v == "水洼")
                {
                    min = 20; max = 20;
                }
                else
                {
                    min = 12; max = 12;
                }
            }
        }        
        if (type == "荆棘")
        {
            if (prefabMap_v == "")
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 1; max = 4; break;
                    case 2: min = 5; max = 8; break;
                    case 3: min = 9; max = 12; break;
                }
            if (prefabMap_v == "荆棘")
            {
                min = 16; max = 16;
            }
        }
        if (type == "灌木")
        {
            if (prefabMap_v == "")
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 1; max = 6; break;
                    case 2: min = 7; max = 12; break;
                    case 3: min = 13; max = 18; break;
                }
            if (prefabMap_v == "灌木")
            {
                min = 24; max = 24;
            }
        }
        if (type == "毒菇")
        {
            if (prefabMap_v == "")
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 1; max = 3; break;
                    case 2: min = 4; max = 6; break;
                    case 3: min = 7; max = 9; break;
                }
            if (prefabMap_v == "毒菇")
            {
                min = 12; max = 12; 
            }
        }
        if (type == "金枝")
        {
            if (prefabMap_v == "")
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 1; max = 1; break;
                    case 2: min = 2; max = 2; break;
                    case 3: min = 3; max = 3; break;
                }
            if (prefabMap_v == "金枝")
            {
                min = 8; max = 8;
            }
        }
        if (type == "障碍物")
        {
            if (prefabMap_v == "")
                switch (tag)
                {
                    case 0: break;
                    case 1: min = 2; max = 4; break;
                    case 2: min = 5; max = 7; break;
                    case 3: min = 8; max = 10; break;
                }
        }
        if (type == "象征之林")
        {
            min = 0; max = 0;
        }
        return new KeyValuePair<int, int>(min, max);
    }
}
