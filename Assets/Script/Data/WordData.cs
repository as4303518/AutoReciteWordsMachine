using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordData :BaseData
{
    //表面:單字,翻譯,例句,標籤,多功能按鈕,刪除按鈕,單字編號
    //裡面(按鈕裡的功能):單字選項(完成單字，修改單字),單字詳細資訊(考試次數，正確次數，創立日期,備註)
    public string context;
    public string translate;
    public string sentenceContext;
    public string sentenceTranslate;

    public int wordNum { get; private set; }//單字編號
    private int answerFrequency;//考試次數
    private int CorrectFrequency;//正確次數

    public string mTag="";

    public string saveTime { get; private set; }//創立時間

    public WordData(string _context, string _translate, string _saveTime, int _wordNum, string _sentenceContext = null, string _sentenceTranslate = null)
    {
        context = _context;
        translate = _translate;
        saveTime = _saveTime;
        wordNum = _wordNum;
        if (_sentenceContext != null) sentenceContext = _sentenceContext;
        if (_sentenceTranslate != null) sentenceTranslate = _sentenceTranslate;

    }

    public void HaveAnswer()
    {
        answerFrequency += 1;
    }
    public void AnswerCorrent()
    {
        CorrectFrequency += 1;
    }
}
