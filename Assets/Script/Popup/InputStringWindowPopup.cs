using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InputStringWindowPopup : MonoBehaviour
{

    public Text mInputString;

    public Button mCorrectButton;


    public Action<string> ReturnList;

    public Text mTitle;

    public void Init(string _title)
    {

        mTitle.text = _title;

    }

    public void ClickCorrect()
    {
        if (DataManager.Instance.saveData.CheckListTitleRepeat(mInputString.text))
        {
            PopupManager.Instance.OpenHintOnlyStringWindow("創建失敗!","不適用或重複的標題的名字");
        }
        else
        {
            ReturnList(mInputString.text);
            transform.parent.GetComponent<FilterScript>().CloseFilter();
        }


    }




}
