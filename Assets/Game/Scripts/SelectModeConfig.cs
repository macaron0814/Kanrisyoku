using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeConfig : MonoBehaviour
{
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
}
