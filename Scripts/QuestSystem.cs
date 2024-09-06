using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using DG.Tweening;

public class QuestSystem : MonoBehaviour
{
    [Inject] private DiContainer diContainer;
    [Inject] private GameOverlayUI _uiGame;
    [Inject] private PlayerStats _playerStats;

    [Header("Установить в инспекторе")]
    [SerializeField] private GameObject _prefabStar; 
    //Управление заданиями
    [SerializeField] private List<Quest> _questsList;
  //  [SerializeField] private ScrObjQuestMap _dataQuestMap;
   // [SerializeField] private MenuQuests _menuQuests;

    public static UnityEvent<int> OnNewRecordMerge = new();
    private int _numMaxMerge = 0; // максимально достигнутое число мерджа
    
    void Start()
    {
        //_dataQuestMap.LoadAssignment( _menuQuests.GetListAssignments());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Проверить ни получили ли мы первый раз новую тачку
    /// </summary>
    /// <param name="num"></param>
   /* private void CheckOnNewCarMerge( int num, Vector3 startPosCar)
    {
        if (num > _numMaxMerge)
        {
            _numMaxMerge = num;
            SaveNewRecordMerge();
        }
    }*/

    //private int rewardStars = 3;
    private void SaveNewRecordMerge()
    {
        _playerStats.SaveNumMerge(_numMaxMerge);
       // Debug.Log("Новый мердж " + _numMaxMerge);
    }
    public void LoadRecordMerge(int num)
    {
        if (num>=0)
        {
            _numMaxMerge = num;
        }
        else { Debug.LogWarning("Ошибка: Нельзя установить рекорд мерджа:" + num); }

    }

    private int _countStars = 3;
    public bool CheckRecordMerge(int mergeNum, Vector3 startPosCar)
    {
        if (mergeNum<= _numMaxMerge)
        {
            return false;
        }
        else
        {
            _numMaxMerge = mergeNum;
            OnNewRecordMerge.Invoke(mergeNum);
            SaveNewRecordMerge();
            CreateStarsRewardEffect(_countStars, startPosCar);
            return true;
        }
    }

   /* public float jumpPower = 10f;
    public Ease ease = Ease.InCubic;
    public float time = 1f;*/

    /// На точке создаются звезды и отбрасываются в разные стороны
    public float forceExplode=10f; // сила броска звезды
    private float _randomizePos = 10f; //разброс взятия точки
    [SerializeField] private float _heigthPos = 10f; // высота точки( регулирует наклон броска звезды)
    private void CreateStarsRewardEffect(int count, Vector3 startPos)
    {
        for (int i = 0; i < count; i++)
        {
            Rigidbody starRb = CreateStars(startPos).GetComponent<Rigidbody>();
            Vector3 offsetExplode = new(RandomPos(), _heigthPos, RandomPos());
            offsetExplode.Normalize();
            starRb.AddForce(offsetExplode * forceExplode, ForceMode.Impulse);
        }

        float RandomPos()
        {
            return (Random.Range(-_randomizePos, _randomizePos));
        }

        GameObject CreateStars(Vector3 startPos)
        {
            return diContainer.InstantiatePrefab(_prefabStar, startPos, Quaternion.identity, null);
        }
    }

    public void CreateStars(int num)
    {
       

    }


    private void OnDestroy()
    {
        OnNewRecordMerge.RemoveAllListeners();
    }
}
