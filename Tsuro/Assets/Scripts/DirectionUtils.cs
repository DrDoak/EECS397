﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction { UP, RIGHT, DOWN, LEFT , NONE}

public static class DirectionUtils {

	public static int AdjacentPosition(int i) {
		if (i % 2 == 0)
			return ( i + 5 )% 8;
		return (i + 3) % 8;
	}

	public static bool DirectionMatch(Direction r, int pos)
	{
		if (r == Direction.UP && (pos == 0 || pos == 1))
			return true;
		if (r == Direction.RIGHT && (pos == 2 || pos == 3))
			return true;
		if (r == Direction.DOWN && (pos == 4 || pos == 5))
			return true;
		if (r == Direction.LEFT && (pos == 6 || pos == 7))
			return true;
		return false;
	}

	public static Direction IntToDirection(int pos) {
		if (pos < 0 || pos > 7)
			Debug.LogError("Position Integer does not correlate to position on tile. Expected integer in range 0-7");
		
		if (pos == 0 || pos == 1)
			return Direction.UP;
		if (pos == 2 || pos == 3)
			return Direction.RIGHT;
		if (pos == 4 || pos == 5)
			return Direction.DOWN;
		if (pos == 6 || pos == 7)
			return Direction.LEFT;
		return Direction.NONE;
	}

	public static Direction InvertDirection(Direction d){
		switch (d) {
			case Direction.UP:
				return Direction.DOWN;
			case Direction.RIGHT:
				return Direction.LEFT;
			case Direction.DOWN:
				return Direction.UP;
			case Direction.LEFT:
				return Direction.RIGHT;
			default:
				Debug.LogError ("Attempting to invert invalid direction");
				return Direction.NONE;
		}
	}

	public static Vector2Int DirectionToVector(Direction r)
	{
		if (r == Direction.UP)
			return new Vector2Int(0,1);
		if (r == Direction.RIGHT)
			return new Vector2Int(1,0);
		if (r == Direction.DOWN)
			return new Vector2Int(0,-1);
		if (r == Direction.LEFT)
			return new Vector2Int(-1,0);
		return new Vector2Int(0,0);
	}

	public static Direction VectorToDirection(Vector2Int v)
	{
		if (v == new Vector2Int(0,1))
			return Direction.UP;
		if (v == new Vector2Int(1,0))
			return  Direction.RIGHT;
		if (v == new Vector2Int(0,-1))
			return Direction.DOWN;
		if (v == new Vector2Int(-1,0))
			return  Direction.LEFT;
		return Direction.NONE;
	}

	public static void Tests() {
		Debug.Log ("Running Tests in DirectionUtils");

		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		SPlayer p = new SPlayer ();
		p.MyHand.AddToHand (t);
		Debug.Assert ( DirectionMatch(Direction.RIGHT,3), "Direction Match");
		Debug.Assert ( !DirectionMatch(Direction.RIGHT,0), "Direction mismatch found");
		Debug.Assert ( !DirectionMatch(Direction.DOWN,9), "Not match for invalid positions");
		p.PlayTile (t);

		Debug.Assert (IntToDirection(4) == Direction.DOWN, "Integer to correct direction");
		//Debug.Assert (IntToDirection(-1) == Direction.NONE, "Invalid direction found");
		//Debug.Assert (IntToDirection(14) == Direction.NONE, "Invalid direction found");

		p.MoveToPosition (t.Coordinate, 7);
		Debug.Assert (p.IsOnEdge (t.Coordinate, Direction.LEFT), "Detected Correct Direction");

		Debug.Assert (DirectionToVector(Direction.LEFT) == new Vector2Int(-1,0), "Direction to Vector");
		Debug.Assert (DirectionToVector(Direction.NONE) == new Vector2Int(0,0), "Invalid direction found");

		Debug.Assert (VectorToDirection(new Vector2Int(-1,0)) == Direction.LEFT , "Vector to Direction");
		Debug.Assert (VectorToDirection(new Vector2Int(0,0)) == Direction.NONE , "Invalid vector found");

		Debug.Assert (AdjacentPosition (7) == 2, "Correct adjacent on neighboring tile position");
	}
}
