using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : InstanceScript<DataManager>
{

    public SaveData saveData = new SaveData();

    [SerializeField] private int CurrentListNum;//紀錄現在所在的list,儲存這個號碼之後只要在savedata dic裡面撈

    void Awake()
    {
        DontDestoryThis();
        GetSaveData();
    }

    void Start()
    {
        // saveData = new SaveData();
        // saveData.AddNewList("第一課");
        saveData.TraverseMyLists("初始化");
    }

    // Update is called once per frame


    public void SetSaveData()
    {
        Debug.Log("儲存");
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(DataManager.instance.saveData));
    }

    public void GetSaveData()
    {
        Debug.Log("讀取");
        DataManager.instance.saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
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

        public Dictionary<int, WordList> myLists = new Dictionary<int, WordList>();
        
         public List<WordList> myList = new List<WordList>();
         //接著要做顯示，顯示之前儲存的資料,這樣就可以做到刪除與顯示正確


        public WordList AddNewList(string _title)
        {
            //myList.Add(new WordList(_title,ListCount));
            //根據輸入文字來設定title

            myLists.Add(ListCount, new WordList(_title, ListCount));
            ListCount++;
            //TraverseMyLists("建立");
            return myLists[ListCount - 1];
        }

        public void TraverseMyLists(string _purpose)
        {
            Debug.Log("執行字典遍歷" + ListCount);
            foreach (var dic in myLists)
            {
                Debug.Log(_purpose + "鑰匙==>" + dic.Key + "值==>" + dic.Value.mTitle);
            }


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