﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeConfig : MonoBehaviour
{
    [SerializeField]
    private Animator cameraAnim;

    [SerializeField]
    private GameObject titleUI, gameUI,loadUI,tutorialUI, resultUI,sendenUI;

    public enum SCENETYPE
    {
        LOAD,
        SENDEN,
        TITLE,
        GAME,
        RESULT
    }

    public static SCENETYPE sceneType = SCENETYPE.LOAD;

    void Start()
    {
        if(sceneType == SCENETYPE.GAME)
        {
            sceneType = SCENETYPE.GAME;
            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("GameFromTitle");
            StartCoroutine(Sound.SoundPlaySEforCountDown(7,0.1f));
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

        Record.ClearRecord();

    }



    /// <summary>
    /// GameFromTitleに関する処理
    /// </summary>
    IEnumerator GameFromTitle()
    {
        gameUI.SetActive(true); //ゲームUI表示

        yield return new WaitForSeconds(1f);
        cameraAnim.enabled = false;

        yield return new WaitForSeconds(3f);
        tutorialUI.SetActive(false);
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



    public void HomeButton()
    {
        GameModeConfig.sceneType = GameModeConfig.SCENETYPE.TITLE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}