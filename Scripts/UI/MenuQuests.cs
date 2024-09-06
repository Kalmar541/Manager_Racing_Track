using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuQuests : MonoBehaviour
{
    [SerializeField] private float _baseHeigth = 75f; // ������� ������ ����
    [SerializeField] private float _labelHeigth = 35f; // ������ ������� �������� �������
    [SerializeField] private RectTransform _windowQuests; // ���� �� ������� �������
    [SerializeField] private AssigmentTxt _prefabAssigment;     // ������ �� �������� ����� ������� ������ �������
    [SerializeField] private Transform _parentListAssigment;    // ���. ��. ��� ��������� ������ �������
    [Header("���������� ��� ��������� ������ �������")]
    [SerializeField] private Assignment[] _listAllAssignments;  // ��������� ��� ������� �� �����

   // public static UnityEvent<int> OnChangeActivityLabel = new(); // int - �������� �����
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

    //����� ��� ��������� ������� � ��������� ��� ������
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
        //����������� ������������� ��� ��������� �� � ��������� �����
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
        FindAvailableAssignmets(); // ���� ����� �� ����� ����������� �� ������������� ������� � ��������� �� �����������
       // MenuQuests.OnChangeActivityLabel.Invoke(0);
    }

    private void OnDestroy()
    {
       // OnChangeActivityLabel.RemoveAllListeners();
    }
}
