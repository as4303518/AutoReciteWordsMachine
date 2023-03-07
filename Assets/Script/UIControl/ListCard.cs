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
    public bool bntAni = false;//偵測這個物件是否在動畫的狀態，如果有就不要執行其他動畫，以免衝突

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

    public void Init(WordListData _Data, ListCardFunc _func)
    {
        aData = _Data;
        titleText.text = _Data.mTitle;
        ToggleChooseDelete.isOn = false;

        WordsCounts.text = "單字總量:" + (_Data.WordsCountOfList() > 0 ? _Data.WordsCountOfList() : 0);
        FoundingTime.text = "創立時間:" + _Data.mFoundingTime;
        LastOpenTime.text = "最後進入:" + _Data.mLastOpenTime;
        mFunc = _func;

    }

    public void OnToggleChooseDelete()//刪除模式
    {
        if (ToggleChooseDelete.isOn)
        {
            mFunc._AddDeleteFunc(this);
        }
        else
        {
            mFunc._RemoveDeleteFunc(this);
        }
    }

    public void ClickTitleButton()//點擊標題，縮放單字組列表
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

    public void ClickContentButton()
    {
        StartCoroutine(IEContentButton());
        //按鈕不可再按

    }

    public IEnumerator IEContentButton()
    {
        yield return PopupManager.Instance.OpenLoading();
        yield return mFunc._CloseList();
        Debug.Log("進入場景" + aData.mTitle);
        yield return new WaitForSeconds(2);
        yield return mFunc._OpenList();
        yield return PopupManager.Instance.CloseLoading();
        Debug.Log("已過兩秒");

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
        if (bntAni) return;
        expandObject.SetActive(true);
        expand = true;
        StartCoroutine(ToggleYSizeChange(100, 400));
    }

    private void CLoseList()
    {
        if (bntAni) return;
        expand = false;
        StartCoroutine(ToggleYSizeChange(400, 100, 0.2f, () => { expandObject.SetActive(false); }));
    }

    private IEnumerator ToggleXSizeChange(float Start, float End, float time = 0.2f, Action callback = null)//List 刪除按鈕動畫
    {
        ToggleBg.sizeDelta = new Vector2(Start, ToggleBg.sizeDelta.y);
        float addSpeed = (End - Start) / time * Time.deltaTime;
        bool rule = Start < End ? ToggleBg.sizeDelta.x < End : ToggleBg.sizeDelta.x >= End;

        while (rule)
        {
            ToggleBg.sizeDelta = new Vector2(ToggleBg.sizeDelta.x + addSpeed, ToggleBg.sizeDelta.y);
            rule = Start < End ? ToggleBg.sizeDelta.x < End : ToggleBg.sizeDelta.x >= End;
            yield return null;
        }
        if (callback != null) callback();

    }

    private IEnumerator ToggleYSizeChange(float Start, float End, float time = 0.2f, Action callback = null)
    {
        bntAni = true;
        RectTransform Rt = transform.GetComponent<RectTransform>();
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
        bntAni = false;
        if (callback != null) callback();

    }

    private void RefreshContentSizeFitter()
    {
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;

    }


    public class ListCardFunc
    {//調用單字列表的其他腳本func

        public Action<ListCard> _AddDeleteFunc;
        public Action<ListCard> _RemoveDeleteFunc;
        public Func<IEnumerator> _CloseList;//關閉單字組列表  單字組列表-->單字列表

        public Func<IEnumerator> _OpenList;//關閉單字組列表  單字組列表-->單字列表

        public ListCardFunc(Action<ListCard> addDeleteFunc, Action<ListCard> removeDeleteFunc, Func<IEnumerator> closeList,Func<IEnumerator> openList)
        {

            _AddDeleteFunc = addDeleteFunc;
            _RemoveDeleteFunc = removeDeleteFunc;
            _CloseList = closeList;

            _OpenList=openList;

        }

    }





}

