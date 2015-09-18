using UnityEngine;
using System.Collections;

public class CardDisplay : MonoBehaviour {

    public const string path = "cards";

	// Use this for initialization
	void Start () {
        CardContainer cc = CardContainer.Load(path);

        foreach(Card card in cc.cards)
        {
            print((card.name));
        }
	}

}
