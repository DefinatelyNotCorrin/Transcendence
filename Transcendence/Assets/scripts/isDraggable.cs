using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public void OnBeginDrag(PointerEventData data) {
		Debug.Log ("yay");
	}

	public void OnDrag(PointerEventData data) {
		Debug.Log ("yay2");
	}

	public void OnEndDrag(PointerEventData data) {
		Debug.Log ("yay3");
	}
}
