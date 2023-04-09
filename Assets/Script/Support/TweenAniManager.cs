using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TweenAniManager : MonoBehaviour
{

    public static IEnumerator TransparentInGroup(GameObject Parent, float duration = 0.5f, bool GiveSettingColorDefault = true)
    {

        Debug.Log("執行tween in");
        CanvasGroup Cg;
        if (Parent.GetComponent<CanvasGroup>() == null)
        {
            Cg = Parent.AddComponent<CanvasGroup>();
        }
        else
        {
            Cg = Parent.GetComponent<CanvasGroup>();
        }
        Cg.alpha = 0f;
        DOTween.To(() => Cg.alpha, x => Cg.alpha = x, 1, duration);
        yield return new WaitForSeconds(duration);

    }

    public static IEnumerator TransparentOutGroup(GameObject Parent, float duration = 0.5f)
    {
        CanvasGroup Cg;
        if (Parent.GetComponent<CanvasGroup>() == null)
        {
            Cg = Parent.AddComponent<CanvasGroup>();
        }
        else
        {
            Cg = Parent.GetComponent<CanvasGroup>();
        }
        Cg.alpha = 1f;
        DOTween.To(() => Cg.alpha, x => Cg.alpha = x, 0, duration);

        yield return new WaitForSeconds(duration);

    }

    public static IEnumerator SetTransparentGroup(GameObject Parent, float StartTransparentValue, float EndTransparentValue, float duration = 0.5f)
    {
        CanvasGroup Cg;
        if (Parent.GetComponent<CanvasGroup>() == null)
        {
            Cg = Parent.AddComponent<CanvasGroup>();
        }
        else
        {
            Cg = Parent.GetComponent<CanvasGroup>();
        }
        Cg.alpha = StartTransparentValue;
        DOTween.To(() => Cg.alpha, x => Cg.alpha = x, EndTransparentValue, duration);

        yield return new WaitForSeconds(duration);

    }


    public static IEnumerator ColorOrTransparentChange<T>(T _object, Color startColor, Color endColor, Action CallBack = null, float duration = 0.15f) where T : MaskableGraphic
    {

        _object.color = startColor;
        Sequence sq = DOTween.Sequence();
        sq.Append(_object.DOColor(endColor, duration)).AppendCallback(() => { CallBack(); });
        yield return sq.WaitForCompletion();

    }

    public static IEnumerator ColorOrTransparentChange<T>(T _object, Color endColor, Action CallBack = null, float duration = 0.15f) where T : MaskableGraphic
    {

        yield return ColorOrTransparentChange(_object, _object.color, endColor, CallBack, duration);
        // Tween tween = _object.DOColor(endColor, duration);
        // yield return tween.WaitForCompletion();
    }

    public static IEnumerator ByMoveDoTween(GameObject Obj, Vector3 StartPos, Vector3 EndPos, Action Callback = null, float dur = 0.5f)
    {

        Obj.GetComponent<RectTransform>().localPosition = StartPos;
        Sequence sq = DOTween.Sequence();
        sq.Append(Obj.transform.DOLocalMove(StartPos + EndPos, dur)).AppendCallback(() => { Callback(); });


        yield return sq.WaitForCompletion();

    }
    public static IEnumerator ByMoveDoTween(GameObject Obj, Vector3 EndPos, Action Callback = null, float dur = 0.5f)
    {


        yield return ByMoveDoTween(Obj, Obj.GetComponent<RectTransform>().localPosition, EndPos, Callback, dur);
        // Sequence sq=DOTween.Sequence();
        // sq.Append(Obj.transform.DOLocalMove(Obj.GetComponent<RectTransform>().localPosition+EndPos,dur))
        // .AppendCallback(()=>{Callback();});


        // yield return sq.WaitForCompletion();

    }



}
