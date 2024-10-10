using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagicTiles3
{
    [CreateAssetMenu(fileName = "SongDataManager", menuName = "Data/SongDataManager")]
    public class SongDataManager : SingletonScriptableObject<SongDataManager>
    {
        #region Fields

        [SerializeField] private SongData _songDataDefault;
        [SerializeField] private List<SongData> _songDatas = new List<SongData>();

        #endregion

        #region Properties

        #endregion

        #region LifeCycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public SongData GetSongData(SoundTrackEnum soundTrackEnum)
        {
            SongData song = _songDatas.FirstOrDefault(s => s.soundTrackEnum == soundTrackEnum);

            if (song != null)
            {
                Debug.Log("Found song: " + song.soundTrackEnum);
                return song;
            }
            else
            {
                Debug.Log($"Song {soundTrackEnum} not found return default");
                return _songDataDefault;
            }
        }

        #endregion
    }
}