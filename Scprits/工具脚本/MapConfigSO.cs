using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct rowData
    {
        public int[] row;
    }


    public rowData[] rows = new rowData[1]; 
}

[CreateAssetMenu(fileName = "MapSetting", menuName = "MapSO")]
public class MapConfigSO : ScriptableObject
{
    public int Width;
    public int Height;
    public ArrayLayout SpecialGroundGrid;
    public ArrayLayout GroundHealthGrid;

    public int GetSpecialGroundTypeId(int x, int y)
    {
        if (x >= Width || y >= Height)
            return -1;
        if (SpecialGroundGrid.rows.Length <= y)
            return -1;
        if (SpecialGroundGrid.rows[y].row.Length <= x)
            return -1;
        return SpecialGroundGrid.rows[y].row[x];
    }

    public int GetGroundHealth(int x, int y)
    {
        if (x >= Width || y >= Height)
            return -1;
        if (GroundHealthGrid.rows.Length <= y)
            return -1;
        if (GroundHealthGrid.rows[y].row.Length <= x)
            return -1;
        return GroundHealthGrid.rows[y].row[x];
    }
}