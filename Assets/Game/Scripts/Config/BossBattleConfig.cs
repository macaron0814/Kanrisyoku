using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleConfig : MonoBehaviour
{
    [SerializeField] private GameObject[] syain;
    [SerializeField] private GameObject[] syainUI;

    public static GameObject[] sya;
    public static GameObject[] syaUI;
    public static int syainNumber;

    [SerializeField] private GameObject lockBoss;   //ボスバトルをlockするUIのオブジェクト
    [SerializeField] private Text       lockBossUI; //ボスバトルをlockするUIのテキスト

    // Start is called before the first frame update
    void Start()
    {
        sya = syain;
        syaUI = syainUI;

        if (Record.save.runTotalMeter >= 1000) lockBoss.SetActive(false);
        else
        {
            float unLockMeter = 1000 - Record.save.runTotalMeter;
            lockBossUI.text = "あと" + unLockMeter.ToString("F0") + "m\n走ると解放";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
