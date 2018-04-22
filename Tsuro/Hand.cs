using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand {

    public List<Tile> Pieces { get; private set; }

    public Hand()
    {
        Pieces = new List<Tile>();
    }


    public void AddToHand(Tile t)
    {
        Pieces.Add(t);
    }

    public bool IsInHand(Tile t)
    {
        if (Pieces.Contains(t))
        { return true; }
        else { return false; }
    }

    public void RemoveFromHand (Tile t)
    {
        Pieces.Remove(t);
    }
    
}
