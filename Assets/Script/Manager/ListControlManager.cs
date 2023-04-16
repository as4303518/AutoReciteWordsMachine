using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Linq;


public class ListControlManager : InstanceScript<ListControlManager>, PrefabScene
{
    [SerializeField]
    private List<ListCard> CardList = new List<ListCard>();//字母卡陣列

    [SerializeField]
    private List<ListCard> searchCardList = new List<ListCard>();//搜尋的字卡
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

    public Button BelowMenuButton;

    public InputField SearchField;

    public GameObject BelowMenu;//左邊的選單列表

    [Header("Prefabs")]
    public GameObject gListObject;

    /////BaseManager//////
    [HideInInspector] public DragUI mDragManager;

    private bool isDeteDragFunc = false;//偵測拖動中的方法是否有執行

    [Header("Default")]
    private DataManager.WordListDataModle mBaseData;

    public IEnumerator Init(BaseData baseData)
    {
        ControlModel = ManagerStatus.Normal;

        mBaseData = (baseData as DataManager.WordListDataModle);


        mDragManager = GetComponent<DragUI>();

        CreateListOfSaveData();//自己拿的資料存檔
        ButtonSetting();
        yield return null;
        // yield return PageTweenIn();
    }

    private void ButtonSetting()
    {
        ButtonCreateList.onClick.AddListener(ClickNewListButton);
        BelowMenuButton.onClick.AddListener(ClickBelowMenuButton);
        SearchField.onValueChanged.AddListener(SearchOfStr);
        SetDragSettingEvent();
    }

    private void SearchOfStr(string str)//搜尋功能
    {
        Debug.Log("偵測搜尋==>" + str);

        var tempList = CardList.Where(x => x.aData.mTitle.Contains(str));

        CardList.ForEach(c =>
        {
            bool isHaveTemp = false;
            foreach (var card in tempList)
            {

                if (c.aData.mTitle == card.aData.mTitle)
                {

                    c.gameObject.SetActive(true);
                    isHaveTemp = true;

                }
            }
            if (!isHaveTemp)
            {
                c.gameObject.SetActive(false);
            }

        });

    }

    public bool isDrag = false;
    private void SetDragSettingEvent()
    {
        mDragManager.ClearEvent();//重製事件效果，以免其他按鈕拿到不屬於自己的事件

        mDragManager.OnClickCallBack += obj =>
        {

            switch (ControlModel)
            {

                case ManagerStatus.Normal:

                    if (isDrag == false) obj.transform.parent.GetComponent<ListCard>().ClickTitleButton();
                    break;

                case ManagerStatus.Delete:

                    if (isDrag == false) obj.transform.parent.GetComponent<ListCard>().OnToggleChooseDelete();
                    break;

            }


            isDrag = false;//不管有沒有拖曳，都結束
        };

        mDragManager.OnBeginDragCallBack += obj =>
        {
            isDrag = true;
            SetCradTriggerExpand(isDrag);
            StartCoroutine(TweenAniManager.SetTransparentGroup(mDragManager.DragGameObject, 1, 0.5f, 0));

        };

        mDragManager.OnDragCallBack += obj =>
        {
            if (!isDeteDragFunc)
            {
                isDeteDragFunc = true;
                StartCoroutine(DetectViewAngleDown());
            }

        };

        mDragManager.OnEndDragCallBack += obj =>
        {
            StartCoroutine(TweenAniManager.SetTransparentGroup(mDragManager.DragGameObject, 0.5f, 1f, 0));//只要結束拖曳，都回覆顏色
            isDrag = false;
            isDeteDragFunc = false;
            SetCradTriggerExpand(isDrag);

            // if (obj.GetComponent<ListCard>() == null) return;
            if (obj == null || obj.GetComponent<EventTrigger>() == null) return;

            // ListCard temp = CardList[mDragManager.DragGameObject.transform.GetSiblingIndex()];

            CardList.RemoveAt(mDragManager.DragGameObject.transform.GetSiblingIndex());

            CardList.Insert(obj.transform.parent.GetSiblingIndex(), mDragManager.DragGameObject.GetComponent<ListCard>());


            mDragManager.DragGameObject.transform.SetSiblingIndex(obj.transform.parent.GetSiblingIndex());
            CoverDataSort();
            mDragManager.DragGameObject.transform.GetComponent<ListCard>().TransferAnimation();

            //觸發listcard的縮小到放大(增加改位置的視覺效果)


        };//換位置

    }

    private float displayHeigh = 0, canvasRealHeigh = 0;
    private float DragStandard = 300;
    private IEnumerator DetectViewAngleDown()//之後要根據拖曳而偵測滑鼠位置是否要把陣列視角向下或向上
    {


        if (displayHeigh <= 0)
        {
            canvasRealHeigh = transform.root.GetComponent<RectTransform>().sizeDelta.y;//canvas 高度
            displayHeigh = canvasRealHeigh - 540;                //顯示範圍
        }

        RectTransform gViewRect = gViewPanel.GetComponent<RectTransform>();

        while (isDeteDragFunc)
        {
            float maxHeigh = gViewRect.sizeDelta.y - displayHeigh;//偵測可上去的的高度

            float viewDisplayHeigh = gViewRect.localPosition.y;



            if (viewDisplayHeigh < maxHeigh)
            {
                if (Input.mousePosition.y < DragStandard)
                {
                    // gViewRect.localPosition=new Vector2(gViewRect.localPosition.x,(gViewRect.localPosition.y+1)*((250-Input.mousePosition.y)/125));
                    gViewRect.localPosition = new Vector2(gViewRect.localPosition.x, (gViewRect.localPosition.y + 1) + (5 * ((DragStandard - Input.mousePosition.y) / DragStandard)));
                    Debug.Log("向下");
                }

            }

            if (viewDisplayHeigh > -50)
            {
                if (canvasRealHeigh - Input.mousePosition.y < DragStandard)
                {
                    // gViewRect.localPosition=new Vector2(gViewRect.localPosition.x,(gViewRect.localPosition.y-1)*((250-(canvasRealHeigh-Input.mousePosition.y))/125));
                    float yTop = canvasRealHeigh - DragStandard;
                    gViewRect.localPosition = new Vector2(gViewRect.localPosition.x, (gViewRect.localPosition.y - 1) - (5 * (Input.mousePosition.y - yTop) / DragStandard));
                    Debug.Log("向上");
                }

            }


            yield return null;
        }
    }

    private void SetCradTriggerExpand(bool _isDrag)
    {//擴張觸碰判斷

        foreach (ListCard Lc in CardList)
        {
            Lc.IsDragButtonEffect(_isDrag);
        }
    }


    private void CreateListOfSaveData()//從Init接收到的Data(通常是SaveData裡面)新增單字組
    {

        foreach (WordListData Dic in mBaseData.WordListDatas)
        {
            GameObject sp = Instantiate(gListObject);

            sp.transform.SetParent(gViewPanel.transform);


            mDragManager.AddGameObjectEvent(sp);

            sp.transform.localScale = new Vector3(1, 1, 1);
            CardList.Add(sp.GetComponent<ListCard>());
            sp.GetComponent<ListCard>().Init(Dic, CreateListCardFunc());
        }

    }

    private void CreateListObject(string _title)//新增單字組
    {
        GameObject sp = Instantiate(gListObject);

        sp.transform.SetParent(gViewPanel.transform);


        mDragManager.AddGameObjectEvent(sp);

        sp.transform.localScale = new Vector3(1, 1, 1);

        CardList.Add(sp.GetComponent<ListCard>());

        // sp.GetComponent<ListCard>().Init(DataManager.Instance.saveData.AddNewList(_title), CreateListCardFunc());
        sp.GetComponent<ListCard>().Init(DataManager.Instance.saveData.AddNewList(mBaseData, _title), CreateListCardFunc());
    }




    private void CoverDataSort()//覆蓋儲存檔
    {
        List<WordListData> temp = new List<WordListData>();

        foreach (ListCard ld in CardList)
        {
            temp.Add(ld.aData);
        }
        DataManager.Instance.saveData.WordListsOfGroup.WordListDatas = temp;

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
            ClickBelowMenuButton();
        }
        StartCoroutine(PopupManager.Instance.OpenInputStringOneCurrectButtonWindow("請輸入群組名稱", CreateListObject));
    }

    private bool BelowMenuAni = false;
    private bool BelowMenuExpand = false;

    public void ClickBelowMenuButton()//點擊選單列表
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
            //yield return BelowMenu.transform.DOLocalMoveY(-1310, 0.2f).WaitForCompletion();
            yield return BelowMenu.GetComponent<RectTransform>().DOAnchorPosY(-150, 0.2f).WaitForCompletion();
            BelowMenuAni = false;

        }
        else
        {
            BelowMenuExpand = true;
            // yield return BelowMenu.transform.DOLocalMoveY(-1010, 0.2f).WaitForCompletion();
            yield return BelowMenu.GetComponent<RectTransform>().DOAnchorPosY(150, 0.2f).WaitForCompletion();
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


            DataManager.Instance.saveData.WordListsOfGroup.WordListDatas.Remove(dv.aData);

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
