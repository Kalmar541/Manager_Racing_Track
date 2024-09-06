using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuest : MonoBehaviour
{
    [Header("Для задания с созданием первых машин")]
    [SerializeField] private Quest _createCars1;
    [SerializeField] private Quest _createCars2;
    [SerializeField] private Assignment _createTwoCars;
    [SerializeField] private EnergyIndicator _recycler;

    [Header("Для задания первого мерджа")]
    [SerializeField] private Quest _mergeQuest;

    [Header("Для задания Старта первой тачки")]
    [SerializeField] private StartZone _startZone;
    [SerializeField] private Quest _startCarQuest;

    [Header("Для задания Кассы")]
    [SerializeField] private Kassa _kassa;
    [SerializeField] private Quest _kassaQuest;

    [SerializeField] private ControllerMoveCamera _cameraPlayer;
    [SerializeField] private Quest _lock;
    void Start()
    {
        _cameraPlayer.BlockingPlayer(false);
        _lock.BlockingON();


    }

    public void ActvateQuestCreatingCars()
    {
        _recycler.OnOverflow.AddListener(CompleteQuest);

         void CompleteQuest()
        {
            if (!_createCars1.IsRepairComplete)
            {
                _createCars1.Repair();
                return;
            }
            if (_createCars1.IsRepairComplete && !_createCars2.IsRepairComplete)
            {
                _createCars2.Repair();
                _recycler.OnOverflow.RemoveListener(CompleteQuest);
            }

        }
    }
    
    public void ActivateQuestFirstMerge()
    {
        ParkingPoint.OnUpgradeCar.AddListener(CheckMerge);
        void CheckMerge()
        {
            if (!_mergeQuest.IsRepairComplete)
            {
                _mergeQuest.Repair();
            }
        }
    }

    public void ActivateQuestFirstStartCar()
    {
        if (!_startCarQuest.IsRepairComplete)
        {
            _startZone.OnStartCar = _startCarQuest.Repair;
        }
       
        
    }

    public void ActivateQuestKassa()
    {
        _kassa.OnGetMoney.AddListener(_kassaQuest.Repair);
    }

}
