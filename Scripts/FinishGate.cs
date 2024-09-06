using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using DG.Tweening;

public class FinishGate : MonoBehaviour
{

    [Inject] private Kassa _kassa_scr;
    [Inject] private PlayerStats _playerStats;

    [Header("Установить в инспекторе")]
    [SerializeField] private TextMeshPro _textAddingMoney;
    [SerializeField] private float _durationFade = 1f;
    [SerializeField] private PrestigeCounter _prestigeCounter;


    private Camera mainCamera;


    private float _alphaNormalTxt = 1f;
    private float _alphaFadeTxt = 0f;

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
          //  Debug.Log("Сработка");
            
            if (car.state == RaceCar.Location.startingLap)
            {
                RewardPlayer(car.MergeNumber);
            }


        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent(out RaceCar car))
        {
            if (car.state == RaceCar.Location.atParking)
            {
                car.state = RaceCar.Location.startingLap;
            }
            
            /* if (_carsOnLap.ContainsKey(car))
             {
                 _carsOnLap[car] = "На круге";
             }*/


        }
    }

    private float _rewardMoney;
    private void RewardPlayer(int mergeNumber)
    {
        _rewardMoney = mergeNumber * 10 * _playerStats.MyltyplyMoney;

        _kassa_scr.AddMoney(_prestigeCounter.GetPrestigeForQuest());
        ShowAddingMoney(_prestigeCounter.GetPrestigeForQuest());

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
