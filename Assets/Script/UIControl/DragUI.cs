using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventdata)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventdata,raycastResults);

        foreach (var g in raycastResults)
        {
            Debug.Log(g.gameObject.name);

        }
    }
    public void OnDrag(PointerEventData eventdata)
    {
        Debug.Log(eventdata.position);

    }
    public void OnEndDrag(PointerEventData eventdata)
    {
        Debug.Log(eventdata.pointerEnter.name);

    }
}
