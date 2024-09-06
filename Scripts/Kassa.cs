using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Kassa : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;

    [Header("Установить в инспекторе")]
    [SerializeField] private PaletteMoney[] palets;
    [SerializeField] private float _maxCapacityMoney;
    [Space]
    [SerializeField] private float _currentMoney;

   

    public UnityEvent OnGetMoney;

    
    // Start is called before the first frame update
    void Start()
    {
        Initilizate();
    }


    private void Initilizate()
    {
        foreach (var item in palets)
        {
            item.Initializate(this, _maxCapacityMoney/ palets.Length,0f);
        }
    }

    
    public void AddMoney( float money)
    {
        if (money>0f)
        {
            foreach (var palet in palets)
            {
                if (!palet.IsFull)
                {
                   float ostatok = palet.AddMoney(money);
                    _currentMoney += money;
                    if (ostatok==0)
                    {
                        return;
                    }                  
                }
            }
        }
    }


    //собрать деньги с кассы в банк
    public void GiveMoney(float money)
    {
        _playerStats.AddMoney(money);
        _currentMoney -= money;
        OnGetMoney.Invoke();
    }
}
