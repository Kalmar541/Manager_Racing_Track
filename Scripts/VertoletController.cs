using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VertoletController : MonoBehaviour
{
    [SerializeField] private Transform _lopastiMain;
    [SerializeField] private Transform _lopastiBack;
    [SerializeField] private float _speedRotate=0.2f;
    [SerializeField] private GameObject _boxGO;
    [SerializeField] private Transform _pointCreateBox;
    // Start is called before the first frame update
    void Start()
    {
        StartRotatingLopasti();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartRotatingLopasti()
    {
        _lopastiMain.DORotate(new(0f, 360f, 0f), _speedRotate, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear)
           .SetLoops(-1, LoopType.Restart);
        _lopastiBack.DORotate(new(0f, 360f, 0f), _speedRotate * 2f, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear)
           .SetLoops(-1, LoopType.Restart);
    }
    public void DropBox()
    {
        _boxGO.gameObject.SetActive(false);
    }
    public Transform GetPointBox()
    {
        return _pointCreateBox;
    }
}
