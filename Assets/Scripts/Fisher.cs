using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum FisherState
{
    Working,
    Sleeping,
    DeepSleeping,
    WaitingForWorms,
    Hiding
}

[Serializable]
public struct FishingRodUpgrade
{
    public int price;
    [FormerlySerializedAs("dps")] public int fps;
}

[Serializable]
public struct WormsTankUpgrade
{
    public int price;
    public int amount;
}

public class Fisher : MonoBehaviour, IInteractable, IUpdatable
{
    private static readonly int AnimatorState = Animator.StringToHash("State");
    
    public UnityEvent ViewUpdated;

    public RuntimeAnimatorController[] animations;
    public int[] fishPerLevel;
    public FishingRodUpgrade[] rodsUpgrades;
    public WormsTankUpgrade[] wormsTankUpgrades;
    public int umbrellaPrice;
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
            wormsBucket.UpdateView();
        }
    }

    [Space]
    public int level;
    public int wormsTankLevel;
    public int fishingRodLevel;
    public bool hasUmbrella;

    [Space]
    public int sleepiness;
    public int deepSleepingTime;
    public int levelUpTime;

    public FisherState state;
    
    [Space]
    public Animator animator;
    public FishingRod fishingRod;
    public GameObject umbrella;
    public GameObject fisher;
    public WormsBucket wormsBucket;
    public InfoPanel infoPanel;
    public TextMeshProUGUI fisherName;
    public TextMeshProUGUI status;
    public TextMeshProUGUI interactText;
    public CanvasGroup interactKeyMessage;
    public SpriteRenderer renderer;
    public bool ready;

    public int FPS => fishPerLevel[level] + rodsUpgrades[fishingRodLevel].fps;
    public int MaxWormsAmount => wormsTankUpgrades[wormsTankLevel].amount;
    public float ChanceToSleep => sleepingChancePerLevel[level];

    [Header("UI")] 
    [SerializeField] private int wormsAmount;

    private void Start()
    {
        Updater.Instance.Add(this);

        animator.runtimeAnimatorController = animations[Random.Range(0, animations.Length)];

        WormsAmount = wormsAmount;
        gameObject.SetActive(false);
        UpdateView();
    }

    public void GameUpdate()
    {
        
    }

    public void GameTick()
    {
        if (ready == false)
            return;
        
        CheckWeather();
        
        switch (state)
        {
            case FisherState.Working:
                if (WormsAmount > 0)
                {
                    Player.Instance.FishAmount += FPS;
                    TryConsumeWorm();
                    TryLevelUp();
                }
                else
                {
                    SetState(FisherState.WaitingForWorms);
                }
                break;
            
            case FisherState.Sleeping:
                Debug.Log("Sleeping");
                break;
            
            case FisherState.DeepSleeping:
                deepSleepingTime++;
                TryWakeUp();
                return;
            
            case FisherState.WaitingForWorms:
                if (WormsAmount > 0)
                {
                    SetState(FisherState.Working);
                }
                break;
            
            case FisherState.Hiding:
                SetNormalState();
                return;
        }
        
        IncreaseSleepiness();

        if (sleepiness > hardSleepinessThreshold)
        {
            SetState(FisherState.DeepSleeping);
            deepSleepingTime = 0;
        }
        else if (sleepiness > sleepinessThreshold)
        {
            SetState(FisherState.Sleeping);
        }
    }

    private void TryWakeUp()
    {
        if (deepSleepingTime < minimumDeepSleepTime)
            return;
        
        if (Random.value < wakeUpChance)
        {
            SetNormalState();
            sleepiness = 0;
            deepSleepingTime = 0;
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
            UpdateView();
        }
    }

    private void CheckWeather()
    {
        var weather = Weather.Instance;

        switch (weather.state)
        {
            case WeatherState.Stormy:
                SetState(FisherState.Hiding);
                break;
            
            case WeatherState.Rainy when hasUmbrella == false:
                SetState(FisherState.Hiding);
                break;
        }
    }

    private void SetNormalState()
    {
        var weather = Weather.Instance;

        switch (weather.state)
        {
            case WeatherState.Stormy:
                if (state != FisherState.Hiding)
                    SetState(FisherState.Hiding);
                break;
            
            case WeatherState.Rainy when hasUmbrella:
                SetState(FisherState.Working);
                break;
            
            case WeatherState.Rainy:
                if (state != FisherState.Hiding)
                    SetState(FisherState.Hiding);
                break;
            
            default:
                SetState(FisherState.Working);
                break;
        }
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
        }
    }

    public void SetState(FisherState newState)
    {
        state = newState;
        animator.SetInteger(AnimatorState, (int)state);
        UpdateView();
    }

    public void Interact()
    {
        switch (state)
        {
            case FisherState.Working:
            case FisherState.WaitingForWorms:
                GiveWorms();
                break;
            
            case FisherState.Sleeping:
                break;
        }
    }

    public void GiveWorms()
    {
        WormsAmount += Player.Instance.GetWorms(MaxWormsAmount - WormsAmount);
    }

    public void UpdateView()
    {
        if (state is FisherState.Working or FisherState.Sleeping)
        {
            fishingRod.Show(fishingRodLevel);
        }
        else
        {
            fishingRod.Hide();
        }

        umbrella.SetActive(hasUmbrella);
        fisher.SetActive(state is not FisherState.Hiding);
        interactKeyMessage.alpha = state is not FisherState.Hiding and not FisherState.DeepSleeping ? 1 : 0;
        wormsBucket.UpdateView();
        
        if (state == FisherState.Sleeping)
        {
            interactText.text = "Wake up";
        }
        else
        {
            interactText.text = "Give worms";
        }
        
        switch (state)
        {
            case FisherState.Working:
                status.text = "Working...";
                break;
            
            case FisherState.Sleeping:
            case FisherState.DeepSleeping:
                status.text = "Sleeping...";
                break;
            
            case FisherState.WaitingForWorms:
                status.text = "Waiting worms...";
                break;
            
            case FisherState.Hiding:
                status.text = "Hiding...";
                break;
        }
        
        ViewUpdated.Invoke();
    }

    public void BecomeInteractTarget()
    {
        infoPanel.BecomeInteractTarget(this);
    }

    public void StopBeingInteractTarget()
    {
        infoPanel.StopBeingInteractTarget();
    }
}
