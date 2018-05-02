using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public Hand MyHand { get; private set; }
	public PlayerLocation Location { get; private set; }

	public Color PieceColor { get; private set; }
	public readonly Player MyPlayer;

	public SPlayer(Player p = null) {
		MyHand = new Hand ();
		MyPlayer = p;
		Location = new PlayerLocation (new Vector2Int (0, 0), 0);
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
		Debug.Log ("Moved To coord: " + pl.Coordinate + " from: " + Location.Coordinate);
		Location = new PlayerLocation (pl.Coordinate, pl.PositionOnTile);
	}
}