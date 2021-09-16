using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        Debug.Log(sceneType);
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
        if (Input.GetKeyDown(KeyCode.Space))
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
        yield return new WaitForSeconds(1f);

        cameraAnim.enabled = false;
        gameUI.SetActive(true);

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
}
