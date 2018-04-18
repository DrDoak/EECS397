using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public List<Tile> Hand { get; private set; }
    public int PositionOnTile { get; private set; }

	private Color m_color;
	public Vector2Int Coordinate;

	public SPlayer() {
		Hand = new List<Tile> ();
	}

    public void AddToHand(Tile t)
    {
        Hand.Add(t);
    }

    public bool IsInHand (Tile t)
    {
        if (Hand.Contains(t))
        { return true; }
        else { return false; }
    }
	public void PlayTile(Tile t) {
		Hand.Remove (t);
	}

	public bool IsAtPosition(Vector2Int coord, Direction d)
    {
		return (Coordinate == coord && DirectionUtils.DirectionMatch (d, PositionOnTile));
    }

	public bool IsAtPosition(Vector2Int coord, int pos)
	{
		return (Coordinate == coord && PositionOnTile == pos);
	}
	public void MoveToPosition(Vector2Int pos , int tilePos) {
		Coordinate = pos;
		PositionOnTile = tilePos;
	}

	public void EliminatePiece() {
		//Tile.RemovePiece (this);
	}

	public static void Tests() {
		Debug.Log ("Running Tests in SPlayer");

		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		SPlayer p = new SPlayer ();
		p.AddToHand (t);
		Debug.Assert (p.IsInHand (t), "Basic Hand Tile addition");
		p.PlayTile (t);
		Debug.Assert (!p.IsInHand (t), "Basic Hand Tile Playing");

		p.MoveToPosition (new Vector2Int(2,3), 7);
		Debug.Assert (p.IsAtPosition (new Vector2Int(2,3), Direction.LEFT), "Detected Correct Direction");
		Debug.Assert (p.IsAtPosition (new Vector2Int(2,3), 7), "Detected Correct Position");

	}
}
