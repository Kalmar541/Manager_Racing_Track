using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using YG;
using UnityEngine.UI;

public class ShopResources : MonoBehaviour
{
    [Inject] PlayerStats _playerStats;

    public float PriceExchangeGoldToMoney = 100f;
    [Header("Текстовые поля")]
    [SerializeField] private TextMeshProUGUI _tmpCountMoney; // число денег обмениваемых на золото
    [SerializeField] private TextMeshProUGUI _tmpCountGoldForResetRecycler; // число золото для сброса переработчика

    [SerializeField] private TextMeshProUGUI _tmpMaxOfferGold; //3 предложения обмена янов на золото
    [SerializeField] private TextMeshProUGUI _tmpNormOfferGold;
    [SerializeField] private TextMeshProUGUI _tmpMinOfferGold;

    [SerializeField] private TextMeshProUGUI _tmpMaxOfferYan;
    [SerializeField] private TextMeshProUGUI _tmpNormOfferYan;
    [SerializeField] private TextMeshProUGUI _tmpMinOfferYan;

    [Header("Иконки")]
    [SerializeField] private Image _imageMaxOffer;
    [SerializeField] private Image _imageNormOffer;
    [SerializeField] private Image _imageMinOffer;
    [SerializeField] private Image _imageYan1;
    [SerializeField] private Image _imageYan2;
    [SerializeField] private Image _imageYan3;



    


    void Start()
    {
        SetTextLabel();
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextLabel()
    {
        //обмен золота на деньги
        _tmpCountMoney.text = "x" + (int)PriceExchangeGoldToMoney;

        // обмен янов на золото

        _tmpMaxOfferYan.text= YandexGame.purchases[2].price;
        _tmpNormOfferYan.text= YandexGame.purchases[1].price;
        _tmpMinOfferYan.text= YandexGame.purchases[0].price;



        //переработчик
        _tmpCountGoldForResetRecycler.text ="x "+ _playerStats.GoldForResetRecycler.ToString();

    }


    public void OnStart()
    {
        StartCoroutine(setImage());       
    }
   
    private Sprite _yanSprite;
    private Sprite _offer1Sprite;
    private Sprite _offer2Sprite;
    private Sprite _offer3Sprite;
      
    IEnumerator setImage()
    {
        //_yanTexture2D = null;
        //Иконка YAN
        WWW www = new WWW(YandexGame.purchases[0].currencyImageURL);
        yield return www;
        Texture2D _yanTexture2D = www.texture;
        _yanSprite = Sprite.Create(_yanTexture2D, new Rect(0, 0, _yanTexture2D.width, _yanTexture2D.height), new Vector2(0.5f, 0.5f));

        _imageYan1.sprite = _yanSprite;
        _imageYan2.sprite = _yanSprite;
        _imageYan3.sprite = _yanSprite;

        //3 иконки предметов из магазина
        www = new WWW(YandexGame.purchases[0].imageURI);
        yield return www;
        Texture2D _offer12D = www.texture;
        _offer1Sprite = Sprite.Create(_offer12D, new Rect(0, 0, _offer12D.width, _offer12D.height), new Vector2(0.5f, 0.5f));
        _imageMinOffer.sprite = _offer1Sprite;

        www = new WWW(YandexGame.purchases[1].imageURI);
        yield return www;
        Texture2D _offer22D = www.texture;
        _offer2Sprite = Sprite.Create(_offer22D, new Rect(0, 0, _offer22D.width, _offer22D.height), new Vector2(0.5f, 0.5f));
        _imageNormOffer.sprite = _offer2Sprite;

        www = new WWW(YandexGame.purchases[2].imageURI);
        yield return www;
        Texture2D _offer32D = www.texture;
        _offer3Sprite = Sprite.Create(_offer32D, new Rect(0, 0, _offer32D.width, _offer32D.height), new Vector2(0.5f, 0.5f));
        _imageMaxOffer.sprite = _offer3Sprite;

    }

    void SuccessPurchased(string id)
    {
        // Ваш код для обработки покупки. Например:
        if (id == "offer1")
            _playerStats.AddGold(100);
        else if (id == "offer2")
            _playerStats.AddGold(350);
        else if (id == "offer3")
            _playerStats.AddGold(1500);
    }

    public void BUTTON_PayOffer(string idPayment)
    {
        YandexGame.BuyPayments(idPayment);
       
    }


    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
        //YandexGame.PurchaseFailedEvent += FailedPurchased; // Необязательно
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
        //YandexGame.PurchaseFailedEvent -= FailedPurchased; // Необязательно
    }
}
