using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowingBuilding : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _openState;
    [SerializeField] private GameObject _closeState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }
    private void Show()
    {
        _openState.SetActive(true);
        _closeState.SetActive(false);
    }
    private void Hide()
    {
        _openState.SetActive(false);
        _closeState.SetActive(true);
    }
}
