using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector]public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Sibling Index : " + transform.GetSiblingIndex());
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End of drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
