using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class HintPopup : PopupWindow
{

    public Text _Title;
    public Text _Content;

    public void Init(string title, string content)
    {
        _Title.text = title;
        _Content.text = content;


    }

    // public void OnDestory()
    // {

    //     Debug.Log("已刪除視窗");
    //     RemovePopupList(mParentFilter.FilterNum);


    // }


}
