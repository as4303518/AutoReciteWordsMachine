using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpecialFunc : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator CallBackExecute(Action callback,float waitForSceond=0.05f)
    {

        while (true)
        {
            callback();
            yield return new WaitForSeconds(waitForSceond);
        }


    }
}
