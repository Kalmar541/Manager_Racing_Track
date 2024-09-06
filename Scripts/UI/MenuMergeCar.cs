using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Zenject;

public class MenuMergeCar : MonoBehaviour, IPointerDownHandler
{
    [Inject] private PlayerStats _playerStats;
    [Inject] private MergeManager _mergeManager;

    [Header("Установить в инспекторе")]
    
    [SerializeField] private GameObject _pointRotate;
    [SerializeField] private GameObject _moneylabel;
    [SerializeField] private GameObject _goldlabel;

    [SerializeField] float _timeScaling;
    [SerializeField] private TextMeshProUGUI _priceTxt;

    private event Action OnClickReturnCar;
    private event Action OnClickUpgrade;

    private Vector3 _scaleClose = new Vector3(0f, 0f, 0f);
    private Vector3 _scaleOpen = Vector3.one;

    [SerializeField] private Button _btnReturn;
    [SerializeField] private Button _btnUpgrade;

    void Start()
    {
        _pointRotate.transform.localScale = _scaleClose;
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        
    }
    void  Update()
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       /* if (eventData.button == PointerEventData.InputButton.Left)
        {
            CloseMenu();
        }*/
    }

    public void BUTTON_ReturnCar()
    {
        OnClickReturnCar.Invoke();
        CloseMenu();
    }
    public void BUTTON_Upgrade()
    {
        OnClickUpgrade.Invoke();
        CloseMenu();
    }

    public void OpenMenu(MergeCar mergeCar)
    {
        _btnReturn.interactable = mergeCar.isBlocking;

        OnClickReturnCar = mergeCar.UnBlockingMerge;
        

        ShowPriceUpgrade(mergeCar);

        _pointRotate.transform.localScale = _scaleClose;
        _pointRotate.transform.DOScale(_scaleOpen, _timeScaling).SetEase(Ease.OutSine);
    }
    public void CloseMenu()
    {
        // OnClickRepair = null;
        _pointRotate.transform.DOScale(_scaleClose, _timeScaling).SetEase(Ease.OutSine);
    }

    public void ShowPriceUpgrade(MergeCar mergeCar)
    {
        if (mergeCar.MergeNumber+2<= _playerStats.MaxMergeNum)
        {
            _moneylabel.SetActive(true);
            _goldlabel.SetActive(false);

            float priceMoney = _mergeManager.GetPriceUpgradeMoney(mergeCar.MergeNumber);

            if (_playerStats.Money >= priceMoney)
            {
                _btnUpgrade.interactable = true;
                OnClickUpgrade = PayCarForMoney;
            }
            else
            {
                _btnUpgrade.interactable = false;
            }

            _priceTxt.text = priceMoney.ToString("F0");
        }
        else
        {
            _moneylabel.SetActive(false);
            _goldlabel.SetActive(true);

            float priceGold = _mergeManager.GetPriceUpgradeMoney(mergeCar.MergeNumber);

            if (_playerStats.Gold >= mergeCar.MergeNumber*2)
            {
                _btnUpgrade.interactable = true;
                OnClickUpgrade = PayCarForGold;
            }
            else
            {
                _btnUpgrade.interactable = false;
            }

            _priceTxt.text = (mergeCar.MergeNumber * 2).ToString("F0");
        }


        void PayCarForMoney()
        {
            float priceMoney = _mergeManager.GetPriceUpgradeMoney(mergeCar.MergeNumber);
            if (_playerStats.Money >= priceMoney)
            {              
                _playerStats.SubstractMoney(priceMoney);
                _mergeManager.UpPriceCar(mergeCar.MergeNumber);
                mergeCar.UpgradeToNextCar();
            }
        }
        void PayCarForGold()
        {
            int priceGold = _mergeManager.GetPriceUpgradeGold(mergeCar.MergeNumber);
            if (_playerStats.Gold >= priceGold)
            {
                _playerStats.SpendGold(priceGold);               
                mergeCar.UpgradeToNextCar();
            }
        }
    }


}
