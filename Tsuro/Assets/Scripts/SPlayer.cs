using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer {

    public Hand MyHand { get; private set; }
    public int PositionOnTile { get; private set; }


	private Color m_color;
	public Vector2Int Coordinate;

	public SPlayer() {
		MyHand = new Hand ();
	}

	
    
	public Tile PlayTile(Tile t) {
		MyHand.RemoveFromHand (t);
		return t;
	}

	public bool IsOnEdge (Vector2Int coord, Direction d)
    {
		return (Coordinate == coord && DirectionUtils.DirectionMatch (d, PositionOnTile));
    }

	public bool IsAtPosition (Vector2Int coord, int pos)
	{
		return (Coordinate == coord && PositionOnTile == pos);
	}
	public void MoveToPosition(Vector2Int pos , int tilePos) {
		Coordinate = pos;
		PositionOnTile = tilePos;
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
		p.MyHand.AddToHand (t);
		Debug.Assert (p.MyHand.IsInHand (t), "Basic Hand Tile addition");
		p.PlayTile (t);
		Debug.Assert (!p.MyHand.IsInHand (t), "Basic Hand Tile Playing");

		p.MoveToPosition (new Vector2Int(2,3), 7);
		Debug.Assert (p.IsOnEdge (new Vector2Int(2,3), Direction.LEFT), "Detected Correct Direction");
		Debug.Assert (p.IsAtPosition (new Vector2Int(2,3), 7), "Detected Correct Position");

	}
}
