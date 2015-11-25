using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class isDropableSurface : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    // Use this for initialization
    /*
    use a tag for the dragging card, set when the card starts dragging
    use findTag to get the GameObject of the card that is dragging to make sure that combat conditions and 
    */

    public void OnDrop(PointerEventData data)
    {
        isDraggable d = data.pointerDrag.GetComponent<isDraggable>();

        // Placing a card in a field 
        if (data.pointerCurrentRaycast.gameObject.CompareTag("Field") && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced == false)
        {
            //sends it to the new field
            d.parentToReturnTo = this.transform;
            // Make sure the card is still a card
            GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
        }

        // Dragging a card over another one turns on combat
        if (data.pointerCurrentRaycast.gameObject.CompareTag("Card") && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().isExhausted == false)
        {
           // Debug.Log("Combat conditions have been met!");
           GM.combat(GameObject.FindGameObjectWithTag("Dragging"), data.pointerCurrentRaycast.gameObject);        
        }

    }
	public void OnPointerEnter(PointerEventData data)
    {

    }
    public void OnPointerExit(PointerEventData data)
    {

    }
	
}
