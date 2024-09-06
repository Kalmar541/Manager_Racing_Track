using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("Скрипт управляет последовательным стартом компонентов игры")]
    [Header("Установить в инспекторе")]
    [SerializeField] private PlayerStats _playerStats;  //данные прогресса игры
    [SerializeField] private MenuQuests _menuQuests;    //данные о состоянии заданий
    [SerializeField] private ParkingManager _parkingManager; //данные о наличии машин
    [SerializeField] private QuestSystem _questSystem; // хранит рекорд мерджа
    [SerializeField] private MergeManager _mergeManager; // хранит цену апгрейда машин
    [SerializeField] private Reclam _reclam; // хранит цену апгрейда машин

    [Header("Значения")]
    [SerializeField] private float _timerReclam = 300f;
    private void Awake()
    {
        _playerStats.LoadAllData(); // загрузить сохранения с облака
        _menuQuests.LoadSavesQuests(_playerStats); //установить состояние квестов
        _parkingManager.LoadCarOnParkinng(_playerStats.MergeCarAtParking);
        _questSystem.LoadRecordMerge(_playerStats.MaxMergeNum);
        _mergeManager.LoadPriceUpgrade(_playerStats._priceUpgare);

        //реклама
        _reclam.IsWorking = true;
        _reclam.SetTimer(_timerReclam);
    }
    void Start()
    {
        
    }

    
}
