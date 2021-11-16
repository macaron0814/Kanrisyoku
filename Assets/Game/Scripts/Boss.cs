using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] kotodama = new GameObject[4];

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private double bossHP;
    private static double hp;
    private float colorHp;

    private Vector3[] bullet1Pos = { new Vector3(5, 3, 0), new Vector3(5, 0, 0), new Vector3(5, -3, 0) };

    // Start is called before the first frame update
    void Start()
    {
        hp = bossHP;
        colorHp = 1 / (float)bossHP;

        StartCoroutine(BossAction1());
    }

    private void Update()
    {
        var color = colorHp * (float)hp;
        spriteRenderer.color = new Color(1, color, color, 1);
    }

    IEnumerator BossAction1()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            Random.InitState(System.DateTime.Now.Second);

            int cnt = Random.Range(0, 2);
            switch (cnt)
            {
                case 0:
                    yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 2.5f)));
                    break;
                case 1:
                    yield return StartCoroutine(ActionType(1, "isAction2", (2.0f, 2.0f)));
                    break;
            }
        }
    }

    IEnumerator ActionType(int num, string animName, (float, float) speed)
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

                    StartCoroutine(Sound.SoundPlaySEforCountDown(13, 1.0f));

                    yield return new WaitForSeconds(Random.Range(speed.Item1, speed.Item2));

                    actionCount++;
                }
                anim.SetBool(animName, false);
                break;

            //ト
            case 1:

                anim.SetBool(animName, true);
                yield return new WaitForSeconds(2.0f);

                while (actionCount < 7)
                {
                    Random.InitState(System.DateTime.Now.Second);

                    GameObject bullet2 = Instantiate(kotodama[1], new Vector3(Random.Range(9, 17), kotodama[1].transform.localPosition.y, 1), Quaternion.identity);
                    Destroy(bullet2, 7);

                    yield return new WaitForSeconds(speed.Item1);

                    actionCount++;
                }

                anim.SetBool(animName, false);
                break;

            default:
                break;
        }
    }

    public static void Damage(double damage)
    {
        hp -= damage;
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
