using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KohaiRocket : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private GameObject kohaiRocket;

    [HideInInspector]
    public bool isRocket;

    [SerializeField]
    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        if (isRocket)
        {
            RocketFiring();
            Sound.SoundPlaySE(8);
        }
    }



    /// <summary>
    /// コーハイロケット発射
    /// </summary>
    public void RocketFiring()
    {
        //ランダムで発射位置をセット
        Random.InitState(System.DateTime.Now.Second);
        float sx = Random.Range(-7, -2);
        float sy = Random.Range(-2,  3);


        StartCoroutine
        (
            RocketFiringFrame
            (
                player.transform.localPosition, new Vector3(sx, sy, 0), //位置
                transform.eulerAngles, new Vector3(0, 0, -450),         //回転軸
                3                                                       //速度
             )
        );

    }

    /// <summary>
    /// コーハイロケット発射の処理
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

        //移動①
        while (time < 1)
        {
            time += frameCount;
            transform.localPosition = Vector3.Lerp(currentPos, targetPos, time);
            transform.eulerAngles = Vector3.Lerp(currentRot, targetRot, time);
            yield return null; //1フレーム待機
        }

        yield return new WaitForSeconds(0.5f);

        //開始、終了地点変更
        time = 0;
        currentPos = targetPos;
        targetPos.x = 14.5f;

        //移動②
        while (time < 1)
        {
            time += frameCount;
            transform.localPosition = Vector3.Lerp(currentPos, targetPos, time);
            yield return null;
        }
    }



    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"> 接触したコライダー</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!isRocket && col.tag == "Player")
        {
            kohaiRocket = Instantiate(kohaiRocket, player.transform.localPosition, Quaternion.identity);
            kohaiRocket.GetComponent<KohaiRocket>().isRocket = true;
            Destroy(this.gameObject);
        }

        if (isRocket && col.tag == "Joshi")
        {
            //音処理
            Sound.SoundPlaySE(10);

            //ダメージ処理
            Boss.Damage(Parameter.save.atkValue);

            //エフェクト処理
            explosion = Instantiate(explosion, transform.localPosition, Quaternion.identity);
            Destroy(this.explosion, 1);

            //自身を削除
            Destroy(this.gameObject);
        }
    }
}
