using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject[] setNotification;
    public static GameObject[] sNotification = new GameObject[10];

    [SerializeField] private GameObject backGround;

    private void Start()
    {
        sNotification = setNotification;
    }



    /// <summary>
    /// 待機通知
    /// </summary>
    /// <param name="Obj">参照オブジェクト</param>
    public static void WaitNotification(GameObject Obj)
    {
        for (int i = 0; i < SystemData.save.waitNotificationName.Length; i++)
        {
            if(SystemData.save.waitNotificationName[i] == "")
            {
                SystemData.save.waitNotificationName[i] = Obj.name;
                break;
            }
        }
        SystemData.SaveSystemData();
    }

    /// <summary>
    /// 表示される通知
    /// </summary>
    public void ActiveNotification()
    {
        if (!backGround.activeSelf) backGround.SetActive(true);

        for (int i = 0; i < setNotification.Length; i++)
        {
            for (int j = 0; j < SystemData.save.waitNotificationName.Length; j++)
            {
                if (setNotification[i] != null && SystemData.save.waitNotificationName[j] == setNotification[i].name)
                {
                    if (i == 1) SystemData.save.rouletteCount = 0;
                    setNotification[i].SetActive(true);
                    Sound.SoundPlaySE(39);
                    return;
                }
                backGround.SetActive(false);
            }
        }
        SystemData.SaveSystemData();
    }

    /// <summary>
    /// 次の通知
    /// </summary>
    /// <param name="Obj">参照オブジェクト</param>
    public void NextNotification(GameObject Obj)
    {
        Sound.SoundPlaySE(7);

        for (int i = 0; i < SystemData.save.waitNotificationName.Length; i++)
        {
            if (SystemData.save.waitNotificationName[i] == Obj.name)
            {
                SystemData.save.waitNotificationName[i] = "";
                Obj.SetActive(false);
                break;
            }
        }
        ActiveNotification();
    }
}
