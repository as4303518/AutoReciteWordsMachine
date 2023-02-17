using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputStringWindowPopup : MonoBehaviour
{
  
public Text InputString;

public delegate string ReturnText();

public Text Title;

public void Init(string _title){

Title.text=_title;

}


}
