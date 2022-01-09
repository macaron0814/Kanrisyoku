using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveRecord
{
    //save変数(実績)
    public bool     firstrun;           //初めて走った
    public byte     run;                //走った回数
    public float    runTotalMeter;      //走った距離
    public bool[]   dead = new bool[3]; //死んだ種類
    public bool     kohaiJet;           //コーハイジェット発動
    public bool     openBossBattle;     //ボスバトル解放
    public bool     joshi;              //ジョーシ撃破
    public bool     syacho;             //シャチョ撃破
    public bool     kaicho;             //カイチョ撃破
    public bool     perfectJoshi;       //ノーミスでジョーシを撃破
    public bool     perfectSyacho;      //ノーミスでシャチョを撃破
    public bool     perfectKaicho;      //ノーミスでカイチョを撃破

    public string[] recordKey = new string[100]; //実績を解放しているのかを確認

    public long[] bestBossBattleScore = new long[5]; //ハイスコア
}

public static class Record
{
    private static string filePath;//ファイルのパスを登録
    public  static SaveRecord save = new SaveRecord();//セーブデータを取得

    //=================================================
    //save変数を外部から名称で呼び出せるように
    //=================================================
    public enum RecordList
    {
        FIRSTRUN,
        RUNCOUNT,
        RUNTOTALMETER,
        DEAD,
        KOHAIJET,
        OPENBOSSBATTLE,
        JOSHI,
        SYACHO,
        KAICHO,
        PERFECTJOSHI,
        PERFECTSYACHO,
        PERFECTKAICHO
    }

    //=================================================
    //Recordに設定された変数の更新
    //(達成が期待できるタイミングで呼ぶ)
    //=================================================
    public static void UpdateRecord(RecordList list, float value = 0)
    {
        if (list == RecordList.FIRSTRUN)        save.firstrun = true;

        if (list == RecordList.RUNCOUNT)        save.run++;
        if (list == RecordList.RUNTOTALMETER)   save.runTotalMeter += value;

        if (list == RecordList.DEAD)            save.dead[(int)value] = true;

        if (list == RecordList.KOHAIJET)        save.kohaiJet = true;

        if (list == RecordList.OPENBOSSBATTLE)  save.openBossBattle = true;

        if (list == RecordList.JOSHI)  save.joshi = true;
        if (list == RecordList.SYACHO) save.syacho = true;
        if (list == RecordList.KAICHO) save.kaicho = true;

        if (list == RecordList.PERFECTJOSHI) save.perfectJoshi = true;
        if (list == RecordList.PERFECTSYACHO) save.perfectSyacho = true;
        if (list == RecordList.PERFECTKAICHO) save.perfectKaicho = true;

        SaveRecord();
    }

    //=================================================
    //条件を達成してたら送信
    //=================================================
    public static void ClearRecord()
    {
        //合計走行距離
        iOSRankingUtility.ReportProgress("run_totalmeter_500",  save.runTotalMeter * 0.2);
        iOSRankingUtility.ReportProgress("run_totalmeter_2000", save.runTotalMeter * 0.05);
        iOSRankingUtility.ReportProgress("run_totalmeter_5000", save.runTotalMeter * 0.02);

        //はじめての走り
        if (save.firstrun)   { iOSRankingUtility.ReportProgress("first_run", 100); }

        //走った回数
        if (save.run <= 10)  { iOSRankingUtility.ReportProgress("run10", save.run * 10); }
        if (save.run <= 50)  { iOSRankingUtility.ReportProgress("run50", save.run * 2); }
        if (save.run <= 100) { iOSRankingUtility.ReportProgress("run100", save.run); }

        //死んだ種類
        if (save.dead.Distinct().Count() == 1) { iOSRankingUtility.ReportProgress("dead_3", 100); }

        //コーハイジェット発動
        if (save.kohaiJet) { iOSRankingUtility.ReportProgress("kohai_jet", 100); }

        //ボスバトル解放
        if (save.openBossBattle) { iOSRankingUtility.ReportProgress("open_bossbattle", 100); }

        //ボス撃破
        if (save.joshi)  { iOSRankingUtility.ReportProgress("joshi" , 100); }
        if (save.syacho) { iOSRankingUtility.ReportProgress("syacho", 100); }
        if (save.kaicho) { iOSRankingUtility.ReportProgress("kaicho", 100); }

        //ボスノーミス撃破
        if (save.perfectJoshi)  { iOSRankingUtility.ReportProgress("perfect_joshi" , 100); }
        if (save.perfectSyacho) { iOSRankingUtility.ReportProgress("perfect_syacho", 100); }
        if (save.perfectKaicho) { iOSRankingUtility.ReportProgress("perfect_kaicho", 100); }
    }

    //=================================================
    //セーブ
    //=================================================
    public static void SaveRecord()
    {
        filePath = Application.persistentDataPath + "/" + ".saveRecord.json";

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
    private static void LoadRecord()
    {
        filePath = Application.persistentDataPath + "/" + ".saveRecord.json";

        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<SaveRecord>(data);
        }
    }

    //=================================================
    //削除
    //=================================================
    public static void DeleteRecord()
    {
        filePath = Application.persistentDataPath + "/" + ".saveRecord.json";
        File.Delete(filePath);
    }
}


