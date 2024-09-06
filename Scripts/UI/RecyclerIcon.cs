using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclerIcon : MonoBehaviour
{
    [SerializeField] private EnergyIndicator _energyIndicator;
    [SerializeField] private Image _iconIndicator;
    [SerializeField] private Gradient _gradientWorkload;
    [SerializeField] private TextMeshProUGUI _tmpLabel;
    [SerializeField] private TextMeshProUGUI _tmpWorkload;

    [SerializeField] private string _txtLabelNormal;
    [SerializeField] private string _txtLabelOverload;
    // Start is called before the first frame update
    void Start()
    {
        _energyIndicator.OnSetWorkload.AddListener(SetIndikator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetIndikator(float coef)
    {
        if (coef<0.9)
        {
            _tmpLabel.text = _txtLabelNormal;
        }
        else
        {
            _tmpLabel.text = _txtLabelOverload;
        }
        _tmpWorkload.text = $"" + (int)(coef * 100)+"%";
        _iconIndicator.fillAmount = coef;
        _iconIndicator.color = _gradientWorkload.Evaluate(coef);
    }
}
