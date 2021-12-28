using System;
using UnityEngine;
using UnityEngine.UI;

public class coinBoostTime : MonoBehaviour
{
    [SerializeField] private Text CoinBoostTimeText;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemData.save.coinBoost == 0) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Generic.IsDayTimeCount(SystemData.save.coinBoostTime,new TimeSpan(0, 0, 10, 0)))
        {
            SystemData.save.coinBoost = 0;
            SystemData.save.bossLoseCount = UnityEngine.Random.Range(3, 7);
            SystemData.SaveSystemData();
            gameObject.SetActive(false);
        }

        CoinBoostTimeText.text 
            = (9 - Generic.DayTimeSpan(SystemData.save.coinBoostTime).Minutes).ToString("D2") + ":" + 
              (60 - Generic.DayTimeSpan(SystemData.save.coinBoostTime).Seconds).ToString("D2");
    }
}
