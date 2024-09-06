using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Zenject;

public class ParkingManager : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;
   
    [Header("Установить в инспекторе")]
    [SerializeField] private ParkingPoint[] _parkingPoints;
    [SerializeField] private int _startingParkingCell=4;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Recycler _recycler; //этот скрипт сделает анимацию создания машины
    [SerializeField] private ContainerCar _prefabContainerCar;
    [SerializeField] private AudioClip _containerFallSfx;
    public int StartingParkingCell
    {
        get { return _startingParkingCell; }
        set
        {
            if (value > 0)
            {
                _startingParkingCell= Mathf.Clamp(value, 0, _parkingPoints.Length);
            }
            else
            {
                Debug.LogWarning("Число стартовых парковок не может быть <= 0 шт");
            }
        }
    } 


    
    private int _countActivatedParkingCell;
    //[SerializeField] private MergeManager _mergeManager;
    //public MergeCar testPrefab;

    private bool isActivated;

    public UnityEvent OnCreateCarForMoney;
    public UnityEvent OnCreateNewCarFree;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   

    public void Activation(int countActiveParking)
    {
        if (!isActivated)
        {
            isActivated = true;
            for (int i = 0; i < _parkingPoints.Length; i++)
            {
                if (i< _startingParkingCell)
                {
                    //_parkingPoints[i].gameObject.SetActive(true);
                    _parkingPoints[i].Activate();
                }
                else
                {
                    //_parkingPoints[i].gameObject.SetActive(false);
                    //_parkingPoints[i].IsWorking = false;
                }
            }
            _countActivatedParkingCell = countActiveParking;
        }
    }
    public void DeActivation()
    {
        if (isActivated)
        {
            isActivated = false;
        }
        foreach (var item in _parkingPoints)
        {
            item.gameObject.SetActive(false);
        }
    }

    /*public int GetCountActivatedParkingCell()
    {

        return _countActivatedParkingCell;
    }*/
    public void AddNextParkingCell()
    {
        if (_countActivatedParkingCell< _parkingPoints.Length)
        {
            _countActivatedParkingCell++;
            _parkingPoints[_countActivatedParkingCell-1].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Все парковочные места открыты!");
        }
    }

    public void CreateNewCar(int numberMerge )
    {
        int freePos = GetFreePosParking();
        if (freePos != -1)
        {
            MergeCar newCar = _parkingPoints[freePos].CreateNewCar(numberMerge);
            _recycler.AnimCreatingNewCar(newCar, _parkingPoints[freePos].transform);
            OnCreateNewCarFree.Invoke();
            SaveCarsOnParking();
        }
        
    }
    public void CreateNewCarForMoney(int numberMerge)
    {
        int freePos = GetFreePosParking();
        if (freePos != -1)
        {
            MergeCar newCar = _parkingPoints[freePos].CreateNewCar(numberMerge);
            newCar.gameObject.SetActive(false);
            AnimCreatePayedCar(newCar, _parkingPoints[freePos].transform);
            OnCreateCarForMoney.Invoke();
            SaveCarsOnParking();
        }

    }

    private void AnimCreatePayedCar(MergeCar car, Transform posTo)
    {

        Vector3 posCreate = posTo.position + (Vector3.up * 7f);
        ContainerCar container = Instantiate(_prefabContainerCar, posCreate, car.transform.rotation);
        container.SetCarIn(car);
        
        container.transform.DOMove(posTo.position, 1.5f).SetEase(Ease.OutBounce);
        //_audioSource.clip = _containerFallSfx;
    

        _audioSource.PlayOneShot(_containerFallSfx);

    }
    public int GetFreePosParking()
    {
        // for (int i = 0; i < _countActivatedParkingCell; i++)
        for (int i = 0; i < _parkingPoints.Length; i++)
        {
            if (_parkingPoints[i].ContainerCar==null&&
                _parkingPoints[i].IsWorking)
            {
                return i;
            }
            
        }
        return -1; // нет мест
    }
    private void PlayUpgradeSFX()
    {
        if (_audioSource!= null)
        {
            _audioSource.Play();
        }
    }
    public ParkingPoint[] GetAllParkingPoints()
    {
        return _parkingPoints;
    }

    public void SaveCarsOnParking()
    {
        _playerStats.SaveCarOnParking(GetCarOnParking());
    }
    public void LoadCarOnParkinng(int[] NumberMergeCars)
    {
        if (NumberMergeCars == null)
        {
            return;
        }
        for (int i = 0; i < NumberMergeCars.Length; i++)
        {
            if (NumberMergeCars[i]!=-1)
            {
                //создание машин без анимаций
                int freePos = GetFreePosParking();
                if (freePos != -1)
                {
                    MergeCar newCar = _parkingPoints[freePos].CreateNewCar(NumberMergeCars[i]);
                    //OnCreateNewCar.Invoke();

                }
            }
            
        }
        
    }
    private MergeCar[] GetCarOnParking()
    {
       
        MergeCar[] mergeCars =new MergeCar [_parkingPoints.Length];
        for (int i = 0; i < mergeCars.Length; i++)
        {
            mergeCars[i] = _parkingPoints[i].ContainerCar;
        }
        return mergeCars;
    }

    private void OnDestroy()
    {
         
    }
}
