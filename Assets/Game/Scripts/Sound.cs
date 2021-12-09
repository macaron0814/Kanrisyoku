using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound instance;
    public AudioClip[] bgm, se;//bgm,seを取得
    public static AudioClip[] bgm_static, se_static;//bgm,seを取得
    static AudioSource audioSource;//AudioSourceのデータを取得

    void Awake()
    {
        bgm_static = bgm;
        se_static = se;

        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();//AudioSourceのデータを代入
    }

    //BGM再生
    public static void SoundPlayBGM(int playbgm, bool loop = true)//配列番号/ループ
    {
        SoundStop();//停止
        audioSource.clip = bgm_static[playbgm];//clipにbgmを代入
        audioSource.loop = loop;//ループするか
        audioSource.Play();//再生
    }

    public static IEnumerator SoundPlayBGMforCountDown(int playbgm, float time, bool loop = true)//配列番号
    {
        yield return new WaitForSeconds(time);
        SoundStop();//停止

        audioSource.clip = bgm_static[playbgm];//clipにbgmを代入
        audioSource.loop = loop;//ループするか
        audioSource.Play();//再生
    }

    //SE再生
    public static void SoundPlaySE(int playse)//配列番号
    {
        audioSource.PlayOneShot(se_static[playse]);//再生
    }

    public static IEnumerator SoundPlaySEforCountDown(int playse, float time)//配列番号
    {
        yield return new WaitForSeconds(time);
        audioSource.PlayOneShot(se_static[playse]);//再生
    }

    public static void SoundStop()
    {
        audioSource.Stop();//停止
    }
}
