using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool isDragging = false;

    //original location of a card before it is dragged
    public Transform parentToReturnTo = null;

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged() {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		//Debug.Log ("on begin drag");
        isDragging = true;

        //saves the current parent of the object
        parentToReturnTo = this.transform.parent;
        //sets the objects parent up one level (resetting the grid)
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;


        
        
    }

	public void OnDrag(PointerEventData data) {
		//Debug.Log ("dragging");

        //moves the card to the cursor position
        this.transform.position = data.position;

        this.gameObject.tag = "Dragging";

    }

	public void OnEndDrag(PointerEventData data) {
		//Debug.Log ("on end drag");
        isDragging = false;

        //
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
