using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System;
public class PopupManager : InstanceScript<PopupManager>
{

    public GameObject FilterParentCanvas = null;
    public GameObject Filter = null;

    //public List<GameObject> PopupList=new List<GameObject>();

    private int PopupNum = 0;
    public Dictionary<int, GameObject> PopupList = new Dictionary<int, GameObject>();

    public void Awake()
    {
        DontDestoryThis();
    }

    private IEnumerator<string> fff()
    {

        yield return null;



    }

    public GameObject OpenPopup(GameObject _popup, Action _callBack = null)
    {
        string aa = fff().Current;

        if (FilterParentCanvas == null) FilterParentCanvas = GameObject.Find("PopupLayer");

        GameObject sp = Instantiate(_popup);

        GameObject filter = Instantiate(Filter, FilterParentCanvas.transform);
        filter.GetComponent<FilterScript>().Init(PopupNum, ClosePopup);


        //filter.transform.SetParent(FilterParentCanvas.transform);
        sp.transform.SetParent(filter.transform);
        filter.transform.localScale = Vector3.one;
        sp.transform.localScale = Vector3.one;

        PopupList.Add(PopupNum, filter);

        if (_callBack != null)
        {
            filter.GetComponent<FilterScript>().SetButtonFunc(_callBack);
        }

        PopupNum++;
        return sp;
    }

    public void OpenInputStringOneCurrectButtonWindow(Action<string> _correctButton, Action _filterCallBack = null)
    {
        InputStringWindowPopup sp = OpenPopup
        (Resources.Load<GameObject>(ResourcesPath.PopupWindowPath + "InputStringWindowPopup"), _filterCallBack)
        .GetComponent<InputStringWindowPopup>();
        sp.ReturnList += _correctButton;
        sp.Init("請輸入群組名稱");


    }

    public void OpenHintOnlyStringWindow(string title, string Content )
    {
        HintPopup sp = OpenPopup(Resources.Load<GameObject>(ResourcesPath.PopupWindowPath + "HintWindowPopup"))
        .GetComponent<HintPopup>();
        sp.transform.parent.GetComponent<Image>().color=new Color(0,0,0,0);
        sp.Init(title,Content);
        Destroy(sp.transform.parent.gameObject,2);
        
    }


    public void ClosePopup(int filterNum)
    {
        Destroy(PopupList[filterNum]);
        PopupList.Remove(filterNum);

    }

    public void ClosePopup(GameObject filter)
    {
        int filterNum = filter.GetComponent<FilterScript>().FilterNum;
        Destroy(PopupList[filterNum]);
        PopupList.Remove(filterNum);

    }




}
