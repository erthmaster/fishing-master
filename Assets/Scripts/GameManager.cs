using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPlaying;

    private void Awake()
    {
        Instance ??= this;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
        
        if (isPlaying == false && Input.GetKeyDown(KeyCode.Space))
            TogglePause();
    }

    private void TogglePause()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            PausePanel.Instance.Hide();
        }
        else
        {
            PausePanel.Instance.Show();
        }
    }
}
