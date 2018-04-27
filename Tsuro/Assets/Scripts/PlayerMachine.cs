using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMachine : Player
{

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

    /*
     * private Tile ChooseSymmetricTile(Hand h)
    {
        if (HasLegalMoves) 
        {
            iterate through OrderHandBySymmetry(h)
                try all rotations 
                return first rotation that is a LegalPlay
        }
        else
        {
            return OrderHandBySymmetry(h)[0]
        }
    
    }
    */

    /*
     * private Tile ChooseSymmetricTile(Hand h)
    {
        if (HasLegalMoves) 
        {
            iterate through OrderHandByAsymmetry(h)
                iterate through rotations 
                return first rotation that is a LegalPlay
        }
        else
        {
            return OrderHandByAsymmetry(h)[0] in rotation UP

        }
    
    }
    */

    private List<Tile> OrderHandBySymmetry(Hand h)
    {
        Debug.Log("unsorted Hand is " + h);
        Dictionary<Tile, int> scores = new Dictionary<Tile, int>();
        foreach (Tile t in h.Pieces)
        {
            scores.Add(t, t.SymmetryScore());
        }

        Dictionary<Tile, int> sorted = new Dictionary<Tile, int>();
        sorted = (Dictionary<Tile, int>)scores.OrderBy(x => x.Value);

        Debug.Log("sorted Hand is " + sorted.Keys.ToList<Tile>());
        return sorted.Keys.ToList<Tile>();

    }

    private List<Tile> OrderHandByAsymmetry(Hand h)
    {
        Debug.Log("unsorted Hand is " + h);
        Dictionary<Tile, int> scores = new Dictionary<Tile, int>();
        foreach (Tile t in h.Pieces)
        {
            scores.Add(t, t.SymmetryScore());
        }

        Dictionary<Tile, int> sorted = new Dictionary<Tile, int>();
        sorted = (Dictionary<Tile, int>)scores.OrderByDescending(x => x.Value);

        Debug.Log("sorted Hand is " + sorted.Keys.ToList<Tile>());
        return sorted.Keys.ToList<Tile>();

    }

    public override void EndGame(Board b, List<Color> player_colors)
    {
        base.EndGame(b, player_colors);
    }

    public static void Tests()
    {
        Debug.Log("Running Tests in PlayerMachine");
        PlayerMachine p = new PlayerMachine("bot1");

    }
}
