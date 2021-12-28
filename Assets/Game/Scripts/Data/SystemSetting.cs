using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystemSetting
{
    //save変数
    public float bgm = 0.4f;
    public float se  = 0.3f;
}

public static class SystemSetting
{
    private static string filePath;//ファイルのパスを登録
    public static SaveSystemSetting save = new SaveSystemSetting();//セーブデータを取得

    //=================================================
    //セーブ
    //=================================================
    public static void SaveSystemSetting()
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
    private static void LoadSystemSetting()
    {
        filePath = Application.persistentDataPath + "/" + ".saveData.json";

        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<SaveSystemSetting>(data);
        }
    }
}