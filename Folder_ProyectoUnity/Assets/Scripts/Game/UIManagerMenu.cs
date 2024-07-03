using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using System;
public class UIManagerMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer splashScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private RectTransform buttons;
    [SerializeField] private RectTransform[] windows;
    [SerializeField] private RectTransform logo;
    [Header("Transition Properties")]
    [SerializeField] private GameObject transition;
    [SerializeField] private GameObject transitionTarget;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private bool transitionEnded = false;
    private bool isTransitioning = false;
    [SerializeField] private bool skipSplashScreen;
    [Header("Buttons, Windows and Logo Hide/Show Properties")]
    [SerializeField] private float offsetX_Button;
    [SerializeField] private float offsetY_Windows;
    [SerializeField] private float offsetY_Logo;
    [Header("Music Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public Slider MasterSlider
    {
        get
        {
            return masterSlider;
        }
    }
    public Slider MusicSlider
    {
        get
        {
            return musicSlider;
        }
    }
    public Slider SfxSlider
    {
        get
        {
            return sfxSlider;
        }
    }
    public event Action<int> OnWindowShow;
    public  event Action OnWindowHide;
    private void OnEnable()
    {
        splashScreen.started += OnVideoStarted;
        splashScreen.loopPointReached += OnVideoEnded;
        OnWindowHide += ShowLogo;
        OnWindowHide += ShowButtons;
    }
    private void OnDisable()
    {
        splashScreen.started -= OnVideoStarted;
        splashScreen.loopPointReached -= OnVideoEnded;
        OnWindowHide -= ShowLogo;
        OnWindowHide -= ShowButtons;
    }
    private void Start()
    {
        Time.timeScale = 1;
        if (skipSplashScreen == true)
        {
            OnVideoStarted(splashScreen);
            OnVideoEnded(splashScreen);
        }
        else
        {
            splashScreen.Play();
        }
    }
    private void OnVideoStarted(VideoPlayer vp)
    {
        Destroy(blackScreen);
        AudioManager.Instance.PlayMusic(0);
    }
    private void OnVideoEnded(VideoPlayer vp)
    {
        Transition(splashScreen.gameObject, true);
    }
    public void GoMenu(InputAction.CallbackContext context)
    {
        if (context.performed == true && isTransitioning == false && transitionEnded == true)
        {
            transitionEnded = false;
            Transition(startScreen, false);
        }
    }
    public void Transition(GameObject objectToDestroy, bool isSplashScreen)
    {
        if (isTransitioning == false) 
        {
            isTransitioning = true;
            Vector3 initialPosition = transition.transform.position;
            AudioManager.Instance.PlaySfx(2);
            transition.transform.DOMove(transitionTarget.transform.position, duration).SetEase(ease).OnComplete(() =>
            {
                Destroy(objectToDestroy);
                AudioManager.Instance.PlaySfx(1);

                transition.transform.DOMove(initialPosition, duration).SetEase(ease).OnComplete(() =>
                {
                    if (isSplashScreen == true)
                    {
                        transitionEnded = true;
                    }
                    else
                    {
                        ShowButtons();
                        ShowLogo();
                    }
                    isTransitioning = false;
                });
            });
        }
    }
    public void HideLogo()
    {
        logo.DOAnchorPosY(logo.anchoredPosition.y + offsetY_Logo, duration).SetEase(ease);
    }
    public void ShowLogo()
    {
        logo.DOAnchorPosY(logo.anchoredPosition.y - offsetY_Logo, duration).SetEase(ease);
    }
    public void HideButtons()
    {
        buttons.DOAnchorPosX(buttons.anchoredPosition.x - offsetX_Button, duration).SetEase(ease);
    }
    public void ShowButtons()
    {
        buttons.DOAnchorPosX(buttons.anchoredPosition.x + offsetX_Button, duration).SetEase(ease);
    }
    public void ShowWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            HideButtons();
            HideLogo();
            windows[index].DOAnchorPosY(windows[index].anchoredPosition.y - offsetY_Windows, duration).SetEase(ease);
            OnWindowShow?.Invoke(index);
        }
    }
    public void HideWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            windows[index].DOAnchorPosY(windows[index].anchoredPosition.y + offsetY_Windows, duration).SetEase(ease);
            OnWindowHide?.Invoke();
        }
    }
}
