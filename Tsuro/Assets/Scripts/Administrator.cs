using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Administrator {
    
    bool LegalPlay (SPlayer sp, Board b, Tile t) {

        if (!(sp.MyHand.IsInHand(t)))
        {
            Debug.Log("Tile not in player's hand");

            return false;
        }
        
		if (IsEliminatePlayer(sp, b, t) && HasLegalMoves (sp, b))
        {
            Debug.Log("Tile would eliminate player when moves are available");
            return false;
        }
        return true;
      }

    bool HasLegalMoves (SPlayer sp, Board b)
    {
        foreach (Tile t in sp.MyHand.Pieces)
        {
            for (int i = 0; i < 4; i++)
            {
				t.SetRotation((Direction)i);
				if (!IsEliminatePlayer(sp, b, t)) {
                    return true;
                };
            }
        }   
        return false;
    }

	bool IsEliminatePlayer (SPlayer sp, Board b, Tile t)
	{
		return  b.IsPlayerEliminated (sp, t.Coordinate, sp.PositionOnTile, t);
    }


    TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile)
    {
        TurnOutput to = new TurnOutput();
        SPlayer ActivePlayer = PlayersIn[0];
		foreach (SPlayer p in b.GetAdjacentPlayers(PlacedTile))
			b.MovePlayer (p, PlacedTile);
		b.CurrentDeck.DrawCard (ActivePlayer.MyHand);
		to.DrawPile = b.CurrentDeck.DrawDeck;
		to.PlayersIn = b.CurrentPlayersIn;
		to.PlayersOut = b.CurrentPlayersOut;
		to.b = b;
		Debug.Log (PlayersIn.Count);
		to.ContinueGame = (PlayersIn.Count > 1);
        return to;
    }

	public static void Tests() {
        Debug.Log("Running Tests in Administrator");

        Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		SPlayer p2 = new SPlayer ();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 6);

        //List<Vector2Int> testPaths = new List<Vector2Int> ();
        //testPaths.Add (new Vector2Int (0, 4));
        //testPaths.Add (new Vector2Int (1, 3));
        //testPaths.Add (new Vector2Int (2, 6));
        //testPaths.Add (new Vector2Int (5, 7));
        //Tile t = new Tile (testPaths);

        Debug.Log("Player 1 Hand is: first " + p1.MyHand.Pieces[0] + " second " + p1.MyHand.Pieces[1] + " third " + p1.MyHand.Pieces[2]);


        Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), "Determines that the play is illegal for eliminating player");

        Debug.Log("Player 1 Hand is: first " + p1.MyHand.Pieces[0] + " second " + p1.MyHand.Pieces[1] + " third " + p1.MyHand.Pieces[2]);

        Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is illegal for eliminating player");
        Debug.Assert (a.LegalPlay(p1,b,p1.MyHand.Pieces[2]), "Determines that the play is legal");

        b.PlaceTile (p1.MyHand.Pieces[0], new Vector2Int (0, 0), Direction.UP);
		Debug.Assert (a.IsEliminatePlayer (p1, b, p1.MyHand.Pieces[0]), "Detects when a move eliminates a player");
		Debug.Assert (p1.Coordinate == new Vector2Int(0,0), "Elimination test did not actually move player");
		Debug.Assert (a.HasLegalMoves(p1,b), "Finds that player is able to place something");


       	Debug.Assert (!a.LegalPlay(p1,b,b.CurrentDeck.DrawDeck[14]), "Determines that the play is illegal due to tile not being in hand");
		p1.MyHand.AddToHand (b.CurrentDeck.DrawDeck[14]);
        Debug.Log("Last Tile in Hand Is: " + p1.MyHand.Pieces[2]);
		Debug.Assert (!a.LegalPlay(p1,b, p1.MyHand.Pieces[2]), "Determines that the play is illegal due to being eliminating");
		b.PlaceTile (p1.MyHand.Pieces[2], new Vector2Int (0, 0), Direction.RIGHT);
		Debug.Assert (a.LegalPlay(p1,b, p1.MyHand.Pieces[2]), "Determines that the play is legal");
		Debug.Assert (p1.Coordinate == new Vector2Int(0,0), "Determined that LegalPlay did not actually move player");

		Debug.Assert (a.PlayATurn (b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b, p1.MyHand.Pieces[2]).ContinueGame, "Determines that a play did not end the game");
	}
}

public class TurnOutput
{
    public List<Tile> DrawPile;
    public List<SPlayer> PlayersIn;
    public List<SPlayer> PlayersOut;
    public Board b;
    public bool ContinueGame;
}