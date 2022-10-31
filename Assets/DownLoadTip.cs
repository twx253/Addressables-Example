using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DownLoadTip : MonoBehaviour
{
    public string m_LevelName;
    public TextMeshProUGUI downloadsize;
    public GameObject percentpanel;
    public TextMeshProUGUI downloadPercent;
    public Scrollbar downloadSlider;
    public Button downloadbtn;
    public Button cancelbtn;
    private bool downloaded;
    void Start()
    {
        downloadbtn.onClick.AddListener(() =>
        {
            DownloadLevel(m_LevelName);
        });
        cancelbtn.onClick.AddListener(() => { gameObject.SetActive(false); });
        gameObject.SetActive(false);
        percentpanel.SetActive(false);

    }

    private void DownloadLevel(string levelName)
    {
        percentpanel.SetActive(true);
        GameMgr.Instacne.StartDownloadLevel(levelName);
    }

    //--------------------------------------------------
    public void GetDownloadSize(float size)
    {
        this.downloadsize.text = size + " MB";
    }
    public void SetDownloadPercent(float percent)
    {
        downloadSlider.size = percent;
        downloadPercent.text = string.Format("{0:N1}%",percent*100);

    }
    public void DownloadComplete()
    {
        percentpanel.SetActive(false);
        gameObject.SetActive(false);
        downloadSlider.size = 0;
        downloadPercent.text = "0.0%";

    }



}
