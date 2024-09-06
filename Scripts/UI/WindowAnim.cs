using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowAnim : MonoBehaviour
{
    [SerializeField] private Button _btnAccept;

    [SerializeField] private float _timeAnimOpen=0.2f;
    [SerializeField] private float _timeAnimClose=0.2f;
    

    public UnityEvent OnButtonAccept; //Сработает ли кнопка и что должно произойти
    


    private bool _isOpen;
    // Start is called before the first frame update
    void Start()
    {
        _isOpen = gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if (_isOpen) return;
        
        gameObject.SetActive(true);
        AnimOpen();
    }

    public void Close()
    {
        if (!_isOpen) return;
        AnimClose();
    }

    private void AnimOpen()
    {
        transform.DOScale(1f, _timeAnimOpen).SetEase(Ease.OutBack)
            .OnComplete(()=> _isOpen = true);
    }
    private void AnimClose()
    {
        transform.DOScale(0f, _timeAnimClose).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                _isOpen = false;
            });
    }

    //Нажатие кнопки положительного действия( ремонт, покупка)
    public void BUTTON_Accept()
    {
        OnButtonAccept.Invoke();
        Close();
    }

    public void SetActivityButton(bool active)
    {
        _btnAccept.interactable = active;
    }


}
