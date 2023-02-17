using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListCard : MonoBehaviour
{
    //開啟與關閉坐在title上，進入單字選單做在打開後的物件上
    public bool expand = false;

    public GameObject expandObject;

    public Button title;

    public Text titleText;
    public int listNum;

    public void Init(WordList _list)
    {
        titleText.text = _list.title;
        listNum=_list.listNum;


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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
