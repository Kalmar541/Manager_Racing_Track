using System.Collections;
using UnityEngine;

using TMPro;
using UnityEngine.Events;
using Zenject;
using System;
using DG.Tweening;
using YG;

public class GameOverlayUI : MonoBehaviour
{
    [Inject] PlayerStats _playerStats_scr;

    [Header("Установить в инспекторе")]
    [SerializeField] private TextMeshProUGUI _moneyTxt;
    [SerializeField] private Transform _moneyImage;
    //[SerializeField] private GameObject _prefabStar; // префаб  звездочки для создаия
    //[SerializeField] private Transform _pointStarsOnCanvas; // позиция иконки звезносчки на экране
    [SerializeField] private TextMeshProUGUI _starsTxt;
    [SerializeField] private Transform _starsImage;

    [SerializeField] private TextMeshProUGUI _prestigeQuestTxt;
    [SerializeField] private TextMeshProUGUI _prestigeCarTxt;
    [SerializeField] private TextMeshProUGUI _prestigeTotalTxt;
    [SerializeField] private Transform _prestigeImage;

    [SerializeField] private TextMeshProUGUI _goldTxt;
    [SerializeField] private Transform _goldImage;

    [SerializeField] private float _forceStrength = 0.75f;
    [Space]
    [SerializeField] private MenuObj _goMenuObj; // GO меню апгрейда обьекта на сцене
    [SerializeField] private MenuMergeCar _menuMergeCar; // GO меню апгрейда мердж машины на сцене
    [SerializeField] private Camera _mainCamera; //основная камера для работы с уровнем

    [SerializeField] private PrestigeCounter _prestigeCounter;

    //[Inject] private GameOverlayUI ui;
    [Header("Окна")]
    [SerializeField] private GameObject _airdropWindow;

    [Header("Тест")]
    [SerializeField] private TextMeshProUGUI _modeScreen;
    

    public static UnityEvent<Quest> OnCLickLeftBnt = new();
    public static Action <MergeCar> OnShowMenuMergeCar ;
    public static UnityEvent OnOpenAirdropWindow = new();

    private void Awake()
    {
        PlayerStats.OnAddStars.AddListener(UpdateCountStars);
        _prestigeCounter.OnSetPrestige.AddListener(SetPrestige);
        //_prestigeCounter.UpdatePrestige();
        
        _mainCamera = Camera.main;
        
    }
    void Start()
    {
       // Debug.Log(ui.name);
        _playerStats_scr.OnSetMoney.AddListener(SetMoney);
        _playerStats_scr.OnSetGold.AddListener(UpdateCountGold);

        //обновить колличество ресурсов
        SetMoney(_playerStats_scr.Money);
        UpdateCountStars();
        UpdateCountGold();

        OnCLickLeftBnt.AddListener(ShowMenuObj);
        OnOpenAirdropWindow.AddListener(OpenAirdropWindow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)||
            Input.GetKeyDown(KeyCode.W)||
            Input.GetKeyDown(KeyCode.S)||
            Input.GetKeyDown(KeyCode.A)||
            Input.GetKeyDown(KeyCode.D))
        {
            CloseMenuQuest();
            CloseMenuCar();
        }
        if (YandexGame.isFullscreen)
        {
            _modeScreen.text = "Полный экран";
        }
        else
        {
            _modeScreen.text = "Свернутый экран";
        }
       
    }
    public void SetMoney(float count)
    {
        _moneyTxt.text = count.ToString("F0");
        AnimShakeIcon(_moneyImage);
    }

    public void UpdateCountStars()
    {
        _starsTxt.text = _playerStats_scr.Stars.ToString();
        AnimShakeIcon(_starsImage);
    }

    public void UpdateCountGold()
    {
        _goldTxt.text = _playerStats_scr.Gold.ToString();
        AnimShakeIcon(_goldImage);
    }

    public void SetPrestige(int quests, int cars)
    {
        _prestigeQuestTxt.text = $"{quests}";
        _prestigeCarTxt.text = $"{cars}";
        _prestigeTotalTxt.text = $"{(quests + cars)}";

        AnimShakeIcon(_prestigeImage);
    }
    
    private void AnimShakeIcon(Transform icon)
    {
        icon.DOPunchScale(new Vector3(1,1,1)* _forceStrength, 0.25f).OnComplete(() => icon.transform.localScale = Vector3.one);
    }

    private Vector3 _posMenuUI;
    private void ShowMenuObj(Quest quest)
    {
        //_posMenuUI = _mainCamera.WorldToScreenPoint(quest.transform.position);
        _posMenuUI = Input.mousePosition;
        _goMenuObj.transform.position = _posMenuUI;
        _goMenuObj.OpenMenu(quest);
        CloseMenuCar();
    }

    private void CloseMenuQuest()
    {
        _goMenuObj.CloseMenu();     
    }

    public void ShowMenuMergeCar(MergeCar car)
    {
        _posMenuUI = _mainCamera.WorldToScreenPoint(car.transform.position);
        _menuMergeCar.transform.position = _posMenuUI;
        _menuMergeCar.OpenMenu(car);
        CloseMenuQuest();
    }
    private void CloseMenuCar()
    {      
        _menuMergeCar.CloseMenu();

    }

    public void OpenAirdropWindow()
    {
        _airdropWindow.SetActive(true);
    }

    public void CloseAirdropWindow()
    {
        _airdropWindow.SetActive(false);
    }
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += RewardPlayer;
    }

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= RewardPlayer;
    }
    private void OnDestroy()
    {
        OnCLickLeftBnt.RemoveAllListeners();
        OnShowMenuMergeCar = null;
        OnOpenAirdropWindow.RemoveAllListeners();
    }

    public void BUTTON_OpenBox()
    {
        ShowReclam();
        CloseAirdropWindow();
    }
    private void ShowReclam()
    {
        YandexGame.RewVideoShow(1);
    }

    private void RewardPlayer(int id)
    {
        int countGold = 0;
        float chance = UnityEngine.Random.value;
        countGold = GetGold(chance);

        int GetGold(float chance)
        {
            if (chance >= 0.9f) return countGold = 5;
            if (chance >= 0.8f) return countGold = 4;
            if (chance >= 0.7f) return countGold = 3;
            if (chance >= 0.5f) return countGold = 2;
            else { return countGold = 1; }
        }

        if (id == 1)
        {
            _playerStats_scr.AddGold(countGold);
        }
    }

    private void CalculateAvgValueDropGold()
    {
        float avg = 0;
        int countIteration = 100;

        for (int i = 0; i < countIteration; i++)
        {
            CalcChance();
        }

        Debug.Log("среднее: "+(avg / countIteration));

        void CalcChance()
        {
            int countGold = 0;
            float chance = UnityEngine.Random.value;
            avg += GetGold(chance);
            Debug.Log("выпало золота - " + countGold);

            int GetGold(float chance)
            {
                if (chance >= 0.9f) return countGold = 5;
                if (chance >= 0.8f) return countGold = 4;
                if (chance >= 0.7f) return countGold = 3;
                if (chance >= 0.5f)  return countGold = 2; 
                else { return countGold = 1; }
            }
        }
    }
}
