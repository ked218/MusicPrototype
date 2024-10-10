using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicTiles3
{
    [CreateAssetMenu(fileName = "SongData", menuName = "Data/SongData")]
    public class SongData : ScriptableObject
    {
        #region Fields

        #endregion

        #region Prope
        
        public SoundTrackEnum soundTrackEnum;
        public int totalNote = 35, tempo =1200;
        public AudioClip file;

        #endregion

        #region LifeCycle

        #endregion

        #region Private Methods

        #endregion


        #region Public Methods

        #endregion
    }
}