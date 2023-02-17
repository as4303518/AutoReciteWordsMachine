using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FilterScript : MonoBehaviour
{
    public int FilterNum;

    public List<int>ChildWindow=new List<int>();//母彈窗的子視窗們，方便一起關閉，也不會關到其他視窗

    public void Init(int _filterNum){
        FilterNum=_filterNum;
    }

    public void SetButtonFunc(UnityAction _callBack)
    {
        transform.GetComponent<Button>().onClick.AddListener(_callBack);
    }
}
