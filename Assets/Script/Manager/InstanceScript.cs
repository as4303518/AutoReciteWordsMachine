using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceScript<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    // Start is called before the first frame update

    public static T Instance = null;


    public string ShowGameObjectName()
    {
        Debug.Log(typeof(T).Name);
        return this.gameObject.name;
    }

    public void MonoScript()
    {
        if (Instance == null)
        {
            Debug.Log("實例是空");
            Instance = this as T;
        }
        else
        {
            Debug.Log("實力是有"+Instance.gameObject.name);
            Destroy(this);
        }

    }

    public IEnumerator DontDestoryThis()//不能刪除的要在MonoScript初始化,通常dontdestory的組件都在一開始就加載好
    {
        MonoScript();
        yield return Init();

        DontDestroyOnLoad(Instance.transform.root);

    }

    public virtual IEnumerator Init()
    {
        yield return null;
    }


}






