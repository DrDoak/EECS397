using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer {

	string GetName();

	void Initialize(Color m_color, List<Color> player_colors);

	PlayerLocation PlacePawn (Board b);

	Tile PlayTurn (Board b, List<Tile> legalTiles, int Piecescount);

	void EndGame (Board b, List<Color> player_colors);
}
