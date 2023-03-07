using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//[System.Serializable]
[Serializable]
public class WordListData:BaseData
{
    //單字列表裡的管理員，負責顯示這個列表所有的單字數據
    public string mTitle;//標題

    public string mFoundingTime ;//成立時間
    public string mLastOpenTime ;//最後登入

    
    public int mListNum ;//list編號
    public List<WordData> mWords = new List<WordData>();//裡面擁有的單字


    public WordListData(string _title, int _listNum)
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


