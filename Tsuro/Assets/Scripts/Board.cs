using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public List<SPlayer> CurrentPlayersIn;
    public List<SPlayer> CurrentPlayersOut;
	public readonly Vector2Int BoardSize;
	public Dictionary<Vector2Int,Tile> m_placedTiles;
    public readonly Deck CurrentDeck;

	public Board (Vector2Int size) {
		BoardSize = size;
		m_placedTiles = new Dictionary<Vector2Int,Tile> ();
		CurrentPlayersIn = new List<SPlayer>();
		CurrentPlayersOut = new List<SPlayer> ();
		CurrentDeck = new Deck();
	}

	public bool AddNewPlayer(SPlayer sp, PlayerLocation ploc) { 
		if (ploc.IsEdgePosition(BoardSize) &&
			GetPlayersAtLocation (ploc, sp).Count == 0) {
			CurrentPlayersIn.Add(sp);
			sp.MoveToPosition(ploc);
			CurrentDeck.OnPlayerAdd (sp.MyHand, CurrentPlayersIn);
			return true;
		}
		return false;
	}

	public void RemovePlayer(SPlayer p)
	{
		CurrentPlayersOut.Add(p);
		CurrentPlayersIn.Remove(p);
		CurrentDeck.OnPlayerRemove (p.MyHand,CurrentPlayersIn);
	}

	public Tile PlaceTile(Tile t, Vector2Int coordinate, Direction rotation)
    {
		if (!isPositionInBoard (coordinate))
			return null;
		if (m_placedTiles.ContainsKey (coordinate))
			return null;
		m_placedTiles [coordinate] = t;
		t.SetCoordinate (coordinate);
		t.SetRotation (rotation);
		return t;
    }
	public Tile PlaceTile(Tile t) {
		if (!isPositionInBoard (t.Coordinate))
			return null;
		if (m_placedTiles.ContainsKey (t.Coordinate))
			return null;
		m_placedTiles [t.Coordinate] = t;
		return t;
	}

	public List<SPlayer> MovePlayer(SPlayer sp, Tile t)
    {
		List<SPlayer> playersEliminated = new List<SPlayer> ();
		if (sp.Location.Coordinate != t.Coordinate)
			return playersEliminated;
		
		int positionAfterPath = t.GetPathConnection (sp.Location.PositionOnTile);

		Vector2Int newCoord = sp.Location.Coordinate + DirectionUtils.DirectionToVector(DirectionUtils.IntToDirection(positionAfterPath));
		int newPos = DirectionUtils.AdjacentPosition (positionAfterPath);
		PlayerLocation newLoc = new PlayerLocation (newCoord, newPos);

		sp.MoveToPosition (newLoc);
		List<SPlayer> colP = GetPlayersAtLocation (newLoc,sp);

		if (colP.Count > 0) {
			playersEliminated.Add (sp);
			playersEliminated.AddRange (colP);
		}
		if (!isPositionInBoard (newCoord)) {
			playersEliminated.Add (sp);
			return playersEliminated;
		}
		if (m_placedTiles.ContainsKey (newCoord))
			return MovePlayer (sp, m_placedTiles [newCoord]);
		
		return playersEliminated;
    }

	public bool IsPlayerEliminated(SPlayer sp, Tile t) {
		return isPlayerEliminated (sp, sp.Location, t);
	}

	private bool isPlayerEliminated(SPlayer sp, PlayerLocation startLocation , Tile t)
	{
		if (t == null || startLocation.Coordinate != t.Coordinate)
			return false;
				
		int positionAfterPath = t.GetPathConnection (startLocation.PositionOnTile);
        
		Vector2Int newCoord = startLocation.Coordinate + DirectionUtils.DirectionToVector(DirectionUtils.IntToDirection(positionAfterPath));
		int newPos = DirectionUtils.AdjacentPosition (positionAfterPath);
		PlayerLocation pl = new PlayerLocation (newCoord, newPos);

		if (!isPositionInBoard (newCoord) || GetPlayersAtLocation (pl,sp).Count > 0)
			return true;
		
		if (m_placedTiles.ContainsKey (newCoord))
			return isPlayerEliminated (sp, pl, m_placedTiles [newCoord]);
		
		return false;
	}

	public List<SPlayer> GetAdjacentPlayers(Tile placedTile) {
		List<SPlayer> pList = new List<SPlayer> ();
		foreach (SPlayer p in CurrentPlayersIn) {
			Vector2Int diff = p.Location.Coordinate - placedTile.Coordinate;
			Direction d = DirectionUtils.VectorToDirection (diff);
			if (p.Location.Coordinate == placedTile.Coordinate ||
				d != Direction.NONE && p.IsOnEdge(placedTile.Coordinate + diff, DirectionUtils.InvertDirection(d))) {
				pList.Add (p);
			}
		}
		return pList;
	}

	public List<SPlayer> GetPlayersAtLocation(PlayerLocation pl, SPlayer ignorePlayer = null) {
		List<SPlayer> sp = new List<SPlayer> ();
		foreach (SPlayer p in CurrentPlayersIn) {
			if (p != ignorePlayer && p.IsAtPosition (pl))
				sp.Add (p);
		}
		return sp;
	}

	private bool isPositionInBoard (Vector2Int coord) {
		return (coord.x >= 0 && coord.x < BoardSize.x 
			&& coord.y >= 0 && coord.y < BoardSize.y);
	}

	public void AdvancePlayers() {
		if (CurrentPlayersIn.Count > 0) {
			SPlayer temp = CurrentPlayersIn [0];
			CurrentPlayersIn.Remove (temp);
			CurrentPlayersIn.Add (temp);
		}
	}
}