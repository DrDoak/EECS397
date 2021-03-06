﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Administrator {
	public List<Color> AllPlayerColors = new List<Color>() 
	{ Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.black, Color.white};

	public List<SPlayer> Players;
	private List<Color> m_pieceColors;

	private Board m_board;

	public Administrator () {
		m_pieceColors = new List<Color>();
		Players = new List<SPlayer> ();
	}

	public bool LegalPlay (SPlayer sp, Board b, Tile t) {
		if (!(t.Coordinate == sp.Location.Coordinate))
			return false;
		if (!(sp.MyHand.IsInHand(t)))
			return false;
		if (b.IsPlayerEliminated (sp, t) && HasLegalMoves (sp, b))
			return false;
		return true;
	}

	public TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile) {
		b.PlaceTile (PlacedTile);
		SPlayer ActivePlayer = b.GetCurrentPlayer ();
		List<SPlayer> eliminatedPlayers = new List<SPlayer> ();
		foreach (SPlayer p in b.GetAdjacentPlayers(PlacedTile))
			eliminatedPlayers.AddRange(b.MovePlayer (p, PlacedTile));
		foreach (SPlayer sp in eliminatedPlayers)
			b.RemovePlayer (sp);
		if (b.CurrentPlayersIn.Contains(ActivePlayer) && ActivePlayer.MyHand.Pieces.Count < 3)
			b.CurrentDeck.DrawCard (ActivePlayer.MyHand);
		return getTurnOutput (b);
	}

	public bool HasLegalMoves (SPlayer sp, Board b)
	{
		foreach (Tile t in sp.MyHand.Pieces)
		{
			Tile tempT = new Tile(t.OriginalPaths);
			tempT.SetCoordinate (sp.Location.Coordinate);
			if (LegalTilePlayDirections (sp, tempT, b).Count > 0)
				return true;
		}  
		return false;
	}

	public List<Direction> LegalTilePlayDirections(SPlayer sp, Tile t, Board b) {
		List<Direction> LegalDirections = new List<Direction> ();
		for (int i = 0; i < 4; i++) {
			t.SetRotation((Direction)i);
			if (!b.IsPlayerEliminated (sp, t))
				LegalDirections.Add ((Direction)i);
		}
		return LegalDirections;
	}

	public void SetBoard(Board b) {
		m_board = b;
		b.Admin = this;
	}
	public void AddNewPlayer(Player p) {
		SPlayer newSPlayer = new SPlayer (p);
		m_pieceColors.Add (AllPlayerColors [Players.Count]);
		Players.Add (newSPlayer);
	}
	public void InitializeGame() {
		int i = 0;
		foreach (SPlayer splayer in Players) {
			splayer.PieceColor = m_pieceColors [i];
			splayer.MyPlayer.Initialize (m_pieceColors[i],m_pieceColors);
			PlayerLocation pLocation = splayer.MyPlayer.PlacePawn (m_board);
			m_board.AddNewPlayer (splayer, pLocation);
			i++;
		}
	}
	public void Play () {
		TurnOutput currentTurnStatus = new TurnOutput ();
		currentTurnStatus.ContinueGame = true;
		int numTurns = 0;
		while (currentTurnStatus.ContinueGame) {
			SPlayer currentSPlayer = m_board.GetCurrentPlayer ();

			Tile t = currentSPlayer.SelectTile (m_board, m_board.CurrentDeck.Pieces.Count);
			if (!LegalPlay(currentSPlayer,m_board,t)) {
				currentSPlayer.KickPlayerReplaceAI();
				t = currentSPlayer.SelectTile (m_board, m_board.CurrentDeck.Pieces.Count);
			}

			t.SetCoordinate( currentSPlayer.Location.Coordinate);
			currentSPlayer.MyHand.RemoveFromHand (t);

			currentTurnStatus = PlayATurn (m_board.CurrentDeck.Pieces, m_board.CurrentPlayersIn,
				m_board.CurrentPlayersOut, m_board, t);
			currentSPlayer.MyPlayer.EndGame(currentTurnStatus.b, playerToColors(currentTurnStatus.PlayersIn));

			if (!LegalHand(currentSPlayer.MyHand,m_board))
				currentSPlayer.KickPlayerReplaceAI();
			m_board.AdvancePlayers ();
		}
	}

	private bool LegalHand(Hand h , Board b) {
		if (!h.IsValid ())
			return false;
		foreach(Tile t in h.Pieces)
			if (b.IsOnBoard(t))
				return false;
		foreach(Tile t in h.Pieces)
			if (b.CurrentDeck.Pieces.Contains(t))
				return false;
		return true;
	}
	private List<Color> playerToColors(List<SPlayer> playerList) {
		var colorList = new List<Color> ();
		foreach (SPlayer p in playerList) {
			colorList.Add(p.PieceColor);
		}
		return colorList;
	}

	private TurnOutput getTurnOutput(Board b) {
		TurnOutput to = new TurnOutput();
		to.PlayersIn = b.CurrentPlayersIn;
		to.PlayersOut = b.CurrentPlayersOut;
		to.b = b;
		to.DrawPile = b.CurrentDeck.Pieces;
		to.ContinueGame = (to.PlayersIn.Count > 1);
		return to;
	}
}

public class TurnOutput
{
	public List<Tile> DrawPile;
	public List<SPlayer> PlayersIn;
	public List<SPlayer> PlayersOut;
	public Board b;
	public bool ContinueGame;
}
