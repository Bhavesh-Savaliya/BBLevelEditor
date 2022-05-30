using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellContentType
{
    None,
    Ground, //orange
    Destructible,  // brown
    NonDestructible,    //grey
    Decorative,     //green
    Player,     // yellow
    Enemy,      //red
    Cliff,   //black
    Cliff1,
    Cliff2,
    Cliff3
}

public enum Direction
{
    None, Up, Down, Left, Right
}
[System.Serializable]
public class CellContent
{
    [SerializeField] public Direction direction;
    [SerializeField] public CellContentType cellContentType = CellContentType.None;

    public CellContent(CellContentType cct, Direction dir)
    {
        this.cellContentType = cct;
        this.direction = dir;
    }
}

[System.Serializable]
public class Cell
{
    [SerializeField] public Vector2 position;
    [SerializeField] public CellContent cellContent;
}

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    [SerializeField] public string mapName;
    [SerializeField] public int levelNumber;
    [SerializeField] public Cell[] cells = new Cell[3600];
    [SerializeField] public int maxEnemies;
    [SerializeField] public int initEnemies;
    [SerializeField] public float snowLevel;

    public int width => (int)Mathf.Sqrt(cells.Length);
    public int height => (int)Mathf.Sqrt(cells.Length);


    void OnValidate()
    {
        SetCellPositions();
    }

    public void SetCellPositions()
    {
        int index = 0;
        for (int i = 0; i < Mathf.Sqrt(cells.Length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(cells.Length); j++)
            {
                cells[index].position = new Vector2(i, j);
                index++;
            }
        }
    }

    public Vector2 GetPlayerCellPosition()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].cellContent.cellContentType == CellContentType.Player)
            {
                return cells[i].position;
            }
        }
        return Vector2.zero;
    }

    public Vector2 GetPlayerPosition()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].cellContent.cellContentType == CellContentType.Player)
            {
                Vector2 convertedPos = new Vector2(cells[i].position.y - (30), -cells[i].position.x + 30);
                return convertedPos;
            }
        }
        return Vector2.zero;
    }

    public List<Vector2> GetEnemyCellPositions()
    {
        List<Vector2> enemyPositions = new List<Vector2>();
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].cellContent.cellContentType == CellContentType.Enemy)
            {
                enemyPositions.Add(cells[i].position);
            }
        }
        return enemyPositions;
    }

    public List<Vector2> GetEnemyPositions()
    {
        List<Vector2> enemyPositions = new List<Vector2>();
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].cellContent.cellContentType == CellContentType.Enemy)
            {

                Vector2 convertedPos = new Vector2(cells[i].position.y - (30), -cells[i].position.x + 30);
                enemyPositions.Add(convertedPos);
                //Debug.Log("Enemy At " + convertedPos);
            }
        }
        return enemyPositions;
    }

    public void AddContent(int cellIndex, CellContentType cellContentType, Direction direction = Direction.Up)
    {
        cells[cellIndex].cellContent = new CellContent(cellContentType, direction);
    }

    public void RemoveContent(int cellIndex)
    {
        cells[cellIndex].cellContent = new CellContent(CellContentType.None, Direction.Up);
    }

    public CellContent GetCellContent(int x, int y)
    {
        int index = x + (y * width);
        if (index < cells.Length)
        {
            return cells[index].cellContent;
        }

        return null;
    }
}
