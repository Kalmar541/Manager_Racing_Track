using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
public class WindowResetWorkload : MonoBehaviour
{

    [Inject] private PlayerStats _playerStats;

    [SerializeField] private TextMeshProUGUI _tmpCountGoldResetWorkload;
    
    public UnityEvent OnSuccesAction;
    public UnityEvent OnFailAction;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        _tmpCountGoldResetWorkload.text = "x " + _playerStats.GoldForResetRecycler.ToString();
    }

    public void BUTTON_ResetWorkload()
    {
        CheckActivityButton();
    }

    private void CheckActivityButton()
    {
        if (_playerStats.Gold >= _playerStats.GoldForResetRecycler)
        {
            OnSuccesAction.Invoke();
        }
        else
        {
            OnFailAction.Invoke();
            

        }
    }
}
