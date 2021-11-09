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
}
