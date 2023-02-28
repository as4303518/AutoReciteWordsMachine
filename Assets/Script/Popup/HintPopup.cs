using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintPopup : MonoBehaviour
{

public Text _Title;
public Text _Content;



  public void Init(string title,string content){
    _Title.text=title;
    _Content.text=content;


  }
}
