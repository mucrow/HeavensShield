using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Mtd {
  public class AudioManager: MonoBehaviour {
    [SerializeField] GameObject _musicSourcePrefab;
    [SerializeField] GameObject _soundEffectSourcePrefab;
    [SerializeField] int _numSoundEffectSources = 16;

    AudioSource _musicSource;
    AudioSource _jingleEffectSource;
    AudioSource[] _soundEffectSources;

    AudioClip _currentMusicClip;
    int _nextSoundEffectSourceIndex = 0;

    void Awake() {
      _musicSource = InitAudioSource(_musicSourcePrefab, "MusicSource");
      _jingleEffectSource = InitAudioSource(_soundEffectSourcePrefab, "JingleEffectSource");

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
     * A jingle is a short piece of music that temporarily interrupts the current background music.
     *
     * The background music fades back in shortly after the jingle finishes.
     */
    public Task PlayJingle(AudioClip jingleClip, bool resumeMusic) {
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
  }
}
