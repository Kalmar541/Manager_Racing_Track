using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCar : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _Vfx;
    private MergeCar _carIn;
    // Start is called before the first frame update
    
    public void SetCarIn(MergeCar car)
    {
        _carIn = car;
    }
    public void CLICK_OBJECT()
    {
        ClickObj();
    }

    private void ClickObj()
    {
        _model.SetActive(false);
        _Vfx.SetActive(true);
        _Vfx.transform.parent = null;
        _carIn.gameObject.SetActive(true);
        Destroy(this, 5f);
    }
}
