using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Word : MonoBehaviour
{

    // 一個單字的數據
    public string context;
    public string translate;
    public string sentenceContext;
    public string sentenceTranslate;


    private int answerFrequency;
    private int CorrectFrequency;
    public int wordNum { get; private set; }
    public string saveTime { get; private set; }

    public Word(string _context, string _translate, string _saveTime, int _wordNum, string _sentenceContext = null, string _sentenceTranslate = null)
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
