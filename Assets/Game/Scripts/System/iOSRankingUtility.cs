//  iOSRankingUtility.cs
//
//  元 http://kan-kikuchi.hatenablog.com/entry/iOSRankingUtility
//  Created by kan.kikuchi on 2016.03.31.
//
//  改良版 https://note.com/08_14/n/n5b5161d5b699
//  Created by macaron on 2021.10.25.

using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if !UNITY_ANDROID
using UnityEngine.SocialPlatforms.GameCenter;
#endif

/// <summary>
/// iOSのランキング用便利クラス
/// </summary>
public static class iOSRankingUtility
{

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// ユーザー認証
    /// </summary>
    public static void Auth(Action<bool> callBack = null)
    {
        //コールバックが設定されていない場合はログを設定
        if (callBack == null)
        {
            callBack = (success) => {
                Debug.Log(success ? "認証成功" : "認証失敗");
            };
        }
        
        Social.localUser.Authenticate(callBack);
    }

    //=================================================================================
    //ランキング
    //=================================================================================

    /// <summary>
    /// リーダーボードを表示する
    /// </summary>
    public static void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }

    /// <summary>
    /// リーダーボードにスコアを送信する
    /// </summary>
    public static void ReportScore(string leaderboardID, long score, Action<bool> callBack = null)
    {

        //コールバックが設定されていない場合はログを設定
        if (callBack == null)
        {
            callBack = (success) => {
                Debug.Log(success ? "スコア送信成功" : "スコア送信失敗");
            };
        }

        //エディター上で送信すると、エラーが出るので、送信が成功したことにする
#if UNITY_EDITOR

        callBack(true);

#else

    //送信
    Social.ReportScore (score, leaderboardID, callBack);

#endif
    }

    //=================================================================================
    //実績
    //=================================================================================

    /// <summary>
    /// 実績一覧を表示する
    /// </summary>
    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    /// <summary>
    /// 実績の進捗状況を送信する
    /// </summary>
    public static void ReportProgress(string achievementKey, double percent, Action<bool> callBack = null)
    {
        //値が100を超えていたら100に戻す
        if (percent >= 100) { percent = 100; }

        //値が100なら通知を出す
        if (percent == 100) { RecordAchievement.Action(achievementKey); }

        //エディター上で送信すると、エラーが出るので、送信が成功したことにする
#if UNITY_EDITOR

#else
        //送信
        for(int i = 0; i < 100; i++){ GKAchievementReporter.ReportAchievement(achievementKey, (float)percent, false); }
#endif
    }
}