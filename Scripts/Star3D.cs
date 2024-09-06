using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Zenject;
using UnityEngine.Events;

public class Star3D : MonoBehaviour, IPointerClickHandler
{
    [Inject] private PlayerStats playerStats_scr;

    [SerializeField] private Transform _modelStar; //моделька для анимации
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _boxColider;
    [SerializeField] private float _speedRotate=90f;
    [SerializeField] private float _speedRotateDeletig = 700f;
    //private bool isAnyCollision = true; //случается сейчас любая коллизия
    private bool isFalling;
    [Space]
    [SerializeField] private AudioSource _audioSource;

    public UnityEvent OnCollect ;

    private bool _isDeleting = false;
    private void Awake()
    {
        _boxColider.isTrigger = true;
    }
    void Start()
    {
        // случайное вращение
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 180f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _speedRotate * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
      
       // isAnyCollision = true;
    }

    ///Изначально Звезда имеет колайдер тригерный и не сталкивается ни с чем, но как только она начинает падать
    ///тригер отключается и она начинается сталкиваться с обьектами
    private void FixedUpdate()
    {
        if (_rigidbody.velocity.y<0f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
        if (isFalling)
        {
            _boxColider.isTrigger = false;
        }
        CheckFallingUnderRoad();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isDeleting)
        {
            _isDeleting = true;
            Collect();
            AnimDeleting();
        }   
    }

    private void CheckFallingUnderRoad() 
    {
        if (transform.position.y<-1f)
        {
            if (!_isDeleting)
            {
                _isDeleting = true;
                Collect();
                AnimDeleting();
            }
        }
    }


   


    private float _timeAnimDelet = 0.5f;
    public void AnimDeleting()
    {
        _speedRotate = _speedRotateDeletig;
        transform.DOMove((transform.position+new Vector3(0f,3f,0f)),0.5f);
        transform.DOScale(Vector3.zero, _timeAnimDelet).SetEase(Ease.InBack)
            .OnComplete(Delete);
    }

    private void AnimFall()
    {
        _modelStar.DOScaleY(0.85f, 0.25f).OnComplete(()=>
          _modelStar.DOScaleY(1f, 0.25f));
    }

    private void Collect()
    {
        playerStats_scr.AddStars(1);
        PlaySFX();
        OnCollect.Invoke();
    }
    private void PlaySFX()
    {
        if(_audioSource != null)
        {
            //_audioSource.time=0.2f;
            _audioSource.Play();
        }
    }
    private void Delete()
    {
        
        Destroy(gameObject, _audioSource.clip.length);
    }
}
