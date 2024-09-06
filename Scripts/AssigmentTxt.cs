using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

/// <summary>
/// ��������� �������� �� ����������� ������� � ���� ������ � ������ �������
/// 
/// </summary>
public class AssigmentTxt : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _description;  //�������� ������� ��������
    [SerializeField] private TextMeshProUGUI _compliting;   //������ ���������� �������� "0/3"
    [Space]
    [Header("������� �� ����������")]
    [SerializeField] private Image _imageReward; //(�� ���������� ����� ������, ���� ���. ������ �������) // ���������� ������� �� ����������
    [SerializeField] private TextMeshProUGUI _rewardItemTxt; // ����������� �������
    [Space]
    [SerializeField] private Image _imageLabel;             // ��� ������
    [SerializeField] private Color _pointerEnterColor;      // ���� ��� ��������� ����
    [SerializeField] private Color _pointerExitColor;       // ���� ��� ������ ����

    private float _heigthLabel=35f; //������ ������

    public Action OnClick; //��� ���������� �� ����� (�������� ����������� ������� �������� ������)


    private void Awake()
    {
      //  MenuQuests.OnChangeActivityLabel.AddListener
    }
    private void Start()
    {
        
    }

    /// <summary>
    /// ���������� �������� ��������
    /// </summary>
    /// <param name="txt"></param>
    public void SetDescriptionLabel(string txt)
    {
        _description.text = txt;
    }
    /// <summary>
    /// ���������� ����������
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

    //�������� ������ ��������� ��������� � �������
    public void SetMessageForPlayer()
    {
        if (OnClick!= null)
        {
            OnClick.Invoke();
        }
        
    }
    //������������ ������� ������� �������
    public void SetVisibleLabel(bool visible)
    {
        //�� �������� ������� ������� vert group �� ������� ��������� ���������
        if (visible)
        {
            ShowGO();
            //�������� ���������
            _rectTransform.sizeDelta = new(_rectTransform.sizeDelta.x, 0f);
            _rectTransform.DOSizeDelta(new(_rectTransform.sizeDelta.x, _heigthLabel), 1f);
            

        }
        else
        {
            //��������
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

    //��������� ������� ������, ��� �������� ��������������� 
    public void PaintLabelAsSelected()
    {
        _imageLabel.color = _pointerEnterColor;
    } 
    public void PaintLabelAsNormal()
    {
        _imageLabel.color = _pointerExitColor;
    }

}
