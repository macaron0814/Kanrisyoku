using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    private ContactFilter2D filter2d;

    private CameraShake cameraShake;

    private Rigidbody2D rb;
    private BoxCollider2D box2d;
    private SpriteRenderer spriteRenderer;
    private bool isJump = false;
    public static bool isJet = false;
    public static bool isGameOver = false;
    private bool isSound;
    public bool isTouched;

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
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
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
    }


    /// <summary>
    /// キー入力操作
    /// </summary>
    void PlayerKey()
    {
        if (isJet) { kohai.SetActive(true); return; }

        isTouched = rb.IsTouching(filter2d);

        //重力をかける
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
        if (!isJump && Input.GetMouseButtonDown(0))
        {
            Vector3 force = new Vector3(0.0f, 575.0f, 0.0f);    // 力を設定
            rb.AddForce(force);  // 力を加える

            anim.SetBool("Jump", true);
            isJump = true;
        }
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

        //ジャンプ
        Vector3 force = new Vector3(0.0f, 575.0f, 0.0f);    // 力を設定
        rb.AddForce(force);  // 力を加える
        anim.SetBool("Jump", true);
        isJump = true;

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
        itemSystem.AddStamina(100);
        ItemSystem.gameoverPattern = type;

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spriteRenderer.sortingLayerName = "GameOver";

        anim.SetBool("GameOver", true);
        isGameOver = true;

        GameModeConfig.ChangeResultScene();
        StartCoroutine(ResultFromGameOverandPlaySound(type));
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



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Line")
        {
            if (!isTouched && rb.gravityScale == 7) { Sound.SoundPlaySE(6); }
            if (isJump && rb.gravityScale == 2) Sound.SoundPlaySE(5);

            anim.SetBool("Jump", false);
            isJump = false;

            if (rb.gravityScale == 7)
            {
                effect.SetActive(true);
                Invoke(nameof(DestroyEffect), 0.25f);
                cameraShake.Shake(0.2f, 0.05f);
                itemSystem.AddStamina(-5);

                //オートジャンプ発動
                if(PlayerAutoJumpCountDown() > 0.9f)
                {
                    Vector3 force = new Vector3(0.0f, 700.0f, 0.0f);    // 力を設定
                    rb.AddForce(force);  // 力を加える

                    anim.SetBool("Jump", true);
                    isJump = true;
                }
            }
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
}
