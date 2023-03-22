using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : InstanceScript<GameManager>
{
    // 紀錄場景轉換

    public GameObject MainManagerParent = null;
    void Awake()
    {
   
        MainManagerParent = GameObject.Find("MainManager");
        StartCoroutine(DontDestoryThis());
        StartCoroutine(InstanceAllManager());
    }


    private IEnumerator InstanceAllManager()
    {
        yield return BindManager<SceneManager>();//場景轉換
        yield return BindManager<PopupManager>();//彈窗
        yield return BindManager<DataManager>();//數據管理員
        yield return BindManager<LanguageTranstale>();//語言切換
        //之後還有音樂等
        yield return SceneManager.Instance.ChangeScene(SceneManager.SceneType.ListControlManager);//進入場景
    }
    // Update is called once per frame


    private IEnumerator BindManager<T>(string _managerGameName = "") where T : InstanceScript<T>
    {
        if (_managerGameName == "")//如果沒特別取名,則名字是class的名字
        {
            _managerGameName = typeof(T).Name;
        }
        GameObject sp = new GameObject(_managerGameName);
        sp.transform.SetParent(MainManagerParent.transform);
        T t = sp.AddComponent<T>();
        yield return t.DontDestoryThis();//這裡會呼叫管理員的init,要拿一些父物件等等，如假設popup管理員是泛型T,popup管理員要在init把PopupWindow預設成彈窗的父物件
        // t.ShowGameObjectName();
    }



}
