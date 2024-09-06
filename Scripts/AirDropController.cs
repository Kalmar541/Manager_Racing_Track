using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirDropController : MonoBehaviour
{
    [SerializeField] private VertoletController _prefabHelicopter;
    [SerializeField] private AirdropBox _prefabBox;
    [SerializeField] private float _durationMove = 30f;
    [SerializeField] private Transform _pointMoveAirDrop;
   

    [Header("Точки следования вертолета")]
    [SerializeField] private Transform _pointStart;
    [SerializeField] private Transform _pointUp;
    [SerializeField] private Transform _pointDrop;
    [SerializeField] private Transform _pointDown;
    [SerializeField] private Transform _pointHide;

    private VertoletController _instHelicopter;
    private AirdropBox _instAirDrop;

    private float _distanceMoving;
    private float _distToUp;
    private float _distToDrop;
    private float _distToDown;
    private float _distToHide;

    public bool isWorking;
    private bool _isFlying;
    private float _timerAirdrop= 240f;
    private float _timeLastFlying;
    private const float _defaultTimer = 240f;
    // Start is called before the first frame update
    void Start()
    {
        CalculateDistanceMoving();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWorking && !_isFlying)
        {
            WorkingAwaitAirdrop();
        }

    }
    private void CreateHelicopter()
    {
        _instHelicopter = Instantiate(_prefabHelicopter, _pointStart.position, _pointStart.rotation);
    }

    private void StartMovingHelicopter()
    {
        CreateHelicopter();

        _instHelicopter.transform.DOMove(_pointUp.position, (_distToUp / _distanceMoving) * _durationMove).SetEase(Ease.Linear)
            .OnComplete(()=> {_instHelicopter.transform.DOMove(_pointDrop.position, (_distToDrop / _distanceMoving) * _durationMove).SetEase(Ease.Linear)              
                .OnComplete(()=> {
                    DropBox();
                    
                    _instHelicopter.transform.DOMove(_pointDown.position, (_distToDown / _distanceMoving) * _durationMove).SetEase(Ease.Linear)
                    .OnComplete(()=> { _instHelicopter.transform.DOMove(_pointHide.position, (_distToHide / _distanceMoving) * _durationMove).SetEase(Ease.Linear)
                        .OnComplete(()=> DeletHelicopter())
                        ;})
                    ;})
                ;
            })
            ;
    }

    private void CalculateDistanceMoving()
    {
        _distToUp = Vector3.Distance(_pointStart.position, _pointUp.position);
        _distToDrop= Vector3.Distance(_pointUp.position, _pointDrop.position);
        _distToDown= Vector3.Distance(_pointDrop.position, _pointDown.position);
        _distToHide= Vector3.Distance(_pointDown.position, _pointHide.position);

        _distanceMoving = _distToUp + _distToDrop + _distToDown + _distToHide;



    }
    private void DeletHelicopter()
    {
        _isFlying = false;
        if (_instHelicopter!= null)
        {
            Destroy(_instHelicopter.gameObject);
            _instHelicopter = null;
        }
    }

    private void DropBox()
    {
        _instHelicopter.DropBox();
        CreateAirDrop();
    }

    private void CreateAirDrop()
    {
        Transform pointCreateBox = _instHelicopter.GetPointBox();
        _instAirDrop = Instantiate(_prefabBox, pointCreateBox.position, pointCreateBox.rotation);
        _instAirDrop.Init(_pointMoveAirDrop);
    }

    private void WorkingAwaitAirdrop()
    {
        if (_timeLastFlying + _timerAirdrop <= Time.time)
        {
            _timeLastFlying = Time.time;
            StartMovingHelicopter();
            if (_instAirDrop!= null)
            {
                _instAirDrop.DeletBox();
            }
            _isFlying = true;
        }
    }
}
