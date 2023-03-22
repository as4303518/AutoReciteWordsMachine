using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using System.Linq;

//專門生成翻譯的json檔案,掛上這個腳本。
// 1.選擇你的語言，並在陣列上選擇要翻譯的關鍵字串，並輸入要顯示的翻譯
//2.可刪除，因為要設置細節項目，所以必須掛在實體上調整

public class LanguageDataSetting : EditorWindow
{
    public Language MyLanguage = Language.Chinese;
    private Language OldLanguage;//紀錄玩家是否有切換語言
    public LanguageSave SaveData;


    private string _Path = "/Data/Language/{0}.json";
    private static string _FileName = "{0}.json";

    /////////////
    private SerializedObject main;
    private SerializedProperty Sp;
    private Vector2 ScrollView = Vector2.zero;
    private GUIStyle nStyle;

    private string Search = "";

    [MenuItem("Uframework/gui查看")]

    public static void InitWindow()
    {
        EditorWindow.GetWindow(typeof(LanguageDataSetting));


    }

    private void OnEnable()
    {
        main = new SerializedObject(this);
        Sp = main.FindProperty("SaveData");
    }
    void OnGUI()
    {

        nStyle = new GUIStyle("box")
        {

            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,

        };
        main.Update();
        if (OldLanguage != MyLanguage)
        {
            OldLanguage = MyLanguage;
            OverrideSaveDataOfJson();
        }
        MyLanguage = (Language)EditorGUILayout.EnumPopup(MyLanguage);//選擇語言彈窗

        Search = EditorGUILayout.TextField("搜尋", Search, "box", GUILayout.MaxWidth(position.x / 3));//搜尋視窗

        ScrollView = GUILayout.BeginScrollView(ScrollView);//滑塊滾輪

        List<string> tempList = new List<string>();//當前已有的標題

        List<string> DeltempList = new List<string>();//要移除的標題

        List<string> AlltempList = new List<string>();//全部的標題(字串)

        MyLabel[] AllLabel = (MyLabel[])Enum.GetValues(typeof(MyLabel));//全部的標題

        //建立全部的標題字串陣列
        foreach (MyLabel m in AllLabel)
        {
            AlltempList.Add(m.ToString());
        }


        //如果翻譯裡的標題還在陣列範圍裡，加入陣列
        foreach (var mlabel in SaveData.MyText)
        {
            if (AlltempList.Contains(mlabel.label))
            {
                tempList.Add(mlabel.label);
            }
            else
            {
                DeltempList.Add(mlabel.label);
                // SaveData.MyText.Remove(mlabel);
            }

        }

        for (int i = 0; i < AllLabel.Length; i++)
        {

            if (!tempList.Contains(AllLabel[i].ToString()))
            {
                SaveData.MyText.Insert(i, new LanguageData(AllLabel[i].ToString(), ""));
            }
        }


        List<LanguageData> newCorrectList = new List<LanguageData>();
        //有在刪除列表裡，需要刪除
        foreach (var v in SaveData.MyText)
        {
            bool needDel = false;
            foreach (string del in DeltempList)
            {
                if (v.label == del)
                {
                    needDel = true;
                }
            }

            if (!needDel)
            {
                newCorrectList.Add(v);
            }

        }
        SaveData.MyText.Clear();
        SaveData.MyText = newCorrectList;



        foreach (var v in SaveData.MyText)//顯示
        {
            if (v.label.ToString().ToLower().Contains(Search.ToLower()))
            {
                DrawLanguageList(v);
            }
        }

        GUILayout.EndScrollView();


        if (GUILayout.Button("myTest"))
        {
            Debug.Log("完全覆寫");
            SetJsonToPath();
        }
        // GUILayout.Space(10);

        // if (GUILayout.Button("overrideTest"))
        // {
        //     Debug.Log("覆寫有的參數");
        //     ReviseJsonFile();
        // }

        // GUILayout.Space(10);

        // if (GUILayout.Button("ReadTest"))
        // {
        //     Debug.Log("讀取有的檔案");
        //     OverrideSaveDataOfJson();
        // }

        main.ApplyModifiedProperties();


    }

    private void DrawLanguageList(LanguageData data)
    {

        GUILayout.BeginHorizontal("box");
        EditorGUILayout.SelectableLabel(data.label.ToString());

        //  GUILayout.Space(10);
        data.TransTaleText = EditorGUILayout.TextField("翻譯=>", data.TransTaleText, nStyle, GUILayout.MinWidth(position.x / 1.5f), GUILayout.MinHeight(40));
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.Space(10);

    }

    public void SetJsonToPath()//覆蓋檔案
    {


        _Path = string.Format(ResourcesPath.LanguagePath + _FileName, MyLanguage);

        if (!Directory.Exists(Application.dataPath + ResourcesPath.LanguagePath))
        {
            Directory.CreateDirectory(Application.dataPath + ResourcesPath.LanguagePath);//生成資料夾
        }

        File.WriteAllText(Application.dataPath + _Path, JsonUtility.ToJson(SaveData));


    }

    public void ReviseJsonFile()//覆蓋有再選項裡的字，保留沒特別被選的
    {
        _Path = string.Format(ResourcesPath.LanguagePath + _FileName, MyLanguage);
        if (File.Exists(Application.dataPath + _Path))
        {

            LanguageSave tempSave = new LanguageSave();
            tempSave.MyText = JsonUtility.FromJson<LanguageSave>(File.ReadAllText(Application.dataPath + _Path)).MyText;

            // Debug.Log("已經有檔案");
            // List<LanguageData> tempList = JsonUtility.FromJson<LanguageSave>(File.ReadAllText(Application.dataPath + _Path)).MyText;

            foreach (LanguageData v in SaveData.MyText)
            {
                bool isHave = false;
                for (int i = 0; i < tempSave.MyText.Count; i++)
                {
                    if (v.label == tempSave.MyText[i].label)//如果有相同的標題名稱，如 load back gamestart等
                    {
                        tempSave.MyText[i].TransTaleText = v.TransTaleText;//舊的資料換成新的
                        isHave = true;
                    }
                }
                if (!isHave)//沒有的選項才需要加進去
                {

                    tempSave.MyText.Add(v);
                }
            }

            File.WriteAllText(Application.dataPath + _Path, JsonUtility.ToJson(tempSave));

        }
        else
        {
            SetJsonToPath();
        }
    }

    public void OverrideSaveDataOfJson()
    {//將json原有的檔案覆蓋在savedata上，方便修正(不需要重新打或新增選項)
        _Path = string.Format(ResourcesPath.LanguagePath + _FileName, MyLanguage);

        if (File.Exists(Application.dataPath + _Path))
        {

            SaveData.MyText = JsonUtility.FromJson<LanguageSave>(File.ReadAllText(Application.dataPath + _Path)).MyText;

        }
        else
        {
            SaveData.MyText = new List<LanguageData>();
            Debug.Log("並沒有該國翻譯文檔案");
        }

    }


}

[Serializable]
sealed public class LanguageSave
{
    public List<LanguageData> MyText = new List<LanguageData>();
}

public enum Language
{
    Chinese,
    English,
    Japan,
    Franch

}

[Serializable]
sealed public class LanguageData
{
    public string label;//myLabel
    //public string OrigineText;
    public string TransTaleText;
    public LanguageData(string _label, string text)
    {
        label = _label;
        text = TransTaleText;
    }
}




