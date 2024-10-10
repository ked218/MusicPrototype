using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicTiles3
{
    public class EndPopupController : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            GameplayController.Instance.Setup(false);
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void RetryButtonOnClick()
        {
            GameplayController.Instance.OpenGameView();
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}