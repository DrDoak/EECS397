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

	private Dictionary<Hand,int> m_handIndex;

    public Deck()
    {
        DrawDeck = new List<Tile>();
		m_handIndex = new Dictionary<Hand,int> ();

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
	public void OnPlayerAdd(Hand addedHand,List<SPlayer> OtherPlayersIn) {
		m_handIndex.Add(addedHand, OtherPlayersIn.Count - 1);
		for (int i = 0; i < 3; i ++ ){ 
			DrawCard (addedHand);
		}
	}
	public void OnPlayerRemove(Hand removedHand,List<SPlayer> OtherPlayersIn) {
		if (!m_handIndex.ContainsKey (removedHand))
			Debug.LogError ("Attempting to remove Hand before being added");
		int removedIndex = m_handIndex [removedHand];
		foreach (Tile t in removedHand.Pieces) {
			DrawDeck.Add (t);
		}
		if (DragonTileHand == removedHand) {
			int index = (removedIndex ) % OtherPlayersIn.Count;
			DragonTileHand = OtherPlayersIn [index].MyHand;
		}
		if (DragonTileHand != null)
			refillHands (OtherPlayersIn, m_handIndex[DragonTileHand]);
		
		m_handIndex.Remove (removedHand);
		for (int i = removedIndex; i < OtherPlayersIn.Count; i ++) {
			m_handIndex[OtherPlayersIn[i].MyHand] = i;
		}
		removedHand.Pieces.Clear ();
	}

	private void refillHands(List<SPlayer> playersIn, int index) {
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
}