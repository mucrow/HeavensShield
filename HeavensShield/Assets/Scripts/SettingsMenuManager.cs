using UnityEngine;

namespace Mtd {
  public class SettingsMenuManager: MonoBehaviour {
    public void OnUIReady() {
      UpdateVolumeTextElements();
    }

    public static void UpdateVolumeTextElements() {
      var audioSettings = Globals.GameManager.SaveData.Settings.Audio;
      float musicVolume = audioSettings.MusicVolume;
      float soundEffectsVolume = audioSettings.SoundEffectsVolume;
      Globals.UI.SettingsMenuMusicVolumeText.text = FormatVolumeText(musicVolume);
      Globals.UI.SettingsMenuSoundEffectsVolumeText.text = FormatVolumeText(soundEffectsVolume);
    }

    static string FormatVolumeText(float value) {
      return Mathf.Floor(value * 100f) + "%";
    }
  }
}
