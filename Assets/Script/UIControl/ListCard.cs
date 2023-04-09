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

    public GameObject expandObject;//擴張後的資料


    [Header("UI")]

    public Text WordsCounts;
    public Text FoundingTime;
    public Text LastOpenTime;
    public Text mTitleText;
    public Button ButtonTitle;
    public Button ButtonExam;
    public Toggle ToggleChooseDelete;

    public Button IntoGameButton;

    public GameObject mTriggerEventAera;

    private static readonly Vector2 mTriggerEventAeraDefaultSize = new Vector2(1000, 100);

    public RectTransform ToggleBg;

    private ListCardFunc mFunc;

    public WordListData aData;

    public void Init(WordListData _Data, ListCardFunc _func)
    {
        aData = _Data;
        mTitleText.text = _Data.mTitle;
        ToggleChooseDelete.isOn = false;
        SetButtonSetting();
        WordsCounts.text = "單字總量:" + (_Data.WordsCountOfList() > 0 ? _Data.WordsCountOfList() : 0);
        FoundingTime.text = "創立時間:" + _Data.mFoundingTime;
        LastOpenTime.text = "最後進入:" + _Data.mLastOpenTime;
        mFunc = _func;
        Debug.Log(transform.gameObject.name + "的順序==>" + transform.GetSiblingIndex());
    }

    private void SetButtonSetting()
    {
        IntoGameButton.onClick.AddListener(ClickContentButton);
        mTriggerEventAera.GetComponent<RectTransform>().sizeDelta = mTriggerEventAeraDefaultSize;
    }




    public void OnToggleChooseDelete()//刪除模式
    {
        Debug.Log("listcard 刪除button");
        if (!ToggleChooseDelete.isOn)
        {
            ToggleChooseDelete.isOn = true;
            mFunc._AddDeleteFunc(this);
        }
        else
        {
            ToggleChooseDelete.isOn = false;
            mFunc._RemoveDeleteFunc(this);
        }

    }

    public void ClickTitleButton()//點擊標題，縮放單字組列表
    {
        if (!ListControlManager.Instance.isDrag)//不是拖曳的狀態
        {
            if (expand)
            {
                CLoseList();
                IntoGameButton.gameObject.SetActive(false);
            }
            else
            {
                OpenList();
                IntoGameButton.gameObject.SetActive(true);
            }
        }

    }

    public void ClickContentButton()
    {
        SceneManager.Instance.StartChangScene(SceneManager.SceneType.WordControlManager, aData);
    }

    public void IsDragButtonEffect(bool isDrag)
    {//當有按鈕被拖曳時期他按鈕的效果
        RectTransform mTri=mTriggerEventAera.GetComponent<RectTransform>();
        if (isDrag)
        {
            if (expand)
            {
                mTri.sizeDelta = new Vector2(mTri.sizeDelta.x,450);

            }
            else
            {
                mTri.sizeDelta = new Vector2(mTri.sizeDelta.x,150);
            }


        }
        else
        {
            if (expand)
            {
                mTri.sizeDelta = new Vector2(mTri.sizeDelta.x,400);
            }
            else
            {
                mTri.sizeDelta = new Vector2(mTri.sizeDelta.x,100);
            }

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




    private void OpenList()//開啟ListCard畫面
    {
        if (bntAni) return;
        expandObject.SetActive(true);
        expand = true;
        StartCoroutine(ToggleYSizeChange(this.gameObject, 100, 400));
        StartCoroutine(ToggleYSizeChange(mTriggerEventAera, 100, 400));
    }

    private void CLoseList()
    {
        if (bntAni) return;
        expand = false;
        StartCoroutine(ToggleYSizeChange(this.gameObject, 400, 100, 0.2f, () => { expandObject.SetActive(false); }));
        StartCoroutine(ToggleYSizeChange(mTriggerEventAera, 400, 100, 0.2f));
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

    private IEnumerator ToggleYSizeChange(GameObject obj, float Start, float End, float time = 0.2f, Action callback = null)
    {
        bntAni = true;
        //RectTransform Rt=transform.GetComponent<RectTransform>();
        RectTransform Rt = obj.transform.GetComponent<RectTransform>();
        Rt.sizeDelta = new Vector2(obj.transform.GetComponent<RectTransform>().sizeDelta.x, Start);

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

    private void RefreshContentSizeFitter()//刷新Content filter(改變母物件大小)
    {
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;

    }


    sealed public class ListCardFunc
    {//調用單字列表的其他腳本func

        public Action<ListCard> _AddDeleteFunc;
        public Action<ListCard> _RemoveDeleteFunc;


        public ListCardFunc(Action<ListCard> addDeleteFunc, Action<ListCard> removeDeleteFunc)
        {

            _AddDeleteFunc = addDeleteFunc;

            _RemoveDeleteFunc = removeDeleteFunc;


        }

    }





}

