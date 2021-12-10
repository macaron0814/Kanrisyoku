using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameConfig : MonoBehaviour
{
    [SerializeField] private Material     def;
    [SerializeField] private Material     night;

    [SerializeField] private SGBFilter    sgb_filter;
    [SerializeField] private Texture2D[]  colorPalette;
    [SerializeField] private GameObject[] selectFrame;

    /// <summary>
    /// フレームの変更
    /// </summary>
    /// <param name="num">選択されたフレーム番号</param>
    public void SetFrame(int num)
    {
        switch (num)
        {
            case 0: //デフォルト
                sgb_filter.enabled = false;
                RenderSettings.skybox = def;
                break;

            case 1: //夜
                sgb_filter.enabled = false;
                RenderSettings.skybox = night;
                break;

            case 2: //GB白黒カラー
                sgb_filter.enabled = true;
                sgb_filter.colorPalette = colorPalette[0];
                break;

            case 3: //GB緑カラー
                sgb_filter.enabled = true;
                sgb_filter.colorPalette = colorPalette[1];
                break;
        }
        SelectFrame(num);
    }

    /// <summary>
    /// 選択されているフレームに枠をつける
    /// </summary>
    /// <param name="num">選択されたフレーム番号</param>
    public void SelectFrame(int num)
    {
        for (int i = 0; i < selectFrame.Length; i++)
        {
            if(i == num) selectFrame[i].SetActive(true);
            else         selectFrame[i].SetActive(false);
        }
    }
}
