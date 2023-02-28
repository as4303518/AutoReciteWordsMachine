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
        if (DataManager.instance.saveData.CheckListTitleRepeat(mInputString.text))
        {
            PopupManager.instance.OpenHintOnlyStringWindow("創建失敗!","你已擁有相同主題的群組");
        }
        else
        {
            ReturnList(mInputString.text);
            transform.parent.GetComponent<FilterScript>().CloseFilter();
        }


    }




}
