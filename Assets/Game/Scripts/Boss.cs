using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameModeConfig gameModeConfig;
    private GameObject player;

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] kotodama = new GameObject[4];
    [SerializeField] private GameObject   landmine;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject flash;

    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject explosion;

    [SerializeField] private double bossHP;
    public static double hp;
    private double explosionHp;
    private float  colorHp;

    private Vector3[] bullet1Pos = { new Vector3(5, 3, 0), new Vector3(5, 0, 0), new Vector3(5, -3, 0) };

    public enum Boss_Name
    {
        JOSHI,
        SYACHO,
        KAICHO
    }
    public Boss_Name bossName;

    public enum Boss_Parameter
    {
        ALIVE,
        DEATH
    }
    public static Boss_Parameter bossPram;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        bossPram = Boss_Parameter.ALIVE;

        BossScore.b_point.bossHp = (int)bossHP;
        hp = bossHP;
        explosionHp = bossHP / 4;
        colorHp = 1 / (float)bossHP;

        if (bossName == Boss_Name.JOSHI)  StartCoroutine(BossAction1());
        if (bossName == Boss_Name.SYACHO) StartCoroutine(BossAction2());
    }

    private void Update()
    {
        //ダメージを受ける度赤くなっていく
        var color = colorHp * (float)hp;
        spriteRenderer.color = new Color(1, color, color, 1);
    }

    /// <summary>
    /// 上司の攻撃パターン
    /// </summary>
    /// <returns></returns>
    IEnumerator BossAction1()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            //死んでたら処理をやめる
            if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { yield break; }

            Random.InitState(System.DateTime.Now.Second);

            int cnt = Random.Range(0, 2);
            switch (cnt)
            {
                case 0:
                    yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 2.5f),10));
                    break;
                case 1:
                    yield return StartCoroutine(ActionType(1, "isAction2", (2.0f, 2.0f),7));
                    break;
            }
        }
    }

    /// <summary>
    /// 社長の攻撃パターン
    /// </summary>
    /// <returns></returns>
    IEnumerator BossAction2()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            //死んでたら処理をやめる
            if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { yield break; }

            Random.InitState(System.DateTime.Now.Second);

            int cnt = 2;
            switch (cnt)
            {
                case 0:
                    yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 1.0f),20,true));
                    break;
                case 1:
                    yield return StartCoroutine(ActionType(1, "isAction2", (2.0f, 2.0f),7));
                    break;
                case 2:
                    yield return StartCoroutine(ActionType(2, "isAction2", (0.75f, 0.75f), 25));
                    break;
            }
        }
    }



    /// <summary>
    /// 攻撃パターン
    /// </summary>
    /// <param name="num">番号で攻撃の種類を選択</param>
    /// <param name="animName">再生するAnimationを選択</param>
    /// <param name="speed">攻撃速度</param>
    /// <param name="shotCount">攻撃回数</param>
    /// <param name="isRot">回転するかどうか</param>
    /// <returns></returns>
    IEnumerator ActionType(int num, string animName, (float, float) speed, int shotCount, bool isRot = false)
    {
        int actionCount = 0;
        Transform bossTrans, playerTrans;
        playerTrans = player.transform;

        switch (num)
        {
            //コ
            case 0:

                anim.SetBool(animName, true);
                yield return new WaitForSeconds(2.0f);

                while (actionCount < shotCount)
                {
                    //死んでたら処理をやめる
                    if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { Generic.DestroyTag("Shot"); yield break; }

                    Random.InitState(System.DateTime.Now.Second);

                    //コの生成
                    GameObject bullet = bullet = Instantiate(kotodama[0], bullet1Pos[Random.Range(0, bullet1Pos.Length)], Quaternion.identity);
                    Destroy(bullet, 5);

                    //角度
                    if (isRot)
                    {
                        bossTrans   = bullet.transform;

                        Vector3[] vecUp   = { new Vector3(0, 0, -15), new Vector3(0, 0, -35) };
                        Vector3[] vecDown = { new Vector3(0, 0,  15), new Vector3(0, 0,  35) };
                        Vector3[] vecCenterUp   = { new Vector3(0, 0, -15), new Vector3(0, 0, -20) };
                        Vector3[] vecCenterDown = { new Vector3(0, 0, 15), new Vector3(0, 0,   20) };

                        if (Random.Range(0, 100) % 2 == 0)
                        {
                            //上下
                            if (bossTrans.localPosition == bullet1Pos[0] && playerTrans.localPosition.y <= 0) bossTrans.Rotate(RandomFixedValue.forVector3(vecDown));
                            if (bossTrans.localPosition == bullet1Pos[2] && playerTrans.localPosition.y >  0) bossTrans.Rotate(RandomFixedValue.forVector3(vecUp));

                            //真ん中
                            if (bossTrans.localPosition == bullet1Pos[1] && playerTrans.localPosition.y <= 0) bossTrans.Rotate(RandomFixedValue.forVector3(vecCenterDown));
                            if (bossTrans.localPosition == bullet1Pos[1] && playerTrans.localPosition.y >  0) bossTrans.Rotate(RandomFixedValue.forVector3(vecCenterUp));
                        }
                    }

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

                while (actionCount < shotCount)
                {
                    //死んでたら処理をやめる
                    if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { Generic.DestroyTag("Shot"); yield break; }

                    Random.InitState(System.DateTime.Now.Second);

                    GameObject bullet2 = Instantiate(kotodama[1], new Vector3(Random.Range(9, 17), kotodama[1].transform.localPosition.y, 1), Quaternion.identity);
                    Destroy(bullet2, 7);

                    yield return new WaitForSeconds(speed.Item1);

                    actionCount++;
                }

                anim.SetBool(animName, false);
                break;

            //ダ
            case 2:
                anim.SetBool(animName, true);
                StartCoroutine(Sound.SoundPlaySEforCountDown(30, 1.0f));
                yield return new WaitForSeconds(2.0f);

                GameObject bullet3 = Instantiate(kotodama[2], new Vector3(0, 0, 1), Quaternion.identity);
                Animator b_anim = bullet3.GetComponent<Animator>();

                yield return new WaitForSeconds(5.0f);

                while (true)
                {
                    //死んでたら処理をやめる
                    if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { Generic.DestroyTag("Shot"); yield break; }

                    //弾を撃ち終えたら終了
                    if (b_anim.GetCurrentAnimatorStateInfo(0).IsName("Bullet3_E")) break;

                    //技の処理
                    while (actionCount < shotCount)
                    {
                        //死んでたら処理をやめる
                        if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { Generic.DestroyTag("Shot"); yield break; }

                        GameObject lm = Instantiate(landmine);

                        Sound.SoundPlaySE(29);
                        yield return new WaitForSeconds(speed.Item1);

                        actionCount++;
                    }
                    b_anim.SetBool("isEnd", true);

                    yield return null;
                }
                Destroy(bullet3, 6);

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
        if (hp <= 0)
        {
            hp = 0;
            StartCoroutine(Death());
        }
    }


    /// <summary>
    /// 死んだときの処理
    /// </summary>
    private IEnumerator Death()
    {
        //シーン切り替え処理
        bossPram = Boss_Parameter.DEATH;
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.BOSSRESULT;

        //アニメーション処理
        anim.SetBool("isAction1", false);
        anim.SetBool("isAction2", false);
        flash.SetActive(true);

        //音処理
        Sound.SoundStop();

        //=========================================
        //オブジェクトの生成と削除
        //=========================================
        {
            //弾削除
            Generic.DestroyTag("Shot");

            //爆破生成
            for (int i = 0; i < 5; i++)
            {
                //音処理
                Sound.SoundPlaySE(15);
                Explosion(true);
                yield return new WaitForSeconds(0.5f);
            }

            //Smake削除
            foreach (Transform n in gameObject.transform) Destroy(n.gameObject);

            //音処理
            Sound.SoundPlaySE(16);

            //アニメーション処理
            anim.SetBool("isAction1", false);
            anim.SetBool("isAction2", false);

            yield return new WaitForSeconds(4.0f);
        }

        //死亡アニメーション処理
        anim.SetBool("isAction1", false);
        anim.SetBool("isAction2", false);
        anim.SetBool("isDeath", true);
        yield return new WaitForSeconds(3.0f);

        //振動
        StartCoroutine(Generic.Shake(0.5f, 0.4f, Camera.main.gameObject));
        //音処理
        Sound.SoundPlaySE(15);

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(gameModeConfig.BossResult());
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
            //音処理
            Sound.SoundPlaySE(14);
        }

        //生成に必要な情報
        Vector3 pos = transform.localPosition;
        Random.InitState(System.DateTime.Now.Second);

        //生成と削除
        GameObject ex = Instantiate(explosion,new Vector3(pos.x + Random.Range(-0.25f, 1.0f), pos.y + Random.Range(-0.5f, 4.0f), pos.z),Quaternion.identity);
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
