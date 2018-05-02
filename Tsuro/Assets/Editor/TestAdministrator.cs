using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestAdministrator {

	[Test]
	public void AddNewSPlayer() {
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

	[Test]
	public void AddNewPlayer() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int (6, 6));
		a.SetBoard (b);
		PlayerMachine pm = new PlayerMachine ("testPlayer1");
		a.AddNewPlayer (pm);
		Assert.AreEqual (1, a.Players.Count, "Player added to m_players");
		Assert.AreEqual (pm, a.Players[0].MyPlayer, "Player added is correct player");

		PlayerMachine pm2 = new PlayerMachine ("testPlayer2");
		a.AddNewPlayer (pm2);
		Assert.AreEqual (2, a.Players.Count, "Player 2 added to m_players");
		Assert.AreEqual (pm2, a.Players[1].MyPlayer, "Player2 added is correct player");
	}

	[Test]
	public void InitializeGame() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int (6, 6));
		a.SetBoard (b);
		PlayerMachine pm = new PlayerMachine ("testPlayer1");
		a.AddNewPlayer (pm);
		PlayerMachine pm2 = new PlayerMachine ("testPlayer2");
		a.AddNewPlayer (pm2);
		a.InitializeGame ();

		Assert.AreEqual (2, b.CurrentPlayersIn.Count, "Correct Num players added to the board");
		Assert.AreEqual (pm, b.CurrentPlayersIn[0].MyPlayer, "Correct player added to the board");
		Assert.AreEqual (pm2, b.CurrentPlayersIn[1].MyPlayer, "Correct player added to the board");
		Assert.IsTrue (b.CurrentPlayersIn [0].Location.IsEdgePosition (b.BoardSize), "Player added at edge position");
	}

	[Test]
	public void Play() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int (6, 6));
		a.SetBoard (b);
		PlayerMachine pm = new PlayerMachine ("testPlayer1");
		a.AddNewPlayer (pm);
		PlayerMachine pm2 = new PlayerMachine ("testPlayer2");
		a.AddNewPlayer (pm2);
		a.InitializeGame ();

		a.Play ();
		Assert.AreEqual (1, b.CurrentPlayersIn.Count, "Only one player is the winner");
		Assert.AreEqual (1, b.CurrentPlayersOut.Count, "A player is the loser");
	}

	[Test]
	public void PlayFourPlayer() {
		//Run 20 test games
		for (int i = 0; i < 20; i++) {
			Administrator a = new Administrator ();
			Board b = new Board (new Vector2Int (6, 6));
			a.SetBoard (b);
			PlayerMachine pm = new PlayerMachine ("testPlayer1");
			a.AddNewPlayer (pm);
			PlayerMachine pm2 = new PlayerMachine ("testPlayer2");
			pm2.AIType = PlayerAIType.ASYMMETRIC;
			a.AddNewPlayer (pm2);
			PlayerMachine pm3 = new PlayerMachine ("testPlayer3");
			pm3.AIType = PlayerAIType.SYMMETRIC;
			a.AddNewPlayer (pm3);
			PlayerMachine pm4 = new PlayerMachine ("testPlayer4");
			a.AddNewPlayer (pm4);

			a.InitializeGame ();

			a.Play ();
			Assert.AreEqual (1, b.CurrentPlayersIn.Count, "Only one player is the winner");
			Assert.AreEqual (3, b.CurrentPlayersOut.Count, "A player is the loser");
		}
	}
}