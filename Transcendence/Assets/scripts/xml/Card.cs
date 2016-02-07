using UnityEngine;
using System;

	public class Card : MonoBehaviour {

    //base values
    public string cardName;
    public string ID;
    public string image;
    public string description;
    public string alliance;
    public string type;
    public string cost;
    public string attack;
    public string health;
    public string defense;
    public string range;
    public string target;
    public string effect;

    //values for combat 
    public int currentID;
    public int currentCost;
    public double currentAttack;
    public double currentHealth;
    public int battleSide;
    public double currentDefense;
    public string currentRange;
    public bool isExhausted;
    public bool hasBeenPlaced;
    public string ownerTag;
    

    void Start()

    {  

    }

	public void setCurrents() {

		//sets the combat values to the base as the object is initialized
		currentID = Int32.Parse (ID);
		currentCost = Int32.Parse (cost);
		currentAttack = Convert.ToDouble (attack);
		currentHealth = Convert.ToDouble (health);
		currentDefense = Convert.ToDouble (defense);
		currentRange = range;
	}

}




