using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateWordPopup : PopupWindow
{

    [Header("UI")]

    public Dropdown mTag;
    public InputField inputWord;

    public InputField inputTransTale;

    public InputField inputSentence;

    public Button establish;

    private Action<WordData, int, int> CreateWord;

    private Func<GameObject, IEnumerator> ClosePopup;
    private int WordCardNum = 0;

    private int CardSquenceNum;


    public void Init(int wordCardNum, int cardSquenceNum, Action<WordData, int, int> createWord, Func<GameObject, IEnumerator> closePopup)
    {
        WordCardNum = wordCardNum;
        CreateWord = createWord;
        ClosePopup = closePopup;
        CardSquenceNum = cardSquenceNum;
        SetTagChoose();

        establish.onClick.AddListener(ClickEstablishWord);

    }

    private void SetTagChoose()//設置tag  如果有單字設置tag這個tag卻被刪除時，會回到預設
    {
        DataManager.Instance.saveData.Classification.ForEach(str =>
        {
            mTag.options.Add(new Dropdown.OptionData(str));
        });
    }

    public void ClickEstablishWord()
    {
        CreateWord(new WordData(inputWord.text, inputTransTale.text, WordCardNum, inputSentence.text, mTag.options[mTag.value].text), CardSquenceNum, mParentFilter.FilterNum);
        // if (DataManager.Instance.saveData.WordListsOfGroup.CheckWordRepeat(inputWord.text))
        // {
        //     StartCoroutine(PopupManager.Instance.OpenHintOnlyStringWindow("創建失敗!", "不適用或重複的標題名字"));
        // }
        // else
        // {
        //     CreateWord(new WordData(inputWord.text, inputTransTale.text, WordCardNum, inputSentence.text, mTag.options[mTag.value].text), CardSquenceNum,mParentFilter.FilterNum);
        //     StartCoroutine(ClosePopup(transform.parent.gameObject));
        // }
    }

    //private Action<>

}
