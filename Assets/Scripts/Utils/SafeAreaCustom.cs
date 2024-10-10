using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class SafeAreaCustom : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    private void Start()
    {
#if UNITY_ANDROID
        SafeAreaApplyAndroid();
#endif

#if UNITY_IOS
        SafeAreaApplyIOS();
#endif
    }

    void SafeAreaApplyAndroid()
    {
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    void SafeAreaApplyIOS()
    {
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        //rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0);
        rectTransform.anchorMax = anchorMax;
    }
}