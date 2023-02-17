using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : InstanceScript<GameManager>
{
    // 紀錄場景轉換
    public string CurScene=null;
    void Awake()
    {
        DontDestoryThisGame();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
