using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestPlayerLocation {

	[Test]
	public void Initialization() {
		Vector2Int v = new Vector2Int(2, 2);
		PlayerLocation pl = new PlayerLocation(v, 3);
		Assert.AreEqual(pl.Coordinate , v, "player position takes given coordinate");
		Assert.AreEqual(pl.PositionOnTile , 3, "player position takes given position");
	}

	[Test]
	public void Equality() {
		Vector2Int v = new Vector2Int(2, 2);
		PlayerLocation pl = new PlayerLocation(v, 3);

		Vector2Int v2 = new Vector2Int(2, 2);
		PlayerLocation pl2 = new PlayerLocation(v2, 3);

		Vector2Int v3 = new Vector2Int(2, 5);
		PlayerLocation pl3 = new PlayerLocation(v3, 3);

		Vector2Int v4 = new Vector2Int(2, 2);
		PlayerLocation pl4 = new PlayerLocation(v4, 7);

		Assert.AreEqual(pl , pl2, "player positions equal");
		Assert.AreNotEqual(pl , pl3, "player positions not equal due to coordinate");
		Assert.AreNotEqual(pl , pl4, "player positions not equal due to tile pos");
	}
}
