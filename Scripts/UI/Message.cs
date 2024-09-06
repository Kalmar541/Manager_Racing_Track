using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.Events;

public class Message : MonoBehaviour
{
    [SerializeField] private RectTransform _MessageWindow;
    [SerializeField] private float _durationOpen = 1.2f;
    [SerializeField] private float _durationClose = 0.6f;
    [SerializeField] private TextMeshProUGUI _textMessage;
    public static UnityEvent<string> OnShowMessage = new();

    private bool _isShowingMessage;
    private void Awake()
    {
        OnShowMessage.AddListener(ShowMessage);
    }
    void Start()
    {      
        
    }

    // јнимаци€ открыти€ сообщени€
    public void ShowMessage(string text)
    {
        if (_isShowingMessage)
        {
            _MessageWindow.DOAnchorPosY(-120f, _durationClose).SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    _isShowingMessage = false;
                    OpenMessage(text);
                });
        }
        else
        {
            OpenMessage(text);
        }
    }
    private void OpenMessage(string text)
    {
        _textMessage.text = text;
        _MessageWindow.DOAnchorPosY(15f, _durationOpen).SetEase(Ease.InOutBack);
        _isShowingMessage = true;
    }

    //спаботает на крестик
    public void CloseMessage()
    {
        _MessageWindow.DOAnchorPosY(-120f, _durationClose).SetEase(Ease.InOutBack);
        _isShowingMessage = false;
    }

    private void OnDestroy()
    {
        OnShowMessage.RemoveAllListeners();
    }
}
