using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Administrator {

    bool LegalPlay (SPlayer sp, Board b, Tile t) {

        if (!(sp.IsInHand(t)))
        {
            return false;
        }
        
        if (EliminatesActivePlayer(sp, b, t) && HasLegalMoves (sp, b))
        {
            return false;
        }
        return true;
      }

    bool HasLegalMoves (SPlayer sp, Board b)
    {
        foreach (Spaces s in b.FreeSpaces)
        {
            foreach (Tile t in sp.Hand)
            {
                for (int i = 0; i < 4; i++)
                {
					t.SetRotation((Direction)i);
                    if (!EliminatesActivePlayer(sp, b, t)) {
                        return true;
                    };
                }
            }
        }
   
        return false;
    }

    bool EliminatesActivePlayer (SPlayer sp, Board b, Tile t)
    {
        TurnOutput to = PlayATurn(b.CurrentDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b, t);
        if (to.PlayersOut.Contains(sp))
        {
            return true;
        }

        return false;
    }


    TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile)
    {
        TurnOutput to = new TurnOutput();
        SPlayer ActivePlayer = PlayersIn[0];

     
        if(!EliminatesActivePlayer(ActivePlayer, b, PlacedTile))
         {
            b.Draw(DrawPile, ActivePlayer);
            b.AdvanceTurns(PlayersIn);
        }
        else {
            b.RemovePlayer(PlayersIn, PlayersOut);
        }
        return to;
    }

	public static Vector2 Tests() {
		int numPassed = 0;
		int totalTests = 0;

		return new Vector2 ( numPassed, totalTests );

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