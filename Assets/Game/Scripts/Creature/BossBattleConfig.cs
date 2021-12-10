using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleConfig : MonoBehaviour
{
    [SerializeField] private GameObject[] syain;
    [SerializeField] private GameObject[] syainUI;

    public static GameObject[] sya;
    public static GameObject[] syaUI;
    public static int syainNumber;

    // Start is called before the first frame update
    void Start()
    {
        sya = syain;
        syaUI = syainUI;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
