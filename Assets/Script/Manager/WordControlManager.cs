using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class WordControlManager : InstanceScript<WordControlManager>, PrefabScene
{

    //根據params 選出對應的字卡陣列，生產出字卡

    public GameObject WordCardPrefabs;

    public GameObject gViewParent;//字卡陣列母物件

    public WordListData aData;

    public List<WordCard> WordCardList = new List<WordCard>();//生成出來的卡陣列

    [SerializeField] private List<WordCard> TempChooseCardList = new List<WordCard>();//將選擇的卡加入此陣列，目前用作刪除用，未來可能會有其他用途，都可以使用此陣列

    private ManagerStatus ControlModel;



    //private int ListCount=1;

    [Header("UI")]
    public Text ListTitle;
    public Text Count;

    public Button BelowButtonGroup;

    public Button DeleteButton;

    public Button CancelDeleteButton;
    public Button CreateButton;
    public Button BackButton;

    void Awake()
    {//到時會使用切換場景的方式呼叫，目前先用awake

        //MonoScript();
        Debug.Log("awake有執行");
        //Init(TestListData());

    }

    private WordListData TestListData()//暫時測試
    {
        WordListData w = new WordListData("第一課程", 1);
        w.mWords.Add(new WordData("Irritated", "煩惱", 451));
        w.mWords.Add(new WordData("apple", "蘋果", 71));
        w.mWords.Add(new WordData("banana", "香蕉", 31));
        return w;
    }


    public IEnumerator Init(BaseData _wordListData)
    {
        aData = _wordListData as WordListData;
        Debug.Log("以初始化==>" + gameObject.name);
        CreateWordCardToViewInSaveData();
        UIUpdate();
        ButtonSetting();
        ControlModel = ManagerStatus.Normal;
        yield return null;
    }

    private void UIUpdate()
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

    }
    private void CreateWordCardToViewInSaveData()//生成data裡的字卡
    {

        int ListCount = 1;

        for (int i = 0; i < aData.mWords.Count; i++)
        {
            CreateNewWord(aData.mWords[i], ListCount, false);
            ListCount++;

        }
    }

    private void SaveWordInfo(WordCard Wc)//更新陣列單字的狀態
    {
        // aData.mWords.Where(v=>{return true;}).FirstOrDefault();
        for (int i = 0; i < aData.mWords.Count; i++)
        {
            if (aData.mWords[i] == Wc.aData)//==在WordData裡有額外的判斷式
            {
                aData.mWords[i] = Wc.aData;//存list裡的陣列資料
                SaveListDataToDataManager();//存savedata裡的資料
                return;
            }
        }
    }
    private void SaveListDataToDataManager()
    {
        Debug.Log("更新陣列的狀態");
        DataManager.Instance.saveData.SetListWordData(aData);
    }

    private void DeleteDataOfTempList()
    {

        foreach (WordCard wc in TempChooseCardList)
        {
            WordCardList.Remove(wc);
            aData.mWords.Remove(wc.aData);
            //wc.destory  //會有個dotween之類
            Destroy(wc.gameObject);
        }


        SaveListDataToDataManager();
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
        LanguageTranstale.Instance.GetStr(MyLabel.SureCloseWindowTip),
        PopupManager.Instance.CloseAllPopup,
        PopupManager.Instance.ClosePopup
        ));
    }

    //AddListData 是否要加到陣列裡,如果是已經在資料上的(原本儲存的)就不需要
    private void CreateNewWord(WordData wd, int CardNum, bool AddListData)//生成新的字卡，加入到aData陣列
    {
        if (AddListData)
        {
            aData.mWords.Add(wd);//以新增卡片，所以把卡的資料加到陣列
            aData.wordCardAllCount += 1;//幫預設id加1
        }
        GameObject sp = Instantiate(WordCardPrefabs, gViewParent.transform);

        WordCard NewCard = sp.GetComponent<WordCard>();

        NewCard.Init(wd, new WordCard.WordCardFunc(
            CardNum,//順序編號
            AddWordCardToTempList,
            RemoveWordCardToTempList
            ));

        //aData.mWords.Count
        WordCardList.Add(NewCard);
        SaveWordInfo(NewCard);
        UIUpdate();
        // sp.GetComponent<WordCard>().Init()

    }
    private void CreateNewWord(WordData wd, int CardNum)//生成新的字卡,初始化時使用
    {

        CreateNewWord(wd, CardNum, true);

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

    }
    private bool Expand = false;

    private IEnumerator clickBelowButtonGroup()
    {
        if (Ani) yield break;//如果在執行動畫，則不給執行以下方法
        Ani = true;
        Transform parT=BelowButtonGroup.transform.parent.transform;

        // Debug.Log("測試顯示==>"+parT.anchoredPosition+"測試顯示2===>"+parT.localPosition);
        if (!Expand)
        {
            Expand = true;
            Tween t = parT.DOLocalMoveY(-810, 0.2f);
            yield return t.WaitForCompletion();
            Ani = false;

        }
        else
        {
            Expand = false;
            Tween t = parT.DOLocalMoveY(-1110, 0.2f);
            yield return t.WaitForCompletion();
            Ani = false;
        }

    }

    public void GoToWordListCard()//返回
    {
        SceneManager.Instance.StartChangScene(SceneManager.SceneType.ListControlManager);
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

    // private int getListNum(){//紀錄增加的數量
    //     ListCount++;
    //     return ListCount;

    // }

    void OnApplicationQuit()
    {


        //如果是在單字列表離開，儲存單字列表的東西(以免發生修改了，卻來不及儲存的狀況)


    }


}
