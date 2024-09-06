using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class MergeManager : MonoBehaviour
{
    [Inject] PlayerStats _playerStats;
    public PlayerStats PlayerStat { get { return _playerStats; }private set { } }
    [SerializeField] private float _percentUppingCost = 15f;
    [SerializeField] private float _myltyplie;
    [SerializeField] private List<MergeCar> _hierarchyMergingCar;
    [SerializeField]
    private Dictionary<int, float> _priceUpgareMoney = new Dictionary<int, float>
    {
        { 0,50f},
        { 1,100f},
        { 2,150f},
        { 3,250f},
        { 4,390f},
        { 5,550f},
        { 6,700f},
        { 7,950f},
        { 8,1400f},
        { 9,2100f},
        { 10,3200f},
        { 11,4700f},
        { 12,7000f},
        { 13,9000f},
        { 14,12500f},
        { 15,18000f},

    };

    public UnityEvent OnUpPrice;
    private void Start()
    {
        _myltyplie = _percentUppingCost * 0.01f + 1f;
    }
    public int GetLenghtListCars()
    {
        return _hierarchyMergingCar.Count;
    }

    public MergeCar GetPrefabCar(int posInList)
    {
        if (posInList< _hierarchyMergingCar.Count&& posInList>-1) // проверка диапозона списка машин
        {
            return _hierarchyMergingCar[posInList];
        }
        else
        {
            return _hierarchyMergingCar[_hierarchyMergingCar.Count-1]; // 
        }
    }
    

    public float GetPriceUpgradeMoney(int numMerge)
    {
        if (_priceUpgareMoney.ContainsKey(numMerge))
        {
            return _priceUpgareMoney[numMerge];
        } 
        return 9999f;
    }


    public int GetPriceUpgradeGold(int numMerge)
    {
        if (_priceUpgareMoney.ContainsKey(numMerge))
        {
            return numMerge*2;
        }
        return 9999;
    }

    public void UpPriceCar( int numMerge)
    {
        if (_priceUpgareMoney.ContainsKey(numMerge))
        {

            _priceUpgareMoney[numMerge] *= _myltyplie;

            _playerStats.SavePriceUpgrade(_priceUpgareMoney);
            OnUpPrice.Invoke();
        }
    }
    public void LoadPriceUpgrade(Dictionary<int,float> dictPrices)
    {
        if (dictPrices.Count>0)
        {
            _priceUpgareMoney = dictPrices;
        }
        
      /*  foreach (var item in _priceUpgare)
        {
            if (dictPrices.TryGetValue(item.Key, out float val))
            {

            }
        }*/
       
    }
}
