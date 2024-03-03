using System;
using System.Collections;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
    public float stormChance;
    public float ligtingChance;
    public Image stormLighting;
    public Color sunnyColor;
    public Color rainyColor;
    public Color stormyColor;

    [SerializeField] private GameObject rainyParticles;
    [SerializeField] private GameObject stormyParticles;
    [SerializeField] private GameObject sunnyParticles;
    [SerializeField] private AudioSource thunderAudioSource;
    
    [SerializeField] private AudioMixerSnapshot stormyClip;
    [SerializeField] private AudioMixerSnapshot sunnyClip;
    [SerializeField] private AudioMixerSnapshot rainyClip;
    
    [SerializeField] private float ticksToWeatherChange;

    private AnimationPlayer _lighting;
    private StatedFluentAnimationPlayer<Color> _cameraColor;

    private void Awake()
    {
        Instance ??= this;

        _lighting = new AnimationPlayer(this, new ITrack[]
        {
            new ImageAlphaTrack(stormLighting, new[]
            {
                new KeyFrame<float>(0, 0, Easing.QuadOut),
                new KeyFrame<float>(150, 0.7f, Easing.QuadOut),
                new KeyFrame<float>(180, 0.2f, Easing.QuadOut),
                new KeyFrame<float>(230, 0.5f, Easing.QuadOut),
                new KeyFrame<float>(260, 0.13f, Easing.QuadOut),
                new KeyFrame<float>(290, 0.2f, Easing.QuadOut),
                new KeyFrame<float>(360, 0, Easing.QuadOut),
            })
        });

        var track = new FluentCameraBackgroundColorTrack(Camera.main, new Transition(1500));

        _cameraColor = new StatedFluentAnimationPlayer<Color>(this, track);
        _cameraColor.StateChanged += (previous, current, options) =>
        {
            track.Set(current, options);
        };
        _cameraColor.SetStateInstant(sunnyColor);
    }

    private void Start()
    {
        Updater.Instance.Add(this);
        state = WeatherState.Sunny;
        sunnyParticles.SetActive(state == WeatherState.Sunny);
        ticksToWeatherChange = Random.Range(MinTicks, MaxTicks);
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

        if (state == WeatherState.Stormy && Random.value < ligtingChance)
        {
            _lighting.Play();
            thunderAudioSource.Play();
            var fisher = Fishers.Instance.FishersList[Random.Range(0, Fishers.Instance.FishersList.Length)];
            fisher.hasUmbrella = false;
            fisher.UpdateView();
        }
    }

    private void ChangeWeather()
    {
        if (state == WeatherState.Sunny)
        {
            if (Random.value < stormChance)
            {
                state = WeatherState.Stormy;
            }
            else
            {
                state = WeatherState.Rainy;
            }
        }
        else
        {
            state = WeatherState.Sunny;
        }
        
        sunnyParticles.SetActive(state == WeatherState.Sunny);
        rainyParticles.SetActive(state == WeatherState.Rainy);
        stormyParticles.SetActive(state == WeatherState.Stormy);
        
        if (state == WeatherState.Stormy)
        {
            PausePanel.Instance.Play(stormyClip);
            _cameraColor.SetState(stormyColor);
        }
        else if (state == WeatherState.Rainy)
        {
            PausePanel.Instance.Play(rainyClip);
            _cameraColor.SetState(rainyColor);
        }
        else if(state == WeatherState.Sunny)
        {
            PausePanel.Instance.Play(sunnyClip);
            _cameraColor.SetState(sunnyColor);
        }
    }
}
