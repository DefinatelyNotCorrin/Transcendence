using UnityEngine;
using System;

	public class Card : MonoBehaviour {

    //base values
    public string name;
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

    //values for combat 
    public int currentID;
    public int currentCost;
    public int currentAttack;
    public int currentHealth;
    public int battleSide;
    public double currentDefense;
    public string currentRange;
    

    void Start()

    {
        

    }

	public void setCurrents() {

		//sets the combat values to the base as the object is initialized
		currentID = Int32.Parse (ID);
		currentCost = Int32.Parse (cost);
		currentAttack = Int32.Parse (attack);
		currentHealth = Int32.Parse (health);
		currentDefense = Convert.ToDouble (defense);
		currentRange = range;
	}

}




