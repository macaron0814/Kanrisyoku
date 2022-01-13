using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveSystemData
{
    //save変数

    //フレーム
    public int setFrame = 0;
    public int frameUnlock = 0;

    //ボス
    public float bossWait = 0;
    public float bossBattleATK = 0;

    //ルーレット
    public int rouletteNolma = 10;
    public int rouletteCount = 0;

    //コインブースト
    public int coinBoost = 0;
    public int[] coinBoostTime = new int[4];

    //コインブーストまでのボスに負けた回数
    public int bossLoseCount = 3;

    //チューリアル
    public bool isTutorial;

    //レビュー
    public bool isReview;

    //通知
    public string[] waitNotificationName = new string[10] { "", "", "", "", "", "", "", "", "", "" };
}

public static class SystemData
{
    private static string filePath;//ファイルのパスを登録
    public static SaveSystemData save = new SaveSystemData();//セーブデータを取得

    //=================================================
    //セーブ
    //=================================================
    public static void SaveSystemData()
    {
        filePath = Application.persistentDataPath + "/" + ".saveSystem.json";

        string json = JsonUtility.ToJson(save);

        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    //=================================================
    //ロード
    //=================================================
    [RuntimeInitializeOnLoadMethod()]
    private static void LoadSystemData()
    {
        filePath = Application.persistentDataPath + "/" + ".saveSystem.json";

        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<SaveSystemData>(data);
        }
    }

    //=================================================
    //削除
    //=================================================
    public static void DeleteSystemData()
    {
        save.bossBattleATK = 0;
        save.bossLoseCount = 3;
        save.bossWait = 0;
        save.coinBoost = 0;
        save.coinBoostTime = new int[4];
        save.frameUnlock = 0;
        save.isReview = false;
        save.isTutorial = false;
        save.rouletteCount = 0;
        save.rouletteNolma = 10;
        save.setFrame = 0;
        save.waitNotificationName = new string[10] { "", "", "", "", "", "", "", "", "", "" };

        SaveSystemData();
    }
}