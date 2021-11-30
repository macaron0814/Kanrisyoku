using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generic
{
    /// <summary>
    /// ダメージを受けた際の点滅処理
    /// </summary>
    /// <returns>0.1秒ごとに表示非表示を切り替える</returns>
    public static IEnumerator DamageFlash(SpriteRenderer sr, float waitTime)
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(waitTime);

            sr.enabled = !sr.enabled;
        }
    }



    /// <summary>
    /// 不要なオブジェクトをタグで削除
    /// </summary>
    /// <param name="tagName">タグの名前</param>
    public static void DestroyTag(string tagName)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(tagName)) { GameObject.Destroy(obj); }
    }



    /// <summary>
    /// オブジェクトを揺らす
    /// </summary>
    /// <param name="duration">  持続時間                  </param>
    /// <param name="magnitude"> 揺れの大きさ              </param>
    /// <param name="obj">       揺らす対象のオブジェクト  </param>
    /// <param name="isUI">      UIなのかどうか            </param>
    /// <returns>                0.1秒ごとに処理を繰り返す </returns>
    public static IEnumerator Shake(float duration, float magnitude, GameObject obj, bool isUI = false)
    {
        Vector3 pos;

        if (!isUI) { pos = obj.transform.localPosition; }
        else { pos = obj.GetComponent<RectTransform>().position; }

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            if (!isUI) { obj.transform.localPosition = new Vector3(x, y, pos.z); }
            else { obj.GetComponent<RectTransform>().position = new Vector3(x, y, pos.z); }

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (!isUI) { obj.transform.localPosition = pos; }
        else { obj.GetComponent<RectTransform>().position = pos; }
    }


    /// <summary>
    /// 2次ベジェ曲線
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">中間点</param>
    /// <param name="p2">終着点</param>
    /// <param name="t">Lerp(a,b)どっちによっているかの比率(a = 0,b = 1)</param>
    /// <returns>実際に移動する座標</returns>
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        var a = Vector3.Lerp(p0, p1, t); // p0からp1の動き
        var b = Vector3.Lerp(p1, p2, t); // p1からp2の動き

        return Vector3.Lerp(a, b, t); // aからbの動き
    }
}



public static class RandomFixedValue
{
    public static Vector3 forVector3(Vector3[] vec)
    {
        Random.InitState(System.DateTime.Now.Second);
        int rand = Random.Range(0, vec.Length);
        for (int i = 0; i < vec.Length; i++) if (i == rand) return vec[i];
        return Vector3.zero;
    }

    public static float forFloat(float[] value)
    {
        Random.InitState(System.DateTime.Now.Second);
        int rand = Random.Range(0, value.Length);
        for (int i = 0; i < value.Length; i++) if (i == rand) return value[i];
        return 0;
    }

    public static int forInt(int[] value)
    {
        Random.InitState(System.DateTime.Now.Second);
        int rand = Random.Range(0, value.Length);
        for (int i = 0; i < value.Length; i++) if (i == rand) return value[i];
        return 0;
    }
}
