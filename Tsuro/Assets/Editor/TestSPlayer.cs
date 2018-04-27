using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestSplayer {

	[Test]
	public void PlayerHand() {
		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		SPlayer p = new SPlayer ();
		p.MyHand.AddToHand (t);
		Assert.True (p.MyHand.IsInHand (t), "Basic Hand Tile addition");
		p.PlayTile (t);
		Assert.False (p.MyHand.IsInHand (t), "Basic Hand Tile Playing");
	}

	[Test]
	public void PositionAndEdge() {
		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		SPlayer p = new SPlayer ();
		p.MoveToPosition (new Vector2Int(2,3), 7);
		Assert.True (p.IsOnEdge (new Vector2Int(2,3), Direction.LEFT), "Detected Correct Direction");
		Assert.True (p.IsAtPosition (new Vector2Int(2,3), 7), "Detected Correct Position");
	}
}
