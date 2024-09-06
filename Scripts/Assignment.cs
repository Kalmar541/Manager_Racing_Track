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

    [Header("Уникальный номер")]
    [SerializeField] private string _nameForSave;
    [Header("Установить в инспекторе")]
    //Для того что бы квесты могли работать по порядку, необходимо на сцене сложить их той же иерархии
    public Quest[] QuestList; // список квестов для выполнения
    [SerializeField][TextArea] private string _description;
    [Space]
    [SerializeField] private bool _isShowMessage; // нужно ли показвать сообщение
    [SerializeField] [TextArea] private string _MessageText; // текст сообщения
    [Header("Награда за выполнение")]
    [SerializeField] private PlayerStats.TypeResource _rewardItem; //чем наградить игрока (сейчас работает только престиж)

    private bool isComplete = false;
    private int _stageCompliting;
    private bool _isAvialable = true;
    private bool IsInitialized;
    private bool _isRewarder = false;
    public bool IsAvialable
    {
        get { return _isAvialable; } // Доступность задания к выполнению
        set
        {
           // AvailableAllQuests(value);
            _isAvialable = value;
           /* if (_assigmentTxt!= null)
            {
                _assigmentTxt.SetVisibleLabel(value);
            }*/
            if (_isShowMessage&& value) // покажем сообщение игроку
            {
                Message.OnShowMessage.Invoke(_MessageText);
            }
        }
    }

    [HideInInspector] public UnityEvent<int,int> OnStageComplete; // сообщать на сколько выполнено задание (текущее, макс значение)
     private Assignment _assigmentParent;       // Квест от выполнения которого зависит доступность выполнения текущего
    private AssigmentTxt _assigmentTxt; // текстовое описание существующее в меню заданий

    public UnityEvent OnComplete; //Задание выполнено
    public UnityEvent OnActivate; //При активации задания
    private void Awake()
    {
        // предполагается что в awake будет установлено выполнено ли это задание
        //и в методе ниже будет установлено может ли работать зависимое задание

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
    //Активация задания, теперь оно доступно к выполнению
    //Метод подписан на ивент выполнения предыдущего, т.о тянется цепочка квестов
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
    /// Заполнить текст стадии выполненности
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
           // Debug.Log($"Выполнено - {name}");
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
    
    //Связать квест в задание
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
    //Устновить доступность квестов для выполнения
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
    //Установить квест который нужно выполнить перед этим квестом
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
