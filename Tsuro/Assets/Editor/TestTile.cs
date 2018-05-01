using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestTile {

	//Tiles 0 and 1 are identical, except one of the paths has the input flipped.
	public static List<Tile> GenerateTiles() {
		List<Tile> tList = new List<Tile> ();
		List<Vector2Int> testPaths = new List<Vector2Int> ();
		testPaths.Add (new Vector2Int (0, 5));
		testPaths.Add (new Vector2Int (1, 3));
		testPaths.Add (new Vector2Int (2, 6));
		testPaths.Add (new Vector2Int (4, 7));
		Tile t = new Tile(testPaths);
		tList.Add (t);

		List<Vector2Int> testPaths2 = new List<Vector2Int>();
		testPaths2.Add(new Vector2Int(5, 0));
		testPaths2.Add(new Vector2Int(1, 3));
		testPaths2.Add(new Vector2Int(2, 6));
		testPaths2.Add(new Vector2Int(4, 7));
		Tile t2 = new Tile (testPaths2);
		tList.Add (t2);

		List<Vector2Int> testPaths3 = new List<Vector2Int>();
		testPaths3.Add(new Vector2Int(0, 1));
		testPaths3.Add(new Vector2Int(2, 3));
		testPaths3.Add(new Vector2Int(4, 6));
		testPaths3.Add(new Vector2Int(5, 7));
		Tile t3 = new Tile(testPaths3);
		tList.Add (t3);

		List<Vector2Int> testPaths4 = new List<Vector2Int>();
		testPaths4.Add(new Vector2Int(2, 1));
		testPaths4.Add(new Vector2Int(0, 3));
		testPaths4.Add(new Vector2Int(7, 6));
		testPaths4.Add(new Vector2Int(5, 4));
		Tile t4 = new Tile(testPaths4);
		tList.Add (t4);

		return tList;
	}

	[Test]
	public void CreatingValidTiles() {
		List<Vector2Int> testPaths = new List<Vector2Int>();
		testPaths.Add(new Vector2Int(0, 1));
		testPaths.Add(new Vector2Int(2, 3));
		testPaths.Add(new Vector2Int(4, 6));
		testPaths.Add(new Vector2Int(5, 7));

		Assert.DoesNotThrow (() => new Tile (testPaths), "Can create valid Tile");

		List<Vector2Int> badPaths = new List<Vector2Int>();
		badPaths.Add(new Vector2Int(0, 1));
		badPaths.Add(new Vector2Int(2, 3));


		Assert.Throws (typeof(System.ArgumentException),() => new Tile (badPaths), "Does not create tile with not enough paths");

		badPaths.Add(new Vector2Int(4, 9));
		badPaths.Add(new Vector2Int(5, 7));

		Assert.Throws (typeof(System.ArgumentException),() => new Tile (badPaths), "Does not create tile with invalid path numbers");

		List<Vector2Int> DuplicatePath = new List<Vector2Int>();
		DuplicatePath.Add(new Vector2Int(0, 1));
		DuplicatePath.Add(new Vector2Int(2, 3));
		DuplicatePath.Add(new Vector2Int(4, 2));
		DuplicatePath.Add(new Vector2Int(5, 7));

		Assert.Throws (typeof(System.ArgumentException),() => new Tile (DuplicatePath), "Does not create tile with duplicate path numbers");
	}

	[Test]
	public void SetCoordinates() {
		List<Tile> testTiles = GenerateTiles ();
		testTiles[0].SetCoordinate (new Vector2Int (2, 3));
		Assert.AreEqual (new Vector2Int(2,3), testTiles[0].Coordinate,  "Coordinates Set");
	}

	[Test]
	public void RotatedCoordinates() {
		List<Tile> testTiles = GenerateTiles ();
		testTiles[0].SetCoordinate (new Vector2Int (2, 3));
		testTiles[0].SetRotation (Direction.LEFT);
		Assert.AreEqual (1, testTiles[0].GetPathConnection (7),  "Sent valid path");
		Assert.AreEqual (new Vector2Int(4,7), testTiles[0].OriginalPaths[3],  "Original paths preserved");

	}

	[Test]
	public void HasPath() {
		List<Tile> testTiles = GenerateTiles ();

		Assert.True(testTiles[0].HasOriginalPath(new Vector2Int(0,5)), "Tile shown to have path.");
		testTiles [1].SetRotation (Direction.DOWN);
		Assert.True(testTiles[0].HasOriginalPath(new Vector2Int(0,5)),  "tiles still has path after rotation");
		Assert.True(testTiles[0].HasOriginalPath(new Vector2Int(5,0)),  "tiles identifies flipped but same path");
		Assert.False(testTiles[0].HasOriginalPath(new Vector2Int(0,4)),  "tiles identifies when it doesn't have a path");
	}

	[Test]
	public void IsEquals() {
		List<Tile> testTiles = GenerateTiles ();

		Assert.True(testTiles[0].Equals(testTiles[1]), "tiles are equal");
		testTiles [1].SetRotation (Direction.DOWN);
		Assert.True(testTiles[0].Equals(testTiles[1]), "tiles are equal");
		Assert.False(testTiles[0].Equals(testTiles[2]), "tiles are not equal");
	}
}
