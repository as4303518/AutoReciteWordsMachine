using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ListControlManager : InstanceScript<ListControlManager>
{
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


    private void Awake()
    {

        MonoScript();
        ControlModel = eListControlModel.Normal;
    }

    public void Start()
    {

        CreateListOfSaveData();


    }

    private void CreateListObject(string _title)
    {
        GameObject sp = Instantiate(gListObject);

        sp.transform.SetParent(gViewPanel.transform);

        sp.transform.localScale = new Vector3(1, 1, 1);

        CardList.Add(sp.GetComponent<ListCard>());


        sp.GetComponent<ListCard>().Init(DataManager.instance.saveData.AddNewList(_title),
            new ListCard.ListCardFunc(
            AddToDeleteCardList,
            RemoveToDeleteCardList
            ));
    }

    private void CreateListOfSaveData()
    {//讀取並生成員有資料的字卡

        foreach (KeyValuePair<int, WordListData> Dic in DataManager.instance.saveData.myLists)
        {
            GameObject sp = Instantiate(gListObject);

            sp.transform.SetParent(gViewPanel.transform);

            sp.transform.localScale = new Vector3(1, 1, 1);
            CardList.Add(sp.GetComponent<ListCard>());
            sp.GetComponent<ListCard>().Init(Dic.Value, new ListCard.ListCardFunc(
                AddToDeleteCardList,
                RemoveToDeleteCardList
                ));
        }
    }



    public void ClickNewListButton()
    {
        CancelDeleteModel();
        PopupManager.instance.OpenInputStringOneCurrectButtonWindow(CreateListObject);
    }



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
        CardList.ForEach(v => { 
            v.CloseDeleteModel(); 
            v.ToggleChooseDelete.isOn=false;
            });//恢復平常模式
        DeleteCardList.Clear();
    }

    private void RemoveCardListDatafoAll()//刪除卡牌
    {

        DeleteCardList.ForEach(dv =>
        {
            DataManager.instance.saveData.myLists.Remove(dv.aData.mListNum);
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
}

enum eListControlModel
{
    Invalid,//切換場景的過場無法使用功能
    Normal,//切換場景的過場
    Delete,//刪除模式，可以刪除list

}
