using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoOptionsWindowPopup : PopupWindow
{

    public Text mTitle;

    public Text mContent;

    public Button mCorrectButton;

    public Button mCancelButton;



    private TwoOptionsWindowPopupFunc mFunc;

    public void Init(TwoOptionsWindowPopupFunc _func)
    {
        mFunc = _func;
        UpdateButton();
        UpdateUI();
    }

    private void UpdateButton()
    {

        mCorrectButton.onClick.AddListener(ClickCorrectButton);
        mCancelButton.onClick.AddListener(ClickCancelButton);

    }

    private void UpdateUI()
    {
        mTitle.text=mFunc.mTitle;
        mContent.text=mFunc.mContent;


    }

    private void ClickCorrectButton()
    {

       StartCoroutine( mFunc._CorrectFunc());
    }

    private void ClickCancelButton()
    {

       StartCoroutine(mFunc._CancelFunc(mParentFilter.FilterNum));
    }


    sealed public class TwoOptionsWindowPopupFunc
    {
        public string mTitle;

        public string mContent;
        public Func<IEnumerator> _CorrectFunc;

        public Func<int,IEnumerator> _CancelFunc;
        public TwoOptionsWindowPopupFunc(string title,string content,  Func<IEnumerator> correctFunc, Func<int,IEnumerator> cancelFunc)
        {
            mTitle=title;
            mContent=content;
            _CorrectFunc = correctFunc;
            _CancelFunc = cancelFunc;

        }


    }

}
