using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestPlayerMachine {

	[Test]
	public void ChooseRandomTileTests() {

	}

	[Test]
	public void PlacePawnTest() {
		Board b = new Board ( new Vector2Int(6,6));
		PlayerMachine pm = new PlayerMachine ("pete");
		//Run 200 tests to ensure it is always on an edge position
		for (int i = 0; i < 200; i++) { 
			PlayerLocation pl = pm.PlacePawn (b);
			Assert.IsTrue (pl.IsEdgePosition (b.BoardSize), "Is correct edge position");
		}
	}

	[Test]
	public void PlayTurnTest() {

	}

	[Test]
	public void SymmetryScoresTest(){
		PlayerMachine pm = new PlayerMachine ("pete");
		List<Tile> testTiles = TestTile.GenerateTiles ();
		Assert.AreEqual (4, pm.UniqueRotationTiles (testTiles [0]), "No rotational symmetry detected");
		Assert.AreEqual (4, pm.UniqueRotationTiles (testTiles [1]), "No rotational symmetry detected");
		Assert.AreEqual (4, pm.UniqueRotationTiles (testTiles [2]), "No rotational symmetry detected");
		Assert.AreEqual (4, pm.UniqueRotationTiles (testTiles [3]), "No rotational symmetry detected");
		Assert.AreEqual (1, pm.UniqueRotationTiles (testTiles [4]), "Two of three other paths are symmetrical to first path");
		Assert.AreEqual (1, pm.UniqueRotationTiles (testTiles [5]), "All paths are symmetrical to each other");
		Assert.AreEqual (2, pm.UniqueRotationTiles (testTiles [6]), "All paths are symmetrical to each other");
	}

	[Test]
	public void ChooseSymmetricTest() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		a.SetBoard (b);
		PlayerMachine pm = new PlayerMachine ("pete");
		a.AddNewPlayer (pm);
		pm.AIType = PlayerAIType.SYMMETRIC;
		List<Tile> testTiles = TestTile.GenerateTiles ();
		Hand h = new Hand ();
		h.AddToHand (testTiles [0]);
		h.AddToHand (testTiles [4]);
		h.AddToHand (testTiles [5]);
		//4 or 5 both work.
		Assert.AreEqual(testTiles[5],pm.PlayTurn(b,h.Pieces,b.CurrentDeck.Pieces.Count));
	}

	[Test]
	public void ChooseAsymmetricTest() {
		Board b = new Board (new Vector2Int(6,6));
		PlayerMachine pm = new PlayerMachine ("pete");
		pm.AIType = PlayerAIType.ASYMMETRIC;
		List<Tile> testTiles = TestTile.GenerateTiles ();
		Hand h = new Hand ();
		h.AddToHand (testTiles [0]);
		h.AddToHand (testTiles [4]);
		h.AddToHand (testTiles [5]);
		Assert.AreEqual(testTiles[0],pm.PlayTurn(b,h.Pieces,b.CurrentDeck.Pieces.Count));
	}

	[Test]
	public void EndGameTest() {

	}
}
