using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveParameter
{
    //save変数
    public int    atkLevel = 1;
    public double atkValue = 1;
    public int    atkCost  = 10;
    public int    defLevel = 1;
    public int    defValue = 1;
    public int    defCost  = 10;
    public int    hpLevel  = 1;
    public int    hpValue  = 100;
    public int    hpCost   = 10;
    public int    coin     = 0;
}

public static class Parameter
{
    private static string filePath;//ファイルのパスを登録
    public  static SaveParameter save = new SaveParameter();//セーブデータを取得

    //=================================================
    //セーブ
    //=================================================
    public static void SaveParameter()
    {
        filePath = Application.persistentDataPath + "/" + ".saveData.json";

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
    private static void LoadParameter()
    {
        filePath = Application.persistentDataPath + "/" + ".saveData.json";

        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<SaveParameter>(data);
        }
    }

    //=================================================
    //削除
    //=================================================
    public static void DeleteParameter()
    {
        filePath = Application.persistentDataPath + "/" + ".saveData.json";
        File.Delete(filePath);
    }
}