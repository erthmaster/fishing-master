using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum WeatherState
{
    Sunny,
    Rainy,
    Stormy,
}

public class Weather : MonoBehaviour, IUpdatable
{
    public static Weather Instance;
    
    public WeatherState state { get; private set; }

    public int MinTicks = 2;
    public int MaxTicks = 4;

    [SerializeField] private GameObject rainyParticles;
    [SerializeField] private GameObject stormyParticles;
    [SerializeField] private GameObject sunnyParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stormyClip;
    [SerializeField] private AudioClip sunnyClip;
    [SerializeField] private AudioClip rainyClip;
    
    [SerializeField] private float ticksToWeatherChange;

    private void Awake()
    {
        Instance ??= this;
    }

    private void Start()
    {
        Updater.Instance.Add(this);
        state = WeatherState.Sunny;
        sunnyParticles.SetActive(state == WeatherState.Sunny);
        ticksToWeatherChange = Random.Range(MinTicks, MaxTicks);
        audioSource.clip = sunnyClip;
        audioSource.Play();
    }

    public void GameUpdate()
    {
           
    }

    public void GameTick()
    {
        ticksToWeatherChange--;

        if (ticksToWeatherChange <= 0)
        {
            ticksToWeatherChange = Random.Range(MinTicks, MaxTicks);
            ChangeWeather();
        }
    }

    private void ChangeWeather()
    {
        state = (WeatherState)Random.Range(0, 3);
        sunnyParticles.SetActive(state == WeatherState.Sunny);
        rainyParticles.SetActive(state == WeatherState.Rainy);
        stormyParticles.SetActive(state == WeatherState.Stormy);
        
        if (state == WeatherState.Stormy)
        {
            audioSource.clip = stormyClip;
        }
        else if (state == WeatherState.Rainy)
        {
            audioSource.clip = rainyClip;
        }
        else if(state == WeatherState.Sunny)
        {
            audioSource.clip = sunnyClip;
        }
        audioSource.Play();
    }
}
