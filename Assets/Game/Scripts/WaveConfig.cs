using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveConfig : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waveCategory;//使用するwaveの種類
    [SerializeField]
    private GameObject[] waveCategoryRamen;//使用するwaveの種類

    [SerializeField]
    private GameObject[] waveCategoryBoss;//使用するwaveの種類

    private GameObject[] waveCategoryRotation = new GameObject[3];//使用しているwave

    [SerializeField]
    private GameObject[] backWaveCategory;//使用するbackwaveの種類
    private GameObject[] backWaveCategoryRotation = new GameObject[3];//使用しているbackwave

    [SerializeField]
    private float scrollSpeed;//スクロールする速度
    public  float startScrollSpeed;//初期状態の速度
    public  float jetBeforeScrollSpeed;//ジェットが発動する前の速度

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < waveCategoryRotation.Length; i++)
        {
            waveCategoryRotation[i] = Instantiate(waveCategory[0], new Vector3(-18 + (i * 18), 0, 0), Quaternion.identity);
            waveCategoryRotation[i].transform.parent = transform;
        }

        for (int i = 0; i < backWaveCategoryRotation.Length; i++)
        {
            int cnt = (i % 2 == 0 ? 0 : 1);

            backWaveCategoryRotation[i] = Instantiate(backWaveCategory[cnt], new Vector3(-24.6f + (i * 18), -4.65f, 0), Quaternion.identity);
            backWaveCategoryRotation[i].transform.parent = transform;
        }

        //初期の速度を代入
        startScrollSpeed = scrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollCommand();
    }

    /// <summary>
    /// スクロールの動きに関する処理
    /// </summary>
    void ScrollCommand()
    {
        ScrollStop();

        //足場
        for (int i = 0; i < waveCategoryRotation.Length; i++)
        {
            Vector3 pos = waveCategoryRotation[i].transform.localPosition;
            pos.x -= scrollSpeed;

            //指定座標まで来たら戻してループ
            if (pos.x < -18)
            {
                if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.GAME)
                {
                    Destroy(waveCategoryRotation[i]);

                    int cnt = (i % 2 == 0 ? 0 : 1);

                    if (cnt == 0 && ItemSystem.ramen < 30) { waveCategoryRotation[i] = Instantiate(waveCategoryRamen[Random.Range(0, waveCategoryRamen.Length)]); }
                    else { waveCategoryRotation[i] = Instantiate(waveCategory[Random.Range(0, waveCategory.Length)]); }
                    waveCategoryRotation[i].transform.parent = transform;
                }

                if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSBATTLE)
                {
                    Destroy(waveCategoryRotation[i]);

                    waveCategoryRotation[i] = Instantiate(waveCategoryBoss[Random.Range(0, waveCategoryBoss.Length)]);
                    waveCategoryRotation[i].transform.parent = transform;
                }

                pos.x = 36;
            }

            waveCategoryRotation[i].transform.localPosition = pos;
        }

        //背景ビル
        for (int i = 0; i < backWaveCategoryRotation.Length; i++)
        {
            Vector3 pos = backWaveCategoryRotation[i].transform.localPosition;
            pos.x -= (scrollSpeed / 10);

            //指定座標まで来たら戻してループ
            if (pos.x < -24.6f) { pos.x = 29.4f; }

            backWaveCategoryRotation[i].transform.localPosition = pos;
        }

        if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.GAME)
        {
            //進んだ距離に比例して徐々に加速
            scrollSpeed = startScrollSpeed * (1 + ItemSystem.metre / 1000.0f);
            if (scrollSpeed > 0.25f && scrollSpeed < 0.3f) { scrollSpeed = 0.2f; }
        }

        if (GameModeConfig.sceneType == GameModeConfig.SCENETYPE.BOSSBATTLE)
        {
            //進んだ距離に比例して徐々に加速
            scrollSpeed = 0.15f;
        }
    }

    /// <summary>
    /// スクロールの終了処理
    /// </summary>
    void ScrollStop()
    {
        if (!Player.isGameOver) return;
        scrollSpeed = 0.0f;
    }
}
