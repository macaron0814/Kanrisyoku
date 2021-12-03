using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramen : MonoBehaviour
{
    [Header("スタミナN%消費で生成")]
    [SerializeField] private float n = 20;

    // Start is called before the first frame update
    void Start()
    {
        float createRamenValue = Parameter.save.hpValue * (n / 100.0f);
        if (ItemSystem.ramen > (100 - createRamenValue)) { gameObject.SetActive(false); }
    }
}
