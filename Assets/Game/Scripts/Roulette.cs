using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject roulette;
    [SerializeField] private GameObject rouletteReward;
    [SerializeField] private GameObject fanfare;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float division = 6;
    private bool isStop;
    private float[] randomDeceleration = { 0.9825f, 0.985f, 0.9875f, 0.99f, 0.991f };
    private float se;

    // Start is called before the first frame update
    void Start()
    {
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        se += speed;
        if (se > 60)
        {
            se = 0;
            Sound.SoundPlaySE(41);
        }

        if (isStop) speed *= randomDeceleration[0];
        if (speed >= 0.075f) roulette.transform.Rotate(new Vector3(0, 0, -speed));//1フレームごとに加算
        else
        {
            button.SetActive(true);
            fanfare.SetActive(true);
            Sound.SoundPlaySE(33);

            for (int i = 1; i < division + 1; i++)
            {
                if (roulette.transform.eulerAngles.z > (i - 1) * (360.0f / division) &&
                    roulette.transform.eulerAngles.z <  i      * (360.0f / division))
                {
                    switch (i - 1)
                    {
                        case 0:
                            Parameter.save.coin += 40;
                            Sound.SoundPlaySE(43);
                            break;
                        case 1:
                            SystemData.save.bossWait = 20.0f;
                            Sound.SoundPlaySE(43);
                            break;
                        case 2:
                            Parameter.save.coin += 20;
                            Sound.SoundPlaySE(43);
                            break;
                        case 3:
                            SystemData.save.bossWait = 10.0f;
                            Sound.SoundPlaySE(43);
                            break;
                        case 4:
                            Parameter.save.coin += 80;
                            Sound.SoundPlaySE(43);
                            break;
                        case 5:
                            Parameter.save.coin += 100;
                            SystemData.save.bossWait = 25.0f;
                            SystemData.save.bossBattleATK = 1.5f;
                            Sound.SoundPlaySE(44);
                            break;
                    }
                    rouletteReward.transform.GetChild(i - 1).gameObject.SetActive(true);
                    if (Parameter.save.coin > 9999) Parameter.save.coin = 9999;

                    //セーブ
                    Parameter.SaveParameter();
                    SystemData.SaveSystemData();
                }
            }
            GetComponent<Roulette>().enabled = false;
        }
    }

    public void StopRoulette()
    {
        isStop = true;
        Sound.SoundPlaySE(42);
        randomDeceleration[0] = randomDeceleration[Random.Range(0, randomDeceleration.Length)];
    }
}
