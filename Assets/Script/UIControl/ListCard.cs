using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListCard : MonoBehaviour
{
    //list實體化腳本
    //開啟與關閉坐在title上，進入單字選單做在打開後的物件上
    public bool expand = false;

    public GameObject expandObject;

    public Text WordsCounts;
    public Text FoundingTime;
    public Text LastOpenTime;

    public Button title;
    public Button exam;

    public Text titleText;
    public int listNum;

    public void Init(WordList _list)
    {
        titleText.text = _list.title;
        listNum = _list.listNum;
        WordsCounts.text="單字總量:"+(_list.WordsCountOfList()>0?_list.WordsCountOfList():0);
        FoundingTime.text="創立時間:"+_list.foundingTime;
        LastOpenTime.text="最後進入:"+_list.lastOpenTime;
    }

    public void ClickTitleButton()
    {
        if (expand)
        {
            CLoseList();
        }
        else
        {
            OpenList();
        }
    }

    private void OpenList()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 400);
        expandObject.SetActive(true);
        expand = true;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    private void CLoseList()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, 100);
        expandObject.SetActive(false);
        expand = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = false;
        transform.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }


}
