using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class WordControlManager : InstanceScript<WordControlManager>, PrefabScene
{

    //根據params 選出對應的字卡陣列，生產出字卡
    [Header("Default")]
    public GameObject WordCardPrefabs;

    public GameObject gViewParent;//字卡陣列母物件

    public WordListData aData;

    public List<WordCard> WordCardList = new List<WordCard>();//生成出來的卡陣列

    [SerializeField] private List<WordCard> TempChooseCardList = new List<WordCard>();//將選擇的卡加入此陣列，目前用作刪除用，未來可能會有其他用途，都可以使用此陣列

    private ManagerStatus ControlModel;

    private DragUI mDragManager;//管理拖曳的組件

    [Header("Prefabs")]
    public GameObject WordInfoPopup;

    //private int ListCount=1;

    [Header("UI")]
    public Text ListTitle;
    public Text Count;

    public Button BelowButtonGroup;

    public Button DeleteButton;

    public Button CancelDeleteButton;
    public Button CreateButton;
    public Button BackButton;

    public InputField SearchField;

    public Dropdown SearchModle;

    // void Awake()
    // {//到時會使用切換場景的方式呼叫，目前先用awake

    //     //MonoScript();
    //     Debug.Log("awake有執行");
    //     //Init(TestListData());

    // }

    // private WordListData TestListData()//暫時測試
    // {
    //     WordListData w = new WordListData("第一課程", 1);
    //     w.mWords.Add(new WordData("Irritated", "煩惱", 451));
    //     w.mWords.Add(new WordData("apple", "蘋果", 71));
    //     w.mWords.Add(new WordData("banana", "香蕉", 31));
    //     return w;
    // }


    public IEnumerator Init(BaseData _wordListData)
    {
        aData = _wordListData as WordListData;
        mDragManager = GetComponent<DragUI>();

        CreateWordCardToViewInSaveData();
        UIUpdate();
        ButtonSetting();
        ControlModel = ManagerStatus.Normal;
        yield return null;
    }

    public void UIUpdate()
    {
        Count.text = "總共字量" + aData.mWords.Count;
        ListTitle.text = "標題:" + aData.mTitle;
    }


    private void ButtonSetting()
    {
        BackButton.onClick.AddListener(GoToWordListCard);
        CreateButton.onClick.AddListener(OpenEstablishPopupWindow);
        DeleteButton.onClick.AddListener(ClickDeleteButton);
        CancelDeleteButton.onClick.AddListener(ClickCancelDeteleButton);
        BelowButtonGroup.onClick.AddListener(ClickBelowButtonGroup);
        SearchField.onValueChanged.AddListener(SearchOfStr);
        SearchModle.onValueChanged.AddListener(SearchModleChange);
        SetDragSettingEvent();
        SetSearchDirectory();

    }
    private void SearchOfStr(string str)//搜尋功能
    {
        Debug.Log("偵測搜尋==>" + str);
        str = str.ToUpper();
        IEnumerable<WordCard> tempList;

        switch (SearchModle.value)
        {

            case 0:

                tempList = WordCardList.Where(x => x.aData.wordText.ToUpper().Contains(str));

                break;
            case 1:

                tempList = WordCardList.Where(x => x.aData.mTag.ToUpper().Contains(str));

                break;
            case 2:

                tempList = WordCardList.Where(x => x.mFunc.mNum.ToString() == str);

                break;

            default:

                tempList = WordCardList.Where(x => x.aData.wordText.ToUpper().Contains(str));

                break;
        }

        WordCardList.ForEach(c =>
        {
            bool isHaveTemp = false;
            foreach (var card in tempList)
            {

                if (c.aData.wordText == card.aData.wordText)
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
    private bool isDrag = false;
    private void SetDragSettingEvent()
    {

        mDragManager.ClearEvent();

        mDragManager.OnClickCallBack += obj =>
        {

            switch (ControlModel)
            {
                case ManagerStatus.Normal:

                    //未來可能有額外的功能
                    break;

                case ManagerStatus.Delete:

                    if (isDrag == false) obj.transform.parent.GetComponent<WordCard>().ChoseDeleteThisCard();

                    break;
            }
        };

        mDragManager.OnBeginDragCallBack += obj =>
        {
            isDrag = true;
            StartCoroutine(TweenAniManager.SetTransparentGroup(mDragManager.DragGameObject, 1, 0.5f, 0));
            SetCradTriggerExpand(isDrag);
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
            StartCoroutine(TweenAniManager.SetTransparentGroup(mDragManager.DragGameObject, 0.5f, 1f, 0));

            isDrag = false;
            isDeteDragFunc = false;
            SetCradTriggerExpand(isDrag);

            if (obj.GetComponent<EventTrigger>() == null) return;

            WordCard getWc = mDragManager.DragGameObject.GetComponent<WordCard>();

            WordCardList.Remove(getWc);

            WordCardList.Insert(obj.transform.parent.GetSiblingIndex(), getWc);

            mDragManager.DragGameObject.transform.SetSiblingIndex(obj.transform.parent.GetSiblingIndex());
            SaveWordInfo();
            getWc.TransferAnimation();


        };

    }

    private void SetSearchDirectory()//設置有哪些搜尋主目標(以名字為主，以標籤為主等)
    {
        SearchModle.options.Add(new Dropdown.OptionData(LanguageTranstale.Instance.GetStr(MyLabel.SearchSortName)));
        SearchModle.options.Add(new Dropdown.OptionData(LanguageTranstale.Instance.GetStr(MyLabel.SearchSortTag)));
        SearchModle.options.Add(new Dropdown.OptionData(LanguageTranstale.Instance.GetStr(MyLabel.SearchSortNum)));
    }

    private void SearchModleChange(int changeModle)
    {//每當選擇不同的篩選時,重製或重新選擇
        Debug.Log("轉換篩選模式到===>" + SearchModle.options[changeModle].text);
        SearchOfStr(SearchField.text);
    }
    public GameObject gViewPanel;//字卡陣列母物件
    private bool isDeteDragFunc = false;//偵測拖動中的方法是否有執行
    private float displayHeigh = 0, canvasRealHeigh = 0;

    private float DragStandard = 300;
    private IEnumerator DetectViewAngleDown()//之後要根據拖曳而偵測滑鼠位置是否要把陣列視角向下或向上
    {

        // Debug.Log("滑鼠位置" + Input.mousePosition);
        // Debug.Log("螢幕長" + Screen.height + "寬==>" + Screen.width);
        //transform.root.getcom<canvas>().he-540 固定長寬比=可顯示部用曾長的全部面積0 ~ ~ view heigh-這個數值


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

            Debug.Log("偵測");

            if (viewDisplayHeigh < maxHeigh)
            {
                if (Input.mousePosition.y < DragStandard)
                {

                    gViewRect.localPosition = new Vector2(gViewRect.localPosition.x, (gViewRect.localPosition.y + 1) + (4 * ((DragStandard - Input.mousePosition.y) / DragStandard)));
                    Debug.Log("向下");
                }

            }

            if (viewDisplayHeigh > -50)
            {
                if (canvasRealHeigh - Input.mousePosition.y < DragStandard)
                {

                    float yTop = canvasRealHeigh - DragStandard;//觸發往上的距離門檻
                    gViewRect.localPosition = new Vector2(gViewRect.localPosition.x, (gViewRect.localPosition.y - 1) - (5 * (Input.mousePosition.y - yTop) / DragStandard));
                    Debug.Log("向上");
                }

            }


            yield return null;
        }
    }

    private void SetCradTriggerExpand(bool _isDrag)
    {
        foreach (WordCard wc in WordCardList)
        {
            wc.IsDragButtonEffect(_isDrag);
        }
    }



    private void CreateWordCardToViewInSaveData()//生成Savedata裡的字卡
    {

        int ListCount = 1;

        for (int i = 0; i < aData.mWords.Count; i++)
        {
            CreateNewWord(aData.mWords[i], ListCount, false);
            ListCount++;

        }
    }


    public void OpenEstablishPopupWindow()//生成單字紀錄單字資訊的彈窗
    {
        if (ControlModel == ManagerStatus.Delete)
        {
            ClickCancelDeteleButton();
        }
        StartCoroutine(PopupManager.Instance.OpenEstablishWordPopup(CreateNewWord, aData.mWords.Count, aData.GetListWordNewNum(), OpenTipTwoOptionsPopup));
    }

    private void OpenTipTwoOptionsPopup()
    {//確定玩家是否真的要放棄建立單字
     //  PopupManager.Instance.OpenTipTwoOptionsButtonWindow("確定要關閉視窗?", "您確定要放棄當前編輯的單字嗎?",

        StartCoroutine(PopupManager.Instance.OpenTipTwoOptionsButtonWindow(
        LanguageTranstale.Instance.GetStr(MyLabel.SureCloseWindowTitle),
        LanguageTranstale.Instance.GetStr(MyLabel.SureCloseWindowTag),
        PopupManager.Instance.CloseAllPopup,
        PopupManager.Instance.ClosePopup
        ));
    }

    //AddListData 是否要加到陣列裡,如果是已經在資料上的(原本儲存的)就不需要
    private void CreateNewWord(WordData wd, int CardNum, bool AddListData)//生成新的字卡，加入到aData陣列
    {

        GameObject sp = Instantiate(WordCardPrefabs, gViewParent.transform);
        mDragManager.AddGameObjectEvent(sp);
        WordCard NewCard = sp.GetComponent<WordCard>();

        NewCard.Init(wd, new WordCard.WordCardFunc(
            CardNum,//順序編號
            AddWordCardToTempList,
            RemoveWordCardToTempList
            ));

        NewCard.aData.mListGroup = aData.mTitle;
        //aData.mWords.Count
        WordCardList.Add(NewCard);
        if (AddListData)
        {
            aData.mWords.Add(wd);//以新增卡片，所以把卡的資料加到陣列
            aData.wordCardAllCount += 1;//幫預設id加1
            SaveWordInfo();
        }

        UIUpdate();
        // sp.GetComponent<WordCard>().Init()

    }

    private void CreateNewWord(WordData wd, int CardNum)//生成新的字卡,初始化時使用
    {

        CreateNewWord(wd, CardNum, true);
    }

    // private void SaveWordInfo(WordCard Wc)//更新某筆資料至陣列單字
    // {
    //     // aData.mWords.Where(v=>{return true;}).FirstOrDefault();
    //     for (int i = 0; i < aData.mWords.Count; i++)
    //     {
    //         if (aData.mWords[i] == Wc.aData)//==在WordData裡有額外的判斷式
    //         {
    //             aData.mWords[i] = Wc.aData;//存list裡的陣列資料
    //             SaveListDataToDataManager();//存savedata裡的資料
    //             return;
    //         }
    //     }
    // }

    private void SaveWordInfo()//更新某筆資料至陣列單字
    {
        Debug.Log("word儲存");
        List<WordData> AllWordData = new List<WordData>();

        foreach (WordCard wc in WordCardList)
        {
            wc.ReSetNumList();
            AllWordData.Add(wc.aData);

        }

        aData.mWords = AllWordData;
        SaveListDataToDataManager();

    }

    private void SaveListDataToDataManager()//更新資料至主Data上
    {
        Debug.Log("更新陣列的狀態");
        DataManager.Instance.saveData.CoverListData(aData);
    }

    private void DeleteDataOfTempList()//刪除資料
    {
        Debug.Log("word儲存");
        foreach (WordCard wc in TempChooseCardList)
        {
            WordCardList.Remove(wc);
            aData.mWords.Remove(wc.aData);
            //wc.destory  //會有個dotween之類
            Destroy(wc.gameObject);
        }


        SaveListDataToDataManager();
    }

    private void AddWordCardToTempList(WordCard wc)//加入字卡進臨時陣列
    {
        TempChooseCardList.Add(wc);

    }

    private void RemoveWordCardToTempList(WordCard wc)//移除字卡出臨時陣列
    {
        TempChooseCardList.Remove(wc);

    }
    private bool Ani = false;
    private void ClickDeleteButton()//開啟單字管理員的刪除模式
    {
        if (Ani) return;//如果在執行動畫，則不給執行以下方法
        Ani = true;
        switch (ControlModel)
        {
            case ManagerStatus.Normal:
                OpenDeleteModel();
                StartCoroutine(TweenAniManager.ColorOrTransparentChange(DeleteButton.GetComponent<Image>(), Color.red));
                CancelDeleteButton.gameObject.SetActive(true);
                StartCoroutine(TweenAniManager.ByMoveDoTween(CancelDeleteButton.gameObject, new Vector3(-250, 0, 0), () => { Ani = false; }));
                break;

            case ManagerStatus.Delete:
                DeleteDataOfTempList();//多了刪除
                ClickCancelDeteleButton();
                //還需要檢查templist有沒有 要被刪除的,如果有刪除
                //或者玩家案取消，則清除templist 不刪除
                break;

            default:
                Debug.Log("無法在其他狀態使用該按鈕");
                break;
        }

    }

    private void ClickCancelDeteleButton()//按下取消刪除
    {
        //玩家取消刪除
        StartCoroutine(TweenAniManager.ColorOrTransparentChange(DeleteButton.GetComponent<Image>(), Color.black));
        StartCoroutine(TweenAniManager.ByMoveDoTween(CancelDeleteButton.gameObject, new Vector3(250, 0, 0), () =>
        {
            Ani = false;
            CancelDeleteButton.DOKill();
            CancelDeleteButton.gameObject.SetActive(false);
        }));
        CloseDeleteModel();


    }

    private void OpenDeleteModel()//開啟刪除模式
    {
        ControlModel = ManagerStatus.Delete;
        TempChooseCardList.Clear();
        WordCardList.ForEach(v =>
        {
            v.OpenDeleteModel();
        });
    }

    private void CloseDeleteModel()//關閉刪除模式
    {
        ControlModel = ManagerStatus.Normal;

        WordCardList.ForEach(v =>
        {
            v.CloseDeleteModel();
        });
        TempChooseCardList.Clear();
    }



    private void ClickBelowButtonGroup()
    {
        StartCoroutine(clickBelowButtonGroup());
        //loading.text= LanguageTranstale.Instance.GetStr(MyLabel.Loading);

    }
    private bool Expand = false;

    private IEnumerator clickBelowButtonGroup()
    {
        if (Ani) yield break;//如果在執行動畫，則不給執行以下方法
        Ani = true;
        Transform parT = BelowButtonGroup.transform.parent.transform;

        // Debug.Log("測試顯示==>"+parT.anchoredPosition+"測試顯示2===>"+parT.localPosition);
        if (!Expand)
        {
            Expand = true;
            Tween t = parT.GetComponent<RectTransform>().DOAnchorPosY(150, 0.2f);
            yield return t.WaitForCompletion();
            Ani = false;

        }
        else
        {
            Expand = false;
            Tween t = parT.GetComponent<RectTransform>().DOAnchorPosY(-150, 0.2f);
            yield return t.WaitForCompletion();
            Ani = false;
        }

    }

    public void GoToWordListCard()//返回
    {
        SceneManager.Instance.StartChangScene(SceneManager.SceneType.ListControlManager, DataManager.Instance.saveData.WordListsOfGroup);
    }

    public IEnumerator PageTweenOut()
    {//轉場去其他頁面
        Debug.Log("離去單字組");
        yield return PopupManager.Instance.OpenLoading();
        yield return TweenAniManager.TransparentOutGroup(transform.gameObject);

    }

    public IEnumerator PageTweenIn()
    {//轉場去其他頁面

        Debug.Log("進入單字組");
        yield return TweenAniManager.TransparentInGroup(transform.gameObject);
        PopupManager.Instance.CloseLoading();

    }



    void OnApplicationQuit()
    {


        //如果是在單字列表離開，儲存單字列表的東西(以免發生修改了，卻來不及儲存的狀況)


    }


}
