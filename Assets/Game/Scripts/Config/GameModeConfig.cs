using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeConfig : MonoBehaviour
{
    [SerializeField]
    private Animator cameraAnim;

    [SerializeField]
    private GameObject titleUI, gameUI, loadUI, tutorialUI, resultUI, sendenUI, bossBattleUI, bossResultUI, contentsUI;

    [SerializeField]
    private GameObject win, lose, fanfare, loseResult, newRecord, rewardBossWait10, rewardBossWait20, rewardATK;

    public GameObject[] resultButton, resultBossButton;

    [SerializeField]
    private Notification notification;

    [SerializeField]
    private Text test;

    [SerializeField]
    private Text coinUI;

    public enum SCENETYPE
    {
        LOAD,
        SENDEN,
        TITLE,
        TUTORIAL,
        GAME,
        RESULT,
        BOSSBATTLE,
        BOSSRESULT
    }

    public static SCENETYPE sceneType = SCENETYPE.LOAD;

    void Start()
    {
        loadUI.SetActive(true); //初回起動時にロードを挟み忘れた場合が多発したため、強制的に表示

        Sound.sliB.value = SystemSetting.save.bgm;
        Sound.sliS.value = SystemSetting.save.se;

        if (sceneType == SCENETYPE.TITLE)
        {
            if (!SystemData.save.isReview && SystemData.save.frameUnlock == 1)
            {
                ReviewManager.Instance.RequestReview();
                SystemData.save.isReview = true;
                SystemData.SaveSystemData();
            }
            notification.ActiveNotification();

            test.text = Social.localUser.ToString();
        }

        if (sceneType == SCENETYPE.GAME)
        {
            if(Generic.IsDayTimeCount(SystemData.save.coinBoostTime, new TimeSpan(0, 0, 10, 0)))
            {
                SystemData.save.coinBoost = 0;
                SystemData.save.bossLoseCount = UnityEngine.Random.Range(3, 7);
                SystemData.SaveSystemData();
            }

            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("GameFromTitle");
            StartCoroutine(Sound.SoundPlaySEforCountDown(7,0.1f));
            contentsUI.SetActive(false);
            resultUI.SetActive(false);
        }

        if (sceneType == SCENETYPE.BOSSRESULT)
        {
            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("BossBattleFromTitle");
            StartCoroutine(Sound.SoundPlaySEforCountDown(18, 0.1f));
            contentsUI.SetActive(false);
            bossResultUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneType)
        {
            case SCENETYPE.SENDEN:
                sendenUI.SetActive(true);
                titleUI.SetActive(false);
                break;

            case SCENETYPE.TITLE:
                sceneType = SCENETYPE.TITLE;
                break;

            case SCENETYPE.GAME:
                break;

            case SCENETYPE.RESULT:
                gameUI.SetActive(false);
                resultUI.SetActive(true);
                break;

            case SCENETYPE.BOSSBATTLE:
                BossScore.b_point.time += Time.deltaTime;
                break;
        }
    }



    /// <summary>
    /// ボスバトルの表示の画面の報酬の有無
    /// </summary>
    public void BossBattlePageReward()
    {
        if (SystemData.save.bossWait == 10.0) rewardBossWait10.SetActive(true);
        if (SystemData.save.bossWait == 20.0) rewardBossWait20.SetActive(true);
        if (SystemData.save.bossBattleATK != 0.0) rewardATK.SetActive(true);
    }



    /// <summary>
    /// ゲーム画面切り替え時に関する処理
    /// </summary>
    public void ChangeGameScene()
    {
        if (!SystemData.save.isTutorial) sceneType = SCENETYPE.TUTORIAL;
        else sceneType = SCENETYPE.GAME;
        cameraAnim.SetBool("GameCamera", true);
        titleUI.SetActive(false);

        //イベント時限定の処理-Event_01======
        if (!SystemData.save.eventBoost)
        {
            SystemData.save.eventBoost = true;
            SystemData.save.eventNotification = true;
            SystemData.save.eventBoostTime[0] = DateTime.Now.Day;
            SystemData.save.eventBoostTime[1] = DateTime.Now.Hour;
            SystemData.save.eventBoostTime[2] = DateTime.Now.Minute;
            SystemData.save.eventBoostTime[3] = DateTime.Now.Second;
            SystemData.SaveSystemData();
        }
        //===================================

        StartCoroutine("GameFromTitle");
        Sound.SoundPlaySE(7);
    }



    /// <summary>
    /// ボスバトル画面切り替え時に関する処理
    /// </summary>
    public void ChangeBossBattleScene()
    {
        cameraAnim.SetBool("GameCamera", true);
        StartCoroutine("BossBattleFromTitle");
        Sound.SoundStop();
        Sound.SoundPlaySE(18);
    }



    /// <summary>
    /// リザルト画面切り替え時に関する処理
    /// </summary>
    public static void ChangeResultScene()
    {
        //リザルト画面に変更
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.RESULT;
        Sound.SoundStop();

        if (Record.save.runTotalMeter >= 1000 && !Record.save.openBossBattle) Notification.WaitNotification(Notification.sNotification[0]);

        //スコア送信
        long score = (long)(ItemSystem.metre * 100);
        iOSRankingUtility.ReportScore("hiScore", score);

        Record.UpdateRecord(Record.RecordList.FIRSTRUN);

        Record.UpdateRecord(Record.RecordList.RUNCOUNT);
        Record.UpdateRecord(Record.RecordList.RUNTOTALMETER, ItemSystem.metre);

        Record.UpdateRecord(Record.RecordList.DEAD, ItemSystem.gameoverPattern);

        if (Record.save.runTotalMeter >= 1000) Record.UpdateRecord(Record.RecordList.OPENBOSSBATTLE);

        //イベント時限定の処理-Event_01======
        if (SystemData.save.eventBoost)
        {
            SystemData.save.eventRunTotalMeter += ItemSystem.metre;
            SystemData.SaveSystemData();
        }
        //===================================

        Record.ClearRecord();

        Parameter.save.coin += ItemSystem.coin;
        if (Parameter.save.coin > 9999) Parameter.save.coin = 9999;

        Parameter.SaveParameter(); //セーブ
    }


    /// <summary>
    /// ゲーム画面への移行処理
    /// </summary>
    IEnumerator GameFromTitle()
    {
        gameUI.SetActive(true); //ゲームUI表示

        yield return new WaitForSeconds(1f);
        cameraAnim.enabled = false;
        if (!SystemData.save.isTutorial) tutorialUI.SetActive(true);
    }



    /// <summary>
    /// ボスバトル画面への移行処理
    /// </summary>
    IEnumerator BossBattleFromTitle()
    {
        bossBattleUI.SetActive(true); //ボスバトルUI表示

        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1f);
        cameraAnim.enabled = false;

        yield return new WaitForSeconds(1f);
        BossBattleConfig.syaUI[BossBattleConfig.syainNumber].SetActive(true);
        Sound.SoundPlaySE(11);

        yield return new WaitForSeconds(3f);
        BossBattleConfig.sya[BossBattleConfig.syainNumber].SetActive(true);

        if (BossBattleConfig.syainNumber == 0) Sound.SoundPlayBGM(2);
        if (BossBattleConfig.syainNumber == 1) Sound.SoundPlayBGM(3);
        if (BossBattleConfig.syainNumber == 2) Sound.SoundPlayBGM(4);

        sceneType = SCENETYPE.BOSSBATTLE;
    }



    /// <summary>
    /// ボスリザルト画面への移行処理
    /// </summary>
    public IEnumerator BossResult()
    {
        sceneType = SCENETYPE.BOSSRESULT;

        SystemData.save.bossWait = 0;
        SystemData.save.bossBattleATK = 0;

        //広告をカウントが超えたら表示
        InterstitialManager.interBossLoseCount++;
        if (InterstitialManager.interBossLoseCount == 3)
        {
            InterstitialManager.OnInterBossLoseAd();
            InterstitialManager.interBossLoseCount = 0;
        }

        bossBattleUI.SetActive(false); //ボスバトルUI非表示
        bossResultUI.SetActive(true); //ボスリザルトUI表示
        Sound.SoundStop();

        //スコア送信
        long score = (long)BossScore.b_point.score;

        //ボスの種類ごとに分けて得点を管理
        if (BossBattleConfig.syainNumber == 0)      iOSRankingUtility.ReportScore("hiBossScore", score);
        else if (BossBattleConfig.syainNumber == 1) iOSRankingUtility.ReportScore("hiBossScore2", score);
        else if (BossBattleConfig.syainNumber == 2) iOSRankingUtility.ReportScore("hiBossScore3", score);

        yield return new WaitForSeconds(1f);
        Sound.SoundPlaySE(25);

        yield return new WaitForSeconds(2.75f);

        if (Boss.bossPram == Boss.Boss_Parameter.DEATH)
        {
            //ボスの種類ごとに分けて得点を管理
            if (BossBattleConfig.syainNumber == 0)
            {
                Record.UpdateRecord(Record.RecordList.JOSHI);
                if (BossScore.b_point.avoid == 1000000) { Record.UpdateRecord(Record.RecordList.PERFECTJOSHI); }
            }
            else if (BossBattleConfig.syainNumber == 1)
            {
                Record.UpdateRecord(Record.RecordList.SYACHO);
                if (BossScore.b_point.avoid == 1000000) { Record.UpdateRecord(Record.RecordList.PERFECTSYACHO); }
            }
            else if (BossBattleConfig.syainNumber == 2)
            {
                Record.UpdateRecord(Record.RecordList.KAICHO);
                if (BossScore.b_point.avoid == 1000000) { Record.UpdateRecord(Record.RecordList.PERFECTKAICHO); }
            }
            Record.ClearRecord();

            win.SetActive(true);
            Sound.SoundPlaySE(26);

            yield return new WaitForSeconds(2.7f);
            fanfare.SetActive(true);
            Sound.SoundPlaySE(33);
        }
        else 
        {
            lose.SetActive(true);
            Sound.SoundPlaySE(27);
            loseResult.SetActive(true);

            if (SystemData.save.bossLoseCount == 0)
            {
                SystemData.save.coinBoost = 2;
                Notification.WaitNotification(Notification.sNotification[2]);
            }
            //else if (SystemData.save.bossLoseCount != 0) SystemData.save.bossLoseCount--;
        }
        SystemData.SaveSystemData();

        for (int i = 0; i < resultBossButton.Length; i++) resultBossButton[i].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //ボスの種類ごとにハイスコア更新
        if (Record.save.bestBossBattleScore[BossBattleConfig.syainNumber] < score)
        {
            Record.save.bestBossBattleScore[BossBattleConfig.syainNumber] = score;
            Record.SaveRecord();
            newRecord.SetActive(true);
            Sound.SoundPlaySE(35);
        }
    }



    public void TitleButton()
    {
        sendenUI.SetActive(false);
        titleUI.SetActive(true);
        sceneType = SCENETYPE.TITLE;
    }



    public void RestartButton()
    {
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.GAME;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void BossRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    public void HomeButton()
    {
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.TITLE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
