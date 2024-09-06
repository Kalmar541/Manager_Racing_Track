using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using YG;
using UnityEngine.UI;

public class ControllerMoveCamera : MonoBehaviour
{
    [Header("Установить в инспекторе")]
    [SerializeField] private Transform _cameraPlayer;
   // [SerializeField] private AudioListener _audioListener;
    [Header("Все для джойстика")]
    [SerializeField] private bl_Joystick JoystickViev;
    [SerializeField] private bl_Joystick JoystickWalk;
    [SerializeField] private GameObject _GO_Joysticks;
    [SerializeField] private Toggle _toggleJoystick;
    [SerializeField] private float mainSpeed = 100.0f; //regular speed
    [SerializeField] private float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    [SerializeField] private float maxShift = 1000.0f; //Maximum speed when holdin gshift
    [SerializeField] private float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;


    [SerializeField] private float _minPosY=0f;
    [SerializeField] private float _maxPosY=20f;
    private bool _isCanMove;
    public bool IsCanMove { get { return _isCanMove;} set { _isCanMove = value; } }
    private bool _isCanRotate;
    public bool IsCanRotate { get { return _isCanRotate; } set { _isCanRotate = value; } }

    private bool _isJoystickInput;
    public bool isJoystickInput
    {
        get { return _isJoystickInput; }
        set
        {

            _GO_Joysticks.SetActive(value);
            _isJoystickInput = value;
            
        }
    }
    private void Start()
    {
        if (!YandexGame.EnvironmentData.isDesktop)
        {
            isJoystickInput = true;
            _toggleJoystick.isOn = true;
        }
        else
        {
            isJoystickInput = false;
            _toggleJoystick.isOn = false;
        }
       
    }

    private Vector3 _posToch;
    private Vector3 _lastToch;
    

    private void Update()
    {
        if (IsCanRotate)
        {
            if (Input.GetMouseButton(1)) // управление для мыши
            {
                lastMouse = Input.mousePosition - lastMouse;
                lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
                lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
                transform.eulerAngles = lastMouse;
            }

            if (Input.touchCount > 0) //коснулся экрана пальцем 
            {
                _posToch.x = Input.touches[0].position.x;
                _posToch.y = Input.touches[0].position.y;

                if (_lastToch != _posToch && _lastToch != Vector3.zero) // тянет пальцем по экрану
                {
                    lastMouse = _posToch - _lastToch;
                    lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
                    lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
                    transform.eulerAngles = lastMouse;                 
                }
                _lastToch = _posToch;              
            }
            else // убрал палец
            {
                _lastToch = Vector3.zero;
            }
           
            lastMouse = Input.mousePosition;
        }

        //Задать поворот камеры
        _cameraPlayer.transform.rotation = transform.rotation;

    }

    [SerializeField] private Rigidbody _rb;
    void FixedUpdate()
    {
        /* if (Input.GetMouseButton(1))
         {
             lastMouse = Input.mousePosition - lastMouse;
             lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
             lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
             transform.eulerAngles = lastMouse;
         }*/


        //Mouse  camera angle done.  
        if (!IsCanMove) return;
        //Keyboard commands
       // float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        { // only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.fixedDeltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position;
           // if (Input.GetKey(KeyCode.Space))
           // { //If player wants to move on X and Z axis only
           //     transform.Translate(p);
           //     newPosition.x = transform.position.x;
           //     newPosition.z = transform.position.z;
           //     transform.position = newPosition;
           // }
           // else
           // {
                // transform.Translate(p); //движение старое

                _rb.AddRelativeForce(p, ForceMode.VelocityChange);
           // } 
        }
    }
    private Vector3 _smothPos;
    public float _timeLerp=0.125f;
    private void LateUpdate()
    {
        _smothPos = Vector3.Lerp(transform.position, _cameraPlayer.position, _timeLerp);
        _cameraPlayer.position = _smothPos;
    }
    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        // вверх  вниз
        if (Input.GetKey(KeyCode.E)|| Input.GetKey(KeyCode.Space))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        return p_Velocity;
    }


    //Заблокировать и перемещение и вращение
    public void BlockingPlayer(bool block)
    { 
            IsCanMove = !block;
            IsCanRotate = !block;      
    }

    private float time = 3f;
    public void Move(Transform point)
    {
        BlockingPlayer(true);
        transform.DOMove(point.position, time).SetEase(Ease.InOutQuart).OnComplete(
            ()=> BlockingPlayer(false) );
        transform.DORotate(point.rotation.eulerAngles, time).SetEase(Ease.InOutQuart);
        
    }
    public void RotateCamera(Transform point)
    {
        BlockingPlayer(true);
        transform.DOLookAt(point.transform.position, time).SetEase(Ease.InOutQuart).
            OnComplete(() => BlockingPlayer(false)); ;

    }

    Vector3 posPlayer;
    private void CheckMinAndMaxPosY()
    {
        if (transform.position.y < _minPosY ||
            transform.position.y >= _maxPosY)
        {
            posPlayer = transform.position;
            posPlayer.y = Mathf.Clamp(posPlayer.y, _minPosY, _maxPosY);
            transform.position = posPlayer;
        }
    }


}