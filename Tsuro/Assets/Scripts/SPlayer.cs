using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { NONE, LEFT, RIGHT, UP, DOWN }

public class SPlayer : MonoBehaviour {

    public List<Tile> Hand { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 PositionOnTile { get; private set; }

    public void AddToHand(Tile t)
    {
        Hand.Add(t);
        return;
    }
        
    private string Color;

    public bool IsInHand (Tile t)
    {
        if (Hand.Contains(t))
        { return true; }
        else { return false; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Direction GetAdjacentDirection(Tile t)
    {
        return Direction.NONE;
    }
	public static Vector2 Tests() {
		int numPassed = 0;
		int totalTests = 0;

		return new Vector2 ( numPassed, totalTests );

	}
}
