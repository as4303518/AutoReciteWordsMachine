using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InputStringWindowPopup : PopupWindow
{

    public Text mInputString;

    public Button mCorrectButton;


    public Action<string,int> CorrectFunc;

    public Text mTitle;

    public  void Init(string _title,Action<string,int>correctFunc)
    {
        
        mTitle.text = _title;
        CorrectFunc=correctFunc;
        

    }

    public void ClickCorrect()
    {
        if (DataManager.Instance.saveData.WordListsOfGroup.CheckListTitleRepeat(mInputString.text))
        {
           StartCoroutine( PopupManager.Instance.OpenHintOnlyStringWindow("創建失敗!","不適用或重複的標題的名字"));
        }
        else
        {
            CorrectFunc(mInputString.text,mParentFilter.FilterNum);
            // transform.parent.GetComponent<FilterScript>().CloseFilter();
            mParentFilter.CloseFilter();
        }


    }




}
