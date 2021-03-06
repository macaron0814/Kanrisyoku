using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleSetting : MonoBehaviour
{
    [SerializeField] private Color  [] selectColor;
    [SerializeField] private Outline[] syainIcon;
    [SerializeField] private GameObject[] syainUI;     
    private void OnEnable()
    {
        SelectSyain(0);

        if (SystemData.save.frameUnlock >= 1) syainUI[1].SetActive(true);
        if (SystemData.save.frameUnlock >= 2) syainUI[2].SetActive(true);
    }

    /// <summary>
    /// 対戦する社員を選択
    /// </summary>
    /// <param name="num">番号</param>
    public void SelectSyain(int num)
    {
        for (int i = 0; i < syainIcon.Length; i++)
        {
            //選択された
            if (num == i)
            {
                syainIcon[i].effectColor = selectColor[0];
                syainIcon[i].effectDistance = new Vector2(10, -10);
            }

            else
            {
                syainIcon[i].effectColor = selectColor[1];
                syainIcon[i].effectDistance = new Vector2(4, -4);
            }
        }

        BossBattleConfig.syainNumber = num;
    }
}
