using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    Vector3 _mousePositions;

    [Header("���������� � ����������")]
    [Tooltip("� �������� ����� ���� ����� ����������� ������ � ������")]
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _distanceRay=150f;
    [SerializeField] private Camera _mainCamera;

    private bool _isLock; //������������� �����������?
    //public GameObject testSphere;

    private void Awake()
    {
        //�������������
        if (_mainCamera == null) _mainCamera = Camera.main;
        
    }
    private Vector3 GetDeltaMouseposObjpos3D()
    {
        Ray rayMouse = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayMouse, out RaycastHit hitInfo, _distanceRay, _targetLayer))
        {
          //  Instantiate(testSphere, hitInfo.point, Quaternion.identity); // ���� ������� ����
          //  Debug.DrawLine(hitInfo.point, transform.position, Color.red,300f);
            return hitInfo.point - transform.position;
        }
        else return Vector3.zero;  
    }

    public void SetLockMove(bool locks)
    {
        _isLock = locks;

    }
    private void OnMouseDown()
    {
        if (!_isLock)
        {
            _mousePositions = transform.position - GetDeltaMouseposObjpos3D();
        }
       
        
    }

    private Vector3 _normPosition; // ������� ����� ��������� �� ��� Y �� 0
    private void OnMouseDrag()
    {
       /* if (!_isLock)
        {
            Ray rayMouse = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouse, out RaycastHit hitInfo, _distanceRay, _targetLayer, QueryTriggerInteraction.Ignore))
            {
                _normPosition = hitInfo.point;
                //_normPosition.y = 0f;
                _normPosition.y = transform.position.y;
                transform.position = _normPosition;
            }
            
        }*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isLock&& eventData.button == PointerEventData.InputButton.Left)
        {
            Ray rayMouse = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouse, out RaycastHit hitInfo, _distanceRay, _targetLayer, QueryTriggerInteraction.Ignore))
            {
                _normPosition = hitInfo.point;
                //_normPosition.y = 0f;
                _normPosition.y = transform.position.y;
                transform.position = _normPosition;
            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      //  throw new System.NotImplementedException();
    }
}
