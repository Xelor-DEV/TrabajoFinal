using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
public class GameManagerMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIManagerMenu uiManager;
    [SerializeField] private AudioManager audioManager;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SaveAudioSettings()
    {
        audioManager.AudioSettings.musicVolume = uiManager.MusicSlider.value;
        audioManager.AudioSettings.sfxVolume = uiManager.SfxSlider.value;
        audioManager.AudioSettings.masterVolume = uiManager.MasterSlider.value;
    }
    public void LoadAudioSettings()
    {
        uiManager.MusicSlider.value = audioManager.AudioSettings.musicVolume;
        uiManager.SfxSlider.value = audioManager.AudioSettings.sfxVolume;
        uiManager.MasterSlider.value = audioManager.AudioSettings.masterVolume;
        audioManager.SetVolumeOfMusic(uiManager.MusicSlider);
        audioManager.SetVolumeOfSfx(uiManager.SfxSlider);
        audioManager.SetVolumeOfMaster(uiManager.MasterSlider);
    }
}