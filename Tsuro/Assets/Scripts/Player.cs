using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	protected string Name = "Player Name";
    protected Color PawnColor;
	public readonly static List<Color> AllPlayerColors = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.black, Color.white};
    protected Vector2Int Coordinate;
    protected int Position;

    public Player(string m_name)
    {
        Name = m_name;
    }

    public virtual string GetName()
    {
        return Name;
    }

    public virtual void Initialize(Color m_color, List<Color> player_colors)
    {
        PawnColor = m_color;
        if (!player_colors.Contains(m_color))
        {
            player_colors.Add(m_color);
        }
        else
        {
            Debug.LogError("Error: Color has already been added.");
        }
    }

    public virtual PlayerLocation PlacePawn(Board b)
    {
        Vector2Int defaultcoord = new Vector2Int(0, 0);
        int defaultpos = 6;
        PlayerLocation pl = new PlayerLocation(defaultcoord, defaultpos);
        return pl;
    }

    public virtual Tile PlayTurn(Board b, Hand h, int drawdeckcount)
    {
        return h.Pieces[0];
    }

    public virtual void EndGame(Board b, List<Color> player_colors)
    {
        int Winners = b.CurrentPlayersIn.Count;
        for (int i = 0; i < Winners; i++)
        {
            Debug.Log(player_colors[i] + " is a winner!");
        }
    }
}