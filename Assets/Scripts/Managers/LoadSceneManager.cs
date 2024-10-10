using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using DG.Tweening;

[RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
public class LoadSceneManager : MonoBehaviour
{
    [Space, Header("UI")] [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingField;
    [SerializeField] private Image loadingSlide;
    [SerializeField] private Image loadingIcon;
    [SerializeField] private float timeInterval;
    [SerializeField] private float loadInterval;
    [SerializeField] private float startLoading;
    [SerializeField] private string targetScene;

    public static LoadSceneManager Instance;
    private Coroutine coroutineCached;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        InitSingleton();
        PlayAnimation();
        DontDestroyOnLoad(this);
#if ENV_PROD
        //DebugLogManager.Instance.gameObject.SetActive(false);
#endif

#if DEBUG_LOG
        Debug.unityLogger.logEnabled = true;
#endif
    }

    private IEnumerator Start()
    {
        Debug.Log("Loading splash scene");
        yield return LoadSceneAsyncStart($"{targetScene}");
    }

    private void InitSingleton()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private IEnumerator LoadSceneAsyncStart(string sceneName)
    {
        loadingPanel.SetActive(true);
        loadingSlide.fillAmount = startLoading;
        while (loadingSlide.fillAmount < 1f)
        {
            loadingSlide.fillAmount += loadInterval;
            yield return new WaitForSeconds(loadInterval);
        }

        yield return new WaitForSeconds(1);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        loadingPanel.SetActive(false);
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);
        loadingSlide.fillAmount = startLoading;
        while (loadingSlide.fillAmount < 1f)
        {
            loadingSlide.fillAmount += loadInterval;
            yield return new WaitForSeconds(loadInterval);
        }

        yield return new WaitForSeconds(1);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        loadingPanel.SetActive(false);
    }


    private void PlayAnimation()
    {
        loadingIcon.rectTransform.DOLocalRotate(new Vector3(0f, 0f, -180f), timeInterval)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }


    public void LoadSceneByName(string name)
    {
        if (coroutineCached != null)
        {
            StopCoroutine(coroutineCached);
        }

        if (SceneExists(name))
        {
            coroutineCached = StartCoroutine(LoadSceneAsync(name));
        }
        else
        {
            Debug.LogError($"Scene {name} not include in build scene");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameWithoutExtension == sceneName)
            {
                return true;
            }
        }

        return false;
    }
}