using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ParkingPoint : MonoBehaviour
{
    [Inject] private MergeManager mergeManager_scr;
    [Inject] private QuestSystem _questSystem_scr;
    [Inject] private GameOverlayUI _gameUi;
    [Inject] private SoundManager _soundManager;
    [Inject] private ParkingManager _parkingManager;
    [Inject] private PlayerStats _playerStats;
    [SerializeField] private MergeCar _containerCar;

    [Inject]  private DiContainer diContainer;

    public bool IsWorking { get; private set; }
    public MergeCar ContainerCar { 
        get { return _containerCar; } 
        set { _containerCar = value;
           // MergeNumber = _containerCar.MergeNumber; 
        } }
    [SerializeField] private GameObject _upgradeVFX;

    public static UnityEvent OnUpgradeCar = new();

    //������������ ����� ������� � ��������
    public void Activate()
    {
        IsWorking = true;
    }
    //������� �� �������� ����� ������
    public MergeCar CreateNewCar(int numMerge)
    {
        if (_containerCar==null)
        {
            GameObject goCar = diContainer.InstantiatePrefab(mergeManager_scr.GetPrefabCar(numMerge), transform.position, transform.rotation, null);
            MergeCar newCar = goCar.GetComponent<MergeCar>();
           // MergeCar newCar = Instantiate(mergeManager_scr.GetPrefabCar(numMerge), transform.position, transform.rotation);
            Mathf.Clamp(numMerge, 0, mergeManager_scr.GetLenghtListCars() - 1);
            newCar.MergeNumber = numMerge;
            ContainerCar = newCar;
            newCar.CurrentParkingPoint = this;
            //_parkingManager.SaveCarsOnParking();
            return newCar;
        }
        return null;
    }


    //������ ������ �� ���������� ������
    public void  UpgradeMergeObject()
    {
        GameObject gonewCar = diContainer.InstantiatePrefab(mergeManager_scr.GetPrefabCar(ContainerCar.MergeNumber + 1), transform.position, transform.rotation, null);
        MergeCar newCar = gonewCar.GetComponent<MergeCar>();
        newCar.MergeNumber = _containerCar.MergeNumber + 1;
        ContainerCar.DeletCar();
        //Destroy(ContainerCar.gameObject);
        ContainerCar = newCar;
        newCar.CurrentParkingPoint = this;
        _questSystem_scr.CheckRecordMerge(newCar.MergeNumber, transform.position);
        _upgradeVFX.SetActive(true);
        OnUpgradeCar.Invoke();
        newCar.PlayStartEngineSfx();
        //_soundManager.PlaySFX(newCar.GetClipStartEngine()); ��������� ���� ��� ������� � ������ �����


    }

    public void UpgradeCarForMoney()
    {
            UpgradeMergeObject();          
            _parkingManager.SaveCarsOnParking();      
    }
    //������������� ������ �� ������� ��������
    public void OverParking(MergeCar car)
    {
        if (IsWorking)
        {
            car.CurrentParkingPoint.ContainerCar = null;
            ContainerCar = car;
            car.CurrentParkingPoint = this;
        }
        car.ReturnCarToParking();
    }
    private void DeleteCar()
    {
        if (_containerCar!= null)
        {
            _containerCar.DeletCar();
           // Destroy(_containerCar.GetRaceCar().gameObject);
           // Destroy(_containerCar.gameObject);
            _containerCar = null;
        }
    }

    //����� ������ ���� ������ ����� ����� �������� � �������� �� ��������
    public void ResetParking()
    {
        DeleteCar();
       
    }

    //������� ������ �� ��������
    public void ReturtCarOnParking()
    {
        if (ContainerCar!=null)
        {
            ContainerCar.transform.position = transform.position;
        }
    }

    //������� ������ �� �������� � ������ �����������
    public void PutCar(MergeCar car)
    {
        //��������
        if (_containerCar==null)
        {
            OverParking(car);           
        }
        else // ������
        {
            //����� ������� �����(���������� ������)
            if (car.MergeNumber == ContainerCar.MergeNumber&& car.MergeNumber<mergeManager_scr.GetLenghtListCars()-1)
            {
                UpgradeMergeObject();
                car.CurrentParkingPoint.ResetParking();
                _parkingManager.SaveCarsOnParking();
            }
            else // ������ ������
            {
                car.ReturnCarToParking();
            }
        }
       
    }
    private void OnDestroy()
    {
        OnUpgradeCar.RemoveAllListeners();
    }
}
