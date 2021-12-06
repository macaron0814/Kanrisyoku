using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramen : MonoBehaviour
{
    [Header("残りHP:N%で生成")]
    [SerializeField] private float n = 20;

    // Start is called before the first frame update
    void Start()
    {
        float createRamenValue = Parameter.save.hpValue * (n / 100.0f);
        if (ItemSystem.ramen > createRamenValue) { gameObject.SetActive(false); }
    }
}
