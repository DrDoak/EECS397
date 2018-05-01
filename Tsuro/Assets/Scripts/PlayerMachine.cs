using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMachine : Player {

    public PlayerMachine(string m_name) : base(m_name) { }

    public override string GetName()
    {
        return base.GetName();
    }

    public override void Initialize(Color m_color, List<Color> player_colors)
    {
        base.Initialize(m_color, player_colors);
    }

    public override PlayerLocation PlacePawn(Board b)
    {
		//while (!(b.AddNewPlayer()))
        return base.PlacePawn(b);
    }

    public override Tile PlayTurn(Board b, Hand h, int drawdeckcount)
    {
        return base.PlayTurn(b, h, drawdeckcount);
    }

    private Tile ChooseRandomTile(Hand h)
    {
        Tile t = h.Pieces[Random.Range(0, h.Pieces.Count)];
        Direction d = (Direction)Random.Range(0, 4);
        t.SetRotation(d);

        return t;

    }

   /* private Tile ChooseSymmetricTile (Hand h)
    {
 
    }

    //private Tile ChooseAsymmetricTile (Hand h)
    //{
    //    ;
    //}*/

    public override void EndGame(Board b, List<Color> player_colors)
    {
        base.EndGame(b, player_colors);
    }

	public int SymmetryScore(Tile t)
	{
		int symmetricpathcount = 0;

		foreach (Vector2Int p in t.OriginalPaths)
		{
			int difference = ((p[1] - p[0]) % 8); 
			if (difference == 4)
			{
				for (int i = 1; i < 4; i++)
				{
					Vector2Int symmetricpath = new Vector2Int((p[0] + i) % 8, (p[1] + i) % 8);
					if (t.HasOriginalPath(symmetricpath))
					{
						symmetricpathcount++;
					}
				}
			}
			else if ((difference % 2) == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2Int symmetricpath = new Vector2Int((p[0] + 2*i + 1) % 8, (p[1] + 2 * i + 1) % 8);
					if (t.HasOriginalPath(symmetricpath))
					{
						symmetricpathcount++;
					}
				}
			}
			else if ((difference % 2) == 1)
			{
				for (int i = 1; i < 4; i++)
				{
					Vector2Int symmetricpath = new Vector2Int((p[0] + 2 * i) % 8, (p[1] + 2 * i) % 8);
					if (t.HasOriginalPath(symmetricpath))
					{
						symmetricpathcount++;
					}
				}
			}

		}
		return ((int) Mathf.Sqrt(symmetricpathcount));
	}


}
