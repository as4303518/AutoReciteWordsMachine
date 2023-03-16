using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateWordPopup : MonoBehaviour
{

    [Header("UI")]

    public Dropdown mTag;
    public InputField inputWord;

    public InputField inputTransTale;

    public InputField inputSentence;

    public Button establish;

    private Action<WordData,int> CreateWord;

    private Action<GameObject> CloseFilter;
    private int WordCardNum = 0;

    private int CardSquenceNum;


    public void Init(int wordCardNum,int cardSquenceNum, Action<WordData,int> createWord, Action<GameObject> closeFilter)
    {
        WordCardNum = wordCardNum;
        CreateWord = createWord;
        CloseFilter = closeFilter;
        CardSquenceNum=cardSquenceNum;
        establish.onClick.AddListener(ClickEstablishWord);

    }

    public void ClickEstablishWord()
    {


        if (DataManager.Instance.saveData.CheckWordRepeat(inputWord.text))
        {
            PopupManager.Instance.OpenHintOnlyStringWindow("創建失敗!", "不適用或重複的標題的名字");
        }
        else
        {
            CreateWord(new WordData(inputWord.text, inputTransTale.text, WordCardNum, inputSentence.text, mTag.options[mTag.value].text),CardSquenceNum);
            CloseFilter(transform.parent.gameObject);
        }


    }

    //private Action<>

}
