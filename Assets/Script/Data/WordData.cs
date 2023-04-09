using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WordData :BaseData
{
    //表面:單字,翻譯,例句,標籤,多功能按鈕,刪除按鈕,單字編號
    //裡面(按鈕裡的功能):單字選項(完成單字，修改單字),單字詳細資訊(考試次數，正確次數，創立日期,備註)
    public string wordText;
    public string translate;
    public string sentenceContext;
    public string Remark;//註記

    public int wordNum { get; private set; }//單字編號
    private int answerFrequency;//考試次數
    private int CorrectFrequency;//正確次數

    public string mTag="";//標籤

    public string mFoundingTime { get; private set; }//創立時間

    public WordData(string _context, string _translate,  int _wordNum, string _sentenceContext = "",string _mTag="Non")
    {
        wordText = _context;
        translate = _translate;
        mFoundingTime = DateTime.Now.ToShortDateString();;
        wordNum = _wordNum;
        if (_sentenceContext != null) sentenceContext = _sentenceContext;
        mTag=_mTag;

    }

    public void HaveAnswer()
    {
        answerFrequency += 1;
    }
    public void AnswerCorrent()
    {
        CorrectFrequency += 1;
    }

     public static bool operator ==(WordData a, WordData b)
    {
        Debug.Log("比對了相同");
        return a.wordNum == b.wordNum;
    }

    public static bool operator !=(WordData a, WordData b)
    {
        Debug.Log("比對了不同");
        return a.wordNum != b.wordNum;
    }

    // public static bool operator ==(WordCard a, WordCard b) => a.aData.wordNum == b.aData.wordNum;
    // public static bool operator !=(WordCard a, WordCard b)=>a.aData.wordNum != b.aData.wordNum;
}
