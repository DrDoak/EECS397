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

	public bool PlaceTile(Tile t, Vector2Int position, Direction rotation)
    {
		if (!isPositionInBoard (position))
			return false;
		m_placedTiles [position] = t;
		t.PlaceTile (position, rotation);
		return true;
    }

	public bool AddNewPlayer(SPlayer sp, Vector2Int position, int positionOnTile) {
		if (isEdgePosition(position,positionOnTile)) {
			CurrentPlayersIn.Add(sp);
			sp.MoveToPosition(position,positionOnTile);
			return true;
		}
		return false;
	}

	/*public List<Tile> Draw(List<Tile> DrawPile, SPlayer sp)
	{
		sp.AddToHand(DrawPile[0]);
		//DrawPile.Remove(DrawPile[0]);

		//return DrawPile;
	}*/

	public void AdvanceTurns(List<SPlayer> PlayersIn)
	{
		PlayersIn.Remove(PlayersIn[0]);
		PlayersIn.Add(PlayersIn[0]);
	}

	public void RemovePlayer(List<SPlayer> PlayersIn, List<SPlayer> PlayersOut)
	{
		PlayersOut.Add(PlayersIn[0]);
		PlayersIn.Remove(PlayersIn[0]);
	}

    public List<SPlayer> MovePieces( Tile t)
    {
        List<SPlayer> Adjacents = new List<SPlayer>();
        foreach (SPlayer sp in CurrentPlayersIn)
        {
            //Direction d = sp.GetAdjacentDirection(t);
            /*if (d != Direction.NONE)
            {
                MovePlayer(sp,t);
            }*/
        }
        return Adjacents;
    }
   	
    public bool MovePlayer(SPlayer sp, Tile t)
    {
		int adjPos = sp.AdjacentPos ();
		SPlayer collidedPlayer = t.GetBlockingPlayer (adjPos);
		if (collidedPlayer != null)
			return false;
		int endPos = t.GetPathConnection (adjPos);
		if (t.GetBlockingPlayer (endPos) == null) {
			sp.MoveToPosition (t.Coordinate,endPos);
			Tile next = GetAdjacentTile (t, DirectionUtils.IntToDirection (endPos));
			if (next == null)
				return !isPositionInBoard (t.Coordinate);
			MovePlayer (sp, next);
		} else {
			return false;
		}
		return false;
    }

	private void MoveAdjacentPlayers(Tile placedTile) {
		foreach (SPlayer p in CurrentPlayersIn) {
			Direction d = DirectionUtils.VectorToDirection (placedTile.Coordinate - p.Coordinate);
			if (d != Direction.NONE && p.IsOnDirection(d)) {
				MovePlayer (p, placedTile);
			}
		}
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

	private List<SPlayer> GetAdjacentPlayers(Tile originTile) {
		List<SPlayer> lp = new List<SPlayer> ();
		int dir = 0;
		foreach (Tile adjacentTile in GetAllAdjacentTiles(originTile)) {
			lp.AddRange(adjacentTile.GetAdjacentPlayers (originTile));
		}
		return lp;
	}

	private List<Tile> GetAllAdjacentTiles(Tile t) {
		List<Tile> l = new List<Tile> ();
		for (int i = 0; i < 4; i++) {
			Tile at = GetAdjacentTile (t, (Direction)i);
			if (at != null)
				l.Add (at);
		}
		return l;
	}

	private Tile GetAdjacentTile(Tile t, Direction d) {
		Vector2Int c = t.Coordinate +  DirectionUtils.DirectionToVector (d);
		if (!isPositionInBoard (c))
			return null;
		if (m_placedTiles.ContainsKey (c))
			return m_placedTiles [c];
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

		Debug.Assert (b.PlaceTile (t, new Vector2Int (0, 4), Direction.DOWN), "On board placement");

		List<Vector2Int> testPaths2 = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 4));
		testPaths.Add (new Vector2Int (1, 5));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (3, 7));
		Tile t2 = new Tile (testPaths2);

		Debug.Assert (!b.PlaceTile (t2, new Vector2Int (0, 6), Direction.DOWN), "Off board placement");

		b.PlaceTile (t2,new Vector2Int(0,5), Direction.DOWN);

		Debug.Assert (b.GetAdjacentTile (t,Direction.LEFT) == null, "AdjacentTile None");
		Debug.Assert (b.GetAdjacentTile (t,Direction.UP) == t2, "Found Adjacent Tile");
		Debug.Assert (b.GetAdjacentTile (t2,Direction.UP) == null, "Detected off board");
		Debug.Assert (b.GetAdjacentTile (t2,Direction.LEFT) == null, "Detected off board");

		List<Vector2Int> testPaths3 = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 1));
		testPaths.Add (new Vector2Int (2, 3));
		testPaths.Add (new Vector2Int (4, 5));
		testPaths.Add (new Vector2Int (6, 7));
		Tile t3 = new Tile (testPaths3);

		b.PlaceTile (t3, new Vector2Int (1, 4), Direction.DOWN);

		Debug.Assert (b.GetAllAdjacentTiles (t).Count == 2, "Found both adjacaent tiles");

		SPlayer sp = new SPlayer ();

		Debug.Assert (!b.isEdgePosition( new Vector2Int(4,4),0), "invalid edge position");
		Debug.Assert (!b.AddNewPlayer (sp, new Vector2Int(0,4),0), "invalid player addition");
		Debug.Assert (b.AddNewPlayer (sp, new Vector2Int(0,4),7), "valid player addition");
	}
}
