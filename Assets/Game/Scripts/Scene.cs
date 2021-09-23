using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
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
            Sound.SoundPlaySE(7);
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
                TitleScene();
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
    /// TitleSceneに関する処理
    /// </summary>
    void TitleScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sceneType = SCENETYPE.GAME;
            cameraAnim.SetBool("GameCamera", true);
            titleUI.SetActive(false);
            StartCoroutine("GameFromTitle");
            Sound.SoundPlaySE(7);
        }
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

    public void RankingButton()
    {
        string score;
        score = ItemSystem.metre.ToString("F2");

        resultUI.SetActive(false);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(double.Parse(score));
    }

    public void TitleButton()
    {
        sendenUI.SetActive(false);
        titleUI.SetActive(true);
        sceneType = SCENETYPE.TITLE;
    }

    //public void RestartButton()
    //{
    //    Scene.sceneType = Scene.SCENETYPE.GAME;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}
