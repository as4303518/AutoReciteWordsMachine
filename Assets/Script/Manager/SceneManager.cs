using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;







//控制轉場切換的管理員
public class SceneManager : InstanceScript<SceneManager>
{

    //儲存開過的場景，可以控制場景

    public GameObject SceneParent = null;

    public GameObject test = null;

    // private Dictionary<SceneType, GameObject> mSceneList = new Dictionary<SceneType,GameObject>();
    private GameObject CurScene = null;
    [SerializeField] private List<SceneType> mSceneSquence = new List<SceneType>();//儲存經過的場景

    private Dictionary<SceneType, string> ScenePathOfResources = new Dictionary<SceneType, string>(){
    {SceneType.ListControlManager,ResourcesPath.PrefabScenePath+"ListWordScene/WordListGroupPage"},
    {SceneType.WordControlManager,ResourcesPath.PrefabScenePath+"WordScene/WordListPage"},
};

    public override IEnumerator Init()
    {

        SceneParent = GameObject.Find("PrefabSceneLayer");
        mSceneSquence.Add(SceneType.Non);
        // mSceneList.Add(SceneType.Non, null);
        yield return null;
    }
    public void StartChangScene(SceneType goToScene, BaseData _data = null)
    {
        StartCoroutine(ChangeScene(goToScene, _data));

    }




    public IEnumerator ChangeScene(SceneType goToScene, BaseData _data = null)
    {
        if (mSceneSquence.Count > 5)
        {//清除空間，不要儲存太多
            mSceneSquence.RemoveAt(0);
        }
        yield return PopupManager.Instance.OpenLoading();



        test = Resources.Load<GameObject>(ScenePathOfResources[SceneType.WordControlManager]);

        yield return CurScene == null ? null : CurScene.GetComponent<PrefabScene>().PageTweenOut();

        Destroy(CurScene);

        ResourceRequest rq = Resources.LoadAsync<GameObject>( ScenePathOfResources[goToScene] );

        yield return new WaitUntil(() => rq.isDone);

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


        scene.GetComponent<InstanceFunc>().MonoScript();

        CurScene = scene;
        
        mSceneSquence.Add(goToScene);

        // if (_data != null && _data.GetType() == typeof(WordListData))
        // {
        //     Debug.Log("裡面的物件" + (_data as WordListData).mWords.Count);
        // }
        yield return preScene.Init(_data);

        yield return preScene.PageTweenIn();

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

    public GameObject GetNowScenePrefabs()
    {
        return CurScene;
    }

    public enum SceneType
    {//儲存能開啟的場景
        Non,
        ListControlManager,
        WordControlManager

    }





}



