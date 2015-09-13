using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool isDragging = false;

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged() {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		Debug.Log ("yay");
        isDragging = true;
	}

	public void OnDrag(PointerEventData data) {
		Debug.Log ("yay2");
	}

	public void OnEndDrag(PointerEventData data) {
		Debug.Log ("yay3");
        isDragging = false;
	}
}
