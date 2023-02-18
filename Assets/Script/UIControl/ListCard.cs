using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    private event Action<ListCard> mAddDeleteFunc;
    private event Action<ListCard> mRemoveDeleteFunc;
    public WordList aData;

    public void Init(WordList _list, Action<ListCard> _addDeleteFunc, Action<ListCard> _removeDeleteFunc)
    {
        aData = _list;
        titleText.text = _list.mTitle;
        ToggleChooseDelete.isOn = false;

        WordsCounts.text = "單字總量:" + (_list.WordsCountOfList() > 0 ? _list.WordsCountOfList() : 0);
        FoundingTime.text = "創立時間:" + _list.mFoundingTime;
        LastOpenTime.text = "最後進入:" + _list.mLastOpenTime;

        mAddDeleteFunc = _addDeleteFunc;
        mRemoveDeleteFunc = _removeDeleteFunc;

    }

    public void OnToggleChooseDelete()//刪除模式
    {
        if (ToggleChooseDelete.isOn)
        {
            mAddDeleteFunc(this);
        }
        else
        {
            mRemoveDeleteFunc(this);
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
    }

    public void CloseDeleteModel()
    {
        ToggleChooseDelete.gameObject.SetActive(false);
    }


    private void OpenList()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 400);
        expandObject.SetActive(true);
        expand = true;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    private void CLoseList()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 100);
        expandObject.SetActive(false);
        expand = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }





}
