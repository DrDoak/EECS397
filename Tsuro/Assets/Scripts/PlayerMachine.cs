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

    //private Tile ChooseSymmetricTile (Hand h)
    //{
    //    ;
    //}

    //private Tile ChooseAsymmetricTile (Hand h)
    //{
    //    ;
    //}

    public override void EndGame(Board b, List<Color> player_colors)
    {
        base.EndGame(b, player_colors);
    }


}
