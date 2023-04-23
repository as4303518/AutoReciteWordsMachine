using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInfoPopup : PopupWindow
{
    [Header("MainPage")]
    public GameObject InfoPage;
    public GameObject EditorPage;//編輯室窗，更改以前的設定
    public GameObject OptionsPage;//選項視窗，可以把單字移到熟背單字表裡，或者更改單字的群組等等

    [Header("UI")]

    public GameObject MoveButton;

    public Button InfoButton;
    public Button EditorButton;//編輯室窗，更改以前的設定
    public Button OptionsButton;//選項視窗，可以把單字移到熟背單字表裡，或者更改單字的群組等等

    [Header("InfoPage")]
    public Text mWordText;

    public Text mTranslate;
    public Text mSentenceContext;

    public Text mExamFrequency;//正確次數

    public Text mExamAnswerRate;//考試對的機率

    public Text mFoundingTime;

    [Header("Button")]

    public Button ChangeListButton;
    public Button FinishListButton;//編輯室窗，更改以前的設定

    [Header("Prefabs")]
    public GameObject ScrollViewListPrefabs;

    // public Button OptionsButton;//選項視窗，可以把單字移到熟背單字表裡，或者更改單字的群組等等


    // public List<考試記錄腳本>//包含考的次數,當時的答題正確或錯誤,當時的答案

    [Header("Other")]
    public WordData aData;

    // public void Start()
    // {
    //     UpdateUI();
    //     SetButtonSetting();
    // }

    public void Init(WordData _Data)
    {
        aData = _Data;
        UpdateUI();
        SetButtonSetting();

    }

    private void UpdateUI()
    {
        mWordText.text = "單字:" + aData.wordText;
        mTranslate.text = "翻譯:" + aData.translate;
        mSentenceContext.text = "例句:" + aData.sentenceContext;
        mFoundingTime.text = "創立時間:" + aData.mFoundingTime;
        mExamFrequency.text = "回答次數:" + aData.answerFrequency + "答對次數:" + aData.CorrectFrequency;

        if (aData.CorrectFrequency > 0)
        {
            mExamAnswerRate.text = "對題率:" + ((aData.CorrectFrequency / aData.answerFrequency) * 100) + "%";
        }
        else
        {
            mExamAnswerRate.text = "對題率:" + 0 + "%";
        }
    }

    private void SetButtonSetting()
    {
        InfoButton.onClick.AddListener(() => { StartCoroutine(ClickInfoButton()); });
        EditorButton.onClick.AddListener(() => { StartCoroutine(ClickEditorButton()); });
        OptionsButton.onClick.AddListener(() => { StartCoroutine(ClickOptionsButton()); });
        ChangeListButton.onClick.AddListener(ClickChangeListButton);
        FinishListButton.onClick.AddListener(ClickFinishListButton);
    }


    private void ClickChangeListButton()//生成條列式視窗，並設置每個按鈕的反應。
    {
        StartCoroutine(PopupManager.Instance.OpenMultOptionsScrollViewPopup(
            ScrollViewListPrefabs,
            DataManager.Instance.saveData.WordListsOfGroup.WordListDatas,
            (v, i) =>
            {
                WordListData tempData = DataManager.Instance.saveData.WordListsOfGroup.WordListDatas[i];
                v.name = tempData.mTitle;
                v.transform.Find("Text").GetComponent<Text>().text = tempData.mTitle;

                v.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("以觸發按鈕" + v.name);

                    IEnumerator Correct()
                    {
                        aData.mListGroup = tempData.mTitle;

                        var tempList = WordControlManager.Instance.WordCardList;

                        foreach (WordCard wc in tempList)
                        {
                            if (wc.aData == aData)
                            {
                                WordControlManager.Instance.WordCardList.Remove(wc);
                                WordControlManager.Instance.aData.mWords.Remove(aData);
                                Destroy(wc.gameObject);
                                Debug.Log("已刪除該卡牌");
                                break;
                            }
                        }
                        tempData.mWords.Add(aData);

                        WordControlManager.Instance.UIUpdate();

                        IEnumerator Hint()
                        {
                            yield return PopupManager.Instance.OpenHintOnlyStringWindow("完成", "以移動單字至" + tempData.mTitle + "群組");
                        }

                        PopupManager.Instance.PopupManagerExecute(Hint);
                        yield return PopupManager.Instance.CloseAllPopup();



                    }

                    StartCoroutine(PopupManager.Instance.OpenTipTwoOptionsButtonWindow("變更群組", "確定要變更群組嗎?", Correct, PopupManager.Instance.ClosePopup));
                });



            }
            ));

    }

    private void ClickFinishListButton()
    {
        // aData



        IEnumerator Correct()
        {

            foreach (WordCard v in WordControlManager.Instance.WordCardList)
            {
                if (v.aData == aData)
                {
                    WordControlManager.Instance.WordCardList.Remove(v);
                    WordControlManager.Instance.aData.mWords.Remove(aData);
                    Destroy(v.gameObject);
                    break;
                }
            }

            bool isHaveList = false;

            foreach (WordListData Wld in DataManager.Instance.saveData.WordListsOfFinishGroup.WordListDatas)
            {
                if (Wld.mTitle == aData.mListGroup)
                {
                    Wld.mWords.Add(aData);
                    isHaveList = true;
                    break;
                }

            }
            if (!isHaveList)
            {
                WordListData wld = new WordListData(WordControlManager.Instance.aData.mTitle, WordControlManager.Instance.aData.mListNum);
                wld.mWords.Add(aData);
                DataManager.Instance.saveData.WordListsOfFinishGroup.WordListDatas.Add(wld);
            }
            yield return PopupManager.Instance.CloseAllPopup();
        }

        // IEnumerator Cancel(){


        // }


        StartCoroutine(PopupManager.Instance.OpenTipTwoOptionsButtonWindow("變更群組", "確定要將單字移動到完成群組嗎?", Correct, PopupManager.Instance.ClosePopup));


    }


    //////////////UI BUTTON

    private bool ButtonMoveAni = false;
    private IEnumerator ClickInfoButton()
    {
        if (ButtonMoveAni) { yield break; }
        ButtonMoveAni = true;

        InfoPage.SetActive(true);
        EditorPage.SetActive(false);
        OptionsPage.SetActive(false);

        Vector3 LocalPos = InfoButton.GetComponent<RectTransform>().localPosition;
        yield return TweenAniManager.ToMoveDoTween(MoveButton, new Vector3(LocalPos.x, 0, 0));
        ButtonMoveAni = false;
        // yield return null;
    }

    private IEnumerator ClickEditorButton()
    {
        if (ButtonMoveAni) { yield break; }
        ButtonMoveAni = true;

        InfoPage.SetActive(false);
        EditorPage.SetActive(true);
        OptionsPage.SetActive(false);

        Vector3 LocalPos = EditorButton.GetComponent<RectTransform>().localPosition;
        yield return TweenAniManager.ToMoveDoTween(MoveButton, new Vector3(LocalPos.x, 0, 0));
        ButtonMoveAni = false;
    }
    private IEnumerator ClickOptionsButton()
    {
        if (ButtonMoveAni) { yield break; }
        ButtonMoveAni = true;

        InfoPage.SetActive(false);
        EditorPage.SetActive(false);
        OptionsPage.SetActive(true);

        Vector3 LocalPos = OptionsButton.GetComponent<RectTransform>().localPosition;
        yield return TweenAniManager.ToMoveDoTween(MoveButton, new Vector3(LocalPos.x, 0, 0));
        ButtonMoveAni = false;
    }











}
