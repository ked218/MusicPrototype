using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicTiles3.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : Singleton<MusicManager>
    {
        #region Fields

        #endregion

        #region Properties

        [SerializeField] private AudioSource _audioSourceComp;

        #endregion

        #region LifeCycle
        

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void PlayMusic(AudioClip _audioClip)
        {
            _audioSourceComp.mute = false;
            _audioSourceComp.clip = _audioClip;
            _audioSourceComp.Play();
        }

        public void PlayMusic()
        {
            _audioSourceComp.mute = false;
            _audioSourceComp.Play();
        }

        public void MuteMusic()
        {
            _audioSourceComp.mute = true;
        }

        public void SetMusicVolume(float _value)
        {
            PlayerPrefsManager.MusicVolume = _value;
            _audioSourceComp.volume = _value;
        }

        public void Stop()
        {
            _audioSourceComp.Stop();
        }

        public void PlayLevelMusic()
        {
            _audioSourceComp.clip =
                GameplayController.Instance.activeSong.file;
            _audioSourceComp.Stop();
            _audioSourceComp.Play();
        }

        #endregion
    }
}