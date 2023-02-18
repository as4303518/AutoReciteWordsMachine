using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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



    private void CreateListObject(string _title)
    {
        GameObject sp = Instantiate(gListObject);
        sp.transform.SetParent(gViewPanel.transform);
        sp.transform.localScale = new Vector3(1, 1, 1);
        CardList.Add(sp.GetComponent<ListCard>());
        sp.GetComponent<ListCard>().Init(DataManager.instance.saveData.AddNewList(_title)
        , AddToDeleteCardList
        , RemoveToDeleteCardList);

    }

    public void ClickNewListButton()
    {
        PopupManager.instance.OpenInputStringOneCurrectButtonWindow(CreateListObject);
    }
    public void ClickDeleteButton()
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
                CardList.ForEach(v => { v.CloseDeleteModel(); });
                break;
        }

    }

    private void RemoveCardListDatafoAll()
    {
        // CardList.ForEach(cv=>{
        //     DeleteCardList.ForEach(dv=>{

        //         Destroy(dv);
        //         });

        // });
        DeleteCardList.ForEach(dv =>
        {
            DataManager.instance.saveData.myLists.Remove(dv.aData.mListNum);
            CardList.Remove(dv);
            Destroy(dv.gameObject);
        });
        
        //DataManager.instance.saveData.TraverseMyLists("刪除後");

    }

    public void AddToDeleteCardList(ListCard _listCard)
    {
        DeleteCardList.Add(_listCard);
    }

    public void RemoveToDeleteCardList(ListCard _listCard)
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
