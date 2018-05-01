using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Main : MonoBehaviour {

	bool test = true;
	public int NumPlayers = 4;
	List<Player> m_players;
	Dictionary<Player,SPlayer> m_playerMap;
	Administrator m_admin;
	Board m_board;

	// Use this for initialization
	/*void Start() {
		m_playerMap = new Dictionary<Player, SPlayer> ();
		m_admin = new Administrator ();
		m_board = new Board (new Vector2Int(6,6));
		m_players = new List<Player> ();
		for (int i = 0; i < NumPlayers; i++) {
			Player p = new Player ("Player: " + i);
			//p.Initialize ();
			m_players.Add(p);
		}
		foreach (Player p in m_players) {
			//p.PlacePawn (b);
		}
	}
	void Update () {
		foreach (Player p in m_players) {
			Tile t = p.PlayTurn (m_board, m_playerMap [p].MyHand, m_board.CurrentDeck.DrawDeck.Count);
			TurnOutput to = m_admin.PlayATurn (m_board.CurrentDeck.DrawDeck, m_board.CurrentPlayersIn,
				m_board.CurrentPlayersOut, m_board, t);
			p.EndGame(to.b, playerToColors(to.PlayersIn));
		}
	}
	private List<Color> playerToColors(List<SPlayer> playerList) {
		//var res = m_playerMap.GroupBy(p => p.Value).ToDictionary(g => g.Key, g => g.Select(pp => pp.Key).ToList());
		var colorList = new List<Color> ();
		foreach (SPlayer p in playerList) {
			colorList.Add(p.PieceColor);
		}
		return colorList;
	}
	public void NextPlayer() {
	}*/
}
