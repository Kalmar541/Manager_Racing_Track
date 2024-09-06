using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("������ ��������� ���������������� ������� ����������� ����")]
    [Header("���������� � ����������")]
    [SerializeField] private PlayerStats _playerStats;  //������ ��������� ����
    [SerializeField] private MenuQuests _menuQuests;    //������ � ��������� �������
    [SerializeField] private ParkingManager _parkingManager; //������ � ������� �����
    [SerializeField] private QuestSystem _questSystem; // ������ ������ ������
    [SerializeField] private MergeManager _mergeManager; // ������ ���� �������� �����
    [SerializeField] private Reclam _reclam; // ������ ���� �������� �����

    [Header("��������")]
    [SerializeField] private float _timerReclam = 300f;
    private void Awake()
    {
        _playerStats.LoadAllData(); // ��������� ���������� � ������
        _menuQuests.LoadSavesQuests(_playerStats); //���������� ��������� �������
        _parkingManager.LoadCarOnParkinng(_playerStats.MergeCarAtParking);
        _questSystem.LoadRecordMerge(_playerStats.MaxMergeNum);
        _mergeManager.LoadPriceUpgrade(_playerStats._priceUpgare);

        //�������
        _reclam.IsWorking = true;
        _reclam.SetTimer(_timerReclam);
    }
    void Start()
    {
        
    }

    
}
