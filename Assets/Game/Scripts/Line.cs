using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            //足元の座標
            float feet = player.transform.localPosition.y - (player.transform.localScale.y * 20);

            if (feet > transform.localPosition.y) { gameObject.GetComponent<BoxCollider2D>().isTrigger = false; }
            else { gameObject.GetComponent<BoxCollider2D>().isTrigger = true; }
        }
    }
}
