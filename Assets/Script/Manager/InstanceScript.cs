using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceScript<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    // Start is called before the first frame update

    public static T Instance = null;



    public void MonoScript()
    {
        if (Instance == null)
        {
            Instance = transform.GetComponent<T>();
        }
        else
        {
            Destroy(this);
        }

    }

    public void DontDestoryThis()
    {
        MonoScript();
        DontDestroyOnLoad(Instance);
    }

}
