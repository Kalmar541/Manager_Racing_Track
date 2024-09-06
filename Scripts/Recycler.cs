using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using UnityEngine.EventSystems;

public class Recycler : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [Inject] private PlayerStats _playerStats;
   
    [Header("Точки для анимаций")]
    [SerializeField] Transform _pointCreateCar;
    [SerializeField] Transform _pointMoveNewCar;
    [SerializeField] Transform _pointMinGarbarge;
    [SerializeField] Transform _pointMaxGarbarge;

    [Header("Настройки анимаций")]
    [SerializeField] private float _durationMove = 1f;
    [SerializeField] private float _jumpPower = 50f;
    [SerializeField] private float _durationJump = 1f;
    [SerializeField] private float _durationRotate = 0.6f;

    [Header("Звук")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _moveCarSfx;
    [SerializeField] private AudioClip _jumpCarSfx;
    [SerializeField] private AudioClip _standCarSfx;

    [Header("Эффекты VFX")]
    [SerializeField] private ParticleSystem _vfxSmoke;

    [Header("Прочие компоненты")]
    [SerializeField] private EnergyIndicator _energyIndicator;
    [SerializeField] private WindowAnim _contextWindow; // окно котороые вызовется по щелчку
    [SerializeField] private GameObject _garbage;


    private ParticleSystem.MinMaxCurve _numCurrentPartical; // число частиц дыма
    private float _numMaxPartical;
    private int _priceResetWorkload; //стоимость очистки

    void Start()
    {
       

        var _emission = _vfxSmoke.emission;
        _numMaxPartical = _emission.rateOverTime.constant;
        _energyIndicator.OnSetWorkload.AddListener(SetEmissionSmoke);
        _energyIndicator.OnSetWorkload.AddListener(SetCapacityGarbage);
        _energyIndicator.OnOverflow.AddListener(SaveValueWorkload);
        _playerStats.OnSetGold.AddListener(SetAccessRepairButtonWindow);

        _energyIndicator.LoadData(_playerStats.RecyclerWorkload);

        //привязать модальное окно
        _contextWindow.OnButtonAccept.AddListener(ResetWorkload);

        //Init
        SetAccessRepairButtonWindow();

        _priceResetWorkload = _playerStats.GoldForResetRecycler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimCreatingNewCar(MergeCar car, Transform pointToMove)
    {
        Rigidbody carRb = car.GetComponent<Rigidbody>();
        carRb.isKinematic = true;


        car.transform.position = _pointCreateCar.position;
        _audioSource.PlayOneShot(_moveCarSfx);

        car.transform.DOMove(_pointMoveNewCar.position, _durationMove).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _audioSource.PlayOneShot(_jumpCarSfx);
                car.transform.DOJump(pointToMove.position, _jumpPower, 1, _durationJump)
                   ;

                car.transform.DORotate(new Vector3(0f, 360f, 0f), _durationRotate, RotateMode.FastBeyond360).OnComplete(() =>
                   _audioSource.PlayOneShot(_standCarSfx));
            }
            );
    }

    //coeff 0-1.0
    public void SetEmissionSmoke(float workload)
    {
        float coeff=0f;
        if (workload >= 0.5f)
        {
          coeff =  Mathf.InverseLerp(0.5f, 1f, workload);
        }
        var _emission = _vfxSmoke.emission;
        _numCurrentPartical = _emission.rateOverTime;
        _numCurrentPartical.constant = _numMaxPartical * coeff;
        _emission.rateOverTime = _numCurrentPartical;
    }

    public void ResetWorkload()
    {
        if (_playerStats.Gold>= _priceResetWorkload)
        {
            _playerStats.SpendGold(_priceResetWorkload);
            _energyIndicator.ResetWorkload();
            
        }
        
    }

    private Vector2 _posMouseClickDown;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _posMouseClickDown = eventData.position;       
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right&&
            eventData.position == _posMouseClickDown)
        {
            if (eventData.pointerPressRaycast.gameObject.CompareTag("ForMouseClick"))
            {
                OpenContextWindow();
            }
                
            
           
        }
    }

    //Доступность кнопки очистки для окна
    private void SetAccessRepairButtonWindow()
    {
        if (_playerStats.Gold>= _priceResetWorkload)
        {
            _contextWindow.SetActivityButton(true);
        }
        else
        {
            _contextWindow.SetActivityButton(false);
        }       
    }

    //заполненность 0 - 1.0
    public void SetCapacityGarbage(float coef)
    {
        Vector3 posGarbage = Vector3.Lerp(_pointMinGarbarge.position, _pointMaxGarbarge.position, coef);
        _garbage.transform.DOMove(posGarbage, 0.5f);
    }

    public void OpenContextWindow()
    {
        _contextWindow.Open();
    }
    
    private void SaveValueWorkload()
    {
        _playerStats.RecyclerWorkload = _energyIndicator.GetCurrentNumWorkload();
    }
}
