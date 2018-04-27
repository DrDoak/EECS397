using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestHand {

	[Test]
	public void BasicAddRemove() {
		Hand h = new Hand ();
		List<Tile> tileList = TestTile.GenerateTiles ();
		h.AddToHand (tileList [0]);
		h.AddToHand (tileList [1]);
		Assert.AreEqual (2, h.Pieces.Count, "Tiles added normally");
		Assert.IsNull ( h.RemoveFromHand(tileList[2]), "Does not attempt to remove tile that is not present");
		Assert.AreEqual ( h.RemoveFromHand(tileList[1]),tileList[1], "Correct tile removed");
		Assert.AreEqual (1, h.Pieces.Count, "Tile removed");
	}

	[Test]
	public void IsInHand() {
		Hand h = new Hand ();
		List<Tile> tileList = TestTile.GenerateTiles ();
		h.AddToHand (tileList [0]);
		h.AddToHand (tileList [2]);
		Assert.False ( h.IsInHand(tileList[3]), "Detects if tile is not in hand");
		Assert.True ( h.IsInHand(tileList[0]),  "Detects if tile is in hand");
		Assert.True ( h.IsInHand(tileList[1]),  "Detects if identical tile is not in hand");
	}
}
