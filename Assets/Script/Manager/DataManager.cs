using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManager : InstanceScript<DataManager>
{

    public SaveData saveData = new SaveData();

    [SerializeField] private int CurrentListNum;//紀錄現在所在的list,儲存這個號碼之後只要在savedata dic裡面撈

    void Awake()
    {
        DontDestoryThis();
        GetSaveData();
    }
    public bool abc = false;
    public bool change = true;
    void Start()
    {
        // // saveData = new SaveData();
        // // saveData.AddNewList("第一課");
        // if (PlayerPrefs.GetString("SaveData") != "")
        // {
        //     saveData.TraverseMyLists("初始化");
        // }


    }



    // Update is called once per frame


    public void SetSaveData()
    {
        Debug.Log("儲存");

        saveData.SetDicDataToList();

        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(DataManager.instance.saveData));

    }

    public void GetSaveData()
    {
        Debug.Log("讀取");
        if (PlayerPrefs.GetString("SaveData") != "")
        {

            DataManager.instance.saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
            saveData.SetJsonListToDic();

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

        public Dictionary<int, WordListData> myLists = new Dictionary<int, WordListData>();

        public List<int> DicKey = new List<int>();
        public List<WordListData> DicValue = new List<WordListData>();

        //public List<WordListData> myList = new List<WordListData>();
        //接著要做顯示，顯示之前儲存的資料,這樣就可以做到刪除與顯示正確
        public WordListData AddNewList(string _title)
        {
            //myList.Add(new WordList(_title,ListCount));
            //根據輸入文字來設定title

            myLists.Add(ListCount, new WordListData(_title, ListCount));
            ListCount++;
            //TraverseMyLists("建立");
            return myLists[ListCount - 1];
        }

        public bool CheckListTitleRepeat(string InPutTitle){
            bool Repeat=false;
            foreach(var Dic in myLists){
                if(InPutTitle==Dic.Value.mTitle){
                    Repeat=true;
                    Debug.Log("標題重複");
                }
            }
            return Repeat;
        }
        // public void TraverseMyLists(string _purpose)
        // {
        //     Debug.Log("執行字典遍歷" + ListCount + "數量===>" + myLists.Count);

        //     foreach (var dic in myLists)
        //     {
        //         Debug.Log(_purpose + "鑰匙==>" + dic.Key + "值==>" + dic.Value.mTitle);
        //     }

        // }

        public void SetDicDataToList()
        {//這樣才能儲存進json
            ClearSaveDataInSaveData();
            DicKey = myLists.Keys.ToList();
            DicValue = myLists.Values.ToList();
            //ClearSaveDataInSaveData();

        }
        public void SetJsonListToDic()
        {
            int Count = 0;
            foreach (int i in DicKey)
            {
                myLists.Add(i, DicValue[Count]);
                Count++;
            }

        }

        private void ClearSaveDataInSaveData()
        {
            DicKey.Clear();
            DicValue.Clear();

        }


    }
}





// void Start()
// {
//     Debug.Log(DateTime.Now.ToShortDateString());
//     Debug.Log("自動背單字機發動 AutoReciteWordsMachine Start");
//     //int timeStamp = Convert.ToInt32(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

// }
// Start is called before the first frame update