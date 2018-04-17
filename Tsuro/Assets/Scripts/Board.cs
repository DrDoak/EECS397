using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public List<Spaces> FreeSpaces;
    public List<Spaces> UsedSpaces;
    public List<SPlayer> CurrentPlayersIn;
    public List<SPlayer> CurrentPlayersOut;
    public List<Tile> CurrentDeck;
	private Dictionary<Vector2Int,Tile> m_placedTiles;

    public void PlaceTile(Tile t)
    {

        return;
    }

	public List<Tile> Draw(List<Tile> DrawPile, SPlayer sp)
	{
		sp.AddToHand(DrawPile[0]);
		DrawPile.Remove(DrawPile[0]);

		return DrawPile;
	}

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
			sp.MoveToPosition (t,endPos);
			Tile next = GetAdjacentTile (t, DirectionUtils.IntToDirection (endPos));
			if (next == null)
				return OffBoard ();
			MovePlayer (sp, next);
		} else {
			return false;
		}
		return false;
    }

	private bool OffBoard () {
		return false;
	}

	private void MoveAdjacentPlayers(Tile placedTile) {
		foreach (SPlayer p in CurrentPlayersIn) {
			Direction d = DirectionToTile (p.Tile, placedTile);
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

	private Tile GetAdjacentTile(Tile t, Direction d) {
		Vector2Int c = t.Coordinate;
		if (d == Direction.UP)
			return m_placedTiles [new Vector2Int (c.x, c.y + 1)];
		if (d == Direction.RIGHT)
			return m_placedTiles [new Vector2Int (c.x + 1, c.y)];
		if (d == Direction.DOWN)
			return m_placedTiles [new Vector2Int (c.x, c.y - 1)];
		if (d == Direction.LEFT)
			return m_placedTiles [new Vector2Int (c.x - 1, c.y)];
		return null;
	}

	private Dictionary<Direction, Tile> GetAdjacentTiles(Tile t) {
		Dictionary<Direction, Tile> adjacentTiles = new Dictionary<Direction, Tile> ();
		Vector2Int c = t.Coordinate;
		Vector2Int vi = new Vector2Int (c.x - 1, c.y);
		if (m_placedTiles.ContainsKey (vi))
			adjacentTiles.Add (Direction.LEFT, m_placedTiles [vi]);
		vi = new Vector2Int (c.x + 1, c.y);
		if (m_placedTiles.ContainsKey (vi))
			adjacentTiles.Add (Direction.RIGHT, m_placedTiles [vi]);
		vi = new Vector2Int (c.x , c.y - 1);
		if (m_placedTiles.ContainsKey (vi))
			adjacentTiles.Add (Direction.DOWN, m_placedTiles [vi]);
		vi = new Vector2Int (c.x, c.y + 1);
		if (m_placedTiles.ContainsKey (vi))
			adjacentTiles.Add (Direction.UP, m_placedTiles [vi]);
		return adjacentTiles;
	}

	public static Vector2 Tests() {
		int numPassed = 0;
		int totalTests = 0;

		return new Vector2 ( numPassed, totalTests );

	}
}
