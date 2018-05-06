using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerAIType { RANDOM, SYMMETRIC, ASYMMETRIC}

public class PlayerMachine : Player {

    public PlayerMachine(string m_name) : base(m_name) { }
	public PlayerAIType AIType = PlayerAIType.RANDOM;
	public Administrator Admin;

    public override string GetName() { return base.GetName(); }

    public override PlayerLocation PlacePawn(Board b)
    {
		PlayerLocation pl = new PlayerLocation (new Vector2Int(0,0),0);
		do {
			Vector2Int coord = new Vector2Int(0,0);
			int positionOnTile = 0;
			if (Random.Range(1,2) == 1) {
				coord.x =  (b.BoardSize.x - 1) * Random.Range ((int)0, (int)1);
				coord.y =  Random.Range(0, b.BoardSize.y - 1);
				if (coord.x == 0) {
					positionOnTile = randomIntAlongDirection(Direction.LEFT);
				} else {
					positionOnTile = randomIntAlongDirection(Direction.RIGHT);
				}
			} else {
				coord.y =  (b.BoardSize.y - 1) * Random.Range ((int)0, (int)1);
				coord.x =  Random.Range(0, b.BoardSize.x - 1);
				if (coord.y == 0) {
					positionOnTile = randomIntAlongDirection(Direction.DOWN);
				} else {
					positionOnTile = randomIntAlongDirection(Direction.UP);
				}
			}
			pl = new PlayerLocation(coord, positionOnTile);
		} while(b.GetPlayersAtLocation(pl).Count > 0);
        return pl;
    }

	private int randomIntAlongDirection( Direction d) {
		switch (d) {
		case Direction.UP:
			return Random.Range((int)0,(int)1);
		case Direction.RIGHT:
			return Random.Range((int)2,(int)3);
		case Direction.DOWN:
			return Random.Range((int)4,(int)5);
		case Direction.LEFT:
			return Random.Range((int)6,(int)7);
		default:
			throw new System.ArgumentException ();
		}
	}
	public override Tile PlayTurn(Board b, List<Tile> legalTiles, int drawdeckcount)
	{
		SortSymmetry (legalTiles);
		switch (AIType) {
		case (PlayerAIType.RANDOM):
			return ChooseRandomTile (legalTiles);
		case(PlayerAIType.SYMMETRIC):
			return legalTiles[0];
		case(PlayerAIType.ASYMMETRIC):
			return legalTiles[legalTiles.Count - 1];
		default:
			return legalTiles [0];
		}
    }

	private Tile ChooseRandomTile(List<Tile> legalTiles)
    {
		Tile t = legalTiles[Random.Range(0, legalTiles.Count - 1)];
		Direction d = t.LegalDirections[Random.Range(0, t.LegalDirections.Count - 1)];
        t.SetRotation(d);
        return t;
    }

	private void SortSymmetry(List<Tile> legalTiles) {
		legalTiles.Sort((a,b) => (UniqueRotationTiles(a).CompareTo(UniqueRotationTiles(b))));
	}

    public override void EndGame(Board b, List<Color> player_colors)
    {
        base.EndGame(b, player_colors);
    }

	public int UniqueRotationTiles(Tile t) {
		List<Tile> uniqueTiles = new List<Tile> ();
		uniqueTiles.Add (t);
		for (int i = 0; i < 4; i++) {
			t.SetRotation ((Direction)i);
			Tile compareTile = new Tile (t.RotatedPaths);
			if (!uniqueTiles.Contains (compareTile))
				uniqueTiles.Add (compareTile);
		}
		return uniqueTiles.Count;
	}
}