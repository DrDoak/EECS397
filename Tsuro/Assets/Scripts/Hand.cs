﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public Tile RemoveFromHand (Tile t)
    {
		if (!Pieces.Contains (t))
			return null;
        Pieces.Remove(t);
		return t;
    }
	public bool IsValid() {
		return (Pieces.Count <= 3 && ((Pieces.Count - Pieces.Distinct ().Count()) == 0));
	}
    
}