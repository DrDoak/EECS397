using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	bool test = true;
	// Use this for initialization
	void Start () {
		if (test) {
			RunTests ();
		}
	}

	void RunTests() {
		Vector2 testResults = new Vector2 ();
		Tile.Tests ();
		SPlayer.Tests ();
		DirectionUtils.Tests ();
		Board.Tests ();
		Administrator.Tests ();

	}
}
