using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource_SE;//AudioSource_SEのデータを取得

    public static Sound instance;
    public AudioClip[] bgm, se;//bgm,seを取得
    public static AudioClip[] bgm_static, se_static;//bgm,seを取得
    static AudioSource audioSource;//AudioSourceのデータを取得
    static AudioSource audio_SE;//AudioSource_SEのデータを取得

    [SerializeField] private Slider slider_BGM, slider_SE;//bgm,seの音量バー
    [SerializeField] private Text bgmVolume, seVolume;//bgm,seの音量バー
    public static Slider sliB, sliS;//bgm,seの音量バー
    static Text bV, sV;//bgm,seの音量バー

    void Awake()
    {
        bgm_static = bgm;
        se_static  = se;
        sliB = slider_BGM;
        sliS = slider_SE;
        bV = bgmVolume;
        sV = seVolume;

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
        audio_SE = audioSource_SE;
        audioSource = gameObject.GetComponent<AudioSource>();//AudioSourceのデータを代入
    }

    void Update()
    {
        SystemSetting.save.bgm = sliB.value;
        SystemSetting.save.se  = sliS.value;
        audioSource.volume = SystemSetting.save.bgm / 100.0f;
        audio_SE.volume    = SystemSetting.save.se  / 100.0f;
        bV.text = SystemSetting.save.bgm.ToString("f0");
        sV.text  = SystemSetting.save.se.ToString("f0");
    }

    public void BackButton()
    {
        SystemSetting.SaveSystemSetting();
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
        audio_SE.PlayOneShot(se_static[playse]);//再生
    }

    public static IEnumerator SoundPlaySEforCountDown(int playse, float time)//配列番号
    {
        yield return new WaitForSeconds(time);
        audio_SE.PlayOneShot(se_static[playse]);//再生
    }

    public static void SoundStop()
    {
        audioSource.Stop();//停止
    }
}
