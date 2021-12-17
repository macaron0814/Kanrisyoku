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

    public static int damageValue; //プレイヤーに与えるダメージ量

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

    [Header("デバック用")]
    [SerializeField] bool isWait;

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

        if (isWait) return;
        if (bossName == Boss_Name.JOSHI)  StartCoroutine(BossAction1());
        if (bossName == Boss_Name.SYACHO) StartCoroutine(BossAction2());
        if (bossName == Boss_Name.KAICHO) StartCoroutine(BossAction3());
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
        damageValue = 30;

        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            //死んでたら処理をやめる
            if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { yield break; }

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
        damageValue = 60;

        while (true)
        {
            yield return new WaitForSeconds(7.5f);

            //死んでたら処理をやめる
            if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { yield break; }

            int cnt = Random.Range(0,3);
            switch (cnt)
            {
                case 0:
                    yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 1.0f),15,true));
                    break;
                case 1:
                    yield return StartCoroutine(ActionType(1, "isAction3", (1.5f, 2.5f), 15, true));
                    break;
                case 2:
                    yield return StartCoroutine(ActionType(2, "isAction2", (0.75f, 0.75f), 20));
                    break;
            }
        }
    }



    /// <summary>
    /// 会長の攻撃パターン
    /// </summary>
    /// <returns></returns>
    IEnumerator BossAction3()
    {
        damageValue = 100;

        while (true)
        {
            yield return new WaitForSeconds(10.0f);

            //死んでたら処理をやめる
            if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { yield break; }

            int cnt = 2;
            switch (cnt)
            {
                case 0:
                    yield return StartCoroutine(ActionType(0, "isAction1", (1.0f, 1.0f), 30, false, true));
                    break;
                case 1:
                    yield return StartCoroutine(ActionType(1, "isAction2", (1.5f, 2.5f), 15, true));
                    break;
                case 2:
                    yield return StartCoroutine(ActionType(2, "isAction3", (0.75f, 0.75f), 20, false, true));
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
    IEnumerator ActionType(int num, string animName, (float, float) speed, int shotCount, bool isSyacho = false, bool isKaicho = false)
    {
        int actionCount = 0;

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

                    if (isKaicho)
                    {
                        int[] rand = { 0, 3 };
                        num = rand[Random.Range(0, rand.Length)];
                    }

                    //コの生成
                    GameObject bullet = Instantiate(kotodama[num], bullet1Pos[Random.Range(0, bullet1Pos.Length)], Quaternion.identity);
                    Destroy(bullet, 5);

                    //角度
                    if (isSyacho) SyachoAction(bullet.transform);

                    //角度(裏)
                    else if (isKaicho)
                    {
                        if(num == 0) SyachoAction(bullet.transform);
                        if(num == 3) KaichoAction(bullet.transform);
                    }
                    else StartCoroutine(Sound.SoundPlaySEforCountDown(13, 1.0f));

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
                    //社長の場合
                    if (isSyacho) num = Random.Range(4, 6);

                    //死んでたら処理をやめる
                    if (bossPram == Boss_Parameter.DEATH || GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { Generic.DestroyTag("Shot"); yield break; }

                    GameObject bullet2 = null;
                    if(isSyacho) bullet2 = Instantiate(kotodama[num], new Vector3(Random.Range(5, 17), kotodama[num].transform.localPosition.y, 1), Quaternion.identity);
                    else         bullet2 = Instantiate(kotodama[num], new Vector3(Random.Range(9, 17), kotodama[num].transform.localPosition.y, 1), Quaternion.identity);
                    Destroy(bullet2, 7);

                    if (speed.Item1 == speed.Item2) yield return new WaitForSeconds(speed.Item1);
                    else yield return new WaitForSeconds(Random.Range(speed.Item1, speed.Item2));

                    actionCount++;
                }

                anim.SetBool(animName, false);
                break;

            //ダ
            case 2:
                anim.SetBool(animName, true);
                StartCoroutine(Sound.SoundPlaySEforCountDown(30, 1.0f));
                yield return new WaitForSeconds(2.0f);

                GameObject bullet3 = Instantiate(kotodama[num], new Vector3(0, 0, 1), Quaternion.identity);
                GameObject bulletParent = null;
                Animator b_animParent = null;

                if (isKaicho)
                {
                    bulletParent = bullet3;
                    bullet3 = bullet3.transform.GetChild(0).gameObject;

                    b_animParent = bulletParent.GetComponent<Animator>();
                    b_animParent.enabled = false;
                }

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
                        if (isKaicho)
                        {
                            lm.transform.localPosition += bulletParent.transform.localPosition;
                            b_animParent.enabled = true;
                        }

                        Sound.SoundPlaySE(29);
                        yield return new WaitForSeconds(speed.Item1);

                        actionCount++;
                    }
                    b_anim.SetBool("isEnd", true);

                    yield return null;
                }
                Destroy(bullet3, 4);

                anim.SetBool(animName, false);
                break;

            default:
                break;
        }
    }



    /// <summary>
    /// 社長、会長専用アクション
    /// </summary>
    /// <param name="bossTrans">社長のTransform</param>
    private void SyachoAction(Transform bossTrans)
    {
        Transform playerTrans;
        playerTrans = player.transform;

        Vector3[] vecUp = { new Vector3(0, 0, -15), new Vector3(0, 0, -35) };
        Vector3[] vecDown = { new Vector3(0, 0, 15), new Vector3(0, 0, 35) };
        Vector3[] vecCenterUp = { new Vector3(0, 0, -15), new Vector3(0, 0, -20) };
        Vector3[] vecCenterDown = { new Vector3(0, 0, 15), new Vector3(0, 0, 20) };

        if (Random.Range(0, 100) % 2 == 0)
        {
            //上下
            if (bossTrans.localPosition == bullet1Pos[0] && playerTrans.localPosition.y <= 0) bossTrans.Rotate(vecDown[Random.Range(0, vecDown.Length)]);
            if (bossTrans.localPosition == bullet1Pos[2] && playerTrans.localPosition.y > 0) bossTrans.Rotate(vecUp[Random.Range(0, vecUp.Length)]);

            //真ん中
            if (bossTrans.localPosition == bullet1Pos[1] && playerTrans.localPosition.y <= 0) bossTrans.Rotate(vecCenterDown[Random.Range(0, vecCenterDown.Length)]);
            if (bossTrans.localPosition == bullet1Pos[1] && playerTrans.localPosition.y > 0) bossTrans.Rotate(vecCenterUp[Random.Range(0, vecCenterUp.Length)]);
        }
        StartCoroutine(Sound.SoundPlaySEforCountDown(13, 1.0f));
    }



    /// <summary>
    /// 会長専用アクション
    /// </summary>
    /// <param name="bossTrans">会長のTransform</param>
    private void KaichoAction(Transform bossTrans)
    {
        Vector3 playerPos;
        playerPos = player.transform.localPosition;

        bool isRot = false;

        foreach (Transform bossChildTrans in bossTrans)
        {
            Vector3[] vecUp = { new Vector3(0, 0, -15), new Vector3(0, 0, -35) };
            Vector3[] vecDown = { new Vector3(0, 0, 15), new Vector3(0, 0, 35) };
            Vector3[] vecCenterUp = { new Vector3(0, 0, -15), new Vector3(0, 0, -20) };
            Vector3[] vecCenterDown = { new Vector3(0, 0, 15), new Vector3(0, 0, 20) };

            //上
            if (bossTrans.localPosition == bullet1Pos[2])
            {
                if (!isRot) isRot = true;
                else bossChildTrans.Rotate(vecUp[1]);
            }

            //真ん中
            if (bossTrans.localPosition == bullet1Pos[1])
            {
                if (!isRot) { bossChildTrans.Rotate(vecCenterUp[1]); isRot = true; }
                else bossChildTrans.Rotate(vecCenterDown[1]);
            }

            //下
            if (bossTrans.localPosition == bullet1Pos[0])
            {
                if (!isRot) isRot = true;
                else bossChildTrans.Rotate(vecDown[Random.Range(0, vecUp.Length)]);
            }
        }
        StartCoroutine(Sound.SoundPlaySEforCountDown(36, 1.0f));
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
        anim.SetBool("isAction3", false);
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
            anim.SetBool("isAction3", false);

            yield return new WaitForSeconds(4.0f);
        }

        //死亡アニメーション処理
        anim.SetBool("isAction1", false);
        anim.SetBool("isAction2", false);
        anim.SetBool("isAction3", false);
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

        //生成と削除
        GameObject ex = Instantiate(explosion,new Vector3(pos.x + Random.Range(-0.25f, 1.0f), pos.y + Random.Range(-0.5f, 4.0f), pos.z),Quaternion.identity);
        Destroy(ex, 0.75f);

        //爆破後に煙を生成
        GameObject smk = Instantiate(smoke, ex.transform.localPosition, Quaternion.identity);
        smk.transform.parent = Camera.main.transform;

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
