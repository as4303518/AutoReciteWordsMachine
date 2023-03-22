using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestTwo : MonoBehaviour
{
    // Start is called before the first frame update


    public Button InToUrlButton;

    public string Url="https://blog.csdn.net/u012741077/article/details/54884623";

    public void Awake(){
        InToUrlButton.onClick.AddListener(IntoUrl);
    }

    public void IntoUrl(){

        Application.OpenURL(Url);

    }

    public IEnumerator Init( UnityAction<string> _aa)
    {
        Debug.Log("開始執行init");//1
        yield return new WaitForSeconds(3);
        _aa("成功了 we did it");

    }

    // public IEnumerator aaa(){
    //     Debug.Log("成功執行a");
    //     yield return new WaitForSeconds(1);
    // }


}
