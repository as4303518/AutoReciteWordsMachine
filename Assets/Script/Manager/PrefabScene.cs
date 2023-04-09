using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public  interface PrefabScene
{
    // Start is called before the first frame update
    public abstract IEnumerator Init(BaseData _data);

    public abstract IEnumerator PageTweenOut();
 
    public abstract IEnumerator PageTweenIn();


}
 public enum ManagerStatus
    {
        Invalid,//切換場景的過場無法使用功能
        Normal,//切換場景的過場
        Delete,//刪除模式，可以刪除list
        Drop,//拖曳字卡
        Search

    }


