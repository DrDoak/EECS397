using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestBoard {

	[Test]
	public void TilePlacements() {
		Board b = new Board (new Vector2Int(6,6));
		List<Tile> tileList = TestTile.GenerateTiles ();
		Assert.IsNotNull (b.PlaceTile (tileList[0], new Vector2Int (0, 0), Direction.UP), "On board placement");
		Assert.IsNull (b.PlaceTile (tileList[0], new Vector2Int (0, 6), Direction.UP), "Off board placement");
		Assert.IsNull (b.PlaceTile (tileList[0], new Vector2Int (-4, 4), Direction.UP), "Off board placement");
		Assert.IsNull (b.PlaceTile (tileList[1], new Vector2Int (0, 0), Direction.UP), "Tile Already in Position");
	}

	[Test]
	public void PlayerPlacements() {
		Board b = new Board (new Vector2Int(6,6));

		SPlayer sp = new SPlayer ();

		Assert.False (b.AddNewPlayer (sp,  new PlayerLocation(new Vector2Int(2,4),0)), "invalid player addition");
		Assert.False (b.AddNewPlayer (sp, new PlayerLocation( new Vector2Int(0,4),0)), "invalid player addition");
		Assert.True (b.AddNewPlayer (sp, new PlayerLocation(new Vector2Int(0,4),7)), "valid player addition");

		SPlayer sp2 = new SPlayer ();
		Assert.False (b.AddNewPlayer (sp2, new PlayerLocation(new Vector2Int (0, 4), 7)), "Cannot insert player due to collision");
		Assert.True (b.AddNewPlayer (sp2, new PlayerLocation( new Vector2Int(0,4),6)), "valid player addition");
	}

	[Test]
	public void GetAdjacentPlayers() {
		Board b = new Board (new Vector2Int(6,6));
		List<Tile> tileList = TestTile.GenerateTiles ();

		SPlayer sp = new SPlayer ();
		b.AddNewPlayer (sp,  new PlayerLocation(new Vector2Int(0,4),7));

		SPlayer sp2 = new SPlayer ();
		b.AddNewPlayer (sp2,  new PlayerLocation(new Vector2Int (0, 4), 6));

		b.PlaceTile (tileList[2], new Vector2Int (1, 4), Direction.UP);
		b.PlaceTile (tileList[1], new Vector2Int (0, 4), Direction.UP);

		Assert.AreEqual (0, b.GetAdjacentPlayers (tileList[2]).Count,  "Did not incorrectly grab a player");
		Assert.AreEqual ( 2, b.GetAdjacentPlayers (tileList[1]).Count , "Found adjacent player");
	}

	[Test]
	public void MovePlayer() {
		Board b = new Board (new Vector2Int(6,6));
		List<Tile> tileList = TestTile.GenerateTiles ();
		SPlayer sp = new SPlayer ();
		b.AddNewPlayer (sp,  new PlayerLocation(new Vector2Int(0,4),7));

		b.MovePlayer (sp, tileList[0]);

		Assert.AreEqual ( new Vector2Int(0,4), sp.Location.Coordinate,  "Moved player to correct coordinate");
		Assert.AreEqual (  7, sp.Location.PositionOnTile , "Moved player to correct position");
	}

	[Test]
	public void IsEliminatePlayer() {
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		b.AddNewPlayer (p1,  new PlayerLocation(new Vector2Int (0, 0), 7));

		p1.MyHand.Pieces [0].SetCoordinate (new Vector2Int (0, 0));
		p1.MyHand.Pieces[0].SetRotation(Direction.UP);

		Assert.True(b.IsPlayerEliminated(p1, p1.MyHand.Pieces[0]), "Determines that the play is illegal for eliminating player");
		Assert.True(b.IsPlayerEliminated(p1, p1.MyHand.Pieces[1]), "Determines that the play is illegal for eliminating player");
		Assert.False(b.IsPlayerEliminated(p1,p1.MyHand.Pieces[2]), "Determines that the play is legal");

		Assert.AreEqual ( new Vector2Int(0,0), p1.Location.Coordinate,  "Elimination test did not actually move player");
	}
}
