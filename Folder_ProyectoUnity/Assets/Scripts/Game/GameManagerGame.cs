using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerGame : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;
    private void Start()
    {
        audioManager.PlayMusic(0);
    }
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
