using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MagicTiles3
{
    public class SliderStarController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image starSlider;
        [SerializeField] private Sprite starActiveSprite;
        [SerializeField] private Sprite starDeActiveSprite;
        [SerializeField] private List<Image> starImageList = new List<Image>();

        #endregion

        #region Properties

        private bool isClear = false;

        #endregion

        #region LifeCycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void UpdateStarsSlider(int amount)
        {
            if (isClear)
                return;
            float amountPercentage = Mathf.Clamp(amount / 150f, 0f, 1f);
            starSlider.fillAmount = amountPercentage;

            if (amount >= 150)
            {
                isClear = true;
                ActivateStar(2);

                StartCoroutine(waitForEventWin(1f, () => { EventDispatcher.Instance.PostEvent(EventID.Win); }));
            }
            else if (amount >= 90)
            {
                ActivateStar(1);
            }
            else if (amount >= 50)
            {
                ActivateStar(0);
            }
        }

        private IEnumerator waitForEventWin(float dur, Action actionCompleted)
        {
            yield return ExtensionClass.GetWaitForSeconds(dur);
            actionCompleted?.Invoke();
        }

        private void ActivateStar(int index)
        {
            starImageList[index].sprite = starActiveSprite;
            starImageList[index].transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                starImageList[index].transform.DOShakeRotation(1f, new Vector3(0, 0, 30f), 5, 50f)
                    .SetEase(Ease.OutBack);
            });
        }

        public void ResetProgress()
        {
            starSlider.fillAmount = 0;
            foreach (var starImage in starImageList)
            {
                starImage.sprite = starDeActiveSprite;
                starImage.DOKill();
                starImage.transform.localScale = Vector3.one;
            }

            isClear = false;
        }

        #endregion
    }
}