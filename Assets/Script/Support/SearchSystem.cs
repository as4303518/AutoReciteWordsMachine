using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class SearchSystem
{


    public static List<T> SortOfCondition<T>(this List<T> mList, Func<T, bool> Condition)//透過某個條件塞選
    {
        List<T> Temp = new List<T>();

        Temp.ForEach(v =>
        {

            if (Condition(v))
            {

                Temp.Add(v);
            }
        });

        return Temp;
    }



}
