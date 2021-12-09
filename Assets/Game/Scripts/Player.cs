using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameModeConfig gameModeConfig;

    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject effect;

    [SerializeField]
    GameObject kohai;

    [SerializeField]
    GameObject concentrationLine;

    [SerializeField]
    GameObject[] resultType;

    [SerializeField]
    GameObject bossBattleHP;

    private ContactFilter2D filter2d;

    private Rigidbody2D rb;
    private BoxCollider2D box2d;
    private SpriteRenderer spriteRenderer;
    public static bool isJet = false;
    public static bool isGameOver = false;
    private bool isSound;
    public bool isTouched;
    private bool isJump;

    //ダメージ点滅中かの有無
    private float damageFlashTime;

    ItemSystem itemSystem = null;
    WaveConfig waveConfig = null;

    private float autoJumpCount;

    // Start is called before the first frame update
    void Start()
    {
        isJet = false;
        isGameOver = false;
        rb = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemSystem = GameObject.Find("GameModeConfig").GetComponent<ItemSystem>();
        waveConfig = GameObject.Find("WaveConfig").GetComponent<WaveConfig>();

        damageFlashTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameModeConfig.sceneType != GameModeConfig.SCENETYPE.GAME && GameModeConfig.sceneType != GameModeConfig.SCENETYPE.BOSSBATTLE) { return; }
        if (ItemSystem.ramen <= 0) { PlayerDestroy(2); }

        if (isGameOver) { return; }

        PlayerKey();//操作
        PlayerFall();//落下判定
        PlayerAutoJumpCountDown();//オートジャンプ
        PlayerDamegeFrashTime();//無敵時間計測

        if (rb.velocity.y > 10.75f) rb.velocity = new Vector2(0, 10.75f);
    }


    /// <summary>
    /// キー入力操作
    /// </summary>
    void PlayerKey()
    {
        if (isJet) { kohai.SetActive(true); return; }

        isTouched = rb.IsTouching(filter2d);

        //高速着地
        if (!isTouched && Input.GetMouseButtonDown(0))
        {
            spriteRenderer.sortingLayerName = "Player";
            autoJumpCount = 1;
            rb.gravityScale = 7;
            return;
        }

        //重力を戻す
        if (isTouched) { rb.gravityScale = 2; }

        //ジャンプ
        if (isTouched && Input.GetMouseButtonDown(0)) Jump();
    }


    /// <summary>
    /// プレイヤーの落下判定
    /// </summary>
    void PlayerFall()
    {
        if (transform.localPosition.y < -7) { PlayerDestroy(1); }
    }


    /// <summary>
    /// プレイヤーのオートジャンプのカウントダウン
    /// </summary>
    float PlayerAutoJumpCountDown()
    {
        if (autoJumpCount == 0) { return 0; }
        return autoJumpCount -= Time.deltaTime;
    }


    /// <summary>
    /// プレイヤーのジェット開始
    /// </summary>
    void PlayerStartJet()
    {
        Sound.SoundPlaySE(8);
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        kohai.SetActive(true);
        isJet = true;
        waveConfig.jetBeforeScrollSpeed = waveConfig.startScrollSpeed;
        waveConfig.startScrollSpeed = 0.4f;
        box2d.isTrigger = true;
        rb.gravityScale = 0;
        spriteRenderer.sortingLayerName = "Jet";
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        itemSystem.AddStamina(100);
        concentrationLine.SetActive(true);

        Record.UpdateRecord(Record.RecordList.KOHAIJET);

        StartCoroutine(PlayerEndJet());
    }

    /// <summary>
    /// プレイヤーのジェット終了
    /// </summary>
    IEnumerator PlayerEndJet()
    {
        yield return new WaitForSeconds(1f);

        Sound.SoundPlaySE(3);

        yield return new WaitForSeconds(6f);

        Jump(); //ジャンプ

        //変更したパラメータを元に戻す
        waveConfig.startScrollSpeed = waveConfig.jetBeforeScrollSpeed;
        box2d.isTrigger = false;
        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        itemSystem.ResetKohai();
        concentrationLine.SetActive(false);
        isJet = false;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sortingLayerName = "Player";
    }

    /// <summary>
    /// プレイヤーの死亡判定
    /// </summary>
    void PlayerDestroy(int type)
    {
        ItemSystem.gameoverPattern = type;

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spriteRenderer.sortingLayerName = "GameOver";

        anim.SetBool("GameOver", true);
        isGameOver = true;

        if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.GAME)
        {
            GameModeConfig.ChangeResultScene();
            StartCoroutine(ResultFromGameOverandPlaySound(type));
        }
        else if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSBATTLE)
        {
            BossScore.b_point.penalty = true;

            //シーン処理
            StartCoroutine(gameModeConfig.BossResult());

            //弾削除
            Generic.DestroyTag("Shot");
        }
    }

    IEnumerator ResultFromGameOverandPlaySound(int type)
    {
        if (isSound) { yield break; }

        yield return new WaitForSeconds(1f);
        Sound.SoundPlayBGM(1);
        resultType[type].SetActive(true);

        yield return new WaitForSeconds(2.25f);
        Sound.SoundPlaySE(9);
        isSound = true;
    }

    void DestroyEffect()
    {
        effect.SetActive(false);
    }

    /// <summary>
    /// 無敵時間計測
    /// </summary>
    void PlayerDamegeFrashTime()
    {
        damageFlashTime -= Time.deltaTime;
        if (damageFlashTime < 0.0f) { damageFlashTime = 0.0f; }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    void Jump()
    {
        if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) { return; }

        Vector3 force = new Vector3(0.0f, 575.0f, 0.0f);    // 力を設定
        if (rb.velocity.y < 5.75f) rb.AddForce(force);  // 力を加える
        anim.SetBool("Jump", true);
        isJump = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Line")
        {
            if (rb.gravityScale == 7) Sound.SoundPlaySE(6);
            if (isJump && rb.gravityScale == 2) Sound.SoundPlaySE(5);

            anim.SetBool("Jump", false);
            isJump = false;

            if (rb.gravityScale == 7)
            {
                effect.SetActive(true);
                Invoke(nameof(DestroyEffect), 0.25f);
                StartCoroutine(Generic.Shake(0.2f, 0.05f, Camera.main.gameObject));
                itemSystem.AddStamina(-5);

                BossScore.b_point.jump++;
            }

            //オートジャンプ発動
            if (PlayerAutoJumpCountDown() > 0.9f) Jump();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isJet || gameObject.tag != "Player") { return; }

        //顔がビルに激突したらゲームオーバー
        if (other.tag == "Stage")
        {
            PlayerDestroy(0);
        }

        //アイテムの獲得
        if (other.tag == "Coin")
        {
            itemSystem.AddCoin();
            Destroy(other.gameObject);
        }
        if (other.tag == "Ramen")
        {
            itemSystem.AddStamina(30);
            Destroy(other.gameObject);
        }
        if (other.tag == "GoldenRamen")
        {
            itemSystem.AddStamina(100);
            Destroy(other.gameObject);
        }
        if (other.tag == "Kohai")
        {
            itemSystem.AddKohai();
            Destroy(other.gameObject);

            //5体集めるとジェット発動
            if (ItemSystem.kohai == 5) PlayerStartJet();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageFlashTime == 0.0f && other.tag == "Shot")
        {
            if (other.transform.localScale.y <= 0.35f) { return; }

            //音処理
            Sound.SoundPlaySE(12);

            //ダメージ処理
            int damage = Parameter.save.defValue - Boss.damageValue;
            if (damage > 0) damage = 0;
            itemSystem.AddStamina(damage);

            //ダメージ効果処理
            damageFlashTime = 2.0f;
            StartCoroutine(Generic.Shake(0.5f, 15.0f, bossBattleHP, true));
            StartCoroutine(Generic.DamageFlash(GetComponent<SpriteRenderer>(), 0.2f));

            BossScore.b_point.avoid++;
        }
    }
}
