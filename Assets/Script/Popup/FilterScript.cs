using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class FilterScript : MonoBehaviour
{
    public int FilterNum;

    public List<int> DependWindow = new List<int>();//母彈窗的子視窗們，方便一起關閉，也不會關到其他視窗
    private Func<int, IEnumerator> CloseWindow;
    // private  object CloseWindow;

    public void Init(int _filterNum, Func<int, IEnumerator> _closeWindow, bool CloseOfFilter = true)
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
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            _callBack();
            //transform.GetComponent<Button>().onClick.RemoveAllListeners();
        });


    }

    public void SetDefaultCloseOfFilter()
    {

        transform.GetComponent<Button>().onClick.AddListener(CloseFilter);
    }
    public void CloseFilter()//有些功能不需要案背景就需要有關閉的效果
    {
        StartCoroutine(CloseWindow(FilterNum));
        //transform.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void DestroyThisPopup(float desTime = 0)
    {

        StartCoroutine(OnDestory(desTime));

    }
    private IEnumerator OnDestory(float desTime = 0)//ui沒辦法觸發，只有遊戲物件才能
    {

        Debug.Log("已刪除父母視窗");

        yield return new WaitForSeconds(desTime);
        StartCoroutine(CloseWindow(FilterNum));


        // Destroy(this.gameObject,desTime);

    }
}
