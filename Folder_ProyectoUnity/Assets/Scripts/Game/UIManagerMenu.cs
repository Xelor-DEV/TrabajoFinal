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
    [Header("Transition Properties")]
    [SerializeField] private GameObject transition;
    [SerializeField] private GameObject transitionTarget;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private bool transitionEnded;
    [Header("Button and Windows Hide/Show Properties")]
    [SerializeField] private float hideShowOffsetX;
    [SerializeField] private float hideShowOffsetY;
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
    }
    private void OnDisable()
    {
        splashScreen.started -= OnVideoStarted;
        splashScreen.loopPointReached -= OnVideoEnded;
    }
    private void Start()
    {
        splashScreen.Play();
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
        if (context.performed == true && transitionEnded == true)
        {
            transitionEnded = false;
            Transition(startScreen, false);
        }
    }
    public void Transition(GameObject objectToDestroy, bool isSplashScreen)
    {
        if (isSplashScreen == true)
        {
            Vector3 initialPosition = transition.transform.position;
            AudioManager.Instance.PlaySfx(2);
            transition.transform.DOMove(transitionTarget.transform.position, duration).SetEase(ease).OnComplete(() =>
            {
                Destroy(objectToDestroy);
                AudioManager.Instance.PlaySfx(1);
                transition.transform.DOMove(initialPosition, duration).SetEase(ease).OnComplete(() =>
                {
                    transitionEnded = true;
                });
            });
        }
        else
        {
            Vector3 initialPosition = transition.transform.position;
            AudioManager.Instance.PlaySfx(2);
            transition.transform.DOMove(transitionTarget.transform.position, duration).SetEase(ease).OnComplete(() =>
            {

                Destroy(objectToDestroy);
                AudioManager.Instance.PlaySfx(1);
                transition.transform.DOMove(initialPosition, duration).SetEase(ease).OnComplete(() =>
                {
                    
                    ShowButtons();
                });
            });
        }
    }
    public void HideButtons()
    {
        buttons.DOAnchorPosX(buttons.anchoredPosition.x - hideShowOffsetX, duration).SetEase(ease);
    }
    public void ShowButtons()
    {
        buttons.DOAnchorPosX(buttons.anchoredPosition.x + hideShowOffsetX, duration).SetEase(ease);
    }
    public void ShowWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            HideButtons();
            windows[index].DOAnchorPosY(windows[index].anchoredPosition.y - hideShowOffsetY, duration).SetEase(ease);
            OnWindowShow?.Invoke(index);
        }
    }
    public void HideWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            ShowButtons();
            windows[index].DOAnchorPosY(windows[index].anchoredPosition.y + hideShowOffsetY, duration).SetEase(ease);
            OnWindowHide?.Invoke();
        }
    }
}
