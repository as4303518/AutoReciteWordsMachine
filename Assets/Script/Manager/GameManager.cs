using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : InstanceScript<GameManager>
{
    // 紀錄場景轉換
    public string CurScene = null;

    public GameObject MainManagerParent = null;
    void Awake()
    {

        MainManagerParent = GameObject.Find("MainManager");
        StartCoroutine(DontDestoryThis());
        StartCoroutine(InstanceAllManager());
    }


    private IEnumerator InstanceAllManager()
    {
        yield return BindManager<SceneManager>();
        yield return BindManager<PopupManager>();
        yield return BindManager<DataManager>();
        yield return SceneManager.Instance.ChangeScene(SceneManager.SceneType.ListControlManager);
    }
    // Update is called once per frame


    private IEnumerator BindManager<T>(string _managerGameName = "") where T : InstanceScript<T>, new()
    {
        if (_managerGameName == "")//如果沒特別取名,則名字是class的名字
        {
            _managerGameName = typeof(T).Name;
        }
        GameObject sp = new GameObject(_managerGameName);
        sp.transform.SetParent(MainManagerParent.transform);
        T t = sp.AddComponent<T>();
        yield return t.DontDestoryThis();//這裡會呼叫管理員的init,要拿一些父物件等等，如popup管理員的PopupWindow當做彈窗的父物件
        // t.ShowGameObjectName();
    }



}
