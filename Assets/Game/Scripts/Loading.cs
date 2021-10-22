using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] Text load;
    [SerializeField] GameObject loadFirst;

    static int loadingCount;
    int rand;

    // Start is called before the first frame update
    void Start()
    {
        if (loadingCount == 0)
        {
            StartCoroutine(LoadingTime());
            rand = Random.Range(2, 5);
        }

        if (GameModeConfig.sceneType != GameModeConfig.SCENETYPE.LOAD) { Sound.SoundPlayBGM(0); Destroy(gameObject); }
    }

    void Update()
    {
        if(GameModeConfig.sceneType != GameModeConfig.SCENETYPE.LOAD) { return; }

        if (loadingCount > rand)
        {
            Destroy(gameObject);
            GameModeConfig.sceneType = GameModeConfig.SCENETYPE.TITLE;
            Sound.SoundPlayBGM(0);
        }
    }

    //==============================
    //Loadingの...をアニメーション
    //==============================
    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(0.5f);

        if (loadingCount != 0 && loadingCount % 3 == 0)
        {
            load.text = "Loading";
        }
        else
        {
            load.text += ".";
        }
        loadingCount++;

        StartCoroutine(LoadingTime());
    }
}
