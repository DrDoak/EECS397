using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	private List<Vector2Int> m_paths;
    public readonly List<Vector2Int> OriginalPaths;
	public Vector2Int Coordinate { get; private set; }
	private Direction m_rotation;

	public Tile(List<Vector2Int> pathList) {
        OriginalPaths = pathList;
		m_rotation = Direction.UP;
		m_paths = pathList;
	}
	public void SetCoordinateAndDirection(Vector2Int c, Direction r) {
		Coordinate = c;
		m_rotation = r;
		SetRotation (r);
	}

	public void SetRotation ( Direction r)
    {
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

	private Vector2Int getRotatedPath(Vector2Int path, Direction r) {
		return new Vector2Int (getRotatedCoordinate(path.x,r),getRotatedCoordinate(path.y,r));
	}
	
	private int getRotatedCoordinate(int coord, Direction r) {
		return (coord + (2 * (int)r)) % 8;
	}
		
		
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

        Debug.Assert (t.getRotatedCoordinate (0, Direction.RIGHT) == 2, "Basic Coordinate Rotation");
		Debug.Assert (t.getRotatedCoordinate (7, Direction.RIGHT) == 1, "Coordinate rotation overflow");
		Vector2Int p = new Vector2Int (0, 5);
		Debug.Assert (t.getRotatedPath (p, Direction.DOWN).Equals(new Vector2Int(4,1)), "Path rotation");

		t.SetCoordinateAndDirection (new Vector2Int (2, 3), Direction.LEFT);
		Debug.Assert (t.m_paths[1].Equals(new Vector2Int(7,1)), "Place Tile rotation");
		Debug.Assert (t.OriginalPaths[3].Equals(new Vector2Int(4,7)), "Original paths preserved");
		Debug.Assert (t.GetPathConnection (7) == 1, "Sent valid path");

        Debug.Assert(t.IsEqual(t2), "tiles are equal");
        Debug.Assert(!(t.IsEqual(t3)), "tiles are not equal");
    }

     public override string ToString()
    {
        string s = "Rotation: " + m_rotation.ToString() + " RotPaths: ";
        foreach (Vector2Int path in m_paths)
        {
            s += path.ToString();
        }
        return s;
    }

    public bool IsEqual(Tile otherTile)
    {
        for (int i = 0; i < m_paths.Count; i++)
        {
            if (OriginalPaths[i] != otherTile.OriginalPaths[i])
                return false;
        }
        return true;
    }
}