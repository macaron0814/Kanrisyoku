using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Explanation : MonoBehaviour
{
    [SerializeField] string[]  titleText;
    [SerializeField] Sprite[]  imageText;
    [Multiline, SerializeField] string[] textText;

    // Start is called before the first frame update
    void OnEnable()
    {
        int rand = Random.Range(0,titleText.Length);

        transform.GetChild(0).GetComponent<Text>().text    = titleText[rand];
        transform.GetChild(1).GetComponent<Image>().sprite = imageText[rand];
        transform.GetChild(2).GetComponent<Text>().text    = textText[rand];
    }
}
