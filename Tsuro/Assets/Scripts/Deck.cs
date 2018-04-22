using System.Collections;
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

    public static void Tests()
    {
        Debug.Log("Running Tests in Deck");

        Board b = new Board(new Vector2Int(6, 6));
        SPlayer p1 = new SPlayer();
        SPlayer p2 = new SPlayer();
        Deck d = b.CurrentDeck;
        Deck fullDeck = new Deck();
        Debug.Assert(b.CurrentDeck.DrawDeck.Count == 35);

        b.AddNewPlayer(p1, new Vector2Int(0, 0), 7);
        b.AddNewPlayer(p2, new Vector2Int(0, 0), 6);

        Debug.Assert(b.CurrentDeck.DrawDeck.Count == 29);

        Debug.Assert(p2.MyHand.Pieces[0].IsEqual(fullDeck.DrawDeck[3]));
        Debug.Assert(p1.MyHand.Pieces[2].IsEqual(fullDeck.DrawDeck[2]));
        

        //List<Vector2Int> testPaths = new List<Vector2Int>();
        //testPaths.Add(new Vector2Int(0, 4));
        //testPaths.Add(new Vector2Int(1, 3));
        //testPaths.Add(new Vector2Int(2, 6));
        //testPaths.Add(new Vector2Int(5, 7));
        //Tile t = new Tile(testPaths);

        //Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), "Determines that the play is illegal for eliminating player");
        //Debug.Assert(!a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is illegal for eliminating player");
        //Debug.Assert(a.LegalPlay(p1, b, p1.MyHand.Pieces[2]), "Determines that the play is legal");

        //b.PlaceTile(t, new Vector2Int(0, 0), Direction.UP);
        //Debug.Assert(a.IsEliminatePlayer(p1, b, t), "Detects when a move eliminates a player");
        //Debug.Assert(p1.Coordinate == new Vector2Int(0, 0), "Elimination test did not actually move player");
        //Debug.Assert(a.HasLegalMoves(p1, b), "Finds that player is able to place something");


        //Debug.Assert(!a.LegalPlay(p1, b, t), "Determines that the play is illegal due to tile not being in hand");
        //p1.MyHand.AddToHand(t);
        //Debug.Assert(!a.LegalPlay(p1, b, t), "Determines that the play is illegal due to being eliminating");
        //b.PlaceTile(t, new Vector2Int(0, 0), Direction.RIGHT);
        //Debug.Assert(a.LegalPlay(p1, b, t), "Determines that the play is legal");
        //Debug.Assert(p1.Coordinate == new Vector2Int(0, 0), "Determined that LegalPlay did not actually move player");

        //Debug.Assert(a.PlayATurn(b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b, t).ContinueGame, "Determines that a play did not end the game");
    }

}

//Debug.Assert(b.CurrentDeck.Count == 35, "Correct number of tiles in deck");
//Debug.Assert(b.CurrentDeck.Count == 32, "Correct number of tiles given to player");
	//	Debug.Assert(sp.MyHand.Count == 3, "Correct number of tiles in player hand");

		

