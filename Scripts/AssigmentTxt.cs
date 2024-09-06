using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

/// <summary>
/// Компонент отвечает за отображение задания в виде строки в списке заданий
/// 
/// </summary>
public class AssigmentTxt : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _description;  //описание задания короткое
    [SerializeField] private TextMeshProUGUI _compliting;   //стадия выполнения например "0/3"
    [Space]
    [Header("Награда за выполнение")]
    [SerializeField] private Image _imageReward; //(не релизована смена иконок, пока исп. только престиж) // отобразить награду за выполнение
    [SerializeField] private TextMeshProUGUI _rewardItemTxt; // колличество награды
    [Space]
    [SerializeField] private Image _imageLabel;             // фон строки
    [SerializeField] private Color _pointerEnterColor;      // цвет при наведении мыши
    [SerializeField] private Color _pointerExitColor;       // цвет при выходе мыши

    private float _heigthLabel=35f; //высота строки

    public Action OnClick; //Что произойдет по щелку (например отображение полного описания игроку)


    private void Awake()
    {
      //  MenuQuests.OnChangeActivityLabel.AddListener
    }
    private void Start()
    {
        
    }

    /// <summary>
    /// Установить короткое описание
    /// </summary>
    /// <param name="txt"></param>
    public void SetDescriptionLabel(string txt)
    {
        _description.text = txt;
    }
    /// <summary>
    /// Отобразить выполнение
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    public void SetComplitingLabel(int current, int max)
    {
        _compliting.text = $"{current}/{max}";
    }

    public void SetRewardTxt(PlayerStats.TypeResource item)
    {
        if (item.resource!=PlayerStats.Resource.none&&
            item.resource==PlayerStats.Resource.prestige)
        {
            _rewardItemTxt.text = "+" + item.Count.ToString();
        }
        else
        {
            _imageReward.enabled = false;
            _rewardItemTxt.text = "";
        }
       
    }

    //Показать игроку подробное сообщение о задании
    public void SetMessageForPlayer()
    {
        if (OnClick!= null)
        {
            OnClick.Invoke();
        }
        
    }
    //Анимированно убурать строчку задания
    public void SetVisibleLabel(bool visible)
    {
        //не работает должным образом vert group не красиво обновляет положение
        if (visible)
        {
            ShowGO();
            //анимация раскрытия
            _rectTransform.sizeDelta = new(_rectTransform.sizeDelta.x, 0f);
            _rectTransform.DOSizeDelta(new(_rectTransform.sizeDelta.x, _heigthLabel), 1f);
            

        }
        else
        {
            //закрытие
            _rectTransform.DOSizeDelta(new(_rectTransform.sizeDelta.x, 0f), 1f).OnComplete(HideGO);
            // transform.DOScaleY(0f, 0.5f).OnComplete(HideGO);
           
            
        }
       
        void HideGO()
        {
            gameObject.SetActive(false);
        }
        void ShowGO()
        {
            gameObject.SetActive(true);
        }
    }

    //Выделение строчки цветом, для придания интерактивности 
    public void PaintLabelAsSelected()
    {
        _imageLabel.color = _pointerEnterColor;
    } 
    public void PaintLabelAsNormal()
    {
        _imageLabel.color = _pointerExitColor;
    }

}
