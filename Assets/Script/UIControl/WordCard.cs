using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class WordCard : MonoBehaviour
{

    [Header("UI")]
    public Button wordInfoButton;
    public Toggle DeleteToggle;
    public Dropdown mTagDropdowm;
    public Text mNumber;
    public Text WordText;
    public Text TransTaleText;
    public Text SentenceText;


    [Header("Default")]

    public GameObject mTriggerEventAera;

    private Vector2 mTriggerEventAeraDefaultSize = new Vector2(880, 500);

    // //單字顯示實體腳本
    // private string wordText;
    // private string translate;
    // private string sentenceContext;
    // private string Remark;//後台的註記

    private WordCardFunc mFunc;

    public WordData aData;


    public void Init(WordData _wordCard, WordCardFunc _func)//mNm 非單字編號,比較像是單字順序固定1~50,非辨識用
    {

        aData = _wordCard;
        mFunc = _func;
        SetTagChoose();
        UIUpdate(mFunc.mNum);
        SetButtonSetting();
    }
    private void UIUpdate(int mNum)
    {
        mNumber.text = "No." + mNum;//mnum是字卡順序，非唯一識別編號
        WordText.text = aData.wordText;
        TransTaleText.text = aData.translate;
        SentenceText.text = "例句:" + aData.sentenceContext;
        bool isHaveTag = false;
        for (int i = 0; i < mTagDropdowm.options.Count; i++)
        {//更新tag至data的tag
            if (mTagDropdowm.options[i].text == aData.mTag)
            {
                mTagDropdowm.value = i;
                isHaveTag = true;
            }
        }
        if (!isHaveTag)
        {//未找到對應標籤(可能是被刪除了)
            mTagDropdowm.value = 0;
            aData.mTag=mTagDropdowm.options[0].text;
        }
    }

    private void SetButtonSetting()
    {
        mTriggerEventAera.GetComponent<RectTransform>().sizeDelta = mTriggerEventAeraDefaultSize;
        mTagDropdowm.onValueChanged.AddListener(ClickTag);
        // DeleteToggle.onValueChanged.AddListener(ChoseDeleteThisCard);

    }

    private void SetTagChoose()
    {

        DataManager.Instance.saveData.Classification.ForEach(str =>
        {
            mTagDropdowm.options.Add(new Dropdown.OptionData(str));
        });
    }

    public void ClickTag(int a)//更換標籤
    {//切換標籤
        aData.mTag = mTagDropdowm.options[a].text;

        //mTag.options.Add()
        Debug.Log("以切換" + aData.wordText + "文字標籤至" + aData.mTag);
    }

    public void ReSetNumList()
    {
        mNumber.text = "No." + (transform.GetSiblingIndex()+1);//mnum是字卡順序，非唯一識別編號
    }
    public void ClickInfoButton()//開啟字卡詳細資訊
    {

        Debug.Log("點擊了" + aData.wordText + "編號==>" + aData.wordNum);

    }


    public void IsDragButtonEffect(bool isDrag)//增加判定範圍
    {

        RectTransform mTri = mTriggerEventAera.GetComponent<RectTransform>();

        if (isDrag)
        {
            mTri.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 500);
        }
        else
        {
            mTri.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 450);
        }
    }


    private bool Ani = false;
    public void ChoseDeleteThisCard()
    {
        if (Ani)
        {
            Debug.Log("正在動畫中，無法回調");
            return;
        }
        Ani = true;

        if (!DeleteToggle.isOn)
        {
            //切換成紅色代表以選
            DeleteToggle.isOn = true;
            mFunc.AddToControlTempList(this);
            StartCoroutine(TweenAniManager.ColorOrTransparentChange(DeleteToggle.GetComponent<Image>(), Color.red, () => { Debug.Log("已完成紅色框"); Ani = false; }));
        }
        else
        {
            DeleteToggle.isOn = false;
            mFunc.RemoveToControlTempList(this);
            StartCoroutine(TweenAniManager.ColorOrTransparentChange(DeleteToggle.GetComponent<Image>(), Color.gray, () => { Ani = false; }));
        }
    }

    public void OpenDeleteModel()//字卡開啟刪除模式
    {
        DeleteToggle.gameObject.SetActive(true);
        wordInfoButton.gameObject.SetActive(false);
        DeleteToggle.GetComponent<Image>().color = Color.gray;
        StautsRepeat();


    }
    public void CloseDeleteModel()//字卡關閉刪除模式
    {

        DeleteToggle.gameObject.SetActive(false);
        wordInfoButton.gameObject.SetActive(true);
        DeleteToggle.GetComponent<Image>().color = Color.gray;
        StautsRepeat();
    }

    private void StautsRepeat()
    {//狀態重製
        Ani = false;
        DeleteToggle.transform.DOKill();
    }

    sealed public class WordCardFunc
    {
        public Action<WordCard> AddToControlTempList;//將自己wordcard加到controlManager的臨時陣列

        public Action<WordCard> RemoveToControlTempList;//將自己wordcard移除在controlManager上的臨時陣列

        public int mNum = 0;//單字的順序,非唯一識別編號
        public WordCardFunc(int _mNum, Action<WordCard> _AddToControlTempList, Action<WordCard> _RemoveToControlTempList)
        {
            mNum = _mNum;
            AddToControlTempList = _AddToControlTempList;
            RemoveToControlTempList = _RemoveToControlTempList;
        }
    }

}
