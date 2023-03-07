using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System;
public class PopupManager : InstanceScript<PopupManager>
{

    public GameObject FilterParentCanvas = null;
    public GameObject LoadingLayer = null;
    public GameObject Filter = null;
    private GameObject LoadingPopupPrefab = null;
    private GameObject LoadingPopup = null;

    private GameObject HintPopup = null;

    private int PopupNum = 0;
    public Dictionary<int, GameObject> PopupList = new Dictionary<int, GameObject>();

    public void Awake()
    {
        DontDestoryThis();
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {

        LoadingLayer = GameObject.Find("LoadingLayer");

        FilterParentCanvas = GameObject.Find("PopupLayer");

        ResourceRequest resRe = Resources.LoadAsync<GameObject>("Prefabs/Popup/LoadingScreen");

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



    public GameObject OpenPopup(GameObject _popup, Action _callBack = null, bool DefaultCloseFilter = true)
    {


        GameObject sp = Instantiate(_popup);

        GameObject filter = Instantiate(Filter, FilterParentCanvas.transform);

        filter.GetComponent<FilterScript>().Init(PopupNum, ClosePopup, DefaultCloseFilter);

        sp.transform.SetParent(filter.transform);
        filter.transform.localScale = Vector3.one;
        sp.transform.localScale = Vector3.one;

        PopupList.Add(PopupNum, filter);

        if (_callBack != null)
        {
            filter.GetComponent<FilterScript>().SetButtonFunc(_callBack);
        }

        PopupNum++;
        return sp;
    }

    public void OpenInputStringOneCurrectButtonWindow(Action<string> _correctButton, Action _filterCallBack = null)
    {
        InputStringWindowPopup sp = OpenPopup
        (Resources.Load<GameObject>(ResourcesPath.PopupWindowPath + "InputStringWindowPopup"), _filterCallBack)
        .GetComponent<InputStringWindowPopup>();
        sp.ReturnList += _correctButton;
        sp.Init("請輸入群組名稱");


    }

    public void OpenHintOnlyStringWindow(string title, string Content)//提示視窗，只有文字(固定時間消失)
    {
        HintPopup sp = OpenPopup(Resources.Load<GameObject>(ResourcesPath.PopupWindowPath + "HintWindowPopup"))
        .GetComponent<HintPopup>();

        sp.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        sp.Init(title, Content);
        Destroy(sp.transform.parent.gameObject, 2);

    }

    public IEnumerator OpenLoading()
    {
        if (LoadingPopup == null)
        {
            LoadingPopup = Instantiate<GameObject>(LoadingPopupPrefab, LoadingLayer.transform);

            StartCoroutine(TweenAniManager.TransparentChange(LoadingPopup.GetComponent<Image>(),new Color(0,0,0,0),new Color(0,0,0,0.4f),0.3f));

            StartCoroutine(TweenAniManager.TransparentChange(LoadingPopup.transform.Find("Text").GetComponent<Text>(),new Color(1,1,1,0),new Color(1,1,1,1),0.3f));

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

            StartCoroutine(TweenAniManager.TransparentChange(LoadingPopup.GetComponent<Image>(),new Color(0,0,0,0.4f),new Color(0,0,0,0),0.3f));

            StartCoroutine(TweenAniManager.TransparentChange(LoadingPopup.transform.Find("Text").GetComponent<Text>(),new Color(1,1,1,1),new Color(1,1,1,0),0.3f));

            yield return new WaitForSeconds(0.3f);
            Destroy(LoadingPopup);
            LoadingPopup = null;
        }
        else
        {
            Debug.Log("沒有讀取畫面");
        }

    }

    //Loading畫面
    //擋住不讓玩家操作透明遮罩

    public void ClosePopup(int filterNum)
    {
        Destroy(PopupList[filterNum]);
        PopupList.Remove(filterNum);

    }

    public void ClosePopup(GameObject filter)
    {
        int filterNum = filter.GetComponent<FilterScript>().FilterNum;
        Destroy(PopupList[filterNum]);
        PopupList.Remove(filterNum);
    }




}
