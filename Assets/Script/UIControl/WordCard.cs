using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCard : MonoBehaviour
{

    //單字顯示實體腳本
    public string context;
    public string translate;
    public string sentenceContext;
    public string sentenceTranslate;

    void Init(WordData _word)
    {
        context = _word.context;
        translate = _word.translate;
        sentenceContext = _word.sentenceContext;
        sentenceTranslate = _word.sentenceTranslate;
    }
}
