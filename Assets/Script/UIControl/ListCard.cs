using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ListCard : MonoBehaviour
{
    //list實體化腳本
    //開啟與關閉坐在title上，進入單字選單做在打開後的物件上
    public bool expand = false;

    public GameObject expandObject;

    [Header("UI")]

    public Text WordsCounts;
    public Text FoundingTime;
    public Text LastOpenTime;
    public Text titleText;
    public Button ButtonTitle;
    public Button ButtonExam;
    public Toggle ToggleChooseDelete;

    public RectTransform ToggleBg;

    private ListCardFunc mFunc;

    public WordListData aData;

    public void Init(WordListData _list, ListCardFunc _func)
    {
        aData = _list;
        titleText.text = _list.mTitle;
        ToggleChooseDelete.isOn = false;

        WordsCounts.text = "單字總量:" + (_list.WordsCountOfList() > 0 ? _list.WordsCountOfList() : 0);
        FoundingTime.text = "創立時間:" + _list.mFoundingTime;
        LastOpenTime.text = "最後進入:" + _list.mLastOpenTime;
        mFunc = _func;

    }

    public void OnToggleChooseDelete()//刪除模式
    {
        if (ToggleChooseDelete.isOn)
        {
            mFunc._addDeleteFunc(this);
        }
        else
        {
            mFunc._removeDeleteFunc(this);
        }
    }

    public void ClickTitleButton()
    {
        if (expand)
        {
            CLoseList();
        }
        else
        {
            OpenList();
        }
    }


    public void OpenDeleteModel()
    {
        ToggleChooseDelete.gameObject.SetActive(true);
        ToggleChooseDelete.interactable = false;
        StartCoroutine(ToggleXSizeChange(0, 80, 0.2f, () => { ToggleChooseDelete.interactable = true; }));


    }

    public void CloseDeleteModel()
    {
        ToggleChooseDelete.gameObject.SetActive(false);
        ToggleChooseDelete.interactable = false;

    }




    private void OpenList()
    {
        expandObject.SetActive(true);
        expand = true;
        StartCoroutine(ToggleYSizeChange(100, 400));


        //transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 400);
        //刷新parent的尺寸
        // RefreshContentSizeFitter();
    }

    private void CLoseList()
    {
        // transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 100);
        // expandObject.SetActive(false);
        expand = false;
        StartCoroutine(ToggleYSizeChange(400, 100,0.2f,()=>{expandObject.SetActive(false);}));

        //刷新parent的尺寸
        // RefreshContentSizeFitter();
    }

    private IEnumerator ToggleXSizeChange(float Start, float End, float time = 0.2f, Action callback = null)
    {
        ToggleBg.sizeDelta = new Vector2(Start, ToggleBg.sizeDelta.y);
        float addSpeed = (End - Start) / time * Time.deltaTime;
        bool rule = Start < End ? ToggleBg.sizeDelta.x < End : ToggleBg.sizeDelta.x >= End;

        while (rule)
        {
            Debug.Log("執行");
            ToggleBg.sizeDelta = new Vector2(ToggleBg.sizeDelta.x + addSpeed, ToggleBg.sizeDelta.y);
            rule = Start < End ? ToggleBg.sizeDelta.x < End : ToggleBg.sizeDelta.x >= End;
            yield return null;
        }
        if(callback!=null)callback();

    }

    private IEnumerator ToggleYSizeChange(float Start, float End, float time = 0.2f, Action callback = null)
    {
        RectTransform Rt=transform.GetComponent<RectTransform>();
        Rt.sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, Start);

        float addSpeed = (End - Start) / time * Time.deltaTime;
        bool rule = Start < End ? Rt.sizeDelta.y < End : Rt.sizeDelta.y >= End;

        while (rule)
        {

            Rt.sizeDelta = new Vector2(Rt.sizeDelta.x, Rt.sizeDelta.y + addSpeed);
            rule = Start < End ? Rt.sizeDelta.y < End : Rt.sizeDelta.y >= End;
            RefreshContentSizeFitter();
            yield return null;
        }
        if(callback!=null)callback();

    }

    private void RefreshContentSizeFitter()
    {
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;

    }


    public class ListCardFunc
    {//調用單字列表的其他腳本func

        public Action<ListCard> _addDeleteFunc;
        public Action<ListCard> _removeDeleteFunc;

        public ListCardFunc(Action<ListCard> addDeleteFunc, Action<ListCard> removeDeleteFunc)
        {

            _addDeleteFunc = addDeleteFunc;
            _removeDeleteFunc = removeDeleteFunc;

        }

    }



}

