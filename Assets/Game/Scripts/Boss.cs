using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] kotodama = new GameObject[4];

    private Vector3[] bullet1Pos = { new Vector3(5, 3, 0), new Vector3(5, 0, 0), new Vector3(5, -3, 0) };

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BossAction1());
    }

    IEnumerator BossAction1()
    {
        while(true)
        {
            yield return new WaitForSeconds(10.0f);

            yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 2.5f)));
        }
    }

    IEnumerator ActionType(int num, string animName, (float,float) speed)
    {
        int actionCount = 0;

        switch (num)
        {
            //コ
            case 0:

                anim.SetBool(animName, true);
                yield return new WaitForSeconds(2.0f);

                while (actionCount < 10)
                {                  
                    Random.InitState(System.DateTime.Now.Second);

                    GameObject bullet = Instantiate(kotodama[0], bullet1Pos[Random.Range(0, bullet1Pos.Length)], Quaternion.identity);
                    Destroy(bullet, 5);

                    yield return new WaitForSeconds(Random.Range(speed.Item1, speed.Item2));

                    actionCount++;
                }
                anim.SetBool(animName, false);
                break;

            default:
                break;
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
            StartCoroutine(Generic.DamageFlash(GetComponent<SpriteRenderer>(),0.1f));
        }
    }
}
