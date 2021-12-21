using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyainRocket : MonoBehaviour
{
    private GameObject player;

    [SerializeField] GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        //自身を削除
        Destroy(this.gameObject,5);

        transform.eulerAngles -= new Vector3(0, 0, 90);

        RocketFiring();
        Sound.SoundPlaySE(8);
    }

    void Update()
    {
        if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSRESULT) Destroy(gameObject);
    }


    /// <summary>
    /// シャインロケット発射
    /// </summary>
    public void RocketFiring()
    {
        StartCoroutine
        (
            RocketFiringFrame
            (
                new Vector3(3.75f, transform.localPosition.y, 0), new Vector3(-10, transform.localPosition.y, 0), //位置
                transform.eulerAngles, new Vector3(0, 0, -450),         //回転軸
                3                                                       //速度
             )
        );

    }

    /// <summary>
    /// シャインロケット発射の処理
    /// </summary>
    /// <param name="currentPos">開始位置</param>
    /// <param name="targetPos">終了位置</param>
    /// <param name="currentRot">開始回転軸</param>
    /// <param name="targetRot">終了回転軸</param>
    /// <param name="speed">速度</param>
    /// <returns></returns>
    IEnumerator RocketFiringFrame(Vector3 currentPos, Vector3 targetPos, Vector3 currentRot, Vector3 targetRot, float speed)
    {
        float time = 0;
        float frameCount = (1.0f / 60.0f * speed);
        Vector3 startPos = new Vector3(7, currentPos.y, 1);

        //移動
        while (time < 1)
        {
            time += (frameCount * 1.5f);
            transform.localPosition = Vector3.Lerp(startPos, currentPos, time);
            yield return null; //1フレーム待機
        }
        time = 0;

        //移動①
        while (time < 1)
        {
            time += frameCount;
            transform.eulerAngles = Vector3.Lerp(currentRot, targetRot, time);
            GetComponent<SpriteRenderer>().color = new Color(1 - (time / 2), 1 - time, 1 - (time / 2), 1);
            yield return null; //1フレーム待機
        }

        yield return new WaitForSeconds(0.5f);

        time = 0;

        //移動②
        while (time < 1)
        {
            time += frameCount;
            transform.localPosition = Vector3.Lerp(currentPos, targetPos, time);
            yield return null;
        }

        //自身を削除
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Player.damageFlashTime == 0.0f && col.tag == "Player")
        {
            GameObject ex = Instantiate(explosion, player.transform.localPosition, Quaternion.identity);
            Destroy(ex, 1);
        }
    }
}
