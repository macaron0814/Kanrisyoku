using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectModeConfig : MonoBehaviour
{
    [SerializeField] private Text[] level;
    [SerializeField] private Text[] pram;
    [SerializeField] private Text[] value;

    [SerializeField] private Text coinUI;           //コインUI
    [SerializeField] private GameObject coinLackUI; //コインが不足していた場合に表示されるUI

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_IOS
        iOSRankingUtility.Auth();
        ShowAttDialog.RequestIDFA();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //Parameterの値を常にUIに更新
        {
            //Levelを取得
            level[0].text = "Lv" + Parameter.save.atkLevel.ToString();
            level[1].text = "Lv" + Parameter.save.defLevel.ToString();
            level[2].text = "Lv" + Parameter.save.hpLevel.ToString();

            //パラメータを取得
            pram[0].text = "ATK : " + Parameter.save.atkValue.ToString();
            pram[1].text = "DEF : " + Parameter.save.defValue.ToString();
            pram[2].text = "HP    : " + Parameter.save.hpValue.ToString();

            //費用を取得
            value[0].text = Parameter.save.atkCost.ToString();
            value[1].text = Parameter.save.defCost.ToString();
            value[2].text = Parameter.save.hpCost.ToString();
        }

        //コインを取得
        coinUI.text = Parameter.save.coin.ToString("D4");
    }

    /// <summary>
    /// ランキング一覧表示
    /// </summary>
    public void RankingButton()
    {
        iOSRankingUtility.ShowLeaderboardUI();
    }

    /// <summary>
    /// 実績一覧表示
    /// </summary>
    public void RecordButton()
    {
        iOSRankingUtility.ShowAchievementsUI();
    }

    /// <summary>
    /// 各種パラメータのLevelを上げるボタン
    /// </summary>
    /// <param name="type">Levelを上げる種類</param>
    public void LevelUpButton(string type)
    {
        switch (type)
        {
            //=======================================================
            //攻撃力
            //=======================================================
            case "ATK":

                //コインが足りない場合はreturn
                if (Parameter.save.coin < Parameter.save.atkCost)
                {
                    StartCoroutine(CoinLack());
                    return;
                }

                //レベル上げ
                Parameter.save.atkLevel++;
                level[0].text = "Lv" + Parameter.save.atkLevel.ToString();

                //攻撃力アップ
                Parameter.save.atkValue += 0.1;
                pram[0].text = "ATK : " + Parameter.save.atkValue.ToString();

                //10回に一度かかるお金引き上げ
                if (Parameter.save.atkLevel % 10 == 0)
                {
                    Parameter.save.atkCost += 20;
                    value[0].text = Parameter.save.atkCost.ToString();
                }

                //コスト分をコインから引く
                Parameter.save.coin -= Parameter.save.atkCost;
                break;

            //=======================================================
            //防御力
            //=======================================================
            case "DEF":

                //コインが足りない場合はreturn
                if (Parameter.save.coin < Parameter.save.defCost)
                {
                    StartCoroutine(CoinLack());
                    return;
                }

                //レベル上げ
                Parameter.save.defLevel++;
                level[1].text = "Lv" + Parameter.save.defLevel.ToString();

                //防御力アップ
                Parameter.save.defValue++;
                pram[1].text = "DEF: " + Parameter.save.defValue.ToString();

                //10回に一度かかるお金引き上げ
                if (Parameter.save.defLevel % 10 == 0)
                {
                    Parameter.save.defCost += 30;
                    value[1].text = Parameter.save.defCost.ToString();
                }

                //コスト分をコインから引く
                Parameter.save.coin -= Parameter.save.defCost;
                break;

            //=======================================================
            //体力
            //=======================================================
            case "HP":

                //コインが足りない場合はreturn
                if (Parameter.save.coin < Parameter.save.hpCost)
                {
                    StartCoroutine(CoinLack());
                    return;
                }

                //レベル上げ
                Parameter.save.hpLevel++;
                level[2].text = "Lv" + Parameter.save.hpLevel.ToString();

                //体力アップ
                Parameter.save.hpValue += 10;
                pram[2].text = "HP    : " + Parameter.save.hpValue.ToString();

                //10回に一度かかるお金引き上げ
                if (Parameter.save.hpLevel % 10 == 0)
                {
                    Parameter.save.hpCost += 10;
                    value[2].text = Parameter.save.hpCost.ToString();
                }
                //コスト分をコインから引く
                Parameter.save.coin -= Parameter.save.hpCost;
                break;

            default:
                break;
        }
        Parameter.SaveParameter(); //セーブ
    }

    /// <summary>
    /// コインが足りなかったときの処理
    /// </summary>
    /// <returns></returns>
    IEnumerator CoinLack()
    {
        coinLackUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        coinLackUI.SetActive(false);
    }
}
