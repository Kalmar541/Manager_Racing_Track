using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;
using UnityEngine.UI;

public class Reclam : MonoBehaviour
{
    [Header("Текстовые поля")]
    [SerializeField] private TextMeshProUGUI _labelReclama;
    [SerializeField] private TextMeshProUGUI _txtTime;

    [Header("Фон")]
    [SerializeField] private GameObject _background;

    public bool IsWorking;

    private float _timerShowing;
    private float _timeLastShow;
    private const float _defaultTimer = 300f;

    // Start is called before the first frame update
    void Start()
    {
        if (_timerShowing< _defaultTimer)
        {
            _timerShowing = _defaultTimer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWorking)
        {
            WorkingTimerReclam();
        }
        
    }

    public void SetTimer( float second)
    {
        if (second < _defaultTimer)
        {
            _timerShowing = _defaultTimer;
        }
        else
        {
            _timerShowing = second;
        }
       
    }

    private void WorkingTimerReclam()
    {
        if (  _timeLastShow+ _timerShowing<= Time.time)
        {
            _timeLastShow = Time.time;
            StartCoroutine(ShowReclamFullScreen());
        }
    }

    private IEnumerator ShowReclamFullScreen()
    {
        _background.SetActive(true);
        _labelReclama.gameObject.SetActive(true);
        _txtTime.gameObject.SetActive(true);
        _txtTime.text = "2";

        yield return new WaitForSeconds(1f);
        _txtTime.text = "1";

        yield return new WaitForSeconds(1f);
        _background.SetActive(false);
        _labelReclama.gameObject.SetActive(false);
        _txtTime.gameObject.SetActive(false);
        YandexGame.FullscreenShow();
    }
}
