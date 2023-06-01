using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mtd {
  public class AudioManager: MonoBehaviour {
    [SerializeField] GameObject _musicSourcePrefab;
    [SerializeField] GameObject _soundEffectSourcePrefab;
    [SerializeField] int _numSoundEffectSources = 16;

    AudioSource _musicSource;
    AudioSource[] _soundEffectSources;

    AudioClip _currentMusicClip;

    void Awake() {
      _musicSource = InitAudioSource(_musicSourcePrefab, "MusicSource");

      _soundEffectSources = new AudioSource[_numSoundEffectSources];
      for (int i = 0; i < _numSoundEffectSources; ++i) {
        char code = (char)('A' + i);
        string name = "SoundEffectSource" + code;
        _soundEffectSources[i] = InitAudioSource(_soundEffectSourcePrefab, name);
      }
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
      _musicSource.Play();
      _currentMusicClip = musicClip;
    }

    /**
     * This function may ignore the caller's request to play a sound effect. This prevents too many
     * sounds from playing at the same time.
     */
    public void PlaySoundEffect(AudioClip soundEffectClip) {
      foreach (var soundEffectSource in _soundEffectSources) {
        if (!soundEffectSource.isPlaying) {
          soundEffectSource.PlayOneShot(soundEffectClip);
          break;
        }
      }
    }
  }
}
