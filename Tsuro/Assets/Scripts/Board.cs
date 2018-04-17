using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    public List<Spaces> FreeSpaces;
    public List<Spaces> UsedSpaces;
    public List<SPlayer> CurrentPlayersIn;
    public List<SPlayer> CurrentPlayersOut;
    public List<Tile> CurrentDeck;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaceTile(Tile t)
    {

        return;
    }

    public List<SPlayer> MovePieces( Tile t)
    {
        List<SPlayer> Adjacents = new List<SPlayer>();
        foreach (SPlayer sp in CurrentPlayersIn)
        {
            Direction d = sp.GetAdjacentDirection(t);
            if (d != Direction.NONE)
            {
                MovePlayer(sp,t);
            }
        }
        return Adjacents;
    }
	public void MovePlayer(SPlayer sp, Tile t)
    {
		while (MovePlayerTile(sp,t))
        {

        }
    }
     
    public bool MovePlayerTile(SPlayer sp, Tile t)
    {
		return false;
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

	public static Vector2 Tests() {
		int numPassed = 0;
		int totalTests = 0;

		return new Vector2 ( numPassed, totalTests );

	}
}
