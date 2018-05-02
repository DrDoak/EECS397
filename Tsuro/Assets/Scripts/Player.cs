using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

	protected string Name = "Player Name";
	public Color PawnColor;
    protected Vector2Int Coordinate;
    protected int Position;
	protected Board FinalBoardState;
	protected List<Player> WinningPlayers;

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
    }

    public virtual PlayerLocation PlacePawn(Board b)
    {
        Vector2Int defaultcoord = new Vector2Int(0, 0);
        int defaultpos = 6;
        PlayerLocation pl = new PlayerLocation(defaultcoord, defaultpos);
        return pl;
    }

	public virtual Tile PlayTurn(Board b, List<Tile> legalTiles, int drawdeckcount)
    {
		return legalTiles[0];
    }

    public virtual void EndGame(Board b, List<Color> player_colors)
    {
		FinalBoardState = b;
        int Winners = b.CurrentPlayersIn.Count;
        for (int i = 0; i < Winners; i++)
        {
            //Debug.Log(player_colors[i] + " is a winner!");
        }
    }
}