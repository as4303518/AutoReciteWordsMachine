using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestTwo : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityAction bb;

    public void Init(ref UnityAction _aa)
    {
        bb = _aa;
        bb+=Btest;

    }

    private void Btest(){

        Debug.Log("我是腳本b的btest");
    }
}
