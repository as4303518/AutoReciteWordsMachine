using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordList : MonoBehaviour
{
    //單字列表裡的管理員，負責顯示這個列表所有的單字數據
    public string title;

    private string foundingTime;
    private string lastOpenTime;

    private int wordsCount=0;

    public int listNum { get; private set; }
    public List<Word> words = new List<Word>();

    public WordList(string _title, int _listNum)
    {
        title=_title;
        listNum=_listNum;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
