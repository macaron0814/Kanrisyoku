using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordAchievement : MonoBehaviour
{
    [SerializeField] private GameObject recordAchievement;
    [SerializeField] private Image      recordColorUI;
    [SerializeField] private Text       recordTitleUI;
    [SerializeField] private Text       recordTextUI;

    [SerializeField] private Color []   recordColor;
    [SerializeField] private string[]   recordTitle;
    [SerializeField] private string[]   recordText;

    [SerializeField] private GameObject recordList;

    //recordAchievementのstatic変数
    [SerializeField] private static GameObject recAch;
    [SerializeField] private static Image      recCorUI;
    [SerializeField] private static Text       recTitUI;
    [SerializeField] private static Text       recTexUI;

    [SerializeField] private static Color [] recCor;
    [SerializeField] private static string[] recTit;
    [SerializeField] private static string[] recTex;

    private static MonoBehaviour mb;

    private static int count;

    private void Start()
    {
        recAch   = recordAchievement;

        recCor   = recordColor;
        recTit   = recordTitle;
        recTex   = recordText;

        recCorUI = recordColorUI;
        recTitUI = recordTitleUI;
        recTexUI = recordTextUI;

        mb = this;
        count = 0;

        RecordAchievementList();
    }



    /// <summary>
    /// 実績のリスト
    /// </summary>
    private void RecordAchievementList()
    {
        for (int i = 0; i < recordList.transform.childCount; i++)
        {
            bool isUnlock = UnlockCheck(recordList.transform.GetChild(i).name);

            //フレームの色変更
            foreach (var image in recordList.transform.GetChild(i).GetComponentsInChildren<Image>())
            {
                if (image.name == "Frame")
                {
                    if(isUnlock) image.color = recordColor[i];
                    else image.color = Color.black;
                }
                if (image.name == "Image")
                {
                    if (isUnlock) image.gameObject.SetActive(true);
                    else image.gameObject.SetActive(false);
                }
            }
            //実績のタイトルと内容を変更
            foreach (Text t in recordList.transform.GetChild(i).GetComponentsInChildren<Text>())
            {
                if (t.name == "Title")
                {
                    if (isUnlock) t.gameObject.SetActive(true);
                    else t.gameObject.SetActive(false);

                    t.text = recordTitle[i];
                }

                if (t.name == "Text")
                {
                    if(isUnlock)t.gameObject.SetActive(true);
                    else t.gameObject.SetActive(false);

                    t.text = recordText[i];
                }

                if (t.name == "Nolma") 
                {
                    if (!isUnlock) t.gameObject.SetActive(true);
                    else t.gameObject.SetActive(false);
                }
            }
        }
    }

    public static void Action(string key)
    {
        foreach (var keys in Record.save.recordKey) if (keys == key) { return; }

        if (count == 0) { count++; mb.StartCoroutine(AnimationCoroutine(key)); }
        else { count++; mb.StartCoroutine(NextAnimationCoroutine(key,count)); }

    }

    private bool UnlockCheck(string key)
    {
        foreach (var keys in Record.save.recordKey) if (keys == key) { return true; }
        return false;
    }

    /// <summary>
    /// 実績を達成した際のAnimation
    /// </summary>
    /// <param name="key">Achievementの名前</param>
    /// <returns></returns>
    private static IEnumerator AnimationCoroutine(string key)
    {
        recCorUI.color = recCor[Type(key)];
        recTitUI.text  = recTit[Type(key)];
        recTexUI.text  = recTex[Type(key)];

        recAch.SetActive(true);

        yield return new WaitForSeconds(5f);
        recAch.SetActive(false);
        count--;
    }

    /// <summary>
    /// 実績を達成した際のAnimation(2回目以降)
    /// </summary>
    /// <param name="key">Achievementの名前</param>
    /// <param name="cnt">現在何個目のAchievementか</param>
    /// <returns></returns>
    private static IEnumerator NextAnimationCoroutine(string key,int cnt)
    {
        yield return new WaitForSeconds((cnt * 5f) - 5f + (cnt / 10.0f));

        recCorUI.color = recCor[Type(key)];
        recTitUI.text  = recTit[Type(key)];
        recTexUI.text  = recTex[Type(key)];

        recAch.SetActive(true);

        yield return new WaitForSeconds(5f);
        recAch.SetActive(false);
        count--;
    }


    private static int Type(string key)
    {
        switch (key)
        {
            case "first_run":
                Record.save.recordKey[0] = key;
                Record.SaveRecord();
                return 0;

            case "dead_3":
                Record.save.recordKey[1] = key;
                Record.SaveRecord();
                return 1;

            case "run10":
                Record.save.recordKey[2] = key;
                Record.SaveRecord();
                return 2;

            case "run50":
                Record.save.recordKey[3] = key;
                Record.SaveRecord();
                return 3;

            case "run100":
                Record.save.recordKey[4] = key;
                Record.SaveRecord();
                return 4;

            case "run_totalmeter_500":
                Record.save.recordKey[5] = key;
                Record.SaveRecord();
                return 5;

            case "run_totalmeter_2000":
                Record.save.recordKey[6] = key;
                Record.SaveRecord();
                return 6;

            case "run_totalmeter_5000":
                Record.save.recordKey[7] = key;
                Record.SaveRecord();
                return 7;

            case "kohai_jet":
                Record.save.recordKey[8] = key;
                Record.SaveRecord();
                return 8;

            case "open_bossbattle":
                Record.save.recordKey[9] = key;
                Record.SaveRecord();
                return 9;

            case "joshi":
                Record.save.recordKey[10] = key;
                Record.SaveRecord();
                return 10;

            case "syacho":
                Record.save.recordKey[11] = key;
                Record.SaveRecord();
                return 11;

            case "kaicho":
                Record.save.recordKey[12] = key;
                Record.SaveRecord();
                return 12;

            case "perfect_joshi":
                Record.save.recordKey[13] = key;
                Record.SaveRecord();
                return 13;

            case "perfect_syacho":
                Record.save.recordKey[14] = key;
                Record.SaveRecord();
                return 14;

            case "perfect_kaicho":
                Record.save.recordKey[15] = key;
                Record.SaveRecord();
                return 15;

            default:
                return -1;
        }
    }
}
