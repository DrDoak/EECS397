using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public Hand MyHand { get; private set; }
    public int PositionOnTile { get; private set; }

	private Color m_color;
	public Vector2Int Coordinate;

	public SPlayer() {
		MyHand = new Hand ();
	}
    
	public Tile PlayTile(Tile t) {
		MyHand.RemoveFromHand (t);
		return t;
	}

	public bool IsOnEdge (Vector2Int coord, Direction d)
    {
		return (Coordinate == coord && DirectionUtils.DirectionMatch (d, PositionOnTile));
    }

	public bool IsAtPosition (Vector2Int coord, int pos)
	{
		return (Coordinate == coord && PositionOnTile == pos);
	}

	public void MoveToPosition(Vector2Int pos , int tilePos) {
		Coordinate = pos;
		PositionOnTile = tilePos;
	}
}