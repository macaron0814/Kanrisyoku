using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isBoost;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (SystemData.save.coinBoost == 2)
        {
            isBoost = true;
            sr.color = Color.magenta;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
