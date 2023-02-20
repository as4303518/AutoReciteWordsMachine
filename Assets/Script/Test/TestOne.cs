using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestOne : MonoBehaviour
{
    // Start is called before the first frame update



    public string CallString = null;

    public TestTwo two;

    public HashSet<string> a = new HashSet<string>();
    public List<string> b = new List<string>();

    void Awake()
    {
        a.Add("一");
        a.Add("二");
        a.Add("三");
        a.Add("四");
        a.Add("五");

        b.Add("一");
        b.Add("二");
        b.Add("三");
        b.Add("二");
        b.Add("一");

        foreach(string str in a){

            Debug.Log("A==>"+str);
        }
        foreach(string str in b){

            Debug.Log("B==>"+str);
        }


        // Debug.Log("開始執行");
        // StartCoroutine(two.Init(Atest));

        // StartCoroutine(WaitCallBack());
        //StartCoroutine(WaitCallBack2());



    }





    private IEnumerator WaitCallBack()
    {
        Debug.Log("等待回傳參數" + CallString);//2
        yield return new WaitUntil(() => CallString != "");
        Debug.Log("已收到回傳值" + CallString);//4
        StopAllCoroutines();
    }

    // private IEnumerator WaitCallBack2()
    // {
    //     Debug.Log("2等待回傳參數" + CallString);//2
    //     yield return new WaitUntil(() => CallString == "失敗");
    //     Debug.Log("2已收到回傳值" + CallString);//4
    //     StopAllCoroutines();
    // }


    private void Atest(string _str)
    {
        CallString = _str;
        Debug.Log("我是腳本A的Atest");//3

    }

}
