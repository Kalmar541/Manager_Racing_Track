using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AirdropBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _Parashut;
    [SerializeField] private Transform _pointBox;
    [SerializeField] private GameObject _prefabBox;
    [SerializeField] private float _delayOpening = 1f;
    [SerializeField] private float _durationFalling = 3f;
    [SerializeField] private GameObject _vfxDropFall;

    private Transform _pointMoveDrop;

    private bool _isActiveForToch;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        MoveToPointDrop();
        yield return new WaitForSeconds(_delayOpening);
        OpenParashut();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float _durationAnimOpening = 0.5f;
    private void OpenParashut()
    {
        
        _Parashut.DOScale(1f, _durationAnimOpening).SetEase(Ease.OutElastic);
    }

    public void Init(Transform pointDrop)
    {
        _Parashut.transform.localScale = Vector3.zero;
        _pointMoveDrop = pointDrop;
        _isActiveForToch = false;
    }

    //падать на точку сброса
    private void MoveToPointDrop( )
    {
        transform.DOMove(_pointMoveDrop.position, _durationFalling).SetEase(Ease.InSine).
            OnComplete(()=> { HideParashut(); });
        transform.DOLocalRotate(_pointMoveDrop.rotation.eulerAngles, _durationFalling);
    }
    private void HideParashut()
    {
        _Parashut.DOScale(0f, _durationAnimOpening).SetEase(Ease.Linear);
        _vfxDropFall.SetActive(true);
        _isActiveForToch = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left &&
            _isActiveForToch == true)
        {
            GameOverlayUI.OnOpenAirdropWindow.Invoke();
            DeletBox();
        }
       
    }

    public void DeletBox() 
    {
        Destroy(this.gameObject);
        _vfxDropFall.SetActive(true);
    }
}
