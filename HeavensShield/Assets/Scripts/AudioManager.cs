using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Mtd {
  public class AudioManager: MonoBehaviour {
    [SerializeField] GameObject _musicSourcePrefab;
    [SerializeField] GameObject _jingleEffectSourcePrefab;
    [SerializeField] GameObject _soundEffectSourcePrefab;

    [SerializeField] AudioMixer _audioMixer;
    const string MusicVolumeParameterName = "MusicVolume";
    const string SoundEffectsVolumeParameterName = "SoundEffectsVolume";

    [SerializeField] int _numSoundEffectSources = 16;

    AudioSource _musicSource;
    AudioSource _jingleEffectSource;
    AudioSource[] _soundEffectSources;

    AudioClip _currentMusicClip;
    int _nextSoundEffectSourceIndex = 0;

    void Awake() {
      _musicSource = InitAudioSource(_musicSourcePrefab, "MusicSource");
      _jingleEffectSource = InitAudioSource(_jingleEffectSourcePrefab, "JingleEffectSource");

      _soundEffectSources = new AudioSource[_numSoundEffectSources];
      for (int i = 0; i < _numSoundEffectSources; ++i) {
        char code = (char)('A' + i);
        string name = "SoundEffectSource" + code;
        _soundEffectSources[i] = InitAudioSource(_soundEffectSourcePrefab, name);
      }
    }

    void Start() {
      var audioSettings = Globals.GameManager.SaveData.Settings.Audio;
      UpdateVolumesForGameStartup(audioSettings.MusicVolume, audioSettings.SoundEffectsVolume);
    }

    AudioSource InitAudioSource(GameObject prefab, string objectName) {
      var newObject = Instantiate(prefab, transform);
      newObject.name = objectName;
      return newObject.GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip musicClip) {
      if (musicClip == _currentMusicClip && _musicSource.isPlaying) {
        return;
      }
      _musicSource.Stop();
      _musicSource.clip = musicClip;
      _musicSource.volume = 1f;
      _musicSource.Play();
      _currentMusicClip = musicClip;
    }

    /**
     * A jingle is a short piece of music that temporarily interrupts the current background music.
     *
     * The background music fades back in shortly after the jingle finishes.
     */
    public Task PlayJingleEffect(AudioClip jingleClip, bool resumeMusic) {
      var tcs = new TaskCompletionSource<bool>();

      LeanTween.value(
        _musicSource.gameObject,
        volume => { _musicSource.volume = volume; },
        1f,
        0f,
        0.1f
      ).setOnComplete(() => {
        if (!resumeMusic) {
          _musicSource.Stop();
          _musicSource.volume = 1f;
        }
        _jingleEffectSource.PlayOneShot(jingleClip);
        StartCoroutine(JingleFinished(jingleClip.length + 0.25f, resumeMusic, tcs));
      }).setIgnoreTimeScale(true);

      return tcs.Task;
    }

    IEnumerator JingleFinished(float waitTime, bool resumeMusic, TaskCompletionSource<bool> tcs) {
      yield return new WaitForSecondsRealtime(waitTime);
      tcs.SetResult(true);
      if (resumeMusic) {
        LeanTween.value(
          _musicSource.gameObject,
          volume => { _musicSource.volume = volume; },
          0f,
          1f,
          1.5f
        ).setIgnoreTimeScale(true);
      }
    }

    /**
     * This function may ignore the caller's request to play a sound effect. This prevents too many
     * sounds from playing at the same time.
     */
    public void PlaySoundEffect(AudioClip soundEffectClip) {
      var source = _soundEffectSources[_nextSoundEffectSourceIndex];
      if (!source.isPlaying) {
        source.PlayOneShot(soundEffectClip);
      }
      _nextSoundEffectSourceIndex = (_nextSoundEffectSourceIndex + 1) % _soundEffectSources.Length;
    }

    public void LowerMusicVolumeSetting() {
      float currentVolume = Globals.GameManager.SaveData.Settings.Audio.MusicVolume;
      float newVolume = SanitizeVolume(currentVolume - 0.05f);
      UpdateMusicVolumeSetting(newVolume);
    }

    public void RaiseMusicVolumeSetting() {
      float currentVolume = Globals.GameManager.SaveData.Settings.Audio.MusicVolume;
      float newVolume = SanitizeVolume(currentVolume + 0.05f);
      UpdateMusicVolumeSetting(newVolume);
    }

    public void LowerSoundEffectsVolumeSetting() {
      float currentVolume = Globals.GameManager.SaveData.Settings.Audio.SoundEffectsVolume;
      float newVolume = SanitizeVolume(currentVolume - 0.05f);
      UpdateSoundEffectsVolumeSetting(newVolume);
    }

    public void RaiseSoundEffectsVolumeSetting() {
      float currentVolume = Globals.GameManager.SaveData.Settings.Audio.SoundEffectsVolume;
      float newVolume = SanitizeVolume(currentVolume + 0.05f);
      UpdateSoundEffectsVolumeSetting(newVolume);
    }

    float SanitizeVolume(float volume) {
      float clamped = Mathf.Clamp01(volume);
      // then round to the nearest 0.05f
      return Mathf.Round(clamped * 20f) / 20f;
    }

    public void UpdateMusicVolumeSetting(float volume) {
      SetMixerVolumeParameter(MusicVolumeParameterName, volume);
      Globals.GameManager.SaveData.Settings.Audio.MusicVolume = volume;
      Globals.GameManager.WriteSaveData();
      SettingsMenuManager.UpdateVolumeTextElements();
    }

    public void UpdateSoundEffectsVolumeSetting(float volume) {
      SetMixerVolumeParameter(SoundEffectsVolumeParameterName, volume);
      Globals.GameManager.SaveData.Settings.Audio.SoundEffectsVolume = volume;
      Globals.GameManager.WriteSaveData();
      SettingsMenuManager.UpdateVolumeTextElements();
    }

    public void UpdateVolumesForGameStartup(float musicVolume, float soundEffectsVolume) {
      SetMixerVolumeParameter(MusicVolumeParameterName, musicVolume);
      SetMixerVolumeParameter(SoundEffectsVolumeParameterName, soundEffectsVolume);
    }

    public void SetMixerVolumeParameter(string parameterName, float volume) {
      var db = Utils.Utils.MapRange(1f, 0f, 0f, -80f, volume, 2f);
      _audioMixer.SetFloat(parameterName, db);
    }
  }
}
