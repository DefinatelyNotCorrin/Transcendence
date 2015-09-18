using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class xmlReader : MonoBehaviour {

    //parts of the card object, and the cardDatabase object
    public ArrayList cardDatabase;
    public string name;
    public int ID;
    public string image;
    public string description;
    public string alliance;
    public string type;
    public int cost;
    public int attack;
    public int health;
    public int defense;
    public string range;
    public string target;

    //load the database from the xml file, and return the database as an ArrayList
    public ArrayList load()
    {
        xmlReader reader = xmlReader.Create("cards.xml");

        while(xmlReader.Read())
        {
            //should check to see if the line is an element, and then if it is called 'card'
            if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "card"))
            {
                //these retreive the name, Id, image, description, alliance, type, cost, attack, health, defense, range, and target, and set them to the local variables
                name = reader.GetAttribute("Name");
                ID = reader.GetAttribute("ID");
                image = reader.GetAttribute("Image");
                description = reader.GetAttribute("Description");
                alliance = reader.GetAttribute("Alliance");
                type = reader.GetAttribute("Type");
                cost = reader.GetAttribute("Cost");
                attack = reader.GetAtttribute("Attack");
                health = reader.GetAttribute("Health");
                defense = reader.GetAttribute("Defense");
                range = reader.GetAttribute("Range");
                target = reader.GetAttribute("Target");

                //takes the values from the local variables, and shoves them in a newly created card
                Card card = card.Create();
                card.setName(name);
                card.setID(ID);
                card.setImage(image);
                card.setDescription(description);
                card.setAlliance(description);
                card.setType(type);
                card.setCost(cost);
                card.setAttack(attack);
                card.setHealth(health);
                card.setDefense(defense);
                card.setRange(range);
                card.setTarget(target);

                //stores that new card in the cardDatabase array list
                cardDatabase.Add(card);

            }
        }

        //returns the cardDatabase after all cards have been added (PLS)
        return cardDatabase;
    }
}
