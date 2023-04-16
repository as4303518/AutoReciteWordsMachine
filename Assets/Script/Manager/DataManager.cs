using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManager : InstanceScript<DataManager>
{

    public BaseData mParams;//轉場暫時儲存數據
    public SaveData saveData = new SaveData();


    [SerializeField] private int CurrentListNum;//紀錄現在所在的list,儲存這個號碼之後只要在savedata dic裡面撈

    public override IEnumerator Init()
    {
        GetSaveData();
        yield return null;

    }


    public void SetSaveData()
    {
        Debug.Log("儲存");

        // saveData.SetDicDataToList();

        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(DataManager.Instance.saveData));

    }

    public void GetSaveData()
    {
        Debug.Log("讀取");
        if (PlayerPrefs.GetString("SaveData") != "")
        {
            DataManager.Instance.saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
            //saveData.SetJsonListToDic();

        }

    }

    void OnApplicationQuit()
    {
        SetSaveData();
    }




    [Serializable]
    public class SaveData : BaseData
    {
        [SerializeField] private int ListCount = 0;
        private int LearnWordCount = 0;

        public Language MySettingLanguage;

        public List<string> Classification = new List<string>() { "Non" };//標籤種類

        // public List<WordListData> WordListsOfGroup = new List<WordListData>();
        public WordListDataModle WordListsOfGroup = new WordListDataModle();

        public WordListDataModle WordListsOfFinishGroup = new WordListDataModle();

        public WordListData AddNewList(WordListDataModle Wldm,string _title)
        {

            WordListData Wld=Wldm.AddNewList(_title,ListCount);
            ListCount++;

            return Wld;
        }



        public bool CheckListTitleRepeat(string InPutTitle)//檢查單字陣列是否有問題
        {

            if (InPutTitle == "" || InPutTitle.Length <= 0)//標題一定要有文字
            {
                return true;
            }


            foreach (var wld in WordListsOfGroup.WordListDatas)
            {
                if (InPutTitle == wld.mTitle)
                {
                    Debug.Log("標題重複");
                    return true;

                }
            }
            return false;
        }

        public bool CheckWordRepeat(string word)//檢查單字有沒有重複
        {
            if (word == "" || word.Length <= 0)//標題一定要有文字
            {
                return true;
            }

            List<string> myWords = new List<string>();

            foreach (WordListData wld in WordListsOfGroup.WordListDatas)
            {
                wld.mWords.ForEach(v =>
                {
                    myWords.Add(v.wordText);
                });
            }
            foreach (string w in myWords)
            {
                if (w == word)
                {
                    Debug.Log("已有單字重複");
                    return true;
                }
            }

            return false;
        }

        public void CoverListData(WordListData wld)
        {
            for (int i = 0; i < WordListsOfGroup.WordListDatas.Count; i++)
            {

                if (WordListsOfGroup.WordListDatas[i].mListNum == wld.mListNum)
                {
                    WordListsOfGroup.WordListDatas[i] = wld;
                }

            }
        }
    }
    [Serializable]
    public class WordListDataModle : BaseData
    {
        public List<WordListData> WordListDatas = new List<WordListData>();
        public WordListData AddNewList(string _title, int ListCount)
        {


            WordListDatas.Add(new WordListData(_title, ListCount));


            return WordListDatas[WordListDatas.Count - 1];
        }


    }
}

