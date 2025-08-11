using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using System;
using System.Collections;
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
    [SerializeField] private bool initialCinematicEnded = false; // Cambiado de transitionEnded
    private bool isTransitioning = false;
    [SerializeField] private bool skipSplashScreen;
    [Header("Target Positions")]
    [SerializeField] private RectTransform buttonsShownPos;
    [SerializeField] private RectTransform buttonsHiddenPos;
    [SerializeField] private RectTransform logoShownPos;
    [SerializeField] private RectTransform logoHiddenPos;
    [SerializeField] private RectTransform windowsShownPos;
    [SerializeField] private RectTransform windowsHiddenPos;
    [Header("Music Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private bool isInitialCinematicActive = false; // Nueva variable para rastrear el estado
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
    public event Action OnWindowHide;
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
            StartCoroutine(SkipSplashScreenRoutine());
        }
        else
        {
            // Iniciamos la preparación del video
            StartCoroutine(PrepareAndPlayVideo());
        }

        HideButtons();
        HideLogo();
    }

    private IEnumerator PrepareAndPlayVideo()
    {
        isInitialCinematicActive = true;

        // Preparar el video antes de reproducir
        splashScreen.Prepare();

        // Esperar hasta que el video esté preparado
        while (!splashScreen.isPrepared)
        {
            yield return null;
        }

        // Reproducir video y audio simultáneamente
        splashScreen.Play();
        AudioManager.Instance.PlayMusic(0);
    }

    private IEnumerator SkipSplashScreenRoutine()
    {
        // Saltar la cinemática pero asegurar la carga
        splashScreen.Prepare();

        // Esperar preparación incluso al saltar
        while (!splashScreen.isPrepared)
        {
            yield return null;
        }

        // Forzar el final de la cinemática
        OnVideoStarted(splashScreen);
        OnVideoEnded(splashScreen);
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        Destroy(blackScreen);
    }
    private void OnVideoEnded(VideoPlayer vp)
    {
        isInitialCinematicActive = false; // La cinemática terminó naturalmente
        Transition(splashScreen.gameObject, true);
    }
    public void SkipOrGoToMenu(InputAction.CallbackContext context)
    {
        if (!context.performed || isTransitioning) return;

        // Saltar cinemática inicial si está activa
        if (isInitialCinematicActive)
        {
            SkipInitialCinematic();
            return;
        }

        // Comportamiento original si ya pasó la cinemática
        if (initialCinematicEnded)
        {
            initialCinematicEnded = false;
            Transition(startScreen, false);
        }
    }

    private void SkipInitialCinematic()
    {
        // Detener y limpiar elementos de la cinemática
        splashScreen.Stop();

        if (blackScreen != null)
        {
            Destroy(blackScreen);
        }

        isInitialCinematicActive = false; // Marcar como inactiva

        // Forzar la transición a menú principal
        Transition(splashScreen.gameObject, true);
    }

    public void Transition(GameObject objectToDestroy, bool isSplashScreen)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        Vector3 initialPosition = transition.transform.position;
        AudioManager.Instance.PlaySfx(2);

        transition.transform.DOMove(transitionTarget.transform.position, duration).SetEase(ease).OnComplete(() =>
        {
            Destroy(objectToDestroy);
            AudioManager.Instance.PlaySfx(1);

            transition.transform.DOMove(initialPosition, duration).SetEase(ease).OnComplete(() =>
            {
                if (isSplashScreen)
                {
                    initialCinematicEnded = true; // Actualizado a nuevo nombre
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

    public void HideLogo()
    {
        logo.DOAnchorPos(logoHiddenPos.anchoredPosition, duration).SetEase(ease);
    }
    public void ShowLogo()
    {
        logo.DOAnchorPos(logoShownPos.anchoredPosition, duration).SetEase(ease);
    }
    public void HideButtons()
    {
        buttons.DOAnchorPos(buttonsHiddenPos.anchoredPosition, duration).SetEase(ease);
    }
    public void ShowButtons()
    {
        buttons.DOAnchorPos(buttonsShownPos.anchoredPosition, duration).SetEase(ease);
    }
    public void ShowWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            HideButtons();
            HideLogo();
            windows[index].DOAnchorPos(windowsShownPos.anchoredPosition, duration).SetEase(ease);
            OnWindowShow?.Invoke(index);
        }
    }
    public void HideWindow(int index)
    {
        if (index >= 0 && index < windows.Length)
        {
            windows[index].DOAnchorPos(windowsHiddenPos.anchoredPosition, duration).SetEase(ease);
            OnWindowHide?.Invoke();
        }
    }
}