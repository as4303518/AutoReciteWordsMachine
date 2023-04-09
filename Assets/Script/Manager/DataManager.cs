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
    public class SaveData
    {
        [SerializeField] private int ListCount = 0;
        private int LearnWordCount = 0;

        public Language MySettingLanguage;

        public List<string> Classification=new List<string>(){"Non"};//標籤種類

        public List<WordListData> myLists = new List<WordListData>();

        // public List<int> DicKey = new List<int>();
        // public List<WordListData> DicValue = new List<WordListData>();

        //public List<WordListData> myList = new List<WordListData>();
        //接著要做顯示，顯示之前儲存的資料,這樣就可以做到刪除與顯示正確
        public WordListData AddNewList(string _title)
        {


            myLists.Add(new WordListData(_title, ListCount));
            ListCount++;

            return myLists[myLists.Count - 1];
        }

        

        public bool CheckListTitleRepeat(string InPutTitle)//檢查單字陣列是否有問題
        {

            if (InPutTitle == "" || InPutTitle.Length <= 0)//標題一定要有文字
            {
                return true;
            }


            foreach (var wld in myLists)
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

            foreach (WordListData wld in myLists)
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
            for(int i=0;i<myLists.Count;i++){

                if(myLists[i].mListNum==wld.mListNum){
                    myLists[i]=wld;
                }

            }
        }

        // public void SetDicDataToList()//將dic的值分開key跟value儲存，這樣才能儲存進json
        // {
        //     ClearSaveDataInSaveData();
        //     DicKey = myLists.Keys.ToList();
        //     DicValue = myLists.Values.ToList();
        //     //ClearSaveDataInSaveData();

        // }
        // public void SetJsonListToDic()
        // {
        //     int Count = 0;
        //     foreach (int i in DicKey)
        //     {
        //         myLists.Add(i, DicValue[Count]);
        //         Count++;
        //     }

        // }

        // private void ClearSaveDataInSaveData()
        // {
        //     DicKey.Clear();
        //     DicValue.Clear();

        // }


    }
}

