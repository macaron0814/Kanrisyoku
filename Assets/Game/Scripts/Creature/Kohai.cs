using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kohai : MonoBehaviour
{
    void Start()
    {
        int rand = Random.Range(0, 100);
        if (rand > 100) { gameObject.SetActive(false); }
    }

    private void Update()
    {
        if (gameObject.tag == "Jet" && !Player.isJet) { gameObject.SetActive(false); }
    }
}
