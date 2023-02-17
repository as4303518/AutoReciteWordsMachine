using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceScript<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    // Start is called before the first frame update

    public static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }

    }

    public void MonoScript()
    {
        if (instance == null)
        {
            instance = transform.GetComponent<T>();
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
