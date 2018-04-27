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

    public Vector2Int PositionSwap(Vector2Int path)
    {
        Vector2Int swappedpath = new Vector2Int(path.y, path.x);
        return swappedpath;
    }

    public List<Vector2Int> SwapAllPositions (List<Vector2Int> paths)
    {
        List<Vector2Int> swappedpathlist = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            swappedpathlist.Add(PositionSwap(paths[i]));
        }
        return swappedpathlist;
    }

    public bool HasPath(Vector2Int p)
    {
        List<Vector2Int> swappedlist = SwapAllPositions(OriginalPaths);
        return (OriginalPaths.Contains(p) || swappedlist.Contains(p));
    }

    public int SymmetryScore()
    {
        int symmetricpathcount = 0;

        foreach (Vector2Int p in OriginalPaths)
        {
            int difference = ((p[1] - p[0]) % 8); 
            if (difference == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2Int symmetricpath = new Vector2Int((p[0] + 2 * i + 5) % 8, (p[1] + 2 * i + 5) % 8);
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
<<<<<<< HEAD
		
	public static void Tests() {
		Debug.Log ("Running Tests in Tile");
		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
        Tile t = new Tile(testPaths);

        List<Vector2Int> testPaths2 = new List<Vector2Int>();
        testPaths2.Add(new Vector2Int(0, 5));
        testPaths2.Add(new Vector2Int(1, 3));
        testPaths2.Add(new Vector2Int(2, 6));
        testPaths2.Add(new Vector2Int(4, 7));
        Tile t2 = new Tile (testPaths2);

        List<Vector2Int> testPaths3 = new List<Vector2Int>();
        testPaths3.Add(new Vector2Int(0, 1));
        testPaths3.Add(new Vector2Int(2, 3));
        testPaths3.Add(new Vector2Int(4, 6));
        testPaths3.Add(new Vector2Int(5, 7));
        Tile t3 = new Tile(testPaths3);

        List<Vector2Int> testPaths4 = new List<Vector2Int>();
        testPaths4.Add(new Vector2Int(0, 1));
        testPaths4.Add(new Vector2Int(2, 3));
        testPaths4.Add(new Vector2Int(4, 5));
        testPaths4.Add(new Vector2Int(6, 7));
        Tile t4 = new Tile(testPaths4);

        List<Vector2Int> testPaths5 = new List<Vector2Int>();
        testPaths5.Add(new Vector2Int(0, 3));
        testPaths5.Add(new Vector2Int(1, 5));
        testPaths5.Add(new Vector2Int(2, 6));
        testPaths5.Add(new Vector2Int(4, 7));
        Tile t5 = new Tile(testPaths5);

        Debug.Assert (t.getRotatedCoordinate (0, Direction.RIGHT) == 2, "Basic Coordinate Rotation");
		Debug.Assert (t.getRotatedCoordinate (7, Direction.RIGHT) == 1, "Coordinate rotation overflow");
		Vector2Int p = new Vector2Int (0, 5);
		Debug.Assert (t.getRotatedPath (p, Direction.DOWN).Equals(new Vector2Int(4,1)), "Path rotation");

		t.SetCoordinate (new Vector2Int (2, 3));
		t.SetRotation (Direction.LEFT);
		Debug.Assert (t.m_paths[1].Equals(new Vector2Int(7,1)), "Place Tile rotation");
		Debug.Assert (t.OriginalPaths[3].Equals(new Vector2Int(4,7)), "Original paths preserved");
		Debug.Assert (t.GetPathConnection (7) == 1, "Sent valid path");

		Debug.Assert(t.IsEquals(t2), "tiles are equal");
		Debug.Assert(!(t.IsEquals(t3)), "tiles are not equal");

        Debug.Assert(t.SymmetryScore() == 0);
        Debug.Assert(t3.SymmetryScore() == 2);
        Debug.Assert(t4.SymmetryScore() == 3);
        Debug.Log(t5.SymmetryScore());
    }
=======
>>>>>>> 74f46f4f12ee3cfba08c1c2b6edf7cdbe2c81cc7
}