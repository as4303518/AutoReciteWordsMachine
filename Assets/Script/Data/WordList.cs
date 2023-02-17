using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WordList : MonoBehaviour
{
    //單字列表裡的管理員，負責顯示這個列表所有的單字數據
    public string title;

    public string foundingTime { get; private set; }
    public string lastOpenTime { get; private set; }



    public int listNum { get; private set; }
    public List<Word> words = new List<Word>();

    public WordList(string _title, int _listNum)
    {
        title = _title;
        listNum = _listNum;
        foundingTime = DateTime.Now.ToShortDateString();
    }

    public void SetLastTime(string _lastTime)
    {
        lastOpenTime = _lastTime;

    }
    public int WordsCountOfList()
    {
        return words.Count;
    }


}
