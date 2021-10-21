﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSystem : MonoBehaviour
{
    [SerializeField] private Slider ramenUI;
    [SerializeField] private Text coinUI,coinResultUI;
    [SerializeField] private Text metreUI, metreResultUI;
    [SerializeField] private Text kohaiUI;
    [SerializeField] private GameObject kohaiObj;

    private int coin = 0;
    public static float metre = 0;
    public static float ramen = 0;
    public static float kohai = 0;
    private bool isKohaiCountInterval; //カウントが連続で増えてしまわないようにフラグで管理

    private void Start()
    {
        metre = 0;
        ramen = 0;
        kohai = 0;
    }

    private void Update()
    {
        if (GameScene.sceneType == GameScene.SCENETYPE.GAME)
        {

            if (!Player.isJet)
            {
                ramenUI.value -= 1.0f / 30.0f;
                ramen = ramenUI.value;
            }

            metre += 1.0f / 10.0f;
            metreResultUI.text = metreUI.text = metre.ToString("F2") + "m";
        }
    }

    public void AddCoin()
    {
        coin++;
        coinResultUI.text = coinUI.text = coin.ToString("D4");
        Sound.SoundPlaySE(0);
    }

    public void AddStamina(int cnt)
    {
        ramenUI.value += cnt;
        if(cnt > 0) Sound.SoundPlaySE(1);
    }

    public void AddKohai()
    {
        //Interval期間であれば加算しない
        if (isKohaiCountInterval) { return; }

        kohai++;
        kohaiUI.text = kohai.ToString() + "/5";
        Sound.SoundPlaySE(2);
        StartCoroutine(KohaiUIShow());
    }

    IEnumerator KohaiUIShow()
    {
        kohaiObj.SetActive(true);
        isKohaiCountInterval = true;

        yield return new WaitForSeconds(0.25f);
        isKohaiCountInterval = false;

        yield return new WaitForSeconds(0.75f);
        kohaiObj.SetActive(false);
    }

    public void ResetKohai()
    {
        kohai = 0;
        kohaiUI.text = kohai.ToString() + "/5";
    }
}
