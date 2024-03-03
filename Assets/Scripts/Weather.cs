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

    public int MinTicks = 20;
    public int MaxTicks = 40;

    [SerializeField] private GameObject rainyParticles;
    [SerializeField] private GameObject stormyParticles;
    [SerializeField] private GameObject sunnyParticles;
    
    [SerializeField] private float ticksToWeatherChange;

    private void Awake()
    {
        Instance ??= this;
    }

    private void Start()
    {
        Updater.Instance.Add(this);       
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
    }
}
