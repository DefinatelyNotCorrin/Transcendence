using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("CardContainer")]
public class CardContainer {
        [XmlArray("Cards")]
        [XmlArrayItem("Card")]
        public List<Card> Cards = new List<Card>();
}


//*public static CardContainer Load(string path)
//{
//    var serializer = new XmlSerializer(typeof(CardContainer));
//    var stream = new FileStream(path, FileMode.Open);
//    var container = serializer.Deserialize(stream) as CardContainer;
//    stream.Close();
//}