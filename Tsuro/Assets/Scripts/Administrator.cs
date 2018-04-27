using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Administrator {
    
    public bool LegalPlay (SPlayer sp, Board b, Tile t) {
		if (!(t.Coordinate == sp.Coordinate)) {
			return false;
		}
        if (!(sp.MyHand.IsInHand(t)))
        {
            return false;
        }
		if (b.IsPlayerEliminated (sp, t) && HasLegalMoves (sp, b))
        {
            return false;
        }
        return true;
      }

    public bool HasLegalMoves (SPlayer sp, Board b)
    {
        foreach (Tile t in sp.MyHand.Pieces)
        {
			Tile tempT = new Tile(t.OriginalPaths);
			tempT.SetCoordinate (sp.Coordinate);
            for (int i = 0; i < 4; i++)
            {
				tempT.SetRotation((Direction)i);
				if (!b.IsPlayerEliminated (sp, tempT)) {
                    return true;
                };
            }
        }  
        return false;
    }

	public TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile)
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
}

public class TurnOutput
{
    public List<Tile> DrawPile;
    public List<SPlayer> PlayersIn;
    public List<SPlayer> PlayersOut;
    public Board b;
    public bool ContinueGame;
}