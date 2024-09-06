using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Zenject;

public class EnergyIndicator : MonoBehaviour
{
    [Inject] private SoundManager _soundManager;

    [Header("Установить в инспекторе")]
    [SerializeField] private GameObject _energyBar; //полоска бара
    [SerializeField] private float _defaultWorkloadValue;
    [SerializeField] private float _currentWorkload = 3; // какого значения нужно достичь
    [SerializeField] private int _maxLimitWorkload=35;
    [SerializeField] private Gradient _colorGradient; // цвет переполнения
    [SerializeField] private Renderer _barRenderer;
    [SerializeField] private float _speedMoveBar = 0.3f;
    [Space]
    [SerializeField] private AudioClip _upValueSfx; //звук пополнения индикатора
    [SerializeField] private Vector2 pitch=new(1f,1.35f); //разброс высоты звука
    [SerializeField] private AudioSource _audioSource;
    private float _currentValue; // заполненность бара
    private Color _currentColor;
    [SerializeField]  private Material _instMaterial;

    public UnityEvent OnOverflow = new();
    public UnityEvent<float> OnSetWorkload = new(); //загруженность
    //public UnityEvent OnReset = new(); // сбросить наполнение
    // Start is called before the first frame update
    private void Awake()
    {
        _instMaterial = _barRenderer.material;
    }
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initializate(float currentValue, float startValue)
    {
        
        //if (maxValue < 1) maxValue = 1;
        if (startValue < 0) startValue = 0;
        _currentWorkload = currentValue;
        //_currentValue = startValue;
        _currentValue = 0f;
        OnSetWorkload.Invoke(GetPercentWorkload());

        
        UpdateRenderBar();
    }

    

    public void UpValue( float value = 1f)
    {
        _currentValue += value;
        if (_currentValue >= _currentWorkload) //Если переполнено
        {
            _currentValue = _currentWorkload;
            OnOverflow.Invoke();
            
        }
        PlaySound();
        UpdateRenderBar();
    }

    public void ResetValue()
    {
        _currentValue = 0f;
        UpMaxCapacity();
        //UpdateRenderBar();
        _energyBar.transform.DOScaleX(0f, 0.1f);
        
        _currentColor = _colorGradient.Evaluate(0f);
        _instMaterial.color = _currentColor;

    }

    //сброс загруженности и ускорение работы
    public void ResetWorkload()
    {
        _currentWorkload = _defaultWorkloadValue;
        OnSetWorkload.Invoke(GetPercentWorkload());
    }
    public void SetWorkload(int value)
    {
        _currentWorkload = value;
        OnSetWorkload.Invoke(GetPercentWorkload());
    }

    public void UpMaxCapacity()
    {
        if (_currentWorkload>=_maxLimitWorkload)
        {
            return;
        }
        _currentWorkload += 1;
        OnSetWorkload.Invoke(GetPercentWorkload());
    }
    private void UpdateRenderBar()
    {
        //длинна полоски
        float lerp = Mathf.InverseLerp(0f, _currentWorkload, _currentValue);      
        _energyBar.transform.DOScaleX(Mathf.Lerp(0f, 1f, lerp), _speedMoveBar);

        //цвет полоски
        float pointGradient=_currentWorkload/ _maxLimitWorkload;
        _currentColor = _colorGradient.Evaluate(pointGradient);
        _instMaterial.color = _currentColor;
    }

    private float percentFilling; // на сколько индикатор заполнен 0-1
    public void PlaySound()
    {
        if (_upValueSfx == null) return; 

        percentFilling = Mathf.InverseLerp(0f, _currentWorkload, _currentValue);
        _audioSource.pitch = Mathf.Lerp(pitch.x, pitch.y, percentFilling);
        _audioSource.PlayOneShot(_upValueSfx);
    }

    private float GetPercentWorkload()
    {
        if (_currentWorkload< _defaultWorkloadValue)
        {
            return _currentWorkload / _maxLimitWorkload;
        }
        else
        {
            return (_currentWorkload- _defaultWorkloadValue) / _maxLimitWorkload;
        }
       
    }

    public float GetCurrentNumWorkload()
    {
        return _currentWorkload;
    }

    public void LoadData(float currentValue)
    {
        _currentWorkload = currentValue;
        Initializate(_currentWorkload, _defaultWorkloadValue);
    }
}
