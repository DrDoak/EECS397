using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	public Vector2Int Coordinate { get; private set; }
	public readonly List<Vector2Int> OriginalPaths;

	private Direction m_rotation;
	public List<Vector2Int> RotatedPaths { get; private set; }
	public List<Direction> LegalDirections;

	public Tile(List<Vector2Int> pathList) {
		if (!isValidPath (pathList)) {
			throw new System.ArgumentException ();
		}
        OriginalPaths = pathList;
		m_rotation = Direction.UP;
		RotatedPaths = pathList;
		LegalDirections = new List<Direction> () { Direction.UP, Direction.RIGHT, Direction.DOWN, Direction.LEFT };
	}
	public void SetCoordinate(Vector2Int coordinate) {
		Coordinate = coordinate;
	}
	public override string ToString()
	{
		string s = "Rotation: " + m_rotation.ToString() + " Rotated Paths: ";
		foreach (Vector2Int path in RotatedPaths)
		{
			s += path.ToString();
		}
		return s;
	}

	public override bool Equals (object obj)
	{
		Tile otherTile = obj as Tile;
		if (otherTile == null)
			return false;
		
		for (int i = 0; i < RotatedPaths.Count; i++)
		{
			if (!HasOriginalPath(otherTile.OriginalPaths[i]))
				return false;
		}
		return true;
	}

	public void SetRotation ( Direction r)
    {
		m_rotation = r;
		RotatedPaths = new List<Vector2Int> ();
		foreach (Vector2Int p in OriginalPaths) {
			RotatedPaths.Add(getRotatedPath(p,r));
		}
    }

	public int GetPathConnection(int startPoint) {
		foreach (Vector2Int v in RotatedPaths) {
			if (v.x == startPoint)
				return v.y;
			if (v.y == startPoint)
				return v.x;
		}
		return -1;
	}

    public bool HasOriginalPath(Vector2Int p)
    {
		return (OriginalPaths.Contains(p) || OriginalPaths.Contains(new Vector2Int(p.y,p.x)));
    }

    private Vector2Int getRotatedPath(Vector2Int path, Direction r) {
		return new Vector2Int (getRotatedCoordinate(path.x,r),getRotatedCoordinate(path.y,r));
	}
	
	private int getRotatedCoordinate(int coord, Direction r) {
		return (coord + (2 * (int)r)) % 8;
	}

	private bool isValidPath(List<Vector2Int> paths) {
		if (paths.Count != 4)
			return false;
		List<int> numbersUsed = new List<int> ();
		foreach (Vector2Int p in paths) {
			if (p.x < 0 || p.x > 7 || p.y < 0 || p.y > 7 ||
				numbersUsed.Contains(p.x) || numbersUsed.Contains(p.y))
				return false;
			numbersUsed.Add (p.x);
			numbersUsed.Add (p.y);
		}
		return true;
	}
}