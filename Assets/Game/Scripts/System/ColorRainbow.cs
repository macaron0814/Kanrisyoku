using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRainbow : MonoBehaviour
{
    [SerializeField] private Text  text;
    [SerializeField] private float speed;

    [SerializeField] private bool  isDestroy;
    [SerializeField] private float destroyCount = 5.5f;

    [SerializeField] private bool  isFinish;
    [SerializeField] private float finishCount = 3.5f;

    private float timeCount;

    private float r,g,b;

    // Start is called before the first frame update
    void Start()
    {
        r = 1.0f;
        g = 1.0f;
        b = 0.0f;
    }

    void OnEnable()
    {
        timeCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if(isDestroy && timeCount > destroyCount) { Destroy(gameObject); }

        if (isFinish && timeCount > finishCount) return;

        if (r == 1.0f && b == 0)
        {
            g -= speed;
            if (g < 0) { g = 0; }
        }
        if (r == 1.0f && g == 0)
        {
            b += speed;
            if (b > 1.0f) { b = 1.0f; }
        }
        if (b == 1.0f && g == 0)
        {
            r -= speed;
            if (r < 0) { r = 0; }
        }
        if (b == 1.0f && r == 0)
        {
            g += speed;
            if (g > 1.0f) { g = 1.0f; }
        }
        if (g == 1.0f && r == 0)
        {
            b -= speed;
            if (b < 0) { b = 0; }
        }
        if (g == 1.0f && b == 0)
        {
            r += speed;
            if (r > 1.0f) { r = 1.0f; }
        }

        text.color = new Color(r, g, b, 1.0f);// 色を指定
    }
}
