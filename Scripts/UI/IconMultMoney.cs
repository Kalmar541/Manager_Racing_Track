using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;
using UnityEngine.Events;

public class IconMultMoney : MonoBehaviour
{
    [SerializeField] private GameObject _iconNormal;
    [SerializeField] private GameObject _iconNoFullscreen;

    [SerializeField] private TextMeshProUGUI _txtTimerNormal;
    [SerializeField] private TextMeshProUGUI _txtTimerNegative;
    [SerializeField] private TextMeshProUGUI _txtMyltiply;

    private float _timerMylt;
    private float _timeForAddMylt=300f;
    private bool isFullscreen;
    private bool _isTimerOn;

    private float _timeToResetMylt=30f;
    private float _timerReset;

    private float _currentMylt=1.1f;
    private float _maxMylt =1.5f;
    private float _defaultMylt = 1f;
    private float _valueAddMylt = 0.1f;

    private string _RusLabelMax = "Макс.";
    private string _labelMaximum;

    public static UnityEvent<float> OnChangeMyltyply = new();
    // Start is called before the first frame update
    void Start()
    {
        _labelMaximum = _RusLabelMax;

        _isTimerOn = _currentMylt < _maxMylt;

    }

    // Update is called once per frame
    void Update()
    {


        isFullscreen = YandexGame.isFullscreen;

        //Переключить иконки множителя
        if (!isFullscreen)
        {
            SetActiveIconNormal();
        }
        else
        {
            SetActiveIconNoFullscreen();
        }

        WorkingTimer();
        
        _txtTimerNegative.text = System.TimeSpan.FromSeconds(_timerReset).ToString(@"m\:ss");
        _txtMyltiply.text = "x" + _currentMylt.ToString("F1");

    }

    private void WorkingTimer()
    {
        if (!isFullscreen)
        {
            if (_isTimerOn)
            {
                if (_timeForAddMylt > _timerMylt)
                {
                    _timerMylt += Time.deltaTime;
                    _txtTimerNormal.text = System.TimeSpan.FromSeconds(_timerMylt).ToString(@"m\:ss");

                    //таймер до негативного действия восствновить
                    if (_timerReset < _timeToResetMylt)
                    {
                        _timerReset += Time.deltaTime;
                    }
                    else
                    {
                        _timerReset = _timeToResetMylt;
                    }
                }
                else
                {
                    //сработает тик таймера на 1 ед
                    _currentMylt += _valueAddMylt;
                    OnChangeMyltyply.Invoke(_currentMylt);

                    if (_currentMylt < _maxMylt)
                    {
                        _isTimerOn = true;
                        _timerMylt = 0;
                        _txtTimerNormal.text = System.TimeSpan.FromSeconds(_timerMylt).ToString(@"m\:ss");
                    }
                    else
                    {
                        _timerMylt = 0;
                        _isTimerOn = false;
                        _txtTimerNormal.text = _labelMaximum;
                    }
                    //перезапуск таймера тика

                    
                }
            }          
        }
        else //негативное действие
        {
            if (_timerReset > 0f)
            {
                _timerReset -= Time.deltaTime;
            }
            else
            {
                _timerReset = 0f;
                _currentMylt = _defaultMylt;
                OnChangeMyltyply.Invoke(_currentMylt);
                _isTimerOn = true;
                //отключить

            }
        }
    }

    public void BUTTON_SetNoFullscreen()
    {
        SetNoFullscreen();
    }

    private void SetNoFullscreen()
    {
        YandexGame.SetFullscreen(false);
    }
    private void SetActiveIconNormal()
    {
        _iconNormal.SetActive(true);
        _iconNoFullscreen.SetActive(false);
    }
    private void SetActiveIconNoFullscreen()
    {
        _iconNormal.SetActive(false);
        _iconNoFullscreen.SetActive(true);
    }

    public void TestSetFullScreen()
    {
        isFullscreen = true;
    }
    public void TestSetNoFull()
    {
        isFullscreen = false;
    }





    private void OnDestroy()
    {
        OnChangeMyltyply.RemoveAllListeners();
    }
}
