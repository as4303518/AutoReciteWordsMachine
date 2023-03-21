using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;

//遊戲執行的翻譯檔
public class LanguageTranstale : InstanceScript<LanguageTranstale>
{

    public Language MyLanguage = Language.Chinese;

    public LanguageSave mLanguageSaveData = new LanguageSave();

    private string _Path = "/Data/Language/{0}.json";

    private static string _FileName = "{0}.json";
    //根據語言獲取翻譯名稱獲取文本
    public override IEnumerator Init()
    {
        GetTextList();
        yield return null;

    }

    private void GetTextList()
    {//根據語言類型獲取翻譯檔案
        mLanguageSaveData = new LanguageSave();//重製文檔案,這樣就可以隨時更換語言，不需要經過初始化
        MyLanguage = DataManager.Instance.saveData.MySettingLanguage;
        _Path = string.Format(ResourcesPath.LanguagePath + _FileName, MyLanguage);

        if (File.Exists(Application.dataPath + _Path))
        {

            mLanguageSaveData = JsonUtility.FromJson<LanguageSave>(File.ReadAllText(Application.dataPath + _Path));

        }
        else
        {
            Debug.Log("該翻譯檔不存在");
        }


    }

    public string GetStr(MyLabel label)//顯示當下翻譯對應的文字
    {
        IEnumerable<LanguageData> data=mLanguageSaveData.MyText.Where(v => v.label == label);
        if(data.Any(v=>v.label==label)){
            return data.FirstOrDefault().TransTaleText;
        }else{
            Debug.Log("在"+MyLanguage+"的翻譯下無"+label+"的翻譯文字");
            return "";
        }

    }

}

public enum MyLabel     //紀錄每個語言切換的關鍵字
{
    Back,
    Start,
    Loading,
    SureCloseWindowTitle,
    SureCloseWindowTip,

}
