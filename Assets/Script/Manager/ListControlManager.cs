using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListControlManager : InstanceScript<ListControlManager>
{
    public Button CreateList;

    public GameObject ViewPanel;

    public GameObject ListObject;


    private void Awake()
    {

        MonoScript();

    }
    
    // public void Init()
    // {
    //     MonoScript();
    // }

    public void CreateListObject()
    {
        GameObject sp = Instantiate(ListObject);
        sp.transform.SetParent(ViewPanel.transform);
        sp.transform.localScale = new Vector3(1, 1, 1);
        sp.GetComponent<ListCard>().Init(DataManager.Instance.saveData.AddNewList("第一課"));

    }

}
