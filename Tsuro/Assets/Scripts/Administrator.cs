using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Administrator {
    
    bool LegalPlay (SPlayer sp, Board b, Tile t) {
		if (!(t.Coordinate == sp.Coordinate)) {
			return false;
		}
        if (!(sp.MyHand.IsInHand(t)))
        {
            return false;
        }
		if (IsEliminatePlayer(sp, b, t) && HasLegalMoves (sp, b))
        {
            return false;
        }
        return true;
      }

    bool HasLegalMoves (SPlayer sp, Board b)
    {
        foreach (Tile t in sp.MyHand.Pieces)
        {
			Tile tempT = new Tile(t.OriginalPaths);
            for (int i = 0; i < 4; i++)
            {
				tempT.SetCoordinateAndDirection(sp.Coordinate,(Direction)i);
				if (!IsEliminatePlayer(sp, b, tempT)) {
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
		List<SPlayer> eliminatedPlayers = new List<SPlayer> ();
		foreach (SPlayer p in b.GetAdjacentPlayers(PlacedTile))
			eliminatedPlayers.AddRange(b.MovePlayer (p, PlacedTile));

		foreach (SPlayer sp in eliminatedPlayers) {
			b.RemovePlayer (sp);
		}
		if (ActivePlayer.MyHand.Pieces.Count < 3)
			b.CurrentDeck.DrawCard (ActivePlayer.MyHand);
		to.DrawPile = b.CurrentDeck.DrawDeck;
		to.PlayersIn = b.CurrentPlayersIn;
		to.PlayersOut = b.CurrentPlayersOut;
		to.b = b;
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

		p1.MyHand.Pieces[0].SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.UP);

		//making a move from the edge
		Debug.Assert (a.IsEliminatePlayer (p1, b, p1.MyHand.Pieces[0]), "Detects when a move eliminates a player");
		Debug.Assert (p1.Coordinate == new Vector2Int(0,0), "Elimination test did not actually move player");
		Debug.Assert (a.HasLegalMoves(p1,b), "Finds that player is able to place something");

		//making an illegal move, specifically where the move is an elimination move, but there are non-elimination moves available
        Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), "Determines that the play is illegal for eliminating player");
        Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is illegal for eliminating player");
        Debug.Assert (a.LegalPlay(p1,b,p1.MyHand.Pieces[2]), "Determines that the play is legal");

		p1.MyHand.Pieces [0].SetCoordinateAndDirection (new Vector2Int (1, 0), Direction.UP);
		Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), " play is illegal due to tile not being in same location as player");

		//making a move where the tile is not placed in its original position (i.e., it is rotated)
		p1.MyHand.Pieces [1].SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.RIGHT);
		Debug.Assert(a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is legal after a rotation");
       	Debug.Assert (!a.LegalPlay(p1,b,b.CurrentDeck.DrawDeck[14]), "Determines that the play is illegal due to tile not being in hand");

		//making a move where multiple players move at once
		Tile oldThirdTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[2]);
		Debug.Assert (p1.MyHand.Pieces.Count == 2, "Player removed a card from deck");
		Debug.Assert (a.PlayATurn (b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b,oldThirdTile).ContinueGame, "Determines that a play did not end the game");
		Debug.Assert (p1.MyHand.Pieces.Count == 3, "Player drew a card back from the deck");
		Debug.Assert (!p1.MyHand.Pieces [2].IsEqual (oldThirdTile), "Card drawn is different from old card");
		Debug.Assert (b.CurrentDeck.DrawDeck.Count == 28, "Deck has been decremented");
		Debug.Assert (p1.Coordinate == new Vector2Int (1, 0), "Player one moved to correct location");
		Debug.Assert (p2.Coordinate == new Vector2Int (0, 1), "Player two moved to correct location");

		//making a move that causes a token to cross multiple tiles

		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 6);
	
		//Just placing a tile on the board for testing
		b.PlaceTile(p1.MyHand.Pieces[1], new Vector2Int(1,0),Direction.UP);
		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[2]);
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.UP);
		//a.PlayATurn (b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b,oldTile);
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);
		Debug.Assert (p1.Coordinate == new Vector2Int (2, 0), "Player moves across two spaces");

		//making a move where multiple players are eliminated
		//moving where no player has the dragon tile before or after
		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		SPlayer p3 = new SPlayer ();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 4);
		b.AddNewPlayer (p3, new Vector2Int (0, 0), 5);

		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "No player has Dragon tile beforehand");

		oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.DOWN);

		TurnOutput to = a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Debug.Assert (to.PlayersOut.Count == 2, "Two players were eliminated");
		Debug.Assert (to.PlayersIn.Count == 1, "Players eliminated removed from Players in");
		Debug.Assert (to.ContinueGame == false, "Game end has been detected");
		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "No player has Dragon tile afterwards");

		//moving where one player has the dragon tile before and no one gets any new tiles
		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 4);

		//Empty the deck
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p2.MyHand);
		}
		Debug.Assert (b.CurrentDeck.DrawDeck.Count == 0, "Deck is empty");
		b.CurrentDeck.DrawCard (p1.MyHand);
		Debug.Assert (p1.MyHand.Pieces.Count == 3, "Player 1 didn't draw any new cards");
		Debug.Assert (b.CurrentDeck.DragonTileHand == p1.MyHand, "Dragon Tile owner is now player 1");

		oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[2]);
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.UP);
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Debug.Assert (b.CurrentDeck.DragonTileHand == p1.MyHand, "Player 1 still has dragon tile afterwards");
		b.CurrentDeck.DrawCard (p2.MyHand);
		Debug.Assert (b.CurrentDeck.DragonTileHand == p1.MyHand, "Player 2 does not steal  dragon tile afterwards");


		//moving where the player that has the dragon tile makes a move that causes an elimination (of another player)
		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		p3 = new SPlayer();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 4);
		b.AddNewPlayer (p3, new Vector2Int (0, 5), 7);
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p2.MyHand);
		}
		b.CurrentDeck.DrawCard (p1.MyHand);

		oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.DOWN);
	
		//P2 is eliminated
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Debug.Assert (p1.MyHand.Pieces.Count == 3, "Player 1 drew a card from the new deck");
		Debug.Assert (b.CurrentDeck.DrawDeck.Count == 28, "P2's cards are returned back to the deck");
		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "Dragon tile returned to deck.");

		//moving where a player that does not have the dragon tile makes a move and it causes an elimination of the player that has the dragon tile
		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		p3 = new SPlayer();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 4);
		b.AddNewPlayer (p3, new Vector2Int (0, 5), 7);
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p1.MyHand);
		}
		b.CurrentDeck.DrawCard (p2.MyHand);

		Debug.Assert (b.CurrentDeck.DragonTileHand == p2.MyHand, "Player 2 has dragon tile");

		oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		p3.MyHand.RemoveFromHand (p3.MyHand.Pieces[1]);

		int oldP1Count = p1.MyHand.Pieces.Count;
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.DOWN);


		//Eliminate Player 2
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);


		Debug.Assert (p3.MyHand.Pieces.Count == 3, "Player 3 drew a card from the new deck");
		Debug.Assert (p1.MyHand.Pieces.Count == oldP1Count, "Player 1 did not draw a card from the deck");
		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "Dragon tile returned to deck.");

		//moving where the player that has the dragon tile makes a move that causes themselves to be eliminated
		b = new Board (new Vector2Int(6,6));
		p1 = new SPlayer ();
		p2 = new SPlayer ();
		p3 = new SPlayer();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 4);
		b.AddNewPlayer (p2, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p3, new Vector2Int (0, 5), 7);
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p1.MyHand);
		}
		b.CurrentDeck.DrawCard (p1.MyHand);

		oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinateAndDirection (new Vector2Int (0, 0), Direction.DOWN);

		p2.MyHand.RemoveFromHand (p2.MyHand.Pieces[1]);
		p3.MyHand.RemoveFromHand (p3.MyHand.Pieces[1]);

		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);


		Debug.Assert (p2.MyHand.Pieces.Count == 3, "Player 2 drew a card from the new deck");
		Debug.Assert (p3.MyHand.Pieces.Count == 3, "Player 3 did not draw a card from the new deck");
		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "Dragon tile returned to deck.");

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