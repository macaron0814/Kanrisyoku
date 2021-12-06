using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramen : MonoBehaviour
{
    [Header("残りHP:N%で生成/確率")]
    [SerializeField] private float n = 20;

    [SerializeField] private bool isRandom;

    // Start is called before the first frame update
    void Start()
    {
        float createRamenValue = Parameter.save.hpValue * (n / 100.0f);
        if (!isRandom && ItemSystem.ramen > createRamenValue) { gameObject.SetActive(false); }
        else if (isRandom && Random.Range(0, 100) > n) { gameObject.SetActive(false); }
    }
}
