using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputStringWindowPopup : MonoBehaviour
{
  
public Text mInputString;

public Button mCorrectButton;


public UnityAction<string> ReturnList;

public Text mTitle;

public void Init(string _title){

mTitle.text=_title;

}

public void ClickCorrect(){

ReturnList(mInputString.text);
transform.parent.GetComponent<FilterScript>().CloseFilter();

}




}
