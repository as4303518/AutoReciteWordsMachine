using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MultOptionScrollViewPopup : PopupWindow
{
    // Start is called before the first frame update

    [Header("Parent")]
    public GameObject ScrollViewParent;


    [Header("ButtonList")]
    public List<GameObject> ScrollViewList = new List<GameObject>();

    [Header("Button")]

    public Button Confirm;
    public Button Cancel;

    [Header("Other")]

    public MultOptionScrollViewPopupFunc aData;


    public void Init(MultOptionScrollViewPopupFunc _Data)
    {
        aData = _Data;
        UpdateUI();
        // CreateButtonOfScrollViewList();

    }

    private void SetButtonSetting()
    {
        if (aData.ConfirmFunc != null)
        {
            Confirm.gameObject.SetActive(true);
        }
        else
        {
            Confirm.gameObject.SetActive(false);
        }

        if (aData.CancelFunc != null)
        {
            Cancel.gameObject.SetActive(true);
        }
        else
        {
            Cancel.gameObject.SetActive(false);
        }



    }

    public void UpdateUI()
    {


    }


    public GameObject CreateButtonOfScrollViewList()
    {
        Debug.Log("總執行次數計算!!!!!!!!!!!!!");
        GameObject sp = Instantiate(aData.ScrollViewList, ScrollViewParent.transform);
        ScrollViewList.Add(sp);
        return sp;

    }




    public class MultOptionScrollViewPopupFunc
    {

        [Header("Prefabs")]

        public GameObject ScrollViewList;

        // [Header("Func")]
        // public Action ScrollViewFunc;

        public Action ConfirmFunc = null;

        public Action CancelFunc = null;
    }



}
