using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

[SelectionBase]
[DisallowMultipleComponent]
public class Quest : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Inject] private PlayerStats _playerStats;
    //Предназначен для хранения одного задания

    [SerializeField] private string _idQuest;
   
    [SerializeField] private PlayerStats.TypeResource _priceRerair;
    [SerializeField] private GameObject[] _stockModel; //модели расположенные в поряд в порядке улучшения
    [SerializeField] private GameObject _repairVFX;
    [Space]
    [SerializeField] private GameObject[] _upgradeModels; // это варианты фишиных моделей( например разные цвета или разные стили)

    [SerializeField] private int _indexStock;        // текущий индекс стоковой модели который активен
    [SerializeField] private int _indexUpgrade;      // индекс модели улучшенной, той что нужно отобразить как выбор игрока

    [SerializeField] private Outline[] _outlineObjects; // что нужно выделять
    public bool IsRepairComplete { get; private set; }  // прокачан ли до конца обьект, можно отображать одну из конечных моделей

    [SerializeField] private bool _isBlockToch;
    public bool IsBlockToch { get { return _isBlockToch; } private set { _isBlockToch = value; } } // можно ли вызвать меню нажав
    public bool IsThereUpdates { get { if (_upgradeModels.Length > 1) { return true; } else { return false; } } private set {} }

    public static Quest _lastChoise; //последний выбранный обьект


    
    [Header("Сработает когда обьект будет полностью отремонтирван")]
    public UnityEvent OnCompleteRepair = new(); // Сообщить о том что квест выполнен

    [Serializable]
    public class Data
    {
        public string   idQuest;
        public int      indexStock;
        public int      indexUpgrade;

        public Data(Quest quest)
        {
            if (quest != null)
            {
                idQuest = quest._idQuest;
                indexStock = quest._indexStock;
                indexUpgrade = quest._indexUpgrade;
            }
        }       
    }
    void Start()
    {
        // кеширование Outline
        if (_stockModel != null && _stockModel.Length > 0)
        {
            _outlineObjects = new Outline[_stockModel.Length];
            for (int i = 0; i < _stockModel.Length; i++)
            {
                if (_stockModel[i].TryGetComponent(out Outline outline)) 
                {
                    _outlineObjects[i] = outline;
                }                  
            }                                 
        }
    }

    private Vector2 _posMouseClickDown;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _posMouseClickDown = eventData.position;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right &&
            eventData.position == _posMouseClickDown &&
            !IsBlockToch)
        {
            _lastChoise = this;
            GameOverlayUI.OnCLickLeftBnt.Invoke(_lastChoise);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       /* if (eventData.button == PointerEventData.InputButton.Left &&
            !IsBlockToch)
        {
            _lastChoise = this;
            // GameOverlayUI.OnCLickLeftBnt.AddListener(SetNextStockModel);
            GameOverlayUI.OnCLickLeftBnt.Invoke(_lastChoise);
            //Debug.Log("нажатие");
        }*/

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_outlineObjects[_indexStock] != null)
        {

            _outlineObjects[_indexStock].enabled = true;
            // Debug.Log("Включение");
            //Color colorStart = _outlineObjects[_indexStock].OutlineColor;
            _outlineObjects[_indexStock].OutlineColor = Color.white;
            //colorStart = Color.white;
           // colorStart.a = 0f;
            //_outlineObjects[_indexStock].OutlineColor = colorStart;

           // Color colorEnd = colorStart;
            //colorEnd.a = 1f;

           // DOTween.To(() => _outlineObjects[_indexStock].OutlineColor, x => _outlineObjects[_indexStock].OutlineColor = x, colorEnd, 0.3f).SetOptions(true).
               // OnComplete(() => OutlineOff());


        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_outlineObjects[_indexStock] != null)
        {

            _outlineObjects[_indexStock].enabled = false;

        }
    }
    public void Repair()
    {
        if (_playerStats.Pay(_priceRerair))
        {
            if (_stockModel.Length == 1) // единственная модель
            {
                _stockModel[0].SetActive(false);
                IsRepairComplete = true;
                OnCompleteRepair.Invoke();
                if (!IsThereUpdates)
                {
                    BlockingON();
                }
            }
            if (_indexStock < _stockModel.Length - 1) // множество моделей ремонта
            {
                _stockModel[_indexStock].SetActive(false);
                _indexStock++;
                _stockModel[_indexStock].SetActive(true);

                //зевершен ли ремонт
                if (_indexStock == _stockModel.Length - 1)
                {
                    IsRepairComplete = true;
                    OnCompleteRepair.Invoke();
                    if (!IsThereUpdates)
                    {
                        BlockingON();
                    }
                }
            }
            _playerStats.SaveQuest(this);
            PlayRepairVFX();
        }
        //Debug.Log("Отремонтирован");
    }

    /// <summary>
    /// Установит задание выполненным
    /// </summary>
    public void SetComlete()
    {
        IsRepairComplete = true;

        OnCompleteRepair.Invoke();
        if (_upgradeModels.Length < 1)
        {
            BlockingON();
        }
    }
    public void LoadState(int indexRepair, int indexUpgrade)
    {
        indexRepair = Math.Clamp(indexRepair, 0, _stockModel.Length - 1);
        _indexStock = indexRepair;
        _indexUpgrade = indexUpgrade;

        if (indexRepair == _stockModel.Length - 1)
        {
            SetComlete();
        }
        else
        {
            IsRepairComplete = false;
        }

        //отобразить модель как стадию ремонта
        for (int i = 0; i < _stockModel.Length; i++)
        {
            if (indexRepair == i)
            {
                _stockModel[i].SetActive(true);
            }
            else
            {
                _stockModel[i].SetActive(false);
            }           
        }      
    }

    public void Upgrade()
    {
        _upgradeModels[_indexUpgrade].SetActive(false);
        if (_indexUpgrade < _upgradeModels.Length - 1)
        {
            
            _indexUpgrade++;
            
        }
        else
        {
            _indexUpgrade = 0;
        }
        _upgradeModels[_indexUpgrade].SetActive(true);

        Debug.Log("Апгрейд сменен");
    }

   
    public void BlockingON()
    {
        IsBlockToch = true;
    }
    public void UnBlocking()
    {
        IsBlockToch = false;
    }

    private void PlayRepairVFX()
    {
        if (_repairVFX != null)
        {
            _repairVFX.gameObject.SetActive(true);
        }

    }

    //Стоимость ремонта этого обьекта
    public PlayerStats.TypeResource GetPrice()
    {
        return _priceRerair;
    }
 
    //Получить данный о выполнении квеста( для сохранения)
    public Data GetData()
    {
        Data data = new(this);
       // data = data.SetData(this);
        return data;
    }
    public string GetID()
    {
        return _idQuest;
    }

    public void ShowEffectOutline()
    {
        OutlineOn();
    }
    private void OutlineOn()
    {
        if (_stockModel != null && _stockModel.Length > 0)
        {
            if (_stockModel[_indexStock].TryGetComponent(out Outline outline))
            {
                outline.enabled = true;
               // Debug.Log("Включение");
                Color colorStart = outline.OutlineColor;
                colorStart = Color.yellow;
                colorStart.a = 0f;
                outline.OutlineColor = colorStart;

                Color colorEnd = colorStart;
                colorEnd.a = 1f;
                DOTween.To(() => outline.OutlineColor, x => outline.OutlineColor = x, colorEnd, 1f).SetOptions(true).
                    OnComplete(()=> OutlineOff());
            }
        }          
    }
    private void OutlineOff()
    {
        if (_stockModel != null && _stockModel.Length > 0)
        {
            if (_stockModel[_indexStock].TryGetComponent(out Outline outline))
            {
                
               // Debug.Log("Отключение");
                Color colorStart = outline.OutlineColor;
                colorStart.a = 1f;
                outline.OutlineColor = colorStart;

                Color colorEnd = colorStart;
                colorEnd.a = 0f;
                DOTween.To(() => outline.OutlineColor, x => outline.OutlineColor = x, colorEnd, 3f).SetOptions(true).
                    OnComplete(()=>outline.enabled = false);
            }
        }

    }

    
}
