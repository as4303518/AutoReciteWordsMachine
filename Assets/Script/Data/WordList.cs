using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//[System.Serializable]
[Serializable]
public class WordList
{
    //單字列表裡的管理員，負責顯示這個列表所有的單字數據
    public string mTitle;

    public string mFoundingTime { get; private set; }
    public string mLastOpenTime { get; private set; }


    public int mListNum { get; private set; }
    public List<Word> mWords = new List<Word>();


    public WordList(string _title, int _listNum)
    {
        
        mTitle = _title;
        mListNum = _listNum;
        mFoundingTime = DateTime.Now.ToShortDateString();
    }

    public void SetLastTime(string _lastTime)
    {
        mLastOpenTime = _lastTime;

    }
    public int WordsCountOfList()
    {
        return mWords.Count;
    }

}


