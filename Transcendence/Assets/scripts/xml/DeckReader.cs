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
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "card"))
             {
                //generates new empty card object
                Card card = new Card();

                    //set values of new card Object from xml attributes
                    card.name = reader.GetAttribute("name");
                    card.ID = reader.GetAttribute("ID");
                    card.image = reader.GetAttribute("image");
                    card.description = reader.GetAttribute("description");
                    card.alliance = reader.GetAttribute("alliance");
                    card.type = reader.GetAttribute("type");
                    card.cost = reader.GetAttribute("cost");
                    card.attack = reader.GetAttribute("attack");
                    card.defense = reader.GetAttribute("defense");
                    card.range = reader.GetAttribute("range");
                    card.target = reader.GetAttribute("target");
                    

                //stores that new card in the DeckReader array list
                arr.Add(card);

            }
        }

        //returns the card arr after all cards have been added (PLS)
        return arr;
    }
}
