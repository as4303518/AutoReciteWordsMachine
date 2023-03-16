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

    public Button DeleteButton;
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
        CreateWordCardToView();
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


    }
    private void CreateWordCardToView()//生成data裡的字卡
    {

        int ListCount = 1;

        for (int i = 0; i < aData.mWords.Count; i++)
        {
            CreateNewWord(aData.mWords[i], ListCount, false);
            ListCount++;

        }
    }

    public void SaveWordInfo(WordCard Wc)//更新陣列單字的狀態---待驗證
    {

        for (int i = 0; i < aData.mWords.Count; i++)
        {
            if (aData.mWords[i] == Wc.aData)//==在WordData裡有額外的判斷式
            {
                aData.mWords[i] = Wc.aData;
                DataManager.Instance.saveData.myLists[aData.mListNum] = aData;
                Debug.Log("更新陣列的狀態");
            }
        }
    }

    public void OpenEstablishPopupWindow()//生成單字前的談窗
    {
        PopupManager.Instance.OpenEstablishWordPopup(CreateNewWord, aData.mWords.Count, aData.GetListWordNewNum());

    }

    private void CreateNewWord(WordData wd, int CardNum, bool AddListData)//生成新的字卡
    {
        if (AddListData)
        {
            aData.mWords.Add(wd);//以新增卡片，所以把卡的資料加到陣列
            aData.wordCardAllCount += 1;//幫預設id加1
        }
        GameObject sp = Instantiate(WordCardPrefabs, gViewParent.transform);

        sp.GetComponent<WordCard>().Init(wd, new WordCard.WordCardFunc(
            CardNum,//順序編號
            AddWordCardToTempList,
            RemoveWordCardToTempList
            ));

        //aData.mWords.Count
        WordCardList.Add(sp.GetComponent<WordCard>());

        // sp.GetComponent<WordCard>().Init()

    }
    private void CreateNewWord(WordData wd, int CardNum)//生成新的字卡
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

    private void ClickDeleteButton()
    {
        switch (ControlModel)
        {
            case ManagerStatus.Normal:
                OpenDeleteModel();
                break;

            case ManagerStatus.Delete:
                CloseDeleteModel();
                //還需要檢查templist有沒有 要被刪除的,如果有刪除
                //或者玩家案取消，則清除templist 不刪除
                break;

            default:
                Debug.Log("無法在其他狀態使用該按鈕");
                break;

        }

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
    }

    public void GoToWordListCard()
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
