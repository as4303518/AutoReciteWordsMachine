using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject DragGameObject = null;

    // public event Action<GameObject> OnEnterCallBack = null;
    // public event Action<GameObject> OnExitCallBack = null;

    public event Action<GameObject> OnClickCallBack = null;
    public event Action<GameObject> OnBeginDragCallBack = null;
    public event Action<GameObject> OnEndDragCallBack = null;
    public event Action<GameObject> OnDragCallBack = null;

    public object aData;


    //目前遇到的瓶頸，無法觸發底下listcard的擴展按鈕
    //要如何決定拖曳或點擊
    //無法關閉image raycast

    public void AddGameObjectEvent(GameObject _obj)
    {

        GameObject TriObJ;
        if (_obj.transform.Find("TriggerEventArea") == null)
        {

            TriObJ = new GameObject("TriggerEventArea", typeof(RectTransform));
            TriObJ.AddComponent<EventTrigger>();
            TriObJ.transform.SetParent(_obj.transform);

        }
        else
        {
            TriObJ = _obj.transform.Find("TriggerEventArea").gameObject;
        }

        AddEvent(TriObJ, EventTriggerType.PointerClick, eve => { onClick(eve, TriObJ); });
        // AddEvent(TriObJ, EventTriggerType.PointerEnter, delegate { OnEnter(TriObJ); });
        // AddEvent(TriObJ, EventTriggerType.PointerExit, delegate { OnExit(TriObJ); });
        AddEvent(TriObJ, EventTriggerType.BeginDrag, eve => { OnBeginDrag(eve, TriObJ); });
        AddEvent(TriObJ, EventTriggerType.EndDrag, eve => { OnEndDrag(eve, TriObJ); });
        AddEvent(TriObJ, EventTriggerType.Drag, eve => { OnDrag(eve, TriObJ); });

    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger eTri = obj.GetComponent<EventTrigger>();
        EventTrigger.Entry _event = new EventTrigger.Entry();
        _event.eventID = type;
        _event.callback.AddListener(action);
        eTri.triggers.Add(_event);
    }



    public void onClick(BaseEventData eve, GameObject Obj)//進入有偵測的地方(拖曳期間進入也會觸發)
    {

        if (OnClickCallBack != null) OnClickCallBack(Obj);
        Debug.Log("事件OnClick==>" + Obj.transform.parent.name);

    }


    // public void OnEnter(GameObject Obj)//進入有偵測的地方(拖曳期間進入也會觸發)
    // {
    //     //if (OnEnterCallBack != null) OnEnterCallBack(Obj);

    //     // DragGameObject = Obj;
    //     //DragGameObject.GetComponent<Image>().raycastTarget=false;
    //     Debug.Log("事件OnEnter==>" + Obj.name);

    // }
    // public void OnExit(GameObject Obj)//退出有偵測的地方(拖曳期間退出也會觸發)
    // {
    //     // if (OnExitCallBack != null) OnExitCallBack(Obj);

    //     // DragGameObject.GetComponent<Image>().raycastTarget=true;
    //     Debug.Log("事件OnExit===>" + Obj.name);

    // }
    public void OnBeginDrag(BaseEventData eve, GameObject Obj)//開始拖曳(如果按住不動則不會出發)
    {

        DragGameObject = Obj.transform.parent.gameObject;
        if (OnBeginDragCallBack != null) OnBeginDragCallBack(Obj);

        // Debug.Log("事件OnBeginDrag===>" + Obj.transform.parent.name);



        //物件變透明,代表正被拖曳,記錄其陣列位置

    }
    //結束拖曳(如果按住不動則不會出發)(一定要有拖曳開始，才會有結束)
    public void OnEndDrag(BaseEventData eve, GameObject Obj)//
    {
        Debug.Log("拖曳結束===>"+Obj.name);
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (OnEndDragCallBack != null) OnEndDragCallBack(raycastResults[i].gameObject);

        }
        if(raycastResults.Count<=0){
            OnEndDragCallBack(null);
        }

        DragGameObject = null;

    }
    public void OnDrag(BaseEventData eve, GameObject Obj)//拖曳中
    {
        if (OnDragCallBack != null) OnDragCallBack(Obj);


    }

    public void ClearEvent()
    {
        // OnEnterCallBack = null;
        // OnExitCallBack = null;
        OnClickCallBack = null;
        OnBeginDragCallBack = null;
        OnEndDragCallBack = null;
        OnDragCallBack = null;
    }


}
