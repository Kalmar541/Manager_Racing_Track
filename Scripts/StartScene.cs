using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class StartScene : MonoBehaviour
{
    [Header("Закрытое письмо")]
    [SerializeField] private Transform _parentLetter;
    [SerializeField] private Transform _startPosLetter;
    [SerializeField] private Transform _endPosLetter;
    [SerializeField] private float _forceJump = 50f;
    [SerializeField] private int _countJump = 1;
    [SerializeField] private float _jumpDuration = 2f;

    [SerializeField] private Transform _closeLetter;
    [SerializeField] private Transform _OpenLetterFullImage;
    [SerializeField] private Transform _OpenLetterPartImage;
    [SerializeField] private Button _openLetterBnt;

    [Header("Перед открытием")]
    [SerializeField] private float _shakeDuration = 1f;
    [SerializeField] private float _shakeStrength = 10f;

    [Header("Открытие")]
    [SerializeField] private Transform _paperParent;
    [SerializeField] private Transform _pointStartPaper;
    [SerializeField] private Transform _pointEndPaper;
    [SerializeField] private float _durationMovePaper =1f;
    [SerializeField] private Button _continueBnt;
    [SerializeField] private Button _startBnt;

    [Header("Анимация затенения")]
    [SerializeField] private Image _fadeBlackFon;
    [SerializeField] private float _timeFade;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _enterColor;
    [SerializeField] private Image _closeLetterImg;
    [SerializeField] private Image _barLoadingScene;
    [SerializeField] private Image _fonBarLoadingScene;
    private void Awake()
    {
        _startBnt.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (YandexGame.savesData.CompleteStartScene)
        {
            _parentLetter.gameObject.SetActive(false);
            StartCoroutine(LoadAsyncScene());
            _fadeBlackFon.enabled = true;
            _fadeBlackFon.DOFade(0f, 0.5f);
        }
        else
        {
            ShadowAnim();
            yield return new WaitForSeconds(3f);
            AnimStartLetter();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BUTTON_OpenLetter()
    {
        StartCoroutine(OpenLetter());
    }

    private void ShadowAnim()
    {
        _fadeBlackFon.enabled = true;
        _fadeBlackFon.DOFade(0f, _timeFade);

        //подготовка
        _closeLetter.gameObject.SetActive(true);
        _OpenLetterFullImage.gameObject.SetActive(false);
        _OpenLetterPartImage.gameObject.SetActive(false);
        _openLetterBnt.enabled = false;
        _parentLetter.position = _startPosLetter.position;
        _parentLetter.localScale = Vector3.zero;
        _closeLetterImg.raycastTarget = false;
        _barLoadingScene.fillAmount = 0f;
        _fonBarLoadingScene.gameObject.SetActive(false);
    }
    public void BUTTON_StartGame()
    {

        ShadowAnimClose();
    }
    private void AnimStartLetter()
    {

        _parentLetter.DOScale(1, _jumpDuration);
        _parentLetter.DOJump(_endPosLetter.position, _forceJump, _countJump, _jumpDuration).
            OnComplete(()=> {
                _openLetterBnt.enabled = true;
                _closeLetterImg.raycastTarget = true;
            });
    }
    private IEnumerator OpenLetter()
    {
        _paperParent.position = _pointStartPaper.position;
      
        _parentLetter.transform.DOShakePosition(_shakeDuration, _shakeStrength);
        yield return new WaitForSeconds(_shakeDuration * 0.5f);

        _closeLetter.gameObject.SetActive(false);
        _OpenLetterFullImage.gameObject.SetActive(true);
        _OpenLetterPartImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        _parentLetter.DOScale(1.5f, 1f);
        _paperParent.DOMove(_pointEndPaper.position, _durationMovePaper).
       OnComplete(() =>
       {
           _startBnt.gameObject.SetActive(true);
          
       });
    }

    public void SetNormalColor()
    {
        _closeLetterImg.color = _normalColor;
        _parentLetter.DOScale(1f, 0.5f);
    }
    public void SetEnteredColor()
    {
        _closeLetterImg.color = _enterColor;
        _parentLetter.DOScale(1.1f, 0.5f);
    }

    public void ShadowAnimClose()
    {
        _startBnt.enabled = false;
        _fadeBlackFon.enabled = true;
        LoadScaneRace();
        _fonBarLoadingScene.gameObject.SetActive(true);
        _fadeBlackFon.DOFade(1f, 0.5f).OnComplete(()=> {
           
            ScreenIsBlack = true;
        });
    }
    
    public void LoadScaneRace()
    {
        YandexGame.savesData.CompleteStartScene = true;
        YandexGame.SaveProgress();
        //SceneManager.LoadScene("RaceTrackScane");
        StartCoroutine(LoadAsyncScene());
        
    }

    private bool ScreenIsBlack = false;
    IEnumerator LoadAsyncScene()
    {
        float progress;
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync("RaceTrackScane");

         while (!LoadAsync.isDone&& !ScreenIsBlack)
         {
              progress = LoadAsync.progress;
             _barLoadingScene.fillAmount = progress;
             yield return null;
         }
        
    }
}
