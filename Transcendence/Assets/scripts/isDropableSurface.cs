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

        if (data.pointerCurrentRaycast.gameObject.CompareTag("Field"))
        {
            d.parentToReturnTo = this.transform;
            GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
        }

        if (data.pointerCurrentRaycast.gameObject.CompareTag("Card"))
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
