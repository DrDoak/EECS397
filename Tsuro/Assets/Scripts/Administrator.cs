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
        return false;
    }

    bool EliminatesActivePlayer (SPlayer sp, Board b, Tile t)
	{
		return b.MovePlayer (sp, t).Contains (sp);
    }


    TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile)
    {
        TurnOutput to = new TurnOutput();
        SPlayer ActivePlayer = PlayersIn[0];

     
        if(!EliminatesActivePlayer(ActivePlayer, b, PlacedTile))
         {
           // b.Draw(DrawPile, ActivePlayer);
          //  b.AdvanceTurns(PlayersIn);
        }
        else {
            //b.RemovePlayer(PlayersIn, PlayersOut);
        }
        return to;
    }

	public static void Tests() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		SPlayer p2 = new SPlayer ();
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 7);
		b.AddNewPlayer (p1, new Vector2Int (0, 0), 6);
		//Debug.Assert (a.EliminatesActivePlayer (p1, b, p1.Hand [0]), "Player remains on board");
		//Debug.Assert (a.HasLegalMoves (p1, b));
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