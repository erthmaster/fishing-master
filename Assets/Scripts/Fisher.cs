using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum FisherState
{
    Working,
    Sleeping,
    DeepSleeping,
    WaitingForWorms,
    Hiding
}

public class Fisher : MonoBehaviour, IInteractable, IUpdatable
{
    public UnityEvent LevelChanged;
    
    public int[] fishPerLevel;
    public int[] fishPerRodLevel;
    public float[] sleepingChancePerLevel;
    public float chanceToConsumeWorm;
    public int hardSleepinessThreshold;
    public int sleepinessThreshold;
    public int minimumDeepSleepTime;
    public float wakeUpChance;
    public int[] levelUpTimes;

    public int WormsAmount
    {
        get => wormsAmount;
        set
        {
            wormsAmount = value;
            wormsText.text = $"Worms:\n{wormsAmount} / {maxWormsAmount}";
        }
    }
    [Space]
    public int maxWormsAmount;

    [Space]
    public int level;
    public int fishingRodLevel;
    public bool umbrella;

    [Space]
    public int sleepiness;
    public int deepSleepingTime;
    public int levelUpTime;

    public FisherState state;

    public int FPS => fishPerLevel[level] + fishPerRodLevel[fishingRodLevel];
    public float ChanceToSleep => sleepingChancePerLevel[level];

    [Header("UI")] 
    [SerializeField] private TextMeshPro wormsText;
    [SerializeField] private int wormsAmount;

    private void Start()
    {
        Updater.Instance.Add(this);
        Fishers.Instance.Add(this);

        WormsAmount = wormsAmount;
        gameObject.SetActive(false);
    }

    public void GameUpdate()
    {
        
    }

    public void GameTick()
    {
        Debug.Log("Tick");
        switch (state)
        {
            case FisherState.Working:
                if (WormsAmount > 0)
                {
                    Debug.Log($"Working: " + FPS);
                    TryConsumeWorm();
                    TryLevelUp();
                }
                else
                {
                    state = FisherState.WaitingForWorms;
                }
                break;
            
            case FisherState.Sleeping:
                Debug.Log("Sleeping");
                break;
            
            case FisherState.DeepSleeping:
                Debug.Log("Deep sleeping");
                deepSleepingTime++;
                TryWakeUp();
                return;
            
            case FisherState.WaitingForWorms:
                Debug.Log("Waiting for worms");
                if (WormsAmount > 0)
                {
                    state = FisherState.Working;
                }
                break;
            
            case FisherState.Hiding:
                return;
        }
        
        IncreaseSleepiness();

        if (sleepiness > hardSleepinessThreshold)
        {
            state = FisherState.DeepSleeping;
            deepSleepingTime = 0;
        }
        else if (sleepiness > sleepinessThreshold)
        {
            state = FisherState.Sleeping;
        }
    }

    private void TryWakeUp()
    {
        if (deepSleepingTime < minimumDeepSleepTime)
            return;
        
        if (Random.value < wakeUpChance)
        {
            Debug.Log("Woke up");
            SetNormalState();
            sleepiness = 0;
            deepSleepingTime = 0;
        }
        else
        {
            Debug.Log("Failed to wake up");
        }
    }

    private void TryLevelUp()
    {
        if (level >= levelUpTimes.Length)
            return;

        levelUpTime++;

        if (levelUpTime >= levelUpTimes[level - 1])
        {
            level++;
            levelUpTime = 0;
            LevelChanged.Invoke();
        }
    }

    private void SetNormalState()
    {
        state = FisherState.Working;
    }

    private void IncreaseSleepiness()
    {
        if (Random.value < ChanceToSleep)
            sleepiness++;
    }

    private void TryConsumeWorm()
    {
        if (Random.value < chanceToConsumeWorm)
        {
            WormsAmount--;
            Debug.Log("Consumed worm");
        }
    }

    public void Show()
    {
        wormsText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        wormsText.gameObject.SetActive(false);
    }

    public void Interact()
    {
        WormsAmount += Player.Instance.GetWorms(maxWormsAmount - WormsAmount);
    }

    public void BecomeInteractTarget()
    {
        Show();
    }

    public void StopBeingInteractTarget()
    {
        Hide();
    }
}
