using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MagicTiles3.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        #region Fields

        [SerializeField] private AudioSource audioSourceComp;
        [SerializeField] private AudioSource uIAudioSource;
        [SerializeField] private AudioClip uIOnClickAudioClip;

        #endregion

        #region Properties

        #endregion

        #region LifeCycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void PlaySoundOneShot(AudioClip _audioClip)
        {
            //audioSourceComp.PlayOneShot(_audioClip, 1);
           
            audioSourceComp.mute = false;
            audioSourceComp.clip = _audioClip;
            audioSourceComp.Play();
            
        }

        public void PlaySoundOneShot(AudioSource _audioSource, AudioClip _audioClip)
        {
            _audioSource.PlayOneShot(_audioClip, PlayerPrefsManager.SoundVolume);
        }

        public void PlayUIClick()
        {
            uIAudioSource.PlayOneShot(uIOnClickAudioClip, PlayerPrefsManager.SoundVolume);
        }

        public void SetSoundVolume(float _value)
        {
            PlayerPrefsManager.SoundVolume = _value;
        }

        #endregion
    }
}