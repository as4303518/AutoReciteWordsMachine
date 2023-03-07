using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WordCard : MonoBehaviour
{

    [Header("UI")]
    public Button wordInfoButton;
    public Dropdown mTagDropdowm;
    public Text mNumber;
    public Text WordTitleText;
    public Text TransTaleText;
    public Text SentenceText;


    [Header("Default")]

    //單字顯示實體腳本
    private string context;
    private string translate;
    private string sentenceContext;
    private string Remark;

    
    public WordData aData;


    void Init(WordData _word)
    {
        mTagDropdowm.onValueChanged.AddListener(ClickTag);

        aData = _word;
        context = aData.context;
        translate = aData.translate;
        sentenceContext = aData.sentenceContext;
        Remark = aData.sentenceTranslate;
    }

    private void ClickTag(int a)
    {//切換標籤
        aData.mTag = mTagDropdowm.options[a].text;
        //mTag.options.Add()
        Debug.Log("以切換" + context + "文字標籤至" + aData.mTag);
    }



}
