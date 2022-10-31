using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instacne;
    public AsyncOperationHandle<SceneInstance> sceneInstance;


    public GameObject mainUI;
    public string activeScene;

    AsyncOperationHandle downloadDependcies;
    private void Awake()
    {
        Instacne = this;
    }
    void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
    }

    //��ȡ������С
    public void GetDownloadLevelSize(string levelName)
    {
        Debug.Log("��ȡ " + levelName + " ��С");
        AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync(levelName);
        Debug.Log(handle.Status);
        handle.Completed += LoadSize_Completed;
    }



    private void LoadSize_Completed(AsyncOperationHandle<long> obj)
    {
        float downloadsize = Byte2M(obj.Result);
        Debug.Log("size = " + downloadsize);
        UIMgr.Instance.GetDownloadSizeCompleted(downloadsize);
    }


    //���س���
    public void StartDownloadLevel(string levelName)
    {
        Debug.Log("��ʼ����");
        StartCoroutine(DownloadScene(levelName));
    }

    IEnumerator DownloadScene(string levelName)
    {
        downloadDependcies = Addressables.DownloadDependenciesAsync(levelName);
        downloadDependcies.Completed += SceneDependenciesComplete;
        while (!downloadDependcies.IsDone)
        {
            DownloadStatus status = downloadDependcies.GetDownloadStatus();
            float percent = status.Percent;
            UIMgr.Instance.SetDownloadPercent(percent);
            yield return 0;
        }
    }

    private void SceneDependenciesComplete(AsyncOperationHandle obj)
    {
        Debug.Log("�������");
        UIMgr.Instance.DownLoadComplete(obj);

    }



    //��ճ�������
    public void ClearLevelCache(string levelName)
    {
        Addressables.ClearDependencyCacheAsync(levelName);
        Addressables.Release(downloadDependcies);
    }

    //���س���
    public void LoadLevel(string sceneName)
    {
        sceneInstance = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        sceneInstance.Completed += (AsyncOperationHandle<SceneInstance> obj) =>
        {
            mainUI.GetComponent<CanvasGroup>().alpha = 0;
            activeScene = obj.Result.Scene.name;
        };
    }


    //
    //ж�س���
    public void UnLoadLevel()
    {
        var handle = Addressables.UnloadSceneAsync(sceneInstance);
        handle.Completed += UnloadCompleted;
    }

    public void UnloadCompleted(AsyncOperationHandle<SceneInstance> scene)
    {
        mainUI.GetComponent<CanvasGroup>().alpha = 1;
        activeScene = SceneManager.GetActiveScene().name;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private float Byte2M(long b)
    {
        float m = b / (Mathf.Pow(1024, 2));
        return m;
    }
}
