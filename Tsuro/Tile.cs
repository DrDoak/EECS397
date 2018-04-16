using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation { UP, LEFT, DOWN, RIGHT };

public class Tile : MonoBehaviour {

    public List<Vector2> Paths;
    private Vector2 Coordinate;
    private Rotation myrotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	} 
    public Tile SetRotation (Tile t, Rotation r)
    {
        t.myrotation = r;
        return t;
    }
}
