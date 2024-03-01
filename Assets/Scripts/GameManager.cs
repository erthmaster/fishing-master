using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPlaying;

    [FormerlySerializedAs("_pausePanel")]
    [FormerlySerializedAs("_startPanel")]
    [Header("UI")] 
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        Instance ??= this;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void TogglePause()
    {
        isPlaying = !isPlaying;
        pausePanel.SetActive(!isPlaying);
    }
}
