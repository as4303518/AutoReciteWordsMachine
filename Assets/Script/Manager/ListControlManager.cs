using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ListControlManager : InstanceScript<ListControlManager>
{
    public GameObject MainScene;
    [SerializeField]
    private List<ListCard> CardList = new List<ListCard>();
    [SerializeField]
    private List<ListCard> DeleteCardList = new List<ListCard>();

    private eListControlModel ControlModel;

    public GameObject gViewPanel;
    [Header("ButtonUI")]
    public Button ButtonDelete;
    public Button ButtonCreateList;
    public Button ButtonBack;
    public Button ButtonExam;
    public GameObject gListObject;

    public GameObject LiftMenu;//左邊的選單列表



    private void Awake()
    {
        MonoScript();
        ControlModel = eListControlModel.Normal;
    }

    public void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        CreateListOfSaveData();
        yield return PageTweenIn();
    }



    private void CreateListObject(string _title)//新增單字組
    {
        GameObject sp = Instantiate(gListObject);

        sp.transform.SetParent(gViewPanel.transform);

        sp.transform.localScale = new Vector3(1, 1, 1);

        CardList.Add(sp.GetComponent<ListCard>());

        sp.GetComponent<ListCard>().Init(DataManager.Instance.saveData.AddNewList(_title), CreateListCardFunc());
    }

    private void CreateListOfSaveData()//從savedata新增單字組
    {

        foreach (KeyValuePair<int, WordListData> Dic in DataManager.Instance.saveData.myLists)
        {
            GameObject sp = Instantiate(gListObject);

            sp.transform.SetParent(gViewPanel.transform);

            sp.transform.localScale = new Vector3(1, 1, 1);
            CardList.Add(sp.GetComponent<ListCard>());
            sp.GetComponent<ListCard>().Init(Dic.Value, CreateListCardFunc());
        }
    }

    private ListCard.ListCardFunc CreateListCardFunc()//將控制中央面板的方法，覆值在每個cardList上
    {

        ListCard.ListCardFunc func = new ListCard.ListCardFunc(
                AddToDeleteCardList,
                RemoveToDeleteCardList,
                PageTweenOut,
                PageTweenIn
        );
        return func;
    }


    public void ClickNewListButton()//開啟取名字的視窗
    {
        CancelDeleteModel();
        PopupManager.Instance.OpenInputStringOneCurrectButtonWindow(CreateListObject);
    }

    private bool LiftMenuAni = false;
    private bool LiftMenuExpand = false;

    public void ClickLiftMenuButton()//點擊選單列表
    {
        if (LiftMenuAni) return;

        if (LiftMenuExpand)
        {
            LiftMenu.transform.DOLocalMoveX(0, 0.3f);
            LiftMenuExpand = false;
        }
        else
        {
            LiftMenu.transform.DOLocalMoveX(670, 0.3f);
            LiftMenuExpand = true;
        }

    }


    //以下為刪除功能
    public void ClickDeleteButton()//切換刪除按鈕功能
    {
        switch (ControlModel)
        {
            case eListControlModel.Invalid:
                break;
            case eListControlModel.Normal:
                ButtonDelete.GetComponent<Image>().color = Color.red;
                ControlModel = eListControlModel.Delete;
                DeleteCardList.Clear();
                CardList.ForEach(v => { v.OpenDeleteModel(); });
                break;
            case eListControlModel.Delete:
                ButtonDelete.GetComponent<Image>().color = Color.black;
                ControlModel = eListControlModel.Normal;
                RemoveCardListDatafoAll();
                //刪除那些被移除的list並從新整理Datamanager
                DeleteCardList.Clear();
                CardList.ForEach(v => { v.CloseDeleteModel(); });//恢復平常模式
                break;
        }

    }

    private void CancelDeleteModel()
    {
        // DeleteCardList.ForEach(v => { v.ToggleChooseDelete.isOn = false; });

        ButtonDelete.GetComponent<Image>().color = Color.black;
        ControlModel = eListControlModel.Normal;
        CardList.ForEach(v =>
        {
            v.CloseDeleteModel();
            v.ToggleChooseDelete.isOn = false;
        });//恢復平常模式
        DeleteCardList.Clear();
    }

    private void RemoveCardListDatafoAll()//刪除卡牌
    {

        DeleteCardList.ForEach(dv =>
        {
            DataManager.Instance.saveData.myLists.Remove(dv.aData.mListNum);
            CardList.Remove(dv);
            Destroy(dv.gameObject);
        });

    }

    public void AddToDeleteCardList(ListCard _listCard)//將卡牌新增到刪除列表
    {
        DeleteCardList.Add(_listCard);
    }

    public void RemoveToDeleteCardList(ListCard _listCard)//將卡牌移除刪除列表
    {
        DeleteCardList.Remove(_listCard);
    }

    public IEnumerator PageTweenOut()
    {//轉場去其他頁面
        Debug.Log("離去單字組");
        yield return PopupManager.Instance.OpenLoading();
        yield return TweenAniManager.TransparentOutGroup(MainScene);

    }

    public IEnumerator PageTweenIn()
    {//轉場去其他頁面
        Debug.Log("進入單字組");
        yield return TweenAniManager.TransparentInGroup(MainScene);
        PopupManager.Instance.CloseLoading();

    }





}

enum eListControlModel
{
    Invalid,//切換場景的過場無法使用功能
    Normal,//切換場景的過場
    Delete,//刪除模式，可以刪除list

}
