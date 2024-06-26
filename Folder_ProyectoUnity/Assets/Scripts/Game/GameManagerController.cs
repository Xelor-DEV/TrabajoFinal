using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManagerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIManagerMenu uiManagerMenu;
    [SerializeField] private UIManager uiManagerGame;
    [SerializeField] private AudioManager audioManager;
    public UnityEvent onStart;
    private void Start()
    {
        onStart?.Invoke();
        LoadAudioSettings();
    }
    public void LoadScene(string sceneName)
    {
        DOTween.KillAll();
        SaveAudioSettings();
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SaveAudioSettings()
    {
        if(uiManagerGame != null)
        {
            audioManager.AudioSettings.musicVolume = uiManagerGame.MusicSlider.value;
            audioManager.AudioSettings.sfxVolume = uiManagerGame.SfxSlider.value;
            audioManager.AudioSettings.masterVolume = uiManagerGame.MasterSlider.value;
        }
        else if(uiManagerMenu != null)
        {
            audioManager.AudioSettings.musicVolume = uiManagerMenu.MusicSlider.value;
            audioManager.AudioSettings.sfxVolume = uiManagerMenu.SfxSlider.value;
            audioManager.AudioSettings.masterVolume = uiManagerMenu.MasterSlider.value;
        }
        else
        {
            Debug.LogError("No existe un UI Manager, ambos son nulos");
        }
    }
    public void LoadAudioSettings()
    {
        if (uiManagerGame != null)
        {
            uiManagerGame.MusicSlider.value = audioManager.AudioSettings.musicVolume;
            uiManagerGame.SfxSlider.value = audioManager.AudioSettings.sfxVolume;
            uiManagerGame.MasterSlider.value = audioManager.AudioSettings.masterVolume;
            audioManager.SetVolumeOfMusic(uiManagerGame.MusicSlider);
            audioManager.SetVolumeOfSfx(uiManagerGame.SfxSlider);
            audioManager.SetVolumeOfMaster(uiManagerGame.MasterSlider);
        }
        else if (uiManagerMenu != null)
        {
            uiManagerMenu.MusicSlider.value = audioManager.AudioSettings.musicVolume;
            uiManagerMenu.SfxSlider.value = audioManager.AudioSettings.sfxVolume;
            uiManagerMenu.MasterSlider.value = audioManager.AudioSettings.masterVolume;
            audioManager.SetVolumeOfMusic(uiManagerMenu.MusicSlider);
            audioManager.SetVolumeOfSfx(uiManagerMenu.SfxSlider);
            audioManager.SetVolumeOfMaster(uiManagerMenu.MasterSlider);
        }
        else
        {
            Debug.LogError("No existe un UI Manager, ambos son nulos");
        }

    }

}
