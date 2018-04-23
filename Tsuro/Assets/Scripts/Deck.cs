﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    
    private static readonly int[,,] CompleteDeck = new int[35, 4, 2]
      {
        {{0, 1}, {2, 3}, {4, 5}, {6, 7}},
        {{0, 1}, {2, 4}, {3, 6}, {5, 7}},
        {{0, 6}, {1, 5}, {2, 4}, {3, 7}},
        {{0, 5}, {1, 4}, {2, 7}, {3, 6}},
        {{0, 2}, {1, 4}, {3, 7}, {5, 6}},
        {{0, 4}, {1, 7}, {2, 3}, {5, 6}},
        {{0, 1}, {2, 6}, {3, 7}, {4, 5}},
        {{0, 2}, {1, 6}, {3, 7}, {4, 5}},
        {{0, 4}, {1, 5}, {2, 6}, {3, 7}},
        {{0, 1}, {2, 7}, {3, 4}, {5, 6}},
        {{0, 2}, {1, 7}, {3, 4}, {5, 6}},
        {{0, 3}, {1, 5}, {2, 7}, {4, 6}},
        {{0, 4}, {1, 3}, {2, 7}, {5, 6}},
        {{0, 3}, {1, 7}, {2, 6}, {4, 5}},
        {{0, 1}, {2, 5}, {3, 6}, {4, 7}},
        {{0, 3}, {1, 6}, {2, 5}, {4, 7}},
        {{0, 1}, {2, 7}, {3, 5}, {4, 6}},
        {{0, 7}, {1, 6}, {2, 3}, {4, 5}},
        {{0, 7}, {1, 2}, {3, 4}, {5, 6}},
        {{0, 2}, {1, 4}, {3, 6}, {5, 7}},
        {{0, 7}, {1, 3}, {2, 5}, {4, 6}},
        {{0, 7}, {1, 5}, {2, 6}, {3, 4}},
        {{0, 4}, {1, 5}, {2, 7}, {3, 6}},
        {{0, 1}, {2, 4}, {3, 5}, {6, 7}},
        {{0, 2}, {1, 7}, {3, 5}, {4, 6}},
        {{0, 7}, {1, 5}, {2, 3}, {4, 6}},
        {{0, 4}, {1, 3}, {2, 6}, {5, 7}},
        {{0, 6}, {1, 3}, {2, 5}, {4, 7}},
        {{0, 1}, {2, 7}, {3, 6}, {4, 5}},
        {{0, 3}, {1, 2}, {4, 6}, {5, 7}},
        {{0, 3}, {1, 5}, {2, 6}, {4, 7}},
        {{0, 7}, {1, 6}, {2, 5}, {3, 4}},
        {{0, 2}, {1, 3}, {4, 6}, {5, 7}},
        {{0, 5}, {1, 6}, {2, 7}, {3, 4}},
        {{0, 5}, {1, 3}, {2, 6}, {4, 7}}
      };

    public Hand DragonTileHand;

    public List<Tile> DrawDeck { get; private set; }

    public Deck()
    {
        DrawDeck = new List<Tile>();

        for (int i = 0; i < 35; i++)
        {
            List<Vector2Int> AllPaths = new List<Vector2Int>();
            for (int j = 0; j < 4; j++)
            {
                Vector2Int path = new Vector2Int();
                path.x = CompleteDeck[i, j, 0];
                path.y = CompleteDeck[i, j, 1];
                AllPaths.Add(path);
            }
            DrawDeck.Add(new Tile(AllPaths));       
        }
    }
	public void DrawCard(Hand h)
    {
        if (DrawDeck.Count > 0)
        {
            Tile t = DrawDeck[0];
            h.AddToHand(t);
            DrawDeck.Remove(t);
        }
        else if (DragonTileHand == null)
        {
            DragonTileHand = h;
        }
    }
	public void OnPlayerRemove(Hand removedHand,List<SPlayer> OtherPlayersIn) {
		//Hand goes back in the deck
		foreach (Tile t in removedHand.Pieces) {
			DrawDeck.Add (t);
		}
		if (DragonTileHand == removedHand) {
			int index = (removedHand.PlayerIndex ) % OtherPlayersIn.Count;
			DragonTileHand = OtherPlayersIn [index].MyHand;
		}
		if (DragonTileHand != null)
			m_RefillHands (OtherPlayersIn, DragonTileHand.PlayerIndex);

	}
	void m_RefillHands(List<SPlayer> playersIn, int index) {
		int numberWithThree = 0;
		while (DrawDeck.Count > 0) {
			if (DragonTileHand.Pieces.Count >= 3) {
				numberWithThree++;
				if (numberWithThree == (playersIn.Count))
					break;
				index = (index + 1) % playersIn.Count;
				DragonTileHand = playersIn [index].MyHand;
				continue;
			}
			DrawCard (DragonTileHand);
			index = (index + 1) % playersIn.Count;
			DragonTileHand = playersIn [index].MyHand;

		}
		DragonTileHand = null;
	}
    public static void Tests()
    {
        Debug.Log("Running Tests in Deck");

        Board b = new Board(new Vector2Int(6, 6));
        SPlayer p1 = new SPlayer();
        SPlayer p2 = new SPlayer();

        Deck fullDeck = new Deck();
        Debug.Assert(b.CurrentDeck.DrawDeck.Count == 35);

        b.AddNewPlayer(p1, new Vector2Int(0, 0), 7);
        b.AddNewPlayer(p2, new Vector2Int(0, 0), 6);

        Debug.Assert(b.CurrentDeck.DrawDeck.Count == 29);

        Debug.Assert(p2.MyHand.Pieces[0].IsEqual(fullDeck.DrawDeck[3]));
        Debug.Assert(p1.MyHand.Pieces[2].IsEqual(fullDeck.DrawDeck[2]));
        
    }
}

//Debug.Assert(b.CurrentDeck.Count == 35, "Correct number of tiles in deck");
//Debug.Assert(b.CurrentDeck.Count == 32, "Correct number of tiles given to player");
	//	Debug.Assert(sp.MyHand.Count == 3, "Correct number of tiles in player hand");

		

