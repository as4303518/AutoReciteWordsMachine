using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestTwo : MonoBehaviour
{
    // Start is called before the first frame update



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
