using ArcadeVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCar : MonoBehaviour
{
    [SerializeField] Rigidbody Rigidbody;
    [SerializeField] WaypointProgressTracker way;

    private int _mergeNumber;
    public int MergeNumber { get { return _mergeNumber; }  set { _mergeNumber = value; } }
    public enum Location
    {
        atParking,
       startingLap,
       driving
    }

    public Location state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void ResetWay()
    {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        way.Reset();
        transform.position = Vector3.zero;
    }
}
