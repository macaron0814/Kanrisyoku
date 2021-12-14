using System.Collections;
using System.Collections.Generic;
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
    private GameObject win, lose, fanfare, loseResult;

    [SerializeField]
    private Text coinUI;

    public enum SCENETYPE
    {
        LOAD,
        SENDEN,
        TITLE,
        GAME,
        RESULT,
        BOSSBATTLE,
        BOSSRESULT
    }

    public static SCENETYPE sceneType = SCENETYPE.LOAD;

    void Start()
    {
        loadUI.SetActive(true); //初回起動時にロードを挟み忘れた場合が多発したため、強制的に表示

        if (sceneType == SCENETYPE.GAME)
        {
            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("GameFromTitle");
            StartCoroutine(Sound.SoundPlaySEforCountDown(7,0.1f));
            contentsUI.SetActive(false);
        }

        if (sceneType == SCENETYPE.BOSSRESULT)
        {
            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("BossBattleFromTitle");
            StartCoroutine(Sound.SoundPlaySEforCountDown(18, 0.1f));
            contentsUI.SetActive(false);
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
    /// ゲーム画面切り替え時に関する処理
    /// </summary>
    public void ChangeGameScene()
    {
        sceneType = SCENETYPE.GAME;
        cameraAnim.SetBool("GameCamera", true);
        titleUI.SetActive(false);
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

        //スコア送信
        long score = (long)(ItemSystem.metre * 100);
        iOSRankingUtility.ReportScore("hiScore", score);

        Record.UpdateRecord(Record.RecordList.FIRSTRUN);

        Record.UpdateRecord(Record.RecordList.RUNCOUNT);
        Record.UpdateRecord(Record.RecordList.RUNTOTALMETER, ItemSystem.metre);

        Record.UpdateRecord(Record.RecordList.DEAD, ItemSystem.gameoverPattern);

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

        yield return new WaitForSeconds(3f);
        tutorialUI.SetActive(false);
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

        sceneType = SCENETYPE.BOSSBATTLE;
    }



    /// <summary>
    /// ボスリザルト画面への移行処理
    /// </summary>
    public IEnumerator BossResult()
    {
        //スコア送信
        long score = (long)BossScore.b_point.score;

        //ボスの種類ごとに分けて得点を管理
        if (BossBattleConfig.syainNumber == 0) iOSRankingUtility.ReportScore("hiBossScore",  score);
        if (BossBattleConfig.syainNumber == 1) iOSRankingUtility.ReportScore("hiBossScore2", score);
        if (BossBattleConfig.syainNumber == 2) iOSRankingUtility.ReportScore("hiBossScore3", score);

        sceneType = SCENETYPE.BOSSRESULT;

        bossBattleUI.SetActive(false); //ボスバトルUI非表示
        bossResultUI.SetActive(true); //ボスリザルトUI表示
        Sound.SoundStop();

        yield return new WaitForSeconds(1f);
        Sound.SoundPlaySE(25);

        yield return new WaitForSeconds(2.75f);

        if (Boss.bossPram == Boss.Boss_Parameter.DEATH)
        {
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
