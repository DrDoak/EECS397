using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation
{

    public readonly Vector2Int Coordinate;
    public readonly int Position;

    public PlayerLocation(Vector2Int m_coordinate, int m_position)
    {
        Coordinate = m_coordinate;
        Position = m_position;
    }


    public static void Tests()
    {
        Debug.Log("Running Tests in PlayerLocation");

        Vector2Int v = new Vector2Int(2, 2);
        PlayerLocation pl = new PlayerLocation(v, 3);
        Debug.Assert(pl.Coordinate == v, "player position takes given coordinate");
        Debug.Assert(pl.Position == 3, "player position takes given position");

    }
}
