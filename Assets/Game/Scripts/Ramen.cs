using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramen : MonoBehaviour
{
    [SerializeField]
    private float createRamenValue = 20; //どのくらいスタミナが減っていたらラーメンを生成するのか

    // Start is called before the first frame update
    void Start()
    {
        if (ItemSystem.ramen > (100 - createRamenValue)) { gameObject.SetActive(false); }
    }
}
