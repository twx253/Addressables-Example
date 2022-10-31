using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class Level
{
    public string levelName;
    public Button loadLevelBtn;
    public Button downloadBtn;
    public Button unloadBtn;
}
public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance;
    public List<Level> levels;
    public Transform m_Download_tip;

    private Level currentLevel;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            Level level = levels[i];
            level.downloadBtn.onClick.AddListener(() =>
            {

                m_Download_tip.GetComponent<DownLoadTip>().m_LevelName = level.levelName;
                DownLoadLevel(level.levelName);
                currentLevel = level;

            });

            level.unloadBtn.onClick.AddListener(() => {
                currentLevel = level;
                ClearLevelCache(level.levelName);
            });

            level.loadLevelBtn.onClick.AddListener(() =>
            {
                currentLevel = level;
                GameMgr.Instacne.LoadLevel(level.levelName);
            });

            //if (CheckLevelCache(level.levelName))
            //{

            //}
        }
    }
    private bool CheckLevelCache(string levelName)
    {
        bool res = false;
        return res;
    }
    
    //清空下载的场景
    private void ClearLevelCache(string LevelName)
    {
        Debug.Log("清空 " + LevelName);
        currentLevel.downloadBtn.gameObject.SetActive(true);
        currentLevel.unloadBtn.gameObject.SetActive(false);
        GameMgr.Instacne.ClearLevelCache(LevelName);
    }

    //下载场景
    private void DownLoadLevel(string levelName)
    {
        GameMgr.Instacne.GetDownloadLevelSize(levelName);
    }

    public void GetDownloadSizeCompleted(float size)
    {
        m_Download_tip.gameObject.SetActive(true);
        m_Download_tip.GetComponent<DownLoadTip>().GetDownloadSize(size);
    }

    public void SetDownloadPercent(float percent)
    {
        m_Download_tip.GetComponent<DownLoadTip>().SetDownloadPercent(percent);
    }

    public void DownLoadComplete(AsyncOperationHandle obj)
    {
        m_Download_tip.GetComponent<DownLoadTip>().DownloadComplete();
        currentLevel.downloadBtn.gameObject.SetActive(false);
        currentLevel.unloadBtn.gameObject.SetActive(true);
        currentLevel.loadLevelBtn.enabled = true;
    }
}
