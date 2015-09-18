using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class xmlReader : MonoBehaviour {

    //parts of the card object, and the deck object
    public ArrayList deck;
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

    //the path of the deck you want to load
    public string path;

    //load the database from the xml file, and return the database as an ArrayList
    public ArrayList load(string path)
    {

        //finds the path of the xml, and loads it into a reader
        this.path = path;
        XmlReader reader = XmlReader.Create(path);

        while(reader.Read())
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
                attack = reader.GetAttribute("Attack");
                health = reader.GetAttribute("Health");
                defense = reader.GetAttribute("Defense");
                range = reader.GetAttribute("Range");
                target = reader.GetAttribute("Target");

                //takes the values from the local variables, and shoves them in a newly created card
                Card card = new Card();
                card.name = name;
                card.ID = ID;
                card.image = image;
                card.description = description;
                card.alliance = alliance;
                card.type = type;
                card.cost = cost;
                card.attack = attack;
                card.health = health;
                card.defense = defense;
                card.range = range;
                card.target = target;

                //stores that new card in the cardDatabase array list
                deck.Add(card);

            }
        }

        //returns the cardDatabase after all cards have been added (PLS)
        return deck;
    }
}
