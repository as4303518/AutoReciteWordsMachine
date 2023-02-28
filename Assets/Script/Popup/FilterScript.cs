using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class FilterScript : MonoBehaviour
{
    public int FilterNum;

    public List<int> ChildWindow = new List<int>();//母彈窗的子視窗們，方便一起關閉，也不會關到其他視窗
    private UnityAction<int> CloseWindow;

    public void Init(int _filterNum, UnityAction<int> _closeWindow)
    {
        FilterNum = _filterNum;
        CloseWindow += _closeWindow;
        SetDefaultCloseOfFilter();
        
    }

    public void SetButtonFunc(Action _callBack)
    {
        transform.GetComponent<Button>().onClick.AddListener(()=>{_callBack();});


    }

    public void SetDefaultCloseOfFilter()
    {

        transform.GetComponent<Button>().onClick.AddListener(CloseFilter);
    }
    public void CloseFilter()
    {
        CloseWindow(FilterNum);
    }

}
