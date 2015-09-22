using System.Collections;
using System.Xml;

public class DeckReader {

    //arr of card objects
    public ArrayList arr;

    //the path of the xml card data you want to load
    public string path;

    //load the card data from the xml file, and return the database as an ArrayList of Cards
    public ArrayList load(string inputPath)
    {

        //follows the path of the xml, and loads it into a reader
        arr = new ArrayList();
        path = inputPath;
        XmlReader reader = XmlReader.Create(path);

        while(reader.Read())
        {

            //should check to see if the line is an element, and then if it is called 'card'
            if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "card"))

            {
                //generates card object based on read values
                Card card = new Card();
                card.name = reader.GetAttribute("Name");
                card.ID = reader.GetAttribute("ID");
                card.image = reader.GetAttribute("Image");
                card.description = reader.GetAttribute("Description");
                card.alliance = reader.GetAttribute("Alliance");
                card.type = reader.GetAttribute("Type");
                card.cost = reader.GetAttribute("Cost");
                card.attack = reader.GetAttribute("Attack");
                card.health = reader.GetAttribute("Health");
                card.defense = reader.GetAttribute("Defense");
                card.range = reader.GetAttribute("Range");
                card.target = reader.GetAttribute("Target");

                //stores that new card in the DeckReader array list
                arr.Add(card);

            }
        }

        //returns the card arr after all cards have been added (PLS)
        return arr;
    }
}
