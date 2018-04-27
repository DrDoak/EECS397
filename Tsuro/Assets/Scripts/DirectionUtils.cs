using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;


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
			throw new System.ArgumentException ();
		
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
				throw new System.ArgumentException ();
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
}