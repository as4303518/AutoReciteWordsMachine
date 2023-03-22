using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;


//專門生成翻譯的json檔案,掛上這個腳本。
// 1.選擇你的語言，並在陣列上選擇要翻譯的關鍵字串，並輸入要顯示的翻譯
//2.可刪除，因為要設置細節項目，所以必須掛在實體上調整

public class LanguageDataSetting : EditorWindow
{
    public Language MyLanguage = Language.Chinese;

    public LanguageSave SaveData;

    private string _Path = "/Data/Language/{0}.json";
    private static string _FileName = "{0}.json";

    /////////////
    private SerializedObject main;
    private SerializedProperty Sp;
    private Vector2 ScrollView = Vector2.zero;


    [MenuItem("Uframework/gui查看")]

    public static void InitWindow()
    {
        EditorWindow.GetWindow(typeof(LanguageDataSetting));


    }

    private void OnEnable(){
         main = new SerializedObject(this);
         Sp = main.FindProperty("SaveData");
    }
    void OnGUI()
    {
        main.Update();
        MyLanguage = (Language)EditorGUILayout.EnumPopup(MyLanguage);

        ScrollView = GUILayout.BeginScrollView(ScrollView);

        //EditorGUILayout.ObjectField("", SaveData.GetType());
        
        EditorGUILayout.PropertyField(Sp);
        // main.ApplyModifiedProperties();
        GUILayout.EndScrollView();


        if (GUILayout.Button("myTest"))
        {
            Debug.Log("完全覆寫");
            SetJsonToPath();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("overrideTest"))
        {
            Debug.Log("覆寫有的參數");
            ReviseJsonFile();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("ReadTest"))
        {
            Debug.Log("讀取有的檔案");
            OverrideSaveDataOfJson();
        }
        
        main.ApplyModifiedProperties();
        //SceneView.RepaintAll();

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
    English

}
[Serializable]
sealed public class LanguageData
{
    public MyLabel label;
    //public string OrigineText;
    public string TransTaleText;

}




