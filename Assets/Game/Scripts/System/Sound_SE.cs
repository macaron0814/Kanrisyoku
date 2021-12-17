using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_SE : MonoBehaviour
{
    public static Sound_SE instance;

    void Awake()
    {
        // シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}