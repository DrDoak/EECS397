using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public Hand MyHand { get; private set; }
	public PlayerLocation Location { get; private set; }

	public Color PieceColor { get; private set; }

	public SPlayer() {
		MyHand = new Hand ();
	}
    
	public Tile PlayTile(Tile t) {
		MyHand.RemoveFromHand (t);
		return t;
	}

	public bool IsOnEdge (Vector2Int coord, Direction d)
    {
		return (Location.Coordinate == coord && DirectionUtils.DirectionMatch (d, Location.PositionOnTile));
    }

	public bool IsAtPosition (PlayerLocation pl)
	{
		return Location.Equals (pl);
	}

	public void MoveToPosition(PlayerLocation pl) {
		Location = new PlayerLocation (pl.Coordinate, pl.PositionOnTile);
	}
}