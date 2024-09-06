using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ArcadeVP;
using System;

public class StartZone : MonoBehaviour, IEndDragHandler
{
    [Header("Установить в инспекторе")]
    [SerializeField] Transform _pointStart;
    [SerializeField] WaypointCircuit waypoint;

    private List<RaceCar> _carOnStarting;
    

    [SerializeField] private PrestigeCounter _prestigeCounter;

    [SerializeField] private List<MergeCar> _mergeCarsOnStarted; //список машин на круге

    public Action OnStartCar; //новая машина стартовала 

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log("Конц перетаскивания Zone");
        

    }
   /* public void StartingCarOnRoad(GameObject car)
    {
        car.transform.rotation = _pointStart.rotation;
        //car.transform.position = _pointStart.position;
         if (car.TryGetComponent(out ArcadeAiVehicleController carController))
         {
             // что сделать с контроллером тачки
         }
        if (car.TryGetComponent(out WaypointProgressTracker waypointCar))
        {
            waypointCar.circuit = waypoint;
        }

        OnStartCar?.Invoke();
    }*/

    public void StartingCarOnRoad(MergeCar car)
    {
        
        if (!_mergeCarsOnStarted.Contains(car))
        {
            _mergeCarsOnStarted.Add(car);
            RaceCar raceCar = car.GetRaceCar();
            raceCar.transform.position = car.transform.position;
            raceCar.gameObject.transform.rotation = _pointStart.rotation;
            if (car.gameObject.TryGetComponent(out WaypointProgressTracker waypointCar))
            {
                waypointCar.circuit = waypoint;
            }
            car.OnReturnFromStartZone.AddListener(DeletCarOnStartZone);
            _prestigeCounter.UpdatePrestige(_mergeCarsOnStarted);
        }
        OnStartCar?.Invoke();
    }

    public void DeletCarOnStartZone(MergeCar car)
    {
        if (_mergeCarsOnStarted.Contains(car)) 
        {
            _mergeCarsOnStarted.Remove(car);
            _prestigeCounter.UpdatePrestige(_mergeCarsOnStarted);
        }
    }
}
