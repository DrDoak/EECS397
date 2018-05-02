using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Administrator {
	public List<Color> AllPlayerColors = new List<Color>() 
	{ Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.black, Color.white};

	public List<SPlayer> Players;
	private List<Color> m_pieceColors;
	private int m_currentPlayerIndex = 0;
	private Board m_board;

	public Administrator () {
		m_pieceColors = new List<Color>();
		Players = new List<SPlayer> ();
	}

    public bool LegalPlay (SPlayer sp, Board b, Tile t) {
		if (!(t.Coordinate == sp.Location.Coordinate)) {
			return false;
		}
        if (!(sp.MyHand.IsInHand(t)))
        {
            return false;
        }
		if (b.IsPlayerEliminated (sp, t) && HasLegalMoves (sp, b))
        {
            return false;
        }
        return true;
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
		for (int i = 0; i < 4; i++)
		{
			t.SetRotation((Direction)i);
			if (!b.IsPlayerEliminated (sp, t)) {
				LegalDirections.Add ((Direction)i);
			}
		}
		return LegalDirections;
	}

	public TurnOutput PlayATurn(List<Tile> DrawPile, List<SPlayer> PlayersIn, List<SPlayer> PlayersOut, Board b, Tile PlacedTile) {
        TurnOutput to = new TurnOutput();
        SPlayer ActivePlayer = PlayersIn[0];
		List<SPlayer> eliminatedPlayers = new List<SPlayer> ();
		foreach (SPlayer p in b.GetAdjacentPlayers(PlacedTile))
			eliminatedPlayers.AddRange(b.MovePlayer (p, PlacedTile));

		foreach (SPlayer sp in eliminatedPlayers) {
			b.RemovePlayer (sp);
		}
		if (ActivePlayer.MyHand.Pieces.Count < 3)
			b.CurrentDeck.DrawCard (ActivePlayer.MyHand);
		to.DrawPile = b.CurrentDeck.DrawDeck;
		to.PlayersIn = b.CurrentPlayersIn;
		to.PlayersOut = b.CurrentPlayersOut;
		to.b = b;
		to.ContinueGame = (to.PlayersIn.Count > 1);
        return to;
    }

	public void SetBoard(Board b) {
		m_board = b;
	}
	public void AddNewPlayer(Player p) {
		SPlayer newSPlayer = new SPlayer (p);
		m_pieceColors.Add (AllPlayerColors [Players.Count]);
		Players.Add (newSPlayer);
	}
	public void InitializeGame() {
		int i = 0;
		foreach (SPlayer splayer in Players) {
			splayer.MyPlayer.Initialize (m_pieceColors[i],m_pieceColors);
			i++;
			PlayerLocation pLocation = splayer.MyPlayer.PlacePawn (m_board);
			m_board.AddNewPlayer (splayer, pLocation);
		}
	}
	public void Play () {
		TurnOutput currentTurnStatus = new TurnOutput ();
		currentTurnStatus.ContinueGame = true;
		int numTurns = 0;
		m_currentPlayerIndex = -1;
		while (currentTurnStatus.ContinueGame) {
			SPlayer currentSPlayer = GetNextPlayer (m_board.CurrentPlayersIn);

			Player p = currentSPlayer.MyPlayer;
			Tile t = p.PlayTurn (m_board, GetLegalTiles(currentSPlayer), m_board.CurrentDeck.DrawDeck.Count);

			t.SetCoordinate( currentSPlayer.Location.Coordinate);
			currentSPlayer.MyHand.RemoveFromHand (t);

			currentTurnStatus = PlayATurn (m_board.CurrentDeck.DrawDeck, m_board.CurrentPlayersIn,
				m_board.CurrentPlayersOut, m_board, t);
			
			p.EndGame(currentTurnStatus.b, playerToColors(currentTurnStatus.PlayersIn));
		}
	}
	public SPlayer GetNextPlayer(List<SPlayer> playersIn) {
		SPlayer nextPlayer = null;
		do {
			m_currentPlayerIndex = (m_currentPlayerIndex + 1)%Players.Count;
			nextPlayer = Players [m_currentPlayerIndex];
		} while(!playersIn.Contains(nextPlayer));
		return nextPlayer;
	}

	public List<Tile> GetLegalTiles(SPlayer sp) {
		List<Tile> legalTiles = new List<Tile> ();
		foreach (Tile t in sp.MyHand.Pieces) {
			List<Direction> legalDirs = LegalTilePlayDirections (sp, t, m_board);
			if (legalDirs.Count > 0) {
				t.LegalDirections = legalDirs;
				legalTiles.Add (t);
			}
		}
		return legalTiles;
	}

	private List<Color> playerToColors(List<SPlayer> playerList) {
		var colorList = new List<Color> ();
		foreach (SPlayer p in playerList) {
			colorList.Add(p.MyPlayer.PawnColor);
		}
		return colorList;
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