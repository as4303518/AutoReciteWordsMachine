using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ListControlManager : InstanceScript<ListControlManager>, PrefabScene
{
    [SerializeField]
    private List<ListCard> CardList = new List<ListCard>();
    [SerializeField]
    private List<ListCard> DeleteCardList = new List<ListCard>();

    private ManagerStatus ControlModel;

    public GameObject gViewPanel;//字卡陣列母物件
    [Header("ButtonUI")]
    public Button ButtonCreateList;
    public Button ButtonDelete;
    public Button ButtonExam;

    public Button isFinishWordButton;
    public Button Setting;

    public Button ButtonBack;

    public Button LeftMenuButton;

    public GameObject BelowMenu;//左邊的選單列表

    [Header("Prefabs")]
    public GameObject gListObject;





    public IEnumerator Init(BaseData baseData)
    {
        ControlModel = ManagerStatus.Normal;
        CreateListOfSaveData();
        ButtonSetting();
        yield return null;
        // yield return PageTweenIn();
    }



    private void ButtonSetting()
    {

        ButtonCreateList.onClick.AddListener(ClickNewListButton);
        LeftMenuButton.onClick.AddListener(ClickLiftMenuButton);


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
                RemoveToDeleteCardList
        );
        return func;
    }


    public void ClickNewListButton()//開啟單字組陣列取名字的視窗
    {
        CancelDeleteModel();
        if (BelowMenuExpand)
        {//如果左側選單是開的，但已經按下功能按鈕，則關閉選單
            ClickLiftMenuButton();
        }
        StartCoroutine(PopupManager.Instance.OpenInputStringOneCurrectButtonWindow("請輸入群組名稱", CreateListObject));
    }

    private bool BelowMenuAni = false;
    private bool BelowMenuExpand = false;

    public void ClickLiftMenuButton()//點擊選單列表
    {
        StartCoroutine(ToggleLeftMenu());

    }

    private IEnumerator ToggleLeftMenu()
    {

        if (BelowMenuAni) yield break;
        BelowMenuAni = true;

        if (BelowMenuExpand)
        {

            BelowMenuExpand = false;
            yield return BelowMenu.transform.DOLocalMoveY(-1110, 0.2f).WaitForCompletion();
            BelowMenuAni = false;

        }
        else
        {
            BelowMenuExpand = true;
            yield return BelowMenu.transform.DOLocalMoveY(-810, 0.2f).WaitForCompletion();
            BelowMenuAni = false;

        }

    }


    //以下為刪除功能
    public void ClickDeleteButton()//切換刪除按鈕功能
    {
        switch (ControlModel)
        {
            case ManagerStatus.Invalid:
                break;
            case ManagerStatus.Normal:
                StartCoroutine(TweenAniManager.ColorOrTransparentChange(ButtonDelete.GetComponent<Image>(), Color.red));
                //ButtonDelete.GetComponent<Image>().color = Color.red;
                ControlModel = ManagerStatus.Delete;
                DeleteCardList.Clear();
                CardList.ForEach(v => { v.OpenDeleteModel(); });
                break;
            case ManagerStatus.Delete:
                StartCoroutine(TweenAniManager.ColorOrTransparentChange(ButtonDelete.GetComponent<Image>(), Color.black));
                // ButtonDelete.GetComponent<Image>().color = Color.black;
                ControlModel = ManagerStatus.Normal;
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
        ControlModel = ManagerStatus.Normal;
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
        Debug.Log("離去單字組列表");
        yield return PopupManager.Instance.OpenLoading();
        yield return TweenAniManager.TransparentOutGroup(transform.gameObject);
        //Destroy(this.gameObject);
    }

    public IEnumerator PageTweenIn()
    {//轉場去其他頁面
        Debug.Log("進入單字組列表");
        yield return TweenAniManager.TransparentInGroup(transform.gameObject);
        yield return PopupManager.Instance.CloseLoading();

    }


}

// enum eListControlModel
// {
//     Invalid,//切換場景的過場無法使用功能
//     Normal,//切換場景的過場
//     Delete,//刪除模式，可以刪除list
//     Drop//拖曳字卡

// }
