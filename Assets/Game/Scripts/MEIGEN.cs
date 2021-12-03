using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEIGEN : MonoBehaviour
{
    [SerializeField] private string[] meigen;
    private Text meigenUI;

    // Start is called before the first frame update
    void Start()
    {
        meigenUI = GetComponent<Text>();
    }

    private void OnEnable()
    {
        meigenUI.text = meigen[Random.Range(0, meigen.Length)];
    }
}
