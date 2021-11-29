using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = RandomFixedValue.forInt(new int[] { 1, 2, 3 });
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
    }
}
