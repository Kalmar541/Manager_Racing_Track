using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using TMPro;
using Zenject;
/// <summary>
/// ���� �������������� � �������� ���� Quest ( ���������������, ������������� � ������)
/// </summary>
public class MenuObj : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;

    [Header("���������� � ����������")]
    [SerializeField] private GameObject _pointRotate;
    [SerializeField] float _timeScaling;
    [SerializeField] private GameObject _pricePanel; //GO ������ ��������� ��� ���/����
    [SerializeField] private Image _iconResouce; // ����� ��� ����� ���������� ������
    [SerializeField] private TextMeshProUGUI _priceTxt; // �������� ��������� ���������
    [SerializeField] private CanvasGroup _repairButton; //������ �������, ��� �� ������ �� �� ��������� ���������
    [Space]
    [SerializeField] private Sprite _moneyImg; // ������ �����
    [SerializeField] private Sprite _starImg; // ������ �����

    private event Action OnClickRepair ;
    private event Action OnClickUpgrade;

    [SerializeField] private Button _repairBtn;
    [SerializeField] private Button _upgradeBtn;

    private Vector3 _scaleClose = new Vector3(0f, 0f, 0f);
    private Vector3 _scaleOpen = Vector3.one;
  
    void Start()
    {
        _pointRotate.transform.localScale = _scaleClose;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    //������� ����������� ������
    private void UpdateVisual(Quest quest)
    {
        _upgradeBtn.gameObject.SetActive(quest.IsThereUpdates);

        _repairBtn.interactable = !quest.IsRepairComplete;
        _upgradeBtn.interactable = quest.IsRepairComplete;


    }

    public void BUTTON_Repair()
    {
        OnClickRepair.Invoke();
        CloseMenu();
    }
    public void BUTTON_Upgrade()
    {
        OnClickUpgrade.Invoke();
    }

    public void OpenMenu(Quest quest)
    {
        UpdateVisual(quest);
         OnClickRepair = quest.Repair;
        OnClickUpgrade = quest.Upgrade;
        ShowPrice(quest.GetPrice());
        _pointRotate.transform.localScale = _scaleClose;
        _pointRotate.transform.DOScale(_scaleOpen, _timeScaling).SetEase(Ease.OutSine);
    }
    public void CloseMenu()
    {
       // OnClickRepair = null;
        _pointRotate.transform.DOScale(_scaleClose, _timeScaling).SetEase(Ease.OutSine);
    }

    private void ShowPrice(PlayerStats.TypeResource res)
    {
        _repairButton.alpha = 1f;
        switch (res.resource)
        {
            case PlayerStats.Resource.none:
                _pricePanel.SetActive(false);
                _priceTxt.text = "0";
                _repairButton.alpha = 1f;
                break;
            case PlayerStats.Resource.money:
                _pricePanel.SetActive(true);
                _iconResouce.sprite = _moneyImg;
                _priceTxt.text = "" + res.Count.ToString();
                if (_playerStats.Money< res.Count) {_repairButton.alpha = 0.45f;}
                break;
            case PlayerStats.Resource.star:
                _pricePanel.SetActive(true);
                _iconResouce.sprite = _starImg;
                _priceTxt.text = "" + res.Count.ToString();
                if (_playerStats.Stars < res.Count) { _repairButton.alpha = 0.45f; }
                break;
            default:
                break;
        }
    }
}
