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
    private Action<int> CloseWindow;

    public void Init(int _filterNum, Action<int> _closeWindow, bool CloseOfFilter = true)
    {
        FilterNum = _filterNum;
        CloseWindow += _closeWindow;

        if (CloseOfFilter)
        {
            SetDefaultCloseOfFilter();
        }

    }

    public void SetButtonFunc(Action _callBack)
    {
        transform.GetComponent<Button>().onClick.AddListener(() => { _callBack(); });


    }

    public void SetDefaultCloseOfFilter()
    {

        transform.GetComponent<Button>().onClick.AddListener(CloseFilter);
    }
    public void CloseFilter()//有些功能不需要案背景就需要有關閉的效果
    {
        CloseWindow(FilterNum);
    }

}
