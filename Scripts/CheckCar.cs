using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using DG.Tweening;

public class CheckCar : MonoBehaviour
{
    [Inject] private Kassa _kassa_scr;
    [Inject] private PlayerStats _playerStats;

    [Header("Установить в инспекторе")]
    [SerializeField] private TextMeshPro _textAddingMoney;
    [SerializeField] private List<RaceCar> _raceCars = new();
    [SerializeField] private float _durationFade = 1f;
    [SerializeField] private PrestigeCounter _prestigeCounter;
    [SerializeField] private RewardFor _rewardFor; //за что начислять деньги

    private Camera mainCamera;

    private enum RewardFor
    {
        cars, //
        quests
    }

    private float _alphaNormalTxt = 1f;
    private float _alphaFadeTxt=0f;

    private float _money; //для отображения игроку над трибуной
    private void Awake()
    {
        ResetMoneyTxt();
        mainCamera = Camera.main;

    }
    private void Init()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _textAddingMoney.transform.LookAt(mainCamera.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RaceCar car))
        {
            _raceCars.Add(car);
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out RaceCar car))
        {
            if (_raceCars.Contains(car))
            {
                _raceCars.Remove(car);
                RewardPlayer(car.MergeNumber);
               
            }
            
        }
    }

    private float _rewardMoney;
    private void RewardPlayer(int mergeNumber)
    {
        _rewardMoney = mergeNumber * 10 * _playerStats.MyltyplyMoney;
        switch (_rewardFor)
        {
            case RewardFor.cars:
                _kassa_scr.AddMoney(_rewardMoney);
                ShowAddingMoney(_rewardMoney);
                break;
            case RewardFor.quests:
                _kassa_scr.AddMoney(_prestigeCounter.GetPrestigeForQuest());
                ShowAddingMoney(_prestigeCounter.GetPrestigeForQuest());
                break;
            default:
                break;
        }

       

    }

    Tween txtTW;
    public void ShowAddingMoney(float money)
    {
        _money += money;
        _textAddingMoney.text = "+" + _money.ToString("F0");
        _textAddingMoney.alpha = _alphaNormalTxt;
        if (txtTW.IsActive())
        {
            if (txtTW.IsPlaying())
            {
                _textAddingMoney.alpha = _alphaNormalTxt;
                txtTW.Restart();
            }
        }
        else
        {
            txtTW = _textAddingMoney.DOFade(_alphaFadeTxt, _durationFade).OnComplete(ResetMoneyTxt);
        }
        

    }
    private void ResetMoneyTxt()
    {
        _money = 0f;
        _textAddingMoney.text = "";
        txtTW = null;
    }
}
