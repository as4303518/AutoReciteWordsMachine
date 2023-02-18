using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestTwo : MonoBehaviour
{
    // Start is called before the first frame update



    public IEnumerator Init( UnityAction<string> _aa)
    {
    
        yield return new WaitForSeconds(3);
        _aa("回傳參數給你成功");

    }

    public IEnumerator aaa(){
        Debug.Log("成功執行a");
        yield return new WaitForSeconds(1);
    }


}
