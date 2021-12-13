using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] GameObject waveConfig;
    [SerializeField] GameObject energyUI;

    [SerializeField] Color resetColor;
    [SerializeField] int   createinterval = 30;

    public  static float energy = 0;
    private static GameObject eneUI;
    private static bool isGet;

    // Start is called before the first frame update
    void Start()
    {
        isGet  = false;
        energy = 0;
        eneUI  = energyUI;
    }

    // Update is called once per frame
    void Update()
    {
        //waveConfigの要素からWave_BossBattleを探索
        foreach (Transform child in waveConfig.transform)
        {
            if (GameModeConfig.sceneType != GameModeConfig.SCENETYPE.BOSSBATTLE) return;

            if (child.name != "Wave_BossBattle") continue;

            //Wave_BossBattleの要素からEnergyを探索
            foreach (Transform grandChild in child)
            {
                if (!isGet && grandChild.tag == "Energy") ActiveEnergy(grandChild.gameObject);
            }
        }
    }

    /// <summary>
    /// 一定回数に一度オブジェクトを生成する
    /// </summary>
    /// <param name="energyObj">Energyのオブジェクト</param>
    void ActiveEnergy(GameObject energyObj)
    {
        if (energy == createinterval)
        {
            isGet  = true;
            energy = 0;
            return;
        }

        Destroy(energyObj);
        energy++;
    }

    /// <summary>
    /// エネルギーを取得
    /// </summary>
    public static void AddEnergy()
    {
        isGet = false;

        foreach (Transform child in eneUI.transform)
        {
            //取得したら明るくする
            if (child.GetComponent<Image>().color != Color.white)
            {
                child.GetComponent<Image>().color  = Color.white;
                break;
            }

        }
    }
}
