using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;
using Zenject;

public class PlayerStats : MonoBehaviour
{
    [Inject] private PrestigeCounter _prestigeCounter;

    [Serializable]
    public class TypeResource
    {
        public Resource resource;
        public int Count;   
    }
    public enum Resource // ��� ������� ��� ��������
    {
        none,
        money,
        star,
        prestige,
        gold
    }

    //������� ������
    private float _money;
    public float Money { get { return _money;}private set { _money = value; } }
    public int Stars { get; set; }
    public int Gold { get; private set; }
    public float RecyclerWorkload { get; set; }

    // ��������� ��� ��������� � �����������
    public float MyltyplyMoney { get; private set; } = 1;

    //����
    public int GoldForResetRecycler { get; private set; } = 3;
    public int MoneyForGold { get; private set; } = 100;

    public List<Quest.Data> saveQuests;
    public Dictionary<string, Quest.Data> QuestSaves = new();
    public int[] MergeCarAtParking;
    public Dictionary<int, float> _priceUpgare;

    public  UnityEvent<float> OnSetMoney = new();
    public static UnityEvent OnAddStars = new(); //���������� �����
    public UnityEvent OnSetGold = new();

    public int MaxMergeNum;

    [SerializeField] private bool _isDebug;
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        IconMultMoney.OnChangeMyltyply.AddListener((x) => MyltyplyMoney = x);
    }

    public void AddMoney( float count)
    {
        if (count>0f)
        {
            Money += count;
            OnSetMoney.Invoke(Money);
            SaveResource();
        }
    }
    public void SubstractMoney( float count)
    {
        if (count>0f)
        {
            Money -= count;
            OnSetMoney.Invoke(Money);
            SaveResource();
        }
    }

    public float GetBalance() {return Money;}

    public void AddStars(int num)
    {
        Stars += num;
        SaveResource();
        OnAddStars.Invoke();
    }
    public void AddGold(int num)
    {
        Gold += num;
        SaveResource();
        OnSetGold.Invoke();
    }

    //��������� ��������� ������ � ������� � ����������
    public bool Pay(TypeResource res)
    {
        switch (res.resource)
        {
            case Resource.none:
                return true;

            case Resource.money:
                if (Money >= res.Count)
                {
                    SubstractMoney(res.Count);
                    return true;
                }
                else return false;
                
            case Resource.star:
                if (Stars >= res.Count)
                {
                    Stars-=(res.Count);
                    OnAddStars.Invoke();
                    SaveResource();
                    return true;
                }
                else return false;
        }
        return false;
    }

    public bool SpendGold (int count)
    {
        if (count>0&& Gold>=count)
        {
            Gold -= count;
            OnSetGold.Invoke();
            return true;
        }
        return false;

    }

    public void RewardPlayer(TypeResource res)
    {
        switch (res.resource)
        {
            case Resource.none:
                break;
            case Resource.money:
                AddMoney(res.Count);
                break;
            case Resource.star:
                AddStars(res.Count);
                break;
            case Resource.prestige:
                _prestigeCounter.AddPrestigeForQuest(res.Count);
                break;
            case Resource.gold:
                Gold+=res.Count;
                break;
            default:
                break;
        }
    }
    public void SaveResource()
    {
        if (true)//���������� ��� �������
        {
            YandexGame.savesData.Money = Money;
            YandexGame.savesData.Stars = Stars;
            YandexGame.savesData.Gold = Gold;
            YandexGame.savesData.RecyclerWorkload = RecyclerWorkload;
            YandexGame.SaveProgress();
            if (_isDebug) Debug.Log("���������� ��������");
        }
    }
    
    public void SaveQuest( Quest quest)
    {
        Quest.Data data = quest.GetData();
        if (!QuestSaves.ContainsKey(data.idQuest))
        {
            QuestSaves.Add(data.idQuest, data);
        }
        else
        {
            QuestSaves[data.idQuest] = data;
        }           
        YandexGame.savesData.QuestSaves = QuestSaves;
        YandexGame.SaveProgress();
        if (_isDebug) Debug.Log("���������� �������");
    }

    public void SaveCarOnParking(MergeCar[] carsOnParking)
    {
        int[] MergeCars = new int[carsOnParking.Length];
        for (int i = 0; i < carsOnParking.Length; i++)
        {
            if (carsOnParking[i]!= null)
            {
                MergeCars[i] = carsOnParking[i].MergeNumber;
            }
            else
            {
                MergeCars[i] = -1; // ��� ������ ��� � ���� ����� ��� ������, ( 0 - ��� ������ ������)
            }
            
        }
        MergeCarAtParking = MergeCars;
        YandexGame.savesData.MergeNumberCarsOnParking = MergeCars;
        YandexGame.SaveProgress();
        if (_isDebug) Debug.Log("���������� ����� �� ��������");
    }
    public void SaveNumMerge(int num)
    {
        if (num>0)
        {
            MaxMergeNum = num;
            YandexGame.savesData.MaxNumMerge = MaxMergeNum;
            //��� ������������� �������� ������ � ������, ������ ��� ��� ����� ������� � ���������� �������� ������
            //YandexGame.SaveProgress(); 
        }
        else
        {
            Debug.LogWarning("������� ���������� ������� ������: ��������. �������� �����:" + num);
        }
        
    }
    public void SavePriceUpgrade(Dictionary<int, float> dictPrices)
    {
        _priceUpgare = dictPrices;
        YandexGame.savesData._priceUpgare = dictPrices;
        YandexGame.SaveProgress(); 
    }
    public void LoadAllData()
    {
        Money = YandexGame.savesData.Money;
        Stars = YandexGame.savesData.Stars;
        Gold = YandexGame.savesData.Gold;
        RecyclerWorkload = YandexGame.savesData.RecyclerWorkload;

        if (YandexGame.savesData.QuestSaves!= null)
        { QuestSaves = YandexGame.savesData.QuestSaves; }

        MergeCarAtParking = YandexGame.savesData.MergeNumberCarsOnParking;

        MaxMergeNum = YandexGame.savesData.MaxNumMerge;
        _priceUpgare = YandexGame.savesData._priceUpgare;
    }
    private void OnDestroy()
    {
        OnSetMoney.RemoveAllListeners();
        OnAddStars.RemoveAllListeners();
        
    }

    private void OnEnable()
    {
        YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;

        YandexGame.Instance.CloseFullscreenAd.AddListener(SetReclamStateOff);
        YandexGame.Instance.CloseVideoAd.AddListener(SetReclamStateOff);

        YandexGame.Instance.OpenFullscreenAd.AddListener(SetReclamStateOn);
        YandexGame.Instance.OpenVideoAd.AddListener(SetReclamStateOn);
        YandexGame.Instance.RewardVideoAd.AddListener(SetReclamStateOn);
    }

    // ������������ �� ������� ��������/�������� ������� ����
    private void OnDisable()
    {
        YandexGame.onVisibilityWindowGame -= OnVisibilityWindowGame;

        YandexGame.Instance.CloseFullscreenAd.RemoveListener(SetReclamStateOff);
        YandexGame.Instance.CloseVideoAd.RemoveListener(SetReclamStateOff);

        YandexGame.Instance.OpenFullscreenAd.RemoveListener(SetReclamStateOn);
        YandexGame.Instance.OpenVideoAd.RemoveListener(SetReclamStateOn);
        YandexGame.Instance.RewardVideoAd.RemoveListener(SetReclamStateOn);
    }
    private void SetReclamStateOn()
    {
        _reclamShow = true;
    }
    private void SetReclamStateOff()
    {
        _reclamShow = false;
    }
    private bool _reclamShow;
    public void OnVisibilityWindowGame(bool visible)
    {
        if (visible)
        {
            if (!_reclamShow)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
            
        }
        else
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
    }



}
