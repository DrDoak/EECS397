using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestDeck {

	[Test]
	public void InitializeDeck() {
		Board b = new Board(new Vector2Int(6, 6));
		Deck fullDeck = new Deck();
		Assert.AreEqual(35, b.CurrentDeck.DrawDeck.Count, "Deck initialized with correct number of cards");
		Assert.AreEqual(fullDeck.DrawDeck[3] , b.CurrentDeck.DrawDeck[3], "Deck initialized with correct cards");
	}

	[Test]
	public void InitializingPlayers() {
		Board b = new Board(new Vector2Int(6, 6));
		SPlayer p1 = new SPlayer();
		SPlayer p2 = new SPlayer();
		Deck fullDeck = new Deck();

		b.AddNewPlayer(p1,  new PlayerLocation( new Vector2Int(0, 0), 7));
		b.AddNewPlayer(p2, new PlayerLocation( new Vector2Int(0, 0), 6));
		Assert.AreEqual(3, p1.MyHand.Pieces.Count,  "Cards added to player hand");
		Assert.AreEqual(29, b.CurrentDeck.DrawDeck.Count, "Cards removed from deck");

		Assert.AreEqual(fullDeck.DrawDeck[3], p2.MyHand.Pieces[0],  "Correct cards given to player");
		Assert.AreEqual (fullDeck.DrawDeck [2], p1.MyHand.Pieces [2],  "Correct cards given to player");

	}

	[Test]
	public void NoDragonTile() {
		Administrator a = new Administrator ();
		Board b = new Board(new Vector2Int(6, 6));
		SPlayer p1 = new SPlayer();
		SPlayer p2 = new SPlayer();
		SPlayer p3 = new SPlayer ();
		b.AddNewPlayer (p1, new PlayerLocation( new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2,  new PlayerLocation(new Vector2Int (0, 0), 4));
		b.AddNewPlayer (p3,  new PlayerLocation(new Vector2Int (0, 0), 5));

		Debug.Assert (b.CurrentDeck.DragonTileHand == null, "No player has Dragon tile beforehand");

		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinate (new Vector2Int (0, 0));
		oldTile.SetRotation(Direction.DOWN);

		TurnOutput to = a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Assert.AreEqual(2, to.PlayersOut.Count, "Two players were eliminated");
		Assert.AreEqual(1, to.PlayersIn.Count, "Players eliminated removed from Players in");
		Assert.False( to.ContinueGame, "Game end has been detected");
		Assert.IsNull(b.CurrentDeck.DragonTileHand, "No player has Dragon tile afterwards");
	}

	[Test]
	public void DragonTileEliminatesOther() {
		Administrator a = new Administrator ();
		Board b = new Board(new Vector2Int(6, 6));
		SPlayer p1 = new SPlayer();
		SPlayer p2 = new SPlayer();
		SPlayer p3 = new SPlayer ();
		b.AddNewPlayer (p1,  new PlayerLocation(new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2,  new PlayerLocation(new Vector2Int (0, 0), 4));
		b.AddNewPlayer (p3,  new PlayerLocation(new Vector2Int (0, 5), 7));
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p2.MyHand);
		}
		b.CurrentDeck.DrawCard (p1.MyHand);

		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinate (new Vector2Int (0, 0));
		oldTile.SetRotation(Direction.DOWN);

		//P2 is eliminated
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Assert.AreEqual(3, p1.MyHand.Pieces.Count, "Player 1 drew a card from the new deck");
		Assert.AreEqual(28, b.CurrentDeck.DrawDeck.Count , "P2's cards are returned back to the deck");
		Assert.Null (b.CurrentDeck.DragonTileHand, "Dragon tile returned to deck.");
	}

	[Test]
	public void DragonTileEliminated() {
		Administrator a = new Administrator ();
		Board b = new Board(new Vector2Int(6, 6));
		SPlayer p1 = new SPlayer();
		SPlayer p2 = new SPlayer();
		SPlayer p3 = new SPlayer ();
		b.AddNewPlayer (p1,  new PlayerLocation(new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p2,  new PlayerLocation(new Vector2Int (0, 0), 4));
		b.AddNewPlayer (p3,  new PlayerLocation(new Vector2Int (0, 5), 7));
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p1.MyHand);
		}
		b.CurrentDeck.DrawCard (p2.MyHand);

		Assert.AreEqual(p2.MyHand, b.CurrentDeck.DragonTileHand ,  "Player 2 has dragon tile");

		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		p3.MyHand.RemoveFromHand (p3.MyHand.Pieces[1]);

		int oldP1Count = p1.MyHand.Pieces.Count;
		oldTile.SetCoordinate (new Vector2Int (0, 0));
		oldTile.SetRotation(Direction.DOWN);

		//Eliminate Player 2
		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Assert.AreEqual(3, p3.MyHand.Pieces.Count, "Player 3 drew a card from the new deck");
		Assert.AreEqual(oldP1Count, p1.MyHand.Pieces.Count , "Player 1 did not draw a card from the deck");
		Assert.Null(b.CurrentDeck.DragonTileHand, "Dragon tile returned to deck.");
	}

	[Test]
	public void DragonTileEliminatesSelf() {
		Administrator a = new Administrator ();
		Board b = new Board(new Vector2Int(6, 6));
		SPlayer p1 = new SPlayer();
		SPlayer p2 = new SPlayer();
		SPlayer p3 = new SPlayer ();
		b.AddNewPlayer (p1,  new PlayerLocation(new Vector2Int (0, 0), 4));
		b.AddNewPlayer (p2,  new PlayerLocation(new Vector2Int (0, 0), 7));
		b.AddNewPlayer (p3, new PlayerLocation( new Vector2Int (0, 5), 7));
		while (b.CurrentDeck.DrawDeck.Count > 0) {
			b.CurrentDeck.DrawCard (p1.MyHand);
		}
		b.CurrentDeck.DrawCard (p1.MyHand);

		Tile oldTile = p1.MyHand.RemoveFromHand (p1.MyHand.Pieces[1]);
		oldTile.SetCoordinate (new Vector2Int (0, 0));
		oldTile.SetRotation(Direction.DOWN);

		p2.MyHand.RemoveFromHand (p2.MyHand.Pieces[1]);
		p3.MyHand.RemoveFromHand (p3.MyHand.Pieces[1]);

		a.PlayATurn(b.CurrentDeck.DrawDeck,b.CurrentPlayersIn, b.CurrentPlayersOut, b, oldTile);

		Assert.AreEqual(3, p2.MyHand.Pieces.Count, "Player 2 drew a card from the new deck");
		Assert.AreEqual(3, p3.MyHand.Pieces.Count, "Player 3 did not draw a card from the new deck");
		Assert.Null (b.CurrentDeck.DragonTileHand, "Dragon tile returned to deck.");
	}
}
