using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.Events;
using ArcadeVP;

public class MergeCar : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler, IPointerClickHandler, IParkable, IPointerDownHandler, IPointerUpHandler, IDropHandler
{
    [Inject] private GameOverlayUI _gameOverlay_scr;
    [Inject] private SoundManager _soundManager;
    [Inject] private PrestigeCounter _prestigeCounter;
    [Inject] private StartZone _startZone;

    [Header("”становить в инспекторе:")]
    [SerializeField] private BoxCollider _collider; //коллайдер дл€ проверки коллизий
    [SerializeField] private GameObject _effectBlocking;
    [SerializeField] private DragAndDrop _dragAndDrop_scr;
    [SerializeField] private GameObject _racingPrefab; // префаб дл€ создани€ гоночного авто
    [SerializeField] private Sprite _shopIcon;
    [SerializeField] private TextMeshProUGUI _numberTxt;
    [Space]
    [SerializeField] private AudioClip _engineStartSFX;
    [SerializeField] private AudioSource _audioSource;
    
    //[SerializeField] private BoxCollider _colliderModel;
    [SerializeField] private LayerMask _layerStartZone;
    //private ParkingPoint _parkingPoint;
    [SerializeField] public ParkingPoint CurrentParkingPoint;// { get; set;}
    private int _mergeNumber;
    public int MergeNumber {
        get { return _mergeNumber; }
        set { _mergeNumber = value;
            if (_numberTxt != null) _numberTxt.text = _mergeNumber.ToString();       
        } }
    [SerializeField] private ParkingPoint _collisionParkingPoint;
    public bool isBlocking { get; private set; } = false;
    private RaceCar _racingGO; // GO который должен ездить по трассе

    public UnityEvent<MergeCar> OnReturnFromStartZone; //по возвращению тачки с круга на парковку
    

    // private bool isTochPalyer; // перетаскивает ли игрок предмет

    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        //PlayStartEngineSfx();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Initialize()
    {

    }
    public void CheckParking()
    {
        if (_collisionParkingPoint == null) // обект находитс€ ни над чем
        {
            // CurrentParkingPoint.ReturtCarOnParking();
            AnimPut();


        }
        else // обьект находитс€ на одном из парковочных мест
        {
            _collisionParkingPoint.PutCar(this);
            _collisionParkingPoint = null;
        }
    }
    private void MoveCarOnStarZone()
    {
        _racingGO.gameObject.SetActive(true);
        BlockingMergeON();
    }
    public void BlockingMergeON()
    {
        isBlocking = true;
        _effectBlocking.SetActive(true);
        _dragAndDrop_scr.SetLockMove(true);
    }
    public void UnBlockingMerge( )
    {
        isBlocking = false;
        _effectBlocking.SetActive(false);
        _dragAndDrop_scr.SetLockMove(false);
        _racingGO.gameObject.SetActive(false);
        _racingGO.ResetWay();
        OnReturnFromStartZone.Invoke(this);
        OnReturnFromStartZone.RemoveAllListeners();


    }
    private void CreateRacingCar()
    {
        if (_racingGO== null)
        {
            if (_racingPrefab == null) return;
            _racingGO = Instantiate(_racingPrefab, transform.position, transform.rotation).GetComponent<RaceCar>();
            _racingGO.MergeNumber = MergeNumber;
            _racingGO.gameObject.SetActive(false);
        }
    }

    public RaceCar GetRaceCar()
    {
        if (_racingGO== null)
        {
            CreateRacingCar();
        }
        return _racingGO;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ParkingPoint parkingPoint))
        {
            if (CurrentParkingPoint!= parkingPoint)
            {
                _collisionParkingPoint = parkingPoint;
            }           
        }
        if (other.TryGetComponent(out StartZone startZone))
        {
            if (CurrentParkingPoint != parkingPoint)
            {
                _collisionParkingPoint = parkingPoint;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ParkingPoint parkingPoint))
        {
            _collisionParkingPoint = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left&&
            eventData.button != PointerEventData.InputButton.Right)
        {
            AnimTake();
        }
       // Debug.Log("Ќачало перетаскивани€");

       
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.DrawRay(transform.position, -transform.up* 100f, Color.yellow, 100f);
        if (Physics.Raycast(transform.position,-transform.up* 100f, out RaycastHit  hitInfo, 100f, _layerStartZone, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out StartZone startZone))
            {
                if (_racingPrefab == null) // дл€ ржавой тачки, она не должна поехать
                {
                    ReturnCarToParking();
                    return;
                }
                if (_racingGO == null) // еще не создана гоночна€ модель из префаба
                {
                    CreateRacingCar();
                }
                MoveCarOnStarZone();
                startZone.StartingCarOnRoad(this);
            }
        }

        CheckParking();
       

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("»дет  перетаскивани€");
        

    }
    public void OnDrop(PointerEventData eventData)
    {
       // throw new NotImplementedException();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       /* if (eventData.button == PointerEventData.InputButton.Left &&
            eventData.dragging == false)
        {

            _gameOverlay_scr.ShowMenuMergeCar(this);
            ReturnCarToParking();
            // Debug.Log(" лик по тачке ");
        }*/

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
        if (eventData.button == PointerEventData.InputButton.Right &&
            eventData.position == _posMouseClickDown)
        {

            _gameOverlay_scr.ShowMenuMergeCar(this);
            ReturnCarToParking();
        }
    }

    public void ReturnCarToParking()
    {
        //transform.position = CurrentParkingPoint.transform.position;
        if (_racingGO!= null)
        {
            _racingGO.state = RaceCar.Location.atParking;
        }
        AnimPut();
    }

    public AudioClip GetClipStartEngine()
    {
        return _engineStartSFX;
    }

    private float _offsetYAtLifting = 1.5f;
    private float _durationLifting = 0.35f;

    public Sprite GetImageCar()
    {
        return _shopIcon;
    }
    public float GetMaxSpeed()
    {
        return _racingPrefab.GetComponent<ArcadeAiVehicleController>().MaxSpeed;
    }
    public float GetAcelerate()
    {
        return _racingPrefab.GetComponent<ArcadeAiVehicleController>().accelaration;
    }
    //¬з€ли машину
    public void AnimTake()
    {
        if (!isBlocking)
        {
            transform.DOLocalMoveY(_offsetYAtLifting, _durationLifting).SetEase(Ease.OutExpo);
        }
    }
    //положили машину
    private float _durationFalling = 0.3f;
    public void AnimPut()
    {
        transform.DOMoveX(CurrentParkingPoint.transform.position.x, _durationFalling).SetEase(Ease.OutQuad);
        transform.DOMoveZ(CurrentParkingPoint.transform.position.z, _durationFalling).SetEase(Ease.OutQuad).OnComplete(
            () => transform.DOMoveY(0f, _durationFalling).SetEase(Ease.OutBack)
            ) ;
       
    }
    public void DeletCar()
    {
        if(_racingGO!= null)
        {
            Destroy(_racingGO.gameObject);
        }
        Destroy(this.gameObject);
        _startZone.DeletCarOnStartZone(this);
    }

    public void PlayStartEngineSfx()
    {
        _audioSource.clip = _engineStartSFX;
        _audioSource.Play();
    }

    public void UpgradeToNextCar()
    {
        
            CurrentParkingPoint.UpgradeCarForMoney();
        
    }

 
}
