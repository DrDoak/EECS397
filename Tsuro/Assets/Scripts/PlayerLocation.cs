using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation
{

    public readonly Vector2Int Coordinate;
    public readonly int PositionOnTile;

    public PlayerLocation(Vector2Int m_coordinate, int m_position)
    {
        Coordinate = m_coordinate;
		PositionOnTile = m_position;
    }

	public override bool Equals (object obj)
	{
		PlayerLocation otherLoc = obj as PlayerLocation;
		if (otherLoc == null)
			return false;
		return (Coordinate == otherLoc.Coordinate && PositionOnTile == otherLoc.PositionOnTile);
	}

	public bool IsEdgePosition (Vector2Int boardSize) {
		return ((Coordinate.x == 0 && DirectionUtils.IntToDirection (PositionOnTile) == Direction.LEFT) ||
			(Coordinate.x == boardSize.x - 1 && DirectionUtils.IntToDirection (PositionOnTile) == Direction.RIGHT) ||
			(Coordinate.y == 0 && DirectionUtils.IntToDirection (PositionOnTile) == Direction.DOWN) ||
			(Coordinate.y == boardSize.x - 1 && DirectionUtils.IntToDirection (PositionOnTile) == Direction.UP));
	}
}