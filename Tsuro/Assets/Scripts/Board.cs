using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public List<SPlayer> CurrentPlayersIn;
    public List<SPlayer> CurrentPlayersOut;
	private Vector2Int m_boardSize;
	private Dictionary<Vector2Int,Tile> m_placedTiles;
    public readonly Deck CurrentDeck;

	public Board (Vector2Int size) {
		m_boardSize = size;
		m_placedTiles = new Dictionary<Vector2Int,Tile> ();
		CurrentPlayersIn = new List<SPlayer>();
		CurrentPlayersOut = new List<SPlayer> ();
		CurrentDeck = new Deck();
	}

	public bool AddNewPlayer(SPlayer sp, Vector2Int coord, int positionOnTile) {
		if (isEdgePosition(coord,positionOnTile) &&
			getCollisionPlayer (sp , coord, positionOnTile) == null) {
			CurrentPlayersIn.Add(sp);
			sp.MoveToPosition(coord,positionOnTile);
			CurrentDeck.OnPlayerAdd (sp.MyHand, CurrentPlayersIn);
			return true;
		}
		return false;
	}

	public void AdvanceTurns(List<SPlayer> PlayersIn)
	{
		PlayersIn.Remove(PlayersIn[0]);
		PlayersIn.Add(PlayersIn[0]);
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

	public List<SPlayer> MovePlayer(SPlayer sp, Tile t)
    {
		List<SPlayer> playersEliminated = new List<SPlayer> ();
		if (sp.Coordinate == t.Coordinate) {
			int movedPosition = t.GetPathConnection (sp.PositionOnTile);
			Vector2Int newCoord = sp.Coordinate + DirectionUtils.DirectionToVector(DirectionUtils.IntToDirection(movedPosition));
			int newPos = DirectionUtils.AdjacentPosition (movedPosition);
			sp.MoveToPosition ( newCoord, newPos);
			SPlayer colP = getCollisionPlayer (sp, newCoord, newPos);
			if (colP != null) {
				playersEliminated.Add (sp);
				playersEliminated.Add (colP);
			}

			if (!isPositionInBoard (newCoord)) {
				playersEliminated.Add (sp);
				return playersEliminated;
			}
			if (m_placedTiles.ContainsKey (newCoord))
				return MovePlayer (sp, m_placedTiles [newCoord]);
		}
		return playersEliminated;
    }

	public bool IsPlayerEliminated(SPlayer sp, Tile t) {
		return m_isPlayerEliminated (sp, sp.Coordinate, sp.PositionOnTile, t);
	}

	public bool m_isPlayerEliminated(SPlayer sp, Vector2Int startCoord, int startPos , Tile t)
	{
		if (t == null)
			return false;
		if (startCoord == t.Coordinate) {
			int movedPosition = t.GetPathConnection (startPos);
            
			Vector2Int newCoord = startCoord + DirectionUtils.DirectionToVector(DirectionUtils.IntToDirection(movedPosition));
			int newPos = DirectionUtils.AdjacentPosition (movedPosition);
            SPlayer colP = getCollisionPlayer (sp, newCoord, newPos);
			if (colP != null) {
				return true;
			}

			if (!isPositionInBoard (newCoord)) {
				return true;
			}
			if (m_placedTiles.ContainsKey (newCoord))
				return m_isPlayerEliminated (sp, newCoord, newPos, m_placedTiles [newCoord]);
		}
		return false;
	}

	public List<SPlayer> GetAdjacentPlayers(Tile placedTile) {
		List<SPlayer> pList = new List<SPlayer> ();
		foreach (SPlayer p in CurrentPlayersIn) {
			Vector2Int diff = p.Coordinate - placedTile.Coordinate;
			Direction d = DirectionUtils.VectorToDirection (diff);
			if (p.Coordinate == placedTile.Coordinate ||
				d != Direction.NONE && p.IsOnEdge(placedTile.Coordinate + diff, DirectionUtils.InvertDirection(d))) {
				pList.Add (p);
			}
		}
		return pList;
	}

	private SPlayer getCollisionPlayer(SPlayer sp, Vector2Int coord, int pos) {
		foreach (SPlayer p in CurrentPlayersIn) {
			if (p != sp && p.IsAtPosition (coord, pos))
				return p;
		}
		return null;
	}

	private bool isPositionInBoard (Vector2Int pos) {
		return (pos.x >= 0 && pos.x < m_boardSize.x 
			&& pos.y >= 0 && pos.y < m_boardSize.y);
	}

	private bool isEdgePosition (Vector2Int pos,int positionOnTile) {
		return ((pos.x == 0 && DirectionUtils.IntToDirection (positionOnTile) == Direction.LEFT) ||
			(pos.x == m_boardSize.x - 1 && DirectionUtils.IntToDirection (positionOnTile) == Direction.RIGHT) ||
			(pos.y == 0 && DirectionUtils.IntToDirection (positionOnTile) == Direction.DOWN) ||
			(pos.y == m_boardSize.x - 1 && DirectionUtils.IntToDirection (positionOnTile) == Direction.UP));
	}
}