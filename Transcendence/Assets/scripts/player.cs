using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    public int playerSide = 0;
    public string deckPath = "/scripts/xml/cards.xml";
    public int baseHP = 30;
    public int currentHP;
    public bool isTurn = false;
    public bool hasStartedGoing = false;
    public int currentMana;

    // Use this for initialization
    void Start ()
    {
        currentMana = 1;
        currentHP = 30;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
