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

    [SerializeField] private Color[]   recordColor;
    [SerializeField] private string[]   recordTitle;
    [SerializeField] private string[]   recordText;

    //recordAchievementのstatic変数
    [SerializeField] private static GameObject recAch;
    [SerializeField] private static Image recCorUI;
    [SerializeField] private static Text recTitUI;
    [SerializeField] private static Text recTexUI;

    [SerializeField] private static Color[] recCor;
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
    }

    public static void Action(string key)
    {
        foreach (var keys in Record.save.recordKey) if (keys == key) { return; }

        if (count == 0) { count++; mb.StartCoroutine(AnimationCoroutine(key)); }
        else { count++; mb.StartCoroutine(NextAnimationCoroutine(key,count)); }

    }

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
                return 5;

            default:
                return -1;
        }
    }
}
