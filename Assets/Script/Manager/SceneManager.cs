using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//控制轉場切換的管理員
public class SceneManager : InstanceScript<SceneManager>
{

    //儲存開過的場景，可以控制場景

    public GameObject SceneParent = null;

    public GameObject test = null;

    private Dictionary<SceneType, PrefabScene> mSceneList = new Dictionary<SceneType, PrefabScene>();
    [SerializeField] private List<SceneType> mSceneSquence = new List<SceneType>();//儲存經過的場景

    private Dictionary<SceneType, string> ScenePathOfResources = new Dictionary<SceneType, string>(){
    {SceneType.ListControlManager,ResourcesPath.PrefabScenePath+"ListWordScene/WordListGroupPage"},
    {SceneType.WordControlManager,ResourcesPath.PrefabScenePath+"WordScene/WordListPage"},
};

    public override IEnumerator Init()
    {

        SceneParent = GameObject.Find("PrefabSceneLayer");
        mSceneSquence.Add(SceneType.Non);
        mSceneList.Add(SceneType.Non, null);
        yield return null;
    }
    public void StartChangScene(SceneType goToScene, BaseData _data = null)
    {
        StartCoroutine(ChangeScene(goToScene,_data));

    }




    public IEnumerator ChangeScene(SceneType goToScene, BaseData _data = null)
    {
        if (mSceneSquence.Count > 5)
        {//清除空間，不要儲存太多
            mSceneSquence.RemoveAt(0);
        }
        yield return PopupManager.Instance.OpenLoading();

        Debug.Log("測試" + mSceneList[GetNowScene()]);

        test = Resources.Load<GameObject>(ScenePathOfResources[SceneType.WordControlManager]);

        yield return mSceneList[GetNowScene()] == null ? null : mSceneList[GetNowScene()].PageTweenOut();

        ResourceRequest rq = Resources.LoadAsync<GameObject>(ScenePathOfResources[goToScene]);
        yield return new WaitUntil(()=>rq.isDone);
        GameObject scene = Instantiate(rq.asset, SceneParent.transform) as GameObject;

        CanvasGroup Cg;
        if (scene.GetComponent<CanvasGroup>() == null)
        {
            Cg = scene.AddComponent<CanvasGroup>();
        }
        else
        {
            Cg = scene.GetComponent<CanvasGroup>();
        }

        Cg.alpha = 0f;
        PrefabScene preScene = scene.GetComponent<PrefabScene>();
        preScene.MonoScript();
        mSceneSquence.Add(goToScene);
        mSceneList.Add(goToScene, preScene);
        Debug.Log("測試6");
        yield return preScene.Init(_data);
        Debug.Log("測試7");
        yield return preScene.PageTweenIn();
        Debug.Log("測試8");
        yield return PopupManager.Instance.CloseLoading();

    }



    public SceneType GetNowScene()
    {
        if (mSceneSquence.Count <= 0)
        {
            return SceneType.Non;
        }
        return mSceneSquence[mSceneSquence.Count - 1];
    }

    public SceneType GetLastScene()
    {
        if (mSceneSquence.Count <= 0)
        {
            return SceneType.Non;
        }
        return mSceneSquence[mSceneSquence.Count - 2];
    }

    public PrefabScene GetNowScenePrefabs()
    {

        return mSceneList[mSceneSquence[mSceneSquence.Count - 1]];
    }

    public enum SceneType
    {//儲存能開啟的場景
        Non,
        ListControlManager,
        WordControlManager

    }
}
