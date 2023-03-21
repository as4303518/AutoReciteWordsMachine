using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LanguageDataSetting))]
public class LanguageDataEditor : Editor
{

    private LanguageDataSetting LDS;


    void OnEnable()
    {

        LDS = target as LanguageDataSetting;

    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        // EditorGUI.BeginFoldoutHeaderGroup()
        GUILayout.Space(10);
        if (GUILayout.Button("myTest"))
        {
            Debug.Log("完全覆寫");
            LDS.SetJsonToPath();

        }
        GUILayout.Space(10);
        if (GUILayout.Button("overrideTest"))
        {
            Debug.Log("覆寫有的參數");
            LDS.ReviseJsonFile();

        }

        GUILayout.Space(10);
        if (GUILayout.Button("ReadTest"))
        {
            Debug.Log("讀取有的檔案");
            LDS.OverrideSaveDataOfJson();

        }





    }

}
