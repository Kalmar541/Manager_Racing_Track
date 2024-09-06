using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonPayCar : MonoBehaviour, IPointerDownHandler
{
    [Header("Установить в инспекторе")]
    [SerializeField] private GameObject _movebleButton;
    [SerializeField] private Transform _posClickDown;
    [SerializeField] private Transform _posClickUp;
    [SerializeField] private float _timeMoveDown;
    public  UnityEvent OnButtonClick = new(); // что произойдет по нажатию кнопки

    private Tween TWclickDown;
    private Tween TWclickUp;
    private Sequence click;

    
    private void Awake()
    {
        Initializate();
    }

    void Start()
    {

        
    }

    void Update()
    {
        
    }

    private void Initializate()
    {       
        TWclickDown = _movebleButton.transform.DOLocalMove(_posClickDown.localPosition, _timeMoveDown).SetAutoKill(false).Pause();       
        TWclickUp =_movebleButton.transform.DOLocalMove(_posClickUp.localPosition, _timeMoveDown).SetEase(Ease.OutElastic).SetAutoKill(false).Pause();

        click = DOTween.Sequence().SetAutoKill(false);
        click.Append(TWclickDown);
        click.Append(TWclickUp);
    }

    //Нажатие на кпопку
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnButtonClick.Invoke();
            AnimClick();
        }
        
    }

    //Анимация нажатия
    private void AnimClick()
    {
        click.Restart();
    }

    private void OnDestroy()
    {
        TWclickDown.Kill();
        TWclickUp.Kill();
        click.Kill();
    }
}
