using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Yun.Scripts.Core;
using Random = UnityEngine.Random;

namespace Yun.Scripts.Audios
{
    public partial class AudioManager : MonoSingleton<AudioManager>
    {
        private readonly List<AudioSource> _backgroundSource = new();
        private AudioSource _currentBackgroundSource;
        private readonly List<AudioSource> _audioSources = new();
        private SoundInfo _curBackgroundMusic = new();
        private float _currentMusicVolume;

        private void Start()
        {
            
        }

        private bool _isSoundOn;

        public bool IsSoundOn
        {
            get => _isSoundOn;
            set => _isSoundOn = value;
        }
        
        private bool _isMusicOn;

        public bool IsMusicOn
        {
            get => _isMusicOn;
            set
            {
                _isMusicOn = value;
                if (!_currentBackgroundSource) return;
                if (value)
                    _currentBackgroundSource.Play();
                else
                    _currentBackgroundSource.Pause();
            } 
        }

        public void PlayBackgroundMusic(SoundInfo sound = null)
        {
            if (sound != null)
                _curBackgroundMusic = sound;
            else
            {
                sound = _curBackgroundMusic;
            }

            if (_currentBackgroundSource != null)
                _currentBackgroundSource.Stop();

            _currentBackgroundSource = GetBgAudioSource();
            _currentBackgroundSource.clip = sound.clip;
            if (IsMusicOn)
            {
                _currentBackgroundSource.volume = sound.volume;
                _currentMusicVolume = sound.volume;
                _currentBackgroundSource.Play();
            }
            else
            {
                // _currentBackgroundSource.volume = 0;
            }
        }

        private AudioSource GetBgAudioSource()
        {
            foreach (var t in _backgroundSource.Where(t => !t.isPlaying))
            {
                return t;
            }

            var source = new GameObject("bgSound").AddComponent<AudioSource>();
            source.transform.SetParent(transform);
            _backgroundSource.Add(source);
            source.loop = true;
            return source;
        }

        public void StopBackgroundMusic()
        {
            foreach (var source in _backgroundSource)
            {
                source.DOFade(0, 0.5f).OnComplete(source.Stop);
            }
        }

        public void PauseBackgroundMusic(bool isPause)
        {
            if (isPause)
            {
                foreach (var source in _backgroundSource)
                {
                    source.DOFade(0, 0.5f).OnComplete(source.Pause);
                }
            }
            else
            {
                foreach (var source in _backgroundSource)
                {
                    source.UnPause();
                    source.DOFade(_currentMusicVolume, 0.5f);
                }
            }
        }

        public void StopSoundEffect(SoundInfo t)
        {
            foreach (var audioSource in _audioSources.Where(audioSource => audioSource.clip == t.clip))
            {
                audioSource.DOFade(0, 0.5f).OnComplete(audioSource.Stop);
            }
        }

        public void StopSoundEffectWithFading(SoundInfo t)
        {
            foreach (var audioSource in _audioSources.Where(audioSource => audioSource.clip == t.clip))
            {
                audioSource.DOFade(0, 0.5f).OnComplete(audioSource.Stop);
            }
        }

        public void PlaySoundEffect(SoundInfo sound, bool isLoop = false)
        {
            if (!IsSoundOn)
                return;

            var audioSource = GetAudioSource();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.loop = isLoop;
#if UNITY_EDITOR
            if (sound.clip != null)
                audioSource.gameObject.name = sound.clip.name;
#endif
            audioSource.Play();
        }

        public void PlaySoundEffect(List<SoundInfo> sounds, bool isLoop = false)
        {
            if (!IsSoundOn)
                return;

            var random = Random.Range(0, sounds.Count);

            var audioSource = GetAudioSource();
            audioSource.clip = sounds[random].clip;
            audioSource.volume = sounds[random].volume;
            audioSource.loop = isLoop;
#if UNITY_EDITOR
            if (sounds[random].clip != null)
                audioSource.gameObject.name = sounds[random].clip.name;
#endif
            audioSource.Play();
        }

        private AudioSource GetAudioSource()
        {
            if (_audioSources.Count > 0)
                foreach (var t in _audioSources.Where(t => !t.isPlaying))
                    return t;

            var s = GetNewAudioSource();
            _audioSources.Add(s);
            return s;
        }

        private AudioSource GetNewAudioSource()
        {
            var obj = new GameObject();
            obj.transform.SetParent(transform);
            return obj.AddComponent<AudioSource>();
        }

        public bool IsPlayingAudio(SoundInfo source)
        {
            return _audioSources.Count != 0 && _audioSources.Any(t => t.clip == source.clip && t.isPlaying);
        }
    }

    [Serializable]
    public class SoundInfo
    {
        public AudioClip clip;
        public float volume = 1;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Yun.Scripts.Audios.AudioManager))]
    public class SoundControllerEditor :
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.Editor.OdinEditor
#else
Editor
#endif
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Update References"))
            {
                if (target is Yun.Scripts.Audios.AudioManager audioManager)
                {
                    var listSoundInfos = audioManager.GetType().GetFields().ToList()
                        .FindAll(s => s.FieldType == typeof(SoundInfo));

                    var assets = GetAllAssets<AudioClip>();
                    foreach (var soundInfo in listSoundInfos)
                    {
                        var clip = assets.Find(s =>
                            s.name.ToLower().Replace(" ", "") == soundInfo.Name.ToLower().Replace(" ", ""));
                        if (clip == null) continue;
                        var clipField = soundInfo.FieldType.GetField("clip");
                        clipField.SetValue(soundInfo.GetValue(audioManager), clip);
                        EditorUtility.SetDirty(audioManager);
                    }
                }
            }

            base.OnInspectorGUI();
        }

        private static List<T> GetAllAssets<T>() where T : class
        {
            string[] paths = { "Assets/Yun/Audios/WarBase" };
            var assets = AssetDatabase.FindAssets(null, paths);
            var assetsObj = assets.Select(s =>
                    AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(s))).ToList()
                .FindAll(s => s.GetType() == typeof(T));

            return assetsObj.Select(o => o as T).ToList();
        }
    }
#endif
}