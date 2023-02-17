using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : InstanceScript<DataManager>
{

    public SaveData saveData;

    [SerializeField] private int CurrentListNum;//紀錄現在所在的list,儲存這個號碼之後只要在savedata dic裡面撈

    void Awake()
    {
        DontDestoryThisGame();
    }

    void Start()
    {
        saveData = new SaveData();
        saveData.AddNewList("第一課");
    }

    // Update is called once per frame
  

    public void SetSaveData()
    {
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(saveData));

    }

    public void GetSaveData()
    {
        saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
    }

      void OnApplicationQuit()
    {
        SetSaveData();
    }

}

[System.Serializable]
public class SaveData
{
    [SerializeField] private int ListCount = 0;

    private int LearnWordCount=0;
    

    //[SerializeField]private List<WordList> myList=new List<WordList>();

    [SerializeField] private Dictionary<int, WordList> myLists = new Dictionary<int, WordList>();



    public WordList AddNewList(string _title)
    {
        //myList.Add(new WordList(_title,ListCount));
        //根據輸入文字來設定title
        myLists.Add(ListCount, new WordList(_title, ListCount));
        ListCount++;
        return myLists[ListCount-1];
    }


}
// void Start()
// {
//     Debug.Log(DateTime.Now.ToShortDateString());
//     Debug.Log("自動背單字機發動 AutoReciteWordsMachine Start");
//     //int timeStamp = Convert.ToInt32(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

// }
// Start is called before the first frame update