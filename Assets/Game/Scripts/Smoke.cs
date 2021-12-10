using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private Vector3 startPos;

    private Transform enemyTram;
    private Vector3 enemyPos;
    private Vector3 enemyStartPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;

        enemyTram = GameObject.FindGameObjectWithTag("Syain").transform;
        enemyStartPos = enemyTram.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = enemyTram.localPosition;
        Vector3 vec = enemyStartPos - enemyPos;
        transform.localPosition = new Vector3(startPos.x - vec.x, startPos.y - vec.y, 1);
    }
}
