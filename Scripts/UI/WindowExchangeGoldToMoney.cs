using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowExchangeGoldToMoney : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;
    
    [SerializeField] private TextMeshProUGUI _tmpCountMoneyGorGold;
   
    public UnityEvent OnFailAction;
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Init()
    {
        _tmpCountMoneyGorGold.text = "x " + _playerStats.MoneyForGold.ToString();
    }

    public void BUTTON_PayMoney()
    {
        CheckActivityButton();
    }

    private void CheckActivityButton()
    {
        if (_playerStats.Gold>= 1)
        {
            ExchangeGoldToMoney();
        }
        else
        {
            OnFailAction.Invoke();
            
        }
    }

    private void ExchangeGoldToMoney(int count = 1)
    {
        _playerStats.SpendGold(count);
        _playerStats.AddMoney(count * _playerStats.MoneyForGold);
    }
}
