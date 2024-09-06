using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrestigeCounter : MonoBehaviour
{
    private int _prestige; //общий счет престижа
    private int _maxPrestige;
    private int _prestigeOnlyForCars; // очки престижа за крутые машины
    private int _prestigeForQuests; //очки престижа за пройденный квесты
    public int Prestige
    {
        get { return _prestige; }
        set
        {
            _prestige = value;
            OnSetPrestige.Invoke(_prestigeForQuests, _prestigeOnlyForCars);
        }
    }
    //[SerializeField] private ParkingManager _parkingManager;

    public UnityEvent<int,int> OnSetPrestige; // изменены очки престижа (за квесты / за тачки на трассе)
   // public static UnityEvent OnUpdatePrestige = new(); // обновить очки престижа
    // Start is called before the first frame update

    private void Awake()
    {
       // OnUpdatePrestige.AddListener(UpdatePrestige);
    }
    void Start()
    {
        SetMaxPrestige(50);
    }
    public void AddPrestigeForQuest(int value)
    {
        if (value>0)
        {
            _prestigeForQuests += value;
            Prestige = _prestigeForQuests + _prestigeOnlyForCars;
            
        }
        
    }
    public void SetMaxPrestige(int count)
    {
        if (count > _maxPrestige)
        {
            _maxPrestige = count;
            OnSetPrestige.Invoke(_prestigeForQuests, _prestigeOnlyForCars);
        }            
    }

    public void UpdatePrestige(List<MergeCar> carOnStarted)
    {
        int prestige = 0;
        foreach (MergeCar car in carOnStarted)
        {
            prestige += car.MergeNumber;
        }

        _prestigeOnlyForCars = prestige;
        Prestige = _prestigeForQuests + _prestigeOnlyForCars;
        //OnSetPrestige.Invoke(_prestige, _maxPrestige);
    }

    public int GetPrestigeForQuest()
    {
        return _prestigeForQuests;
    }
    private void OnDestroy()
    {
     //   OnUpdatePrestige.RemoveAllListeners();
    }



}
