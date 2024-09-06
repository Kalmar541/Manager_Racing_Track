using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Визуально тображает пачки денег на палетте в зависимости от их колличества
public class PaletteMoney : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] moneysPack;
    public bool IsFull { get; private set; }
    private float _currentMoney;
    private float _maxMoney;

    private Kassa _kassa_scr;
    [SerializeField] private GameObject _particleVfx;

    void Start()
    {
        
    }


    public void Initializate(Kassa kassa_scr, float maxMoney , float currentMoney)
    {
        _kassa_scr = kassa_scr;
        SetMaxMoney(maxMoney);
        AddMoney(currentMoney);
    }

    private void SetMaxMoney(float value)
    {
        _maxMoney = value;
        
    }

    public float AddMoney(float money)
    {
        _currentMoney += money;
        if (_currentMoney>_maxMoney)
        {
            _currentMoney = _maxMoney;
            AnimPackMoneys();
            IsFull = true;
            return _currentMoney - _maxMoney; //остаток
        }
        AnimPackMoneys();
        IsFull = false;
        return 0f;
    }

    private void AnimPackMoneys()
    {
        float moneyInOnePack = _maxMoney / moneysPack.Length;
        int numActivePacks = (int)(_currentMoney / moneyInOnePack);

        if (numActivePacks==0&& _currentMoney>0)
        {
            moneysPack[0].SetActive(true);
            return;
        }

        for (int i = 0; i < moneysPack.Length; i++)
        {
            if (i<numActivePacks)
            {
                moneysPack[i].SetActive(true);
            }
            else
            {
                moneysPack[i].SetActive(false);
            }
        }
    }

    //отправить деньги с палетты 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentMoney > 0)
        {
            _kassa_scr.GiveMoney(_currentMoney);
            ResetPack();
        }
    }

    private void ResetPack()
    {
        _particleVfx.SetActive(true);

        foreach (GameObject item in moneysPack)
        {
            item.SetActive(false);
        }

        _currentMoney = 0f;
        IsFull = false;
    }
}
