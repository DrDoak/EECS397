using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	public Vector2Int Coordinate { get; private set; }
	public readonly List<Vector2Int> OriginalPaths;

	private Direction m_rotation;
	private List<Vector2Int> m_paths;

	public Tile(List<Vector2Int> pathList) {
		if (!isValidPath (pathList)) {
			throw new System.ArgumentException ();
		}
        OriginalPaths = pathList;
		m_rotation = Direction.UP;
		m_paths = pathList;
	}
	public void SetCoordinate(Vector2Int coordinate) {
		Coordinate = coordinate;
	}
	public override string ToString()
	{
		string s = "Rotation: " + m_rotation.ToString() + " Rotated Paths: ";
		foreach (Vector2Int path in m_paths)
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
		
		for (int i = 0; i < m_paths.Count; i++)
		{
			if (OriginalPaths[i] != otherTile.OriginalPaths[i])
				return false;
		}
		return true;
	}

	public void SetRotation ( Direction r)
    {
		m_rotation = r;
		m_paths = new List<Vector2Int> ();
		foreach (Vector2Int p in OriginalPaths) {
			m_paths.Add(getRotatedPath(p,r));
		}
    }

	public int GetPathConnection(int startPoint) {
		foreach (Vector2Int v in m_paths) {
			if (v.x == startPoint)
				return v.y;
			if (v.y == startPoint)
				return v.x;
		}
		return -1;
	}

    public bool HasPath(Vector2Int p)
    {
        return (OriginalPaths.Contains(p));
    }

    public int SymmetryScore()
    {
        int symmetricpathcount = 0;

        foreach (Vector2Int p in OriginalPaths)
        {
            int difference = ((p[1] - p[0]) % 8); 
            if (difference == 4)
            {
                for (int i = 1; i < 4; i++)
                {
                    Vector2Int symmetricpath = new Vector2Int((p[0] + i) % 8, (p[1] + i) % 8);
                    if (HasPath(symmetricpath))
                    {
                        symmetricpathcount++;
                    }
                }
            }
            else if ((difference % 2) == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2Int symmetricpath = new Vector2Int((p[0] + 2*i + 1) % 8, (p[1] + 2 * i + 1) % 8);
                    if (HasPath(symmetricpath))
                    {
                        symmetricpathcount++;
                    }
                }
            }
            else if ((difference % 2) == 1)
            {
                for (int i = 1; i < 4; i++)
                {
                    Vector2Int symmetricpath = new Vector2Int((p[0] + 2 * i) % 8, (p[1] + 2 * i) % 8);
                    if (HasPath(symmetricpath))
                    {
                        symmetricpathcount++;
                    }
                }
            }
            
        }
        return ((int) Mathf.Sqrt(symmetricpathcount));
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