using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenAniManager : MonoBehaviour
{

    public static IEnumerator TransparentInGroup(GameObject Parent, float duration = 0.5f, bool GiveSettingColorDefault = true)
    {
        Debug.Log("已執行");
        Image[] imgs = Parent.GetComponentsInChildren<Image>();
        Text[] texts = Parent.GetComponentsInChildren<Text>();

        foreach (Image img in imgs)
        {
            Color c = img.color;
            if (img.color.a > 0)
            {
                if (GiveSettingColorDefault)
                {
                    img.color = new Color(c.r, c.g, c.b, 0);
                }

                img.DOColor(new Color(c.r, c.g, c.b, 1), duration).SetEase(Ease.Linear);
            }
        }

        foreach (Text text in texts)
        {
            Color c = text.color;
            if (text.color.a > 0)
            {
                if (GiveSettingColorDefault)
                {
                    text.color = new Color(c.r, c.g, c.b, 0);
                }

                text.DOColor(new Color(c.r, c.g, c.b, 1), duration).SetEase(Ease.Linear);
            }
        }
        yield return new WaitForSeconds(duration);

    }

    public static IEnumerator TransparentOutGroup(GameObject Parent, float duration = 0.5f, bool GiveSettingColorDefault = true)
    {
        
        Debug.Log("已執行");
        Image[] imgs = Parent.GetComponentsInChildren<Image>();
        Text[] texts = Parent.GetComponentsInChildren<Text>();

        foreach (Image img in imgs)
        {
            Color c = img.color;
            if (img.color.a > 0)
            {
                if (GiveSettingColorDefault)
                {
                    img.color = new Color(c.r, c.g, c.b, 1);
                }
                img.DOColor(new Color(c.r, c.g, c.b, 0.001f), duration).SetEase(Ease.Linear);
            }
        }

        foreach (Text text in texts)
        {
            Color c = text.color;
            if (text.color.a > 0)
            {
                if (GiveSettingColorDefault)
                {
                    text.color = new Color(c.r, c.g, c.b, 1);
                }
                text.DOColor(new Color(c.r, c.g, c.b, 0.001f), duration).SetEase(Ease.Linear);
            }
        }
        
        yield return new WaitForSeconds(duration);
  
        
    }
    public static IEnumerator TransparentChange<T>(T _object,Color startColor,Color endColor,float duration=0.5f)where T:MaskableGraphic{

        _object.color=startColor;
        Tween tween=_object.DOColor(endColor,duration);
        yield return tween.WaitForCompletion();


    }





}
