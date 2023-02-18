using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestOne : MonoBehaviour
{
    // Start is called before the first frame update



    public string CallString = null;

    public TestTwo two;
    void Awake()
    {
        // Debug.Log("開始執行");
        // StartCoroutine(two.Init(Atest));
        // StartCoroutine(WaitCallBack());
        

    }
    void Start()
    {

    }

    private IEnumerator WaitCallBackValue(){

        yield return new WaitForSeconds(3);
        
    }


    private IEnumerator WaitCallBack()
    {
        Debug.Log("wait等待回傳參數" + CallString);
        yield return CallString;
        Debug.Log("callstring已收到回傳值" + CallString);

    }
    private void Atest(string _str)
    {
        CallString = _str;
        Debug.Log("我是腳本A的Atest");

    }

}
