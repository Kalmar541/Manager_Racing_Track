using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class Assignment : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;
    [Inject] private ControllerMoveCamera _moveCamera_scr;

    [Header("���������� �����")]
    [SerializeField] private string _nameForSave;
    [Header("���������� � ����������")]
    //��� ���� ��� �� ������ ����� �������� �� �������, ���������� �� ����� ������� �� ��� �� ��������
    public Quest[] QuestList; // ������ ������� ��� ����������
    [SerializeField][TextArea] private string _description;
    [Space]
    [SerializeField] private bool _isShowMessage; // ����� �� ��������� ���������
    [SerializeField] [TextArea] private string _MessageText; // ����� ���������
    [Header("������� �� ����������")]
    [SerializeField] private PlayerStats.TypeResource _rewardItem; //��� ��������� ������ (������ �������� ������ �������)

    private bool isComplete = false;
    private int _stageCompliting;
    private bool _isAvialable = true;
    private bool IsInitialized;
    private bool _isRewarder = false;
    public bool IsAvialable
    {
        get { return _isAvialable; } // ����������� ������� � ����������
        set
        {
           // AvailableAllQuests(value);
            _isAvialable = value;
           /* if (_assigmentTxt!= null)
            {
                _assigmentTxt.SetVisibleLabel(value);
            }*/
            if (_isShowMessage&& value) // ������� ��������� ������
            {
                Message.OnShowMessage.Invoke(_MessageText);
            }
        }
    }

    [HideInInspector] public UnityEvent<int,int> OnStageComplete; // �������� �� ������� ��������� ������� (�������, ���� ��������)
     private Assignment _assigmentParent;       // ����� �� ���������� �������� ������� ����������� ���������� ��������
    private AssigmentTxt _assigmentTxt; // ��������� �������� ������������ � ���� �������

    public UnityEvent OnComplete; //������� ���������
    public UnityEvent OnActivate; //��� ��������� �������
    private void Awake()
    {
        // �������������� ��� � awake ����� ����������� ��������� �� ��� �������
        //� � ������ ���� ����� ����������� ����� �� �������� ��������� �������

       // OnComplete.AddListener(() => _assigmentTxt.SetVisibleLabel(false));
       // OnActivate.AddListener(() => _assigmentTxt.SetVisibleLabel(true));

    }
    void Start()
    {

        
        CombineToQuests();
        //CheckComplete();
      
    }
    public void Initialize()
    {
        if (IsInitialized) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Assignment assigmentChild))
            {
                assigmentChild.SetParentAssignment(this);
                OnComplete.AddListener(assigmentChild.Activation);
            }
        }
        IsInitialized = true;
    }
    //��������� �������, ������ ��� �������� � ����������
    //����� �������� �� ����� ���������� �����������, �.� ������� ������� �������
    public void Activation()
    {
        if (isComplete) return;

        IsAvialable = true;
        AvailableAllQuests(IsAvialable);
        if (_assigmentTxt != null)
        {
            _assigmentTxt.SetVisibleLabel(IsAvialable);
        }
        // MenuQuests.OnChangeActivityLabel.Invoke(-1);
        //TargetQuestObjects();
        OnActivate.Invoke();


    }
    /// <summary>
    /// ��������� ����� ������ �������������
    /// </summary>
    public void InitTxtLabel()
    {
        _assigmentTxt.SetComplitingLabel(0, QuestList.Length);
    }
    public void CheckComplete()
    {
        _stageCompliting = 0;
        foreach (var item in QuestList)
        {
            if (item.IsRepairComplete)
            {
                _stageCompliting++;
            }
        }
        OnStageComplete.Invoke(_stageCompliting, QuestList.Length);
        if (_stageCompliting>= QuestList.Length)
        {
            isComplete = true;
            OnComplete?.Invoke();
            IsAvialable = false;
            if (_assigmentTxt != null)
            {
                _assigmentTxt.SetVisibleLabel(IsAvialable);
            }
            RewardForCompliting();
           // Debug.Log($"��������� - {name}");
        }
        /*if (_assigmentTxt != null)
        {
            _assigmentTxt.SetVisibleLabel(IsAvialable);
        }*/
    }

    public void CheckAvialable()
    {
        if (_assigmentParent != null)
        {
            IsAvialable = _assigmentParent.isComplete;
            
        }
        AvailableAllQuests(IsAvialable);
    }
    
    //������� ����� � �������
    private void CombineToQuests()
    {
        foreach (var item in QuestList)
        {
            item.OnCompleteRepair.AddListener(CheckComplete);
        }
    }
    public string GetDescription()
    {
        return _description;
    }

    public PlayerStats.TypeResource GetRewartItem()
    {
        return _rewardItem;
    }
    public string GetUnicalName()
    {
        return _nameForSave;
    }

    public int GetStageComplete()
    {
        return _stageCompliting;
    }
    //��������� ����������� ������� ��� ����������
    private void AvailableAllQuests(bool available)
    {
        foreach (Quest quest in QuestList)
        {
            if (!available)
            {
                quest.BlockingON();
            }
            else
            {
                if (!quest.IsRepairComplete)
                {
                    quest.UnBlocking();
                }
                
            }
            
        }
    }
    //���������� ����� ������� ����� ��������� ����� ���� �������
    public void SetParentAssignment( Assignment assign)
    {
        _assigmentParent = assign;
    }

    private void ShowMessageForPlayer()
    {
        if (_isShowMessage)
        {
            Message.OnShowMessage.Invoke(_MessageText);
        }
    }
    public void SetAssigmentTxt (AssigmentTxt assigmentTxt)
    {
        _assigmentTxt = assigmentTxt;
        assigmentTxt.OnClick = ShowMessageForPlayer;
        assigmentTxt.OnClick += TargetQuestObjects;
    }

    public void LoadQuestState(Dictionary<string, Quest.Data> QuestSaves)
    {
        foreach (Quest quest in QuestList)
        {
            string nameQuest = quest.GetID();

            if (QuestSaves.ContainsKey(nameQuest))
            {
                quest.LoadState(QuestSaves[nameQuest].indexStock, QuestSaves[nameQuest].indexUpgrade);
            }
            else
            {
                quest.LoadState(0, 0);
            }
        }
        //Initialize();
        CheckComplete();

    }

    private void RewardForCompliting()
    {
        if (!_isRewarder)
        {
            _isRewarder = true;
            _playerStats.RewardPlayer(_rewardItem);
        }
    }

    public void TargetQuestObjects()
    {
        if (QuestList!= null&& QuestList.Length>0)
        {
            _moveCamera_scr.RotateCamera(QuestList[_stageCompliting].transform);
        }
        foreach (var item in QuestList)
        {
            item.ShowEffectOutline();
        }
    }

}
