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
        Drop//拖曳字卡

    }

// public class PrefabScene<U> : InstanceScript<U> where U: MonoBehaviour,new()
// {
//     // Start is called before the first frame update



//     public virtual IEnumerator Init(BaseData _data)
//     {

//         Debug.Log("預設init出現(這個log不應該出現)" + gameObject.name);
//         yield return null;

//     }



//     public virtual IEnumerator PageTweenOut()
//     {
//         yield return null;
//     }
//     public virtual IEnumerator PageTweenIn()
//     {
//         yield return null;
//     }

//     protected enum ManagerStatus
//     {
//         Invalid,//切換場景的過場無法使用功能
//         Normal,//切換場景的過場
//         Delete,//刪除模式，可以刪除list
//         Drop//拖曳字卡

//     }
// }
