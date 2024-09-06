using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
public class Gate : MonoBehaviour
{
    [Inject] private SoundManager _soundManager;

    //—творки должн быть закрыты до запуска сцены
    [SerializeField] private GameObject _stvorkaL;
    private Vector3 _startRotate1;
    private Vector3 _finishRotate1 = new (0f, 88f, 0f);
    

    [SerializeField] private GameObject _stvorkaR;
    private Vector3 _startRotate2;
    private Vector3 _finishRotate2 = new(0f, 91f, 0f);

    public float speedOpening = 3f;
    [SerializeField] private GameObject _lock;
    [SerializeField] private AudioClip _soundShakeGate;

    [SerializeField] private GameObject _starsGO;
    
    // Start is called before the first frame update
    private void Awake()
    {
        //как то загрузить их состо€ние
        /*if (true) // если створки уже открывались
        {
            FastOpenGate();
        }*/

        _startRotate1 = _stvorkaL.transform.rotation.eulerAngles;
        _startRotate2 = _stvorkaR.transform.rotation.eulerAngles;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenGate()
    {
        _stvorkaL.transform.DOLocalRotate(_finishRotate1, speedOpening).SetEase(Ease.OutBounce);
        _stvorkaR.transform.DOLocalRotate(_finishRotate2, speedOpening).SetEase(Ease.OutBounce);
        // анимаци€ пропадани€ замка
        //_lock.SetActive(false);
    }
    //мгновенно сделает створки открытыми
    public void FastOpenGate()
    {
        _stvorkaL.transform.localRotation = Quaternion.Euler(_finishRotate1);
        _stvorkaR.transform.localRotation = Quaternion.Euler(_finishRotate2);
    }

    private float durationShake = 0.15f;
    private Vector3 ofsetAngle = new (0f,3f,0f); 
    public void ShakeGate()
    {
        
        _stvorkaL.transform.DOLocalRotate(_startRotate1- ofsetAngle, durationShake).SetEase(Ease.OutBounce);
        _stvorkaR.transform.DOLocalRotate(_startRotate2 + ofsetAngle, durationShake).SetEase(Ease.OutBounce)
            .OnComplete(()=>
            {
                _stvorkaL.transform.DOLocalRotate(_startRotate1, durationShake);
                _stvorkaR.transform.DOLocalRotate(_startRotate2, durationShake);
            }
                );
        _soundManager.PlaySFX(_soundShakeGate);
    }
    public void DeleteStart()
    {
        _starsGO.SetActive(false);
    }
}
