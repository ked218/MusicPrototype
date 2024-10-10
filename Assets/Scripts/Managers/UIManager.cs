using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MagicTiles3.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        #region Fields

        [SerializeField] private GameObject endPopup;

        #endregion

        #region Properties

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.Lose, EndEventListener);
            EventDispatcher.Instance.RegisterListener(EventID.Win, EndEventListener);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.Lose, EndEventListener);
            EventDispatcher.Instance.RemoveListener(EventID.Win, EndEventListener);
        }

        #endregion

        #region Private Methods

        private void EndEventListener(object param = null)
        {
            endPopup.gameObject.SetActive(true);
        }

        #endregion

        #region Public Methods

        #endregion
    }
}