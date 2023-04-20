using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System;
using System.Linq;
public class PopupManager : InstanceScript<PopupManager>
{

    public GameObject FilterParentCanvas = null;
    public GameObject LoadingLayer = null;
    public GameObject Filter = null;
    private GameObject LoadingPopupPrefab = null;
    private GameObject LoadingPopup = null;

    private GameObject HintPopup = null;

    private int PopupNum = 0;
    public Dictionary<int, FilterScript> PopupList = new Dictionary<int, FilterScript>();

    // public void Awake()
    // {
    //     DontDestoryThis();
    //     StartCoroutine(Init());
    // }

    public override IEnumerator Init()
    {

        LoadingLayer = GameObject.Find("LoadingLayer");

        FilterParentCanvas = GameObject.Find("PopupLayer");

        Filter = Resources.Load<GameObject>(ResourcesPath.PopupWindowPath + "Common/Filter");

        ResourceRequest resRe = Resources.LoadAsync<GameObject>(ResourcesPath.PopupWindowPath + "Common/LoadingScreen");

        yield return resRe;

        if (resRe.isDone)
        {
            LoadingPopupPrefab = resRe.asset as GameObject;
        }
        else
        {
            Debug.Log("PopupManager資源加載失敗");
        }

    }

    public IEnumerator ReSetLayer()//因為切換場景，所以要重新抓父物件
    {
        LoadingLayer = null;
        FilterParentCanvas = null;
        LoadingLayer = GameObject.Find("LoadingLayer");
        FilterParentCanvas = GameObject.Find("PopupLayer");
        yield return new WaitUntil(() => LoadingLayer != null);
        yield return new WaitUntil(() => FilterParentCanvas != null);
    }



    public GameObject OpenPopup(GameObject _popup, Action _callBack = null, bool DefaultCloseFilter = true, bool _transparent = false)//普通彈窗的開啟與關閉
    {

        GameObject sp = Instantiate(_popup);

        GameObject filter = Instantiate(Filter, FilterParentCanvas.transform);
        if (_transparent) filter.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        FilterScript Fs = filter.GetComponent<FilterScript>();
        Fs.Init(PopupNum, ClosePopup, DefaultCloseFilter);

        sp.transform.SetParent(filter.transform);
        sp.GetComponent<PopupWindow>().mParentFilter = Fs;
        filter.transform.localScale = Vector3.one;
        sp.transform.localScale = Vector3.one;
        sp.transform.localPosition = Vector3.zero;

        PopupList.Add(PopupNum, Fs);

        if (_callBack != null)
        {
            Fs.SetButtonFunc(_callBack);
        }
        //tween in
        // tween out
        PopupNum++;
        return sp;
    }
    public GameObject OpenPopup(GameObject _popup, bool _transparent = false)//用於提示視窗，基本上生成後便不歸popupManager控制
    {
        return OpenPopup(_popup, null, false, _transparent);
    }

    public GameObject OpenPopup(GameObject _popup)//用於提示視窗，基本上生成後便不歸popupManager控制
    {
        return OpenPopup(_popup, null, true, false);
    }

    public void PopupManagerExecute(Func<IEnumerator> ie)
    {
        StartCoroutine(ie());
    }
    //兩個按鈕
    public IEnumerator OpenTipTwoOptionsButtonWindow(string _Title, string _Content, Func<IEnumerator> _Correct, Func<int, IEnumerator> _Cancel, Action _filterCallBack = null, bool DefaultCloseFilter = true)
    {

        ResourceRequest spPrefabs = Resources.LoadAsync<GameObject>(ResourcesPath.PopupWindowPath + "TwoOptionsWindowPopup");

        yield return new WaitUntil(() => spPrefabs.isDone);

        TwoOptionsWindowPopup sp = OpenPopup((spPrefabs.asset as GameObject), _filterCallBack, DefaultCloseFilter, false).GetComponent<TwoOptionsWindowPopup>();

        sp.Init(new TwoOptionsWindowPopup.TwoOptionsWindowPopupFunc(_Title, _Content, _Correct, _Cancel));
        yield return PopupWindowTweenIn(sp.gameObject);
    }


    //一個輸入文字一個確定按鈕
    public IEnumerator OpenInputStringOneCurrectButtonWindow(string tip, Action<string> _correctButton, Action _filterCallBack = null)
    {
        ResourceRequest spPrefabs = Resources.LoadAsync<GameObject>(ResourcesPath.PopupWindowPath + "InputStringWindowPopup");

        yield return new WaitUntil(() => spPrefabs.isDone);


        InputStringWindowPopup sp = OpenPopup((spPrefabs.asset as GameObject), _filterCallBack).GetComponent<InputStringWindowPopup>();

        sp.ReturnList += _correctButton;

        sp.Init(tip);

        yield return PopupWindowTweenIn(sp.gameObject);
    }
    //提示視窗，只有文字(固定時間消失)
    public IEnumerator OpenHintOnlyStringWindow(string title, string Content)
    {
        Debug.Log("顯示提示字元");
        ResourceRequest spPrefabs = Resources.LoadAsync<GameObject>(ResourcesPath.PopupWindowPath + "HintWindowPopup");

        yield return new WaitUntil(() => spPrefabs.isDone);

        HintPopup sp = OpenPopup((spPrefabs.asset as GameObject), true)
        .GetComponent<HintPopup>();

        Vector2 mySize = sp.GetComponent<RectTransform>().sizeDelta;
        sp.mParentFilter.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        sp.mParentFilter.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        sp.mParentFilter.GetComponent<RectTransform>().sizeDelta = mySize;

        // sp.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        sp.Init(title, Content);
        sp.mParentFilter.DestroyThisPopup(2);

        yield return PopupWindowTweenIn(sp.gameObject);
    }
    //創建單字視窗
    public IEnumerator OpenEstablishWordPopup(Action<WordData, int> _correctButton, int cardSquenceNum, int wordCardNum, Action _filterCallBack = null)
    {

        ResourceRequest spPrefabs = Resources.LoadAsync<GameObject>("Prefabs/Scene/WordScene/WordObject/Popup/CreateWordPopup");

        yield return new WaitUntil(() => spPrefabs.isDone);

        CreateWordPopup sp = OpenPopup((spPrefabs.asset as GameObject), _filterCallBack, false, true).GetComponent<CreateWordPopup>();

        sp.Init(wordCardNum, cardSquenceNum, _correctButton, ClosePopup);

        yield return PopupWindowTweenIn(sp.gameObject);
    }

    public IEnumerator OpenMultOptionsScrollViewPopup(GameObject Prefabs, IEnumerable createList, Action<GameObject, int> recb, Action _confirm = null, Action _cancel = null, Action cb = null)
    {

        ResourceRequest spPrefabs = Resources.LoadAsync<GameObject>(ResourcesPath.PopupWindowPath + "MultOptionScrollView/MultOptionScrollViewPopup");

        yield return new WaitUntil(() => spPrefabs.isDone);

        MultOptionScrollViewPopup sp = OpenPopup((spPrefabs.asset as GameObject), cb).GetComponent<MultOptionScrollViewPopup>();
        sp.Init(new MultOptionScrollViewPopup.MultOptionScrollViewPopupFunc()
        {
            ConfirmFunc = _confirm,
            CancelFunc = _cancel,
            ScrollViewList = Prefabs
        });
        int CalcCount = 0;
        foreach (var v in createList)
        {
            Debug.Log("偵錯1===>" + v);
            recb(sp.CreateButtonOfScrollViewList(), CalcCount);
            CalcCount++;
        }

    }


    public IEnumerator OpenLoading()
    {
        if (LoadingPopup == null)
        {
            LoadingPopup = Instantiate<GameObject>(LoadingPopupPrefab, LoadingLayer.transform);

            StartCoroutine(TweenAniManager.ColorOrTransparentChange(LoadingPopup.GetComponent<Image>(), new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.4f)));

            StartCoroutine(TweenAniManager.ColorOrTransparentChange(LoadingPopup.transform.Find("Text").GetComponent<Text>(), new Color(1, 1, 1, 0), new Color(1, 1, 1, 1)));

            yield return new WaitForSeconds(0.3f);
            //yield return TweenAniManager.TransparentInGroup(LoadingPopup,0.3f,false);
        }
        else
        {
            Debug.Log("已開啟讀取畫面");
        }
    }

    public IEnumerator CloseLoading()
    {
        if (LoadingPopup != null)
        {
            // yield return TweenAniManager.TransparentOutGroup(LoadingPopup,0.3f,false);

            StartCoroutine(TweenAniManager.ColorOrTransparentChange(LoadingPopup.GetComponent<Image>(), new Color(0, 0, 0, 0.4f), new Color(0, 0, 0, 0)));

            StartCoroutine(TweenAniManager.ColorOrTransparentChange(LoadingPopup.transform.Find("Text").GetComponent<Text>(), new Color(1, 1, 1, 1), new Color(1, 1, 1, 0)));

            yield return new WaitForSeconds(0.3f);
            Destroy(LoadingPopup);
            LoadingPopup = null;
        }
        else
        {
            Debug.Log("沒有讀取畫面");
        }

    }
    public void DirectOpenLoading()//直接開啟Loading畫面(刪除過場，有些過渡不需要過場)
    {
        if (LoadingPopup == null)
        {
            LoadingPopup = Instantiate<GameObject>(LoadingPopupPrefab, LoadingLayer.transform);
        }
        else
        {
            Debug.Log("已開啟讀取畫面");
        }
    }
    public void DirectCloseLoading()//直接關閉Loading畫面(刪除過場，有些過渡不需要過場)
    {
        if (LoadingPopup != null)
        {
            Destroy(LoadingPopup);
            LoadingPopup = null;
        }
        else
        {
            Debug.Log("沒有讀取畫面");
        }

    }

    // public IEnumerator ShowShield()
    // {//展示屏蔽螢幕  小換場用(如:關閉小視窗等)
    //     yield return null;
    // }

    // public IEnumerator CloseShield()
    // {//展示屏蔽螢幕  小換場用(如:關閉小視窗等)
    //     yield return null;
    // }

    //Loading畫面
    //擋住不讓玩家操作透明遮罩

    public IEnumerator ClosePopup(FilterScript fs)
    {
        yield return ClosePopup(fs.FilterNum);
    }

    public IEnumerator ClosePopup(int filterNum)//isremove怕陣列刪除會影響其他在
    {
        //tween out
        FilterScript fs = PopupList[filterNum];
        fs.gameObject.transform.DOKill();
        yield return PopupWindowTweenOut(fs.GetComponentInChildren<PopupWindow>().gameObject);
        // if (fs.DependWindow.Count > 0)
        // {
        //     for (int i = 0; i < fs.DependWindow.Count; i++)
        //     {
        //         Debug.Log("刪除依賴彈窗" + PopupList[fs.DependWindow[i]].gameObject.name);
        //         Destroy(PopupList[fs.DependWindow[i]].gameObject);
        //         PopupList.Remove(fs.DependWindow[i]);

        //     }
        // }

        Destroy(PopupList[filterNum].gameObject);

        PopupList.Remove(filterNum);


    }

    // public IEnumerator ClosePopup(int filterNum)
    // {
    //    yield return ClosePopup(filterNum ,true);
    // }

    public IEnumerator ClosePopup(GameObject filter)
    {
        int filterNum = 0;
        if (filter.GetComponent<FilterScript>() != null)
        {
            filterNum = filter.GetComponent<FilterScript>().FilterNum;
        }
        else if (filter.GetComponentInParent<FilterScript>() != null)
        {
            filterNum = filter.GetComponentInParent<FilterScript>().FilterNum;
        }
        else
        {
            Debug.Log("未找到可以關閉的filterNum");
        }


        yield return ClosePopup(filterNum);
    }

    public IEnumerator CloseAllPopup()//刪除所有彈窗
    {

        foreach (FilterScript filter in PopupList.Values)
        {
            StartCoroutine(ClosePopup(filter.FilterNum));

        }
        yield return new WaitForSeconds(0.2f);//關閉的時間

    }

    // public IEnumerator CloseLastPopup(){



    // }

    public IEnumerator PopupWindowTweenIn(GameObject TweenObject, float dur = 0.2f)
    {

        TweenObject.transform.localScale = new Vector3(0, 0, 0);
        Sequence sq = DOTween.Sequence();
        sq.Append(TweenObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), (dur * 3 / 5))).Append(TweenObject.transform.DOScale(new Vector3(1, 1, 1), (dur * 2 / 5))).SetEase(Ease.Linear);

        yield return sq.WaitForCompletion();
    }

    private IEnumerator PopupWindowTweenOut(GameObject TweenObject, float dur = 0.2f)
    {

        TweenObject.transform.localScale = new Vector3(1, 1, 1);
        Sequence sq = DOTween.Sequence();
        sq.Append(TweenObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), (dur * 2 / 5))).Append(TweenObject.transform.DOScale(new Vector3(0, 0, 0), (dur * 3 / 5))).SetEase(Ease.Linear);

        yield return sq.WaitForCompletion();
    }


}
