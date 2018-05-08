using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public Hand MyHand { get; private set; }
	public PlayerLocation Location { get; private set; }

	public Color PieceColor;
	public IPlayer MyPlayer { get; private set; }

	public SPlayer(Player p = null) {
		MyHand = new Hand ();
		MyPlayer = p;
		Location = new PlayerLocation (new Vector2Int (0, 0), 0);
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

	public Tile SelectTile(Board b, int deckCount) {
		List<Tile> legalTiles = b.GetLegalTiles (this);
		return MyPlayer.PlayTurn (b, legalTiles, deckCount);
	}
	public void KickPlayerReplaceAI(PlayerAIType aitype = PlayerAIType.RANDOM) {
		PlayerMachine pm = new PlayerMachine (MyPlayer.GetName());
		pm.AIType = aitype;
		MyPlayer = pm;
	}
}