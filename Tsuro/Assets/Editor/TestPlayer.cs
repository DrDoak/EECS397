using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestPlayer {

	[Test]
	public void Initialize() {
		Player p = new Player("John");

		Assert.AreEqual("John", p.GetName(), "GetName returns correct name");
		List<Color> playercolorlist = new List<Color>();
		Assert.False(playercolorlist.Contains(Color.red), "Red is not yet in Player color list."); 
		p.Initialize(Player.AllPlayerColors[0], playercolorlist);
		Assert.True(playercolorlist.Contains(Color.red), "Red is now in Player color list.");
	}
}