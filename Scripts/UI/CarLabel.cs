using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CarLabel : MonoBehaviour
{
    [Inject] private ParkingManager _parkingManager;
    [Inject] private PlayerStats _playerStats;

    [Header("Установить в инспекторе")]
    [SerializeField] private TextMeshProUGUI _mergeNumTxt;
    [SerializeField] private Image _iconCar;
    [SerializeField] private GameObject _iconMoney;
    [SerializeField] private GameObject _iconGold;
    [SerializeField] private TextMeshProUGUI _priceCarTxt;
    [SerializeField] private Transform _barAcelerate;
    [SerializeField] private Transform _barMaxSpeed;
    
    [SerializeField] private Button _labelButton;

    private int _mergeNum;
    private bool _isInitializated;
    private MergeManager _mergeManager;
    private float _absoluteMaxSpeed = 45f; //самая большая скорость из всех, для отображения полоски бара
    private float _absoluteAcelerate = 45f; // самое большое ускорение
    private float _offsetMaxSpeed = 25f;
    private float _offseteAcelerate = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int MergeNumber, int RecordMerge, MergeManager mergeManager)
    {


        if (_isInitializated) return;
        _isInitializated = true;
        _mergeManager = mergeManager;

        _mergeNum = MergeNumber;
        _mergeNumTxt.text = MergeNumber.ToString();

        _priceCarTxt.text = mergeManager.GetPriceUpgradeMoney(MergeNumber).ToString("F0");

        MergeCar car = mergeManager.GetPrefabCar(MergeNumber);
        _iconCar.sprite = car.GetImageCar();
        _barAcelerate.localScale = new((car.GetAcelerate()- _offseteAcelerate) / (_absoluteAcelerate- _offseteAcelerate), 1f, 1f);
        _barMaxSpeed.localScale = new((car.GetMaxSpeed()- _offsetMaxSpeed) / (_absoluteMaxSpeed- _offsetMaxSpeed), 1f, 1f);

        QuestSystem.OnNewRecordMerge.AddListener(CheckAvialable);

        CheckAvialable(RecordMerge);

        //_mergeManager.OnUpPrice.AddListener(UprgareCost);
        _parkingManager.OnCreateCarForMoney.AddListener(UprgareCost);
        _parkingManager.OnCreateNewCarFree.AddListener(UprgareCost);
        ParkingPoint.OnUpgradeCar.AddListener(UprgareCost);
        UprgareCost();
    }
    // Update is called once per frame

    public void CheckAvialable(int maxRecordMerge)
    {
        if (maxRecordMerge>= _mergeNum)
        {
            OpenIcon();
        }
        else
        {
            CloseIcon();
        }
    }

    private void OpenIcon()
    {
        _iconCar.color = Color.white;
    }

    private void CloseIcon()
    {
        _iconCar.color = Color.black;
    }

    public void LABEL_CLICK()
    {
       
    }


    public void UprgareCost()
    {

        _labelButton.onClick.RemoveAllListeners();

        if (_mergeNum+2<=_playerStats.MaxMergeNum)
        {
            _iconMoney.SetActive(true);
            _iconGold.SetActive(false);

            _priceCarTxt.text = _mergeManager.GetPriceUpgradeMoney(_mergeNum).ToString("F0");
            
            _labelButton.onClick.AddListener(PayCarForMoney);
        }
        else
        {
            _iconMoney.SetActive(false);
            _iconGold.SetActive(true);

            _priceCarTxt.text = _mergeManager.GetPriceUpgradeGold(_mergeNum).ToString("F0");
          
            _labelButton.onClick.AddListener(PayCarForGold);
        }       
    }

    private void PayCarForMoney()
    {
        float cost = _mergeManager.GetPriceUpgradeMoney(_mergeNum);
        if (_playerStats.Money >= cost)
        {
            if (_parkingManager.GetFreePosParking() != -1)
            {
                _parkingManager.CreateNewCarForMoney(_mergeNum);
                _mergeManager.UpPriceCar(_mergeNum);
                UprgareCost();
                _playerStats.SubstractMoney(cost);
            }
        }
    }
    private void PayCarForGold()
    {
        int cost = _mergeManager.GetPriceUpgradeGold(_mergeNum);
        if (_playerStats.Gold >= cost)
        {
            if (_parkingManager.GetFreePosParking() != -1)
            {
                _parkingManager.CreateNewCarForMoney(_mergeNum);
               // _mergeManager.UpPriceCar(_mergeNum);
                UprgareCost();
                _playerStats.SpendGold(cost);
            }
        }
    }
}
