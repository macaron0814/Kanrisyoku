using System;
using UnityEngine;
using UnityEngine.UI;

public class eventTime : MonoBehaviour
{
    [SerializeField] private Text eventBoostTimeText;

    // Start is called before the first frame update
    void Start()
    {
        if (!SystemData.save.eventBoost) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Generic.IsDayTimeCount(SystemData.save.eventBoostTime, new TimeSpan(0, 0, 10, 0)))
        {
            SystemData.save.eventBoost = false;
            SystemData.SaveSystemData();
            gameObject.SetActive(false);
        }
        eventBoostTimeText.text
            = (9 - Generic.DayTimeSpan(SystemData.save.eventBoostTime).Minutes).ToString("D2") + ":" +
              (60 - Generic.DayTimeSpan(SystemData.save.eventBoostTime).Seconds).ToString("D2");
    }
}
