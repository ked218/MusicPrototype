using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MagicTiles3
{
    public class ScoreCalculate : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<RankScoreClass> rankingScores = new List<RankScoreClass>();

        [SerializeField] private Text scoreStateText;
        [SerializeField] private Text streakText;

        #endregion

        #region Properties

        [SerializeField] private int amount;

        #endregion

        #region LifeCycle

        private void Start()
        {
            amount = 0;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public int CompareToRank(float distance)
        {
            int rankingScoresLen = rankingScores.Count;

            for (int i = 0; i < rankingScores.Count; i++)
            {
                if ((int)distance <= rankingScores[i].distanceToGetPoint)
                {
                    if (rankingScores[i].isContinueStreak)
                        amount++;
                    else
                    {
                        amount = 0;
                    }

                    ShowStreakText();
                    ShowScoreStateText(i);
                    return rankingScores[i].points;
                }
            }

            amount = 0;
            ShowStreakText();
            return rankingScores[rankingScoresLen - 1].points;
        }


        private void ShowStreakText()
        {
            streakText.DOKill();
            streakText.transform.localScale = Vector3.zero;
            if (amount > 0)
            {
                streakText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
                streakText.text = $"x{amount}";
            }
        }

        private void ShowScoreStateText(int i)
        {
            scoreStateText.DOKill();
            scoreStateText.transform.localScale = Vector3.zero;
            scoreStateText.text = String.Empty;
            scoreStateText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
            scoreStateText.text = rankingScores[i].rankingScoresEnum.ToString();
        }

        public void ResetScore()
        {
            streakText.DOKill();
            streakText.transform.localScale = Vector3.zero;
            scoreStateText.DOKill();
            scoreStateText.transform.localScale = Vector3.zero;
        }

        #endregion
    }

    [System.Serializable]
    public class RankScoreClass
    {
        public RankingScoresEnum rankingScoresEnum;
        public int points;
        public bool isContinueStreak;
        public int distanceToGetPoint;
    }
}