using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Main : MonoBehaviour {

	public List<string> PlayerNames = new List<string> ();

	// Use this for initialization
	void Start() {
		Administrator a = new Administrator ();
		Board b = new Board (new Vector2Int(6,6));
		a.SetBoard (b);
		foreach (string s in PlayerNames) {
			PlayerMachine p = new PlayerMachine (s);
			a.AddNewPlayer(p);
		}
	}
}
