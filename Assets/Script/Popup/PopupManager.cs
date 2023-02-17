using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PopupManager : InstanceScript<PopupManager>
{

public GameObject FilterParentCanvas=null;
public GameObject Filter=null;

//public List<GameObject> PopupList=new List<GameObject>();

private int PopupNum=0;
public Dictionary<int,GameObject> PopupList=new Dictionary<int,GameObject>();

public void Awake(){
DontDestoryThis();


}

public void OpenPopup(GameObject _popup,UnityAction _callBack=null){

if(FilterParentCanvas==null)FilterParentCanvas=GameObject.Find("PopupLayer");

GameObject sp=Instantiate(_popup);

GameObject filter=Instantiate(Filter);
filter.GetComponent<FilterScript>().Init(PopupNum);


sp.transform.localScale= Vector3.one;

filter.transform.SetParent(FilterParentCanvas.transform);
sp.transform.SetParent(filter.transform);

PopupList.Add(PopupNum,filter);

if(_callBack!=null)filter.GetComponent<FilterScript>().SetButtonFunc(_callBack);
PopupNum++;
}

public void ClosePopup(){}




}
