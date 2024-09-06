using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuQuests : MonoBehaviour
{
    [SerializeField] private float _baseHeigth = 75f; // базовая высота окна
    [SerializeField] private float _labelHeigth = 35f; // высота строчки названия задания
    [SerializeField] private RectTransform _windowQuests; // окно со списком квестов
    [SerializeField] private AssigmentTxt _prefabAssigment;     // префаб по которому будет создана строка задания
    [SerializeField] private Transform _parentListAssigment;    // род. эл. где создавать строки заданий
    [Header("Установить все доступные игроку задания")]
    [SerializeField] private Assignment[] _listAllAssignments;  // абсолютно все задания со сцены

   // public static UnityEvent<int> OnChangeActivityLabel = new(); // int - поправка строк
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
       
    }

 

    public void AddQuest( Assignment assig)
    {
        AssigmentTxt assigmentTxt =  Instantiate(_prefabAssigment, _parentListAssigment);

    }

    public Assignment[] GetListAssignments() { return _listAllAssignments; }

    //Найти все доступные задания и пополнить ими список
    private void FindAvailableAssignmets()
    {
        foreach (Assignment assignment in _listAllAssignments)
        {
            AssigmentTxt assigmentTxt = Instantiate(_prefabAssigment, _parentListAssigment);
            
            assigmentTxt.SetDescriptionLabel(assignment.GetDescription());
            assigmentTxt.SetRewardTxt(assignment.GetRewartItem());


            assignment.OnStageComplete.AddListener(assigmentTxt.SetComplitingLabel);
            // assignment.CheckComplete();
            assignment.SetAssigmentTxt(assigmentTxt);
            assignment.InitTxtLabel();

            assigmentTxt.gameObject.SetActive(assignment.IsAvialable);


        }
        //пересчитает выполненность что отобразит ее в текстовых полях
        foreach (Assignment assignment in _listAllAssignments)
        {
            assignment.CheckComplete();
        }
        }

    public void LoadSavesQuests(PlayerStats playerStats)
    {
       
        foreach (var assignment in _listAllAssignments)
        {
            assignment.LoadQuestState(playerStats.QuestSaves);
        }
        foreach (var item in _listAllAssignments)
        {
            item.Initialize();
        }
        foreach (var item in _listAllAssignments)
        {
            item.CheckAvialable();
        }
        FindAvailableAssignmets(); // этот метод не долже срабатывать до инициализации заданий и прогрузки их доступности
       // MenuQuests.OnChangeActivityLabel.Invoke(0);
    }

    private void OnDestroy()
    {
       // OnChangeActivityLabel.RemoveAllListeners();
    }
}
