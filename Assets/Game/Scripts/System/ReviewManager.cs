using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour
{
    private static ReviewManager instance;
    public static ReviewManager Instance
    {
        get
        {
            if (!instance)
            {
                System.Type type = typeof(ReviewManager);
                instance = (ReviewManager)FindObjectOfType(type);
                if (!instance)
                {
                    GameObject obj = new GameObject(type.ToString(), type);
                    instance = obj.GetComponent<ReviewManager>();
                }
            }
            if (!instance)
            {
                Debug.LogError("Not Found ReviewManager.");
            }
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RequestReview()
    {
#if UNITY_IOS && !UNITY_EDITOR
        UnityEngine.iOS.Device.RequestStoreReview();
#elif UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(RequestReviewAndroid());
#else
        Debug.LogWarning("This platform is not support RequestReview.");
#endif
    }

#if UNITY_ANDROID
    private IEnumerator RequestReviewAndroid()
    {
        var reviewManager = new Google.Play.Review.ReviewManager();
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        var playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
    }
#endif

}