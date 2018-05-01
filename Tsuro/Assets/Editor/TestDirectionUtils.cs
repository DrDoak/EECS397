using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestDirectionUtils {

	[Test]
	public void DirectionMatch() {
		Debug.Log ("Running Tests in DirectionUtils");

		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);

		SPlayer p = new SPlayer ();
		p.MyHand.AddToHand (t);
		Assert.True ( DirectionUtils.DirectionMatch(Direction.RIGHT,3), "Direction Match");
		Assert.False (DirectionUtils.DirectionMatch(Direction.RIGHT,0), "Direction mismatch found");
		Assert.False (DirectionUtils.DirectionMatch(Direction.DOWN,9), "Not match for invalid positions");
	}
	[Test]
	public void IsOnEdge() {
		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile (testPaths);
		SPlayer p = new SPlayer ();
		p.MoveToPosition (new PlayerLocation(t.Coordinate, 7));
		Assert.True (p.IsOnEdge (t.Coordinate, Direction.LEFT), "Detected Correct Direction");
	}

	[Test]
	public void IntDirectionConversion() {
		Assert.AreEqual (DirectionUtils.DirectionToVector(Direction.LEFT), new Vector2Int(-1,0));
		Assert.AreEqual (DirectionUtils.DirectionToVector(Direction.NONE), new Vector2Int(0,0));

		Assert.AreEqual (DirectionUtils.VectorToDirection(new Vector2Int(-1,0)), Direction.LEFT);
		Assert.AreEqual (DirectionUtils.VectorToDirection(new Vector2Int(0,0)), Direction.NONE );
	}

	[Test]
	public void VectorDirectionConversion() {
		Assert.AreEqual (DirectionUtils.IntToDirection(4), Direction.DOWN);
		Assert.Throws (typeof(System.ArgumentException), () => DirectionUtils.IntToDirection (-1));
		Assert.Throws (typeof(System.ArgumentException), () => DirectionUtils.IntToDirection (14));
	}

	[Test]
	public void AdjacentDirection() {
		Assert.AreEqual (DirectionUtils.AdjacentPosition (7), 2);
		Assert.AreEqual (DirectionUtils.AdjacentPosition (0), 5);
	}

	[Test]
	public void InvertDirection() {
		Assert.Throws (typeof(System.ArgumentException), () => DirectionUtils.InvertDirection (Direction.NONE));
		Assert.DoesNotThrow (() => DirectionUtils.InvertDirection (Direction.LEFT));
	}
}
