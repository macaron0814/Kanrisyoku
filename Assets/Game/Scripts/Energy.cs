﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] GameObject waveConfig;
    [SerializeField] GameObject energyUI;
    [SerializeField] GameObject kohaiEnergyAction;

    [SerializeField] Color resetColor;
    [SerializeField] int   createinterval = 30;

    public  static float energy = 0;
    private static GameObject eneUI;
    private static bool isGet;

    enum ChargeState
    {
        MIN,
        MAX
    }
    private static ChargeState cs;

    // Start is called before the first frame update
    void Start()
    {
        isGet  = false;
        energy = 0;
        eneUI  = energyUI;
        cs = ChargeState.MIN;
    }

    // Update is called once per frame
    void Update()
    {
        //waveConfigの要素からWave_BossBattleを探索
        foreach (Transform child in waveConfig.transform)
        {
            if (GameModeConfig.sceneType != GameModeConfig.SCENETYPE.BOSSBATTLE) return;

            if (child.name != "Wave_BossBattle") continue;

            //Wave_BossBattleの要素からEnergyを探索
            foreach (Transform grandChild in child)
            {
                if (!isGet && grandChild.tag == "Energy") ActiveEnergy(grandChild.gameObject);
            }
        }

        //３つ集めると発動
        if (cs == ChargeState.MAX)
        {
            StartCoroutine(EnergyAction());
            cs = ChargeState.MIN;
        }
    }

    /// <summary>
    /// 一定回数に一度オブジェクトを生成する
    /// </summary>
    /// <param name="energyObj">Energyのオブジェクト</param>
    void ActiveEnergy(GameObject energyObj)
    {
        if (energy == createinterval)
        {
            isGet  = true;
            energy = 0;
            return;
        }

        Destroy(energyObj);
        energy++;
    }

    /// <summary>
    /// エネルギーを取得
    /// </summary>
    public static void AddEnergy()
    {
        int cnt = 0;
        isGet = false;

        foreach (Transform child in eneUI.transform)
        {
            //取得したら明るくする
            if (child.GetComponent<Image>().color != Color.white)
            {
                child.GetComponent<Image>().color  = Color.white;
                break;
            }
            cnt++;
        }
        if(cnt == 2) cs = ChargeState.MAX;
    }

    /// <summary>
    /// 3つ集めると発動するAction
    /// </summary>
    /// <returns>待機時間</returns>
    IEnumerator EnergyAction()
    {
        Animator anim = eneUI.GetComponent<Animator>();
        anim.Play("Charge", 0, 0.0f);

        anim.enabled = true;
        yield return new WaitForSeconds(1);

        Sound.SoundPlaySE(34);
        StartCoroutine(Generic.Shake(0.3f, 0.2f, Camera.main.gameObject));
        yield return new WaitForSeconds(1);

        anim.enabled = false;
        Instantiate(kohaiEnergyAction);

        foreach (Transform child in eneUI.transform) child.GetComponent<Image>().color = resetColor;
    }
}
