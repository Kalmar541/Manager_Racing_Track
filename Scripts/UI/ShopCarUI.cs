using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ShopCarUI : MonoBehaviour
{
    [Inject] private MergeManager _mergeManager;
    [Inject] private PlayerStats _playerStats;
    [Inject] private DiContainer _diContainer;

    [Header("Установить в инспекторе")]
    [SerializeField] private Transform _parentInstaceLabel;
    [SerializeField] private GameObject _prefabLabalCar;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialization()
    {
        int length = _mergeManager.GetLenghtListCars();
        for (int i = 1; i < length; i++)
        {
            GameObject newLabel = _diContainer.InstantiatePrefab(_prefabLabalCar, _parentInstaceLabel);
            //GameObject newLabel = Instantiate(_prefabLabalCar, _parentInstaceLabel);
            CarLabel carlabel = newLabel.GetComponent<CarLabel>();
            carlabel.Init(i, _playerStats.MaxMergeNum, _mergeManager);
        }
    }
}
