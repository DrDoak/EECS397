using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public List<SPlayer> CurrentPlayersIn;
    public List<SPlayer> CurrentPlayersOut;
    public List<Tile> CurrentDeck;
	private Vector2Int m_boardSize;
	private Dictionary<Vector2Int,Tile> m_placedTiles;

	public Board (Vector2Int size) {
		m_boardSize = size;
		m_placedTiles = new Dictionary<Vector2Int,Tile> ();
		CurrentPlayersIn = new List<SPlayer>();
		CurrentPlayersOut = new List<SPlayer> ();
		CurrentDeck = new List<Tile> ();
	}

	public bool AddNewPlayer(SPlayer sp, Vector2Int coord, int positionOnTile) {
		if (isEdgePosition(coord,positionOnTile) &&
			GetCollisionPlayer (sp , coord, positionOnTile) == null) {
			CurrentPlayersIn.Add(sp);
			sp.MoveToPosition(coord,positionOnTile);
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
		CurrentPlayersIn.Add(p);
		CurrentPlayersOut.Remove(p);
	}


	public bool PlaceTile(Tile t, Vector2Int position, Direction rotation)
    {
		if (!isPositionInBoard (position))
			return false;
		m_placedTiles [position] = t;
		t.PlaceTile (position, rotation);
		List<SPlayer> adjacentPlayers = GetAdjacentPlayers (t);
		foreach (SPlayer sp in adjacentPlayers)
			MovePlayer (sp, t);
		return true;
    }

    public void MovePlayer(SPlayer sp, Tile t)
    {
		if (sp.Coordinate == t.Coordinate) {
			int movedPosition = t.GetPathConnection (sp.PositionOnTile);
			Vector2Int newCoord = sp.Coordinate + DirectionUtils.DirectionToVector(DirectionUtils.IntToDirection(movedPosition));
			int newPos = DirectionUtils.AdjacentPos (movedPosition);
			sp.MoveToPosition ( newCoord, newPos);
			SPlayer colP = GetCollisionPlayer (sp, newCoord, newPos);
			if (colP != null) {
				RemovePlayer (sp);
				RemovePlayer (colP);
			}

			if (!isPositionInBoard (newCoord)) {
				RemovePlayer (sp);
				return;
			}
			if (m_placedTiles.ContainsKey (newCoord))
				MovePlayer (sp, m_placedTiles [newCoord]);
		}
    }

	private SPlayer GetCollisionPlayer(SPlayer sp, Vector2Int coord, int pos) {
		foreach (SPlayer p in CurrentPlayersIn) {
			if (p != sp && p.IsAtPosition (coord, pos))
				return p;
		}
		return null;
	}

	private Direction DirectionToTile(Tile fromTile, Tile toTile) {
		Vector2Int diff = toTile.Coordinate - fromTile.Coordinate;
		if (diff.x == 1 && diff.y == 0)
			return Direction.RIGHT;
		if (diff.x == -1 && diff.y == 0)
			return Direction.LEFT;
		if (diff.x == 0 && diff.y == 1)
			return Direction.UP;
		if (diff.x == 0 && diff.y == -1)
			return Direction.DOWN;
		return Direction.NONE;
	}

	private List<SPlayer> GetAdjacentPlayers(Tile placedTile) {
		List<SPlayer> pList = new List<SPlayer> ();
		foreach (SPlayer p in CurrentPlayersIn) {
			Vector2Int diff = p.Coordinate - placedTile.Coordinate;
			Direction d = DirectionUtils.VectorToDirection (diff);
			if (p.Coordinate == placedTile.Coordinate ||
				d != Direction.NONE && p.IsAtPosition(placedTile.Coordinate + diff, DirectionUtils.InvertDirection(d))) {
				pList.Add (p);
			}
		}
		return pList;
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

	public static void Tests() {
		Debug.Log ("Running Tests in Board");
		Board b = new Board (new Vector2Int(6,6));

		Debug.Assert (b.isPositionInBoard (new Vector2Int (0, 0)), "Position in board");
		Debug.Assert (!b.isPositionInBoard (new Vector2Int (-4, 0)), "Position not in board");
		Debug.Assert (!b.isPositionInBoard (new Vector2Int (0, 9)), "Position not in board");

		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		Debug.Assert (b.PlaceTile (t, new Vector2Int (0, 4), Direction.UP), "On board placement");

		List<Vector2Int> testPaths2 = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 4));
		testPaths.Add (new Vector2Int (1, 5));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (3, 7));
		Tile t2 = new Tile (testPaths2);

		Debug.Assert (!b.PlaceTile (t2, new Vector2Int (0, 6), Direction.DOWN), "Off board placement");

		b.PlaceTile (t2,new Vector2Int(0,5), Direction.DOWN);

		List<Vector2Int> testPaths3 = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 1));
		testPaths.Add (new Vector2Int (2, 3));
		testPaths.Add (new Vector2Int (4, 5));
		testPaths.Add (new Vector2Int (6, 7));
		Tile t3 = new Tile (testPaths3);

		b.PlaceTile (t3, new Vector2Int (1, 4), Direction.UP);

		SPlayer sp = new SPlayer ();



		Debug.Assert (!b.isEdgePosition( new Vector2Int(4,4),0), "invalid edge position");
		Debug.Assert (!b.AddNewPlayer (sp, new Vector2Int(0,4),0), "invalid player addition");
		Debug.Assert (b.AddNewPlayer (sp, new Vector2Int(0,4),7), "valid player addition");

		SPlayer sp2 = new SPlayer ();
		Debug.Assert (b.GetCollisionPlayer (sp2, new Vector2Int (0, 4), 7) == sp, "Detected potential collision");
		Debug.Assert (!b.AddNewPlayer (sp2, new Vector2Int (0, 4), 7), "Not insert player due to collision");
			
		Debug.Assert (b.GetAdjacentPlayers (t3).Count == 0, "Did not incorrectly grab a player");
		Debug.Assert (b.GetAdjacentPlayers (t).Count == 1, "Found adjacent player");

		b.MovePlayer (sp, t);

		Debug.Assert ( sp.Coordinate == new Vector2Int(0,3), "Moved player to correct coordinate");
		Debug.Assert ( sp.PositionOnTile == 1, "Moved player to correct position");
	}
}
