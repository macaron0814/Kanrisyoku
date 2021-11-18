using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] kotodama = new GameObject[4];

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject explosion;

    [SerializeField] private double bossHP;
    private static double hp;
    private double explosionHp;
    private float colorHp;

    private Vector3[] bullet1Pos = { new Vector3(5, 3, 0), new Vector3(5, 0, 0), new Vector3(5, -3, 0) };

    enum Boss_Parameter
    {
        ALIVE,
        DEATH
    }
    Boss_Parameter bossPram;

    // Start is called before the first frame update
    void Start()
    {
        bossPram = Boss_Parameter.ALIVE;

        hp = bossHP;
        explosionHp = bossHP / 4;
        colorHp = 1 / (float)bossHP;

        StartCoroutine(BossAction1());
    }

    private void Update()
    {
        //ダメージを受ける度赤くなっていく
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
        //死んでたら処理をやめる
        if(bossPram == Boss_Parameter.DEATH) { yield break; }

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


    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(double damage)
    {
        hp -= damage;
        if(hp <= 0) Death();
    }


    /// <summary>
    /// 死んだときの処理
    /// </summary>
    private void Death()
    {
        bossPram = Boss_Parameter.DEATH;
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.BOSSRESULT;

        anim.SetBool("isAction1", false);
        anim.SetBool("isAction2", false);

        for (int i = 0; i < 5; i++) { Explosion(true); }
    }

    /// <summary>
    /// 爆破Effect生成
    /// </summary>
    public void Explosion(bool isDeath = false)
    {
        //死んでなければ体力での処理
        if (!isDeath)
        {
            if (hp >= (bossHP - explosionHp)) return;
            explosionHp += bossHP / 4;
        }

        //生成に必要な情報
        Vector3 pos = transform.localPosition;
        Random.InitState(System.DateTime.Now.Second);

        //生成と削除
        GameObject ex = Instantiate(explosion,new Vector3(pos.x + Random.Range(-0.25f, 0.5f), pos.y + Random.Range(-0.5f, 2.0f), pos.z),Quaternion.identity);
        Destroy(ex, 0.75f);

        //爆破後に煙を生成
        GameObject smk = Instantiate(smoke, ex.transform.localPosition, Quaternion.identity);
        smk.transform.parent = this.transform;

        //振動
        StartCoroutine(Generic.Shake(0.4f, 0.1f, Camera.main.gameObject));
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
