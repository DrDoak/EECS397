using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestAdministrator {

	[Test]
	public void LegalPlay() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		SPlayer p2 = new SPlayer ();
		b.AddNewPlayer (p1, new PlayerLocation(new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2, new PlayerLocation( new Vector2Int (0, 0), 6));

		p1.MyHand.Pieces [0].SetCoordinate (new Vector2Int (0, 0));
		p1.MyHand.Pieces[0].SetRotation(Direction.UP);

		//making an illegal move, specifically where the move is an elimination move, but there are non-elimination moves available
		Assert.False(a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), "Determines that the play is illegal for eliminating player");
		Assert.False(a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is illegal for eliminating player");
		Assert.True(a.LegalPlay(p1,b,p1.MyHand.Pieces[2]), "Determines that the play is legal");

		p1.MyHand.Pieces [0].SetCoordinate (new Vector2Int (1, 0));
		p1.MyHand.Pieces[0].SetRotation(Direction.UP);
		Assert.False(a.LegalPlay(p1, b, p1.MyHand.Pieces[0]), " play is illegal due to tile not being in same location as player");

		//making a move where the tile is not placed in its original position (i.e., it is rotated)
		p1.MyHand.Pieces [1].SetCoordinate(new Vector2Int (0, 0));
		p1.MyHand.Pieces[1].SetRotation(Direction.RIGHT);
		Assert.True(a.LegalPlay(p1, b, p1.MyHand.Pieces[1]), "Determines that the play is legal after a rotation");
		Assert.False(a.LegalPlay(p1,b,b.CurrentDeck.DrawDeck[14]), "Determines that the play is illegal due to tile not being in hand");

	}

	[Test]
	public void MultipleMovements() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		SPlayer p2 = new SPlayer ();
		b.AddNewPlayer (p1, new PlayerLocation(new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2, new PlayerLocation(new Vector2Int (0, 0), 6));

		//making a move where multiple players move at once
		Tile oldThirdTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[2]);

		Assert.True (a.PlayATurn (b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b,oldThirdTile).ContinueGame, "Determines that a play did not end the game");

		Assert.AreEqual(new Vector2Int (1, 0), p1.Location.Coordinate ,  "Player one moved to correct location");
		Assert.AreEqual (new Vector2Int (0, 1), p2.Location.Coordinate , "Player two moved to correct location");
	}

	[Test]
	public void MultipleTileCross() {
		//making a move that causes a token to cross multiple tiles
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		SPlayer p1 = new SPlayer ();
		SPlayer p2 = new SPlayer ();
		b.AddNewPlayer (p1, new PlayerLocation( new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2, new PlayerLocation( new Vector2Int (0, 0), 6));

		//Just placing a tile on the board for testing
		b.PlaceTile(p1.MyHand.Pieces[1], new Vector2Int(1,0),Direction.UP);
		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[2]);
		oldTile.SetCoordinate (new Vector2Int (0, 0));
		oldTile.SetRotation(Direction.UP);
		//a.PlayATurn (b.CurrentDeck.DrawDeck, b.CurrentPlayersIn, b.CurrentPlayersOut, b,oldTile);
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);
		Debug.Assert (p1.Location.Coordinate == new Vector2Int (2, 0), "Player moves across two spaces");
	}

}