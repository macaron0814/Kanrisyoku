using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion; //爆破エフェクト
    private SpriteRenderer sr;

    [SerializeField]
    private Color[] color;

    private Vector3 startPos; //開始点座標

    [SerializeField]
    private Vector3[] interPos; //中間点座標

    [SerializeField]
    private Vector3[] targetPos; //終着点座標

    [SerializeField]
    private float[] fixedSpeed;
    private float   randSpeed;
    private Vector3 rot;
    private float   cnt;
    private int     rand;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        rot = new Vector3(0, 0, 36);
        startPos = transform.localPosition;

        Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Second - 1);
        rand = Random.Range(0, 3);

        randSpeed = RandomFixedValue.forFloat(new float[] { 0.0075f, 0.01f, 0.0025f });
    }

    // Update is called once per frame
    void Update()
    {
        //カウント
        cnt += randSpeed * fixedSpeed[rand];
        if (cnt > 1) DestroyObj(); //1を超えたら削除

        //座標、回転に関する処理
        transform.localPosition = Generic.GetPoint(startPos, interPos[rand], targetPos[rand], cnt);
        transform.Rotate(rot);

        //スピードに応じて色を変える
        if(randSpeed == 0.0075f) sr.color = color[1];
        else if (randSpeed < 0.0075f) sr.color = color[0];
        else if (randSpeed > 0.0075f) sr.color = color[2];
    }
    


    /// <summary>
    /// 削除処理
    /// </summary>
    private void DestroyObj()
    {
        Sound.SoundPlaySE(28);

        Destroy(gameObject);

        GameObject ex = Instantiate(explosion, targetPos[rand], Quaternion.identity);
        Destroy(ex, 0.75f);
    }
}
