using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.1f);

            sr.enabled = !sr.enabled;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"> 接触したコライダー</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "KohaiRocket" && col.GetComponent<KohaiRocket>().isRocket)
        {
            StartCoroutine(DamageFlash());
        }
    }
}
