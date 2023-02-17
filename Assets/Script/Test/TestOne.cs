using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestOne : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityAction aa;

    public TestTwo two;
    void Awake()
    {
        two.Init(ref aa);
        aa += Atest;

    }
    void Start()
    {
        aa();
    }
    private void Atest()
    {

        Debug.Log("我是腳本A的Atest");
    }

}
