using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MagicTiles3.Managers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MagicTiles3
{
    public class GameplayController : Singleton<GameplayController>
    {
        #region Fields

        #endregion

        #region Properties

        public GameObject nodePrefab, boundary;
        public Transform nodeHook;
        public Transform transContinue;
        public Transform transBestScore;

        RectTransform rectTrans;

        public Text txScore;
        [HideInInspector] public float speed = 0f;
        public float intervalForBg = 10f;
        [HideInInspector] public bool isDead = false;
        [HideInInspector] public bool isStared, isTap = true;

        float nodeWidth = 200f, nodeHeight = 300f;

        float screenHeight, screenWidth, devidedValue;

        Node lastNode, deadNode;
        public int score;
        int noteIndex, nodeIndex;
        int songIndex;

        Transform nodeHolder, boundaryHolder;

        public SongData activeSong;
        public List<Node> actvieNodes = new List<Node>();
        public Image background;
        public List<Sprite> backgroundImgList;

        int mixedNodeCount;
        int songId;
        float lastMixedNodePosY = 0;

        IEnumerator ienChangeBg;
        int bgCount;

        public RectTransform scoreLineRectTransform;
        [SerializeField] private ScoreCalculate _scoreCalculate;
        [SerializeField] private SliderStarController sliderStarController;

        [SerializeField] private Image decorateImage;

        #endregion

        #region LifeCycle

        void Start()
        {
            rectTrans = GetComponent<RectTransform>();
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            devidedValue = screenWidth / 4;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public float getSpeed()
        {
            return speed;
        }

        public void Setup(bool isShow)
        {
            if (rectTrans == null)
                rectTrans = GetComponent<RectTransform>();

            if (!isShow)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                rectTrans.localPosition = Vector3.zero;
                transContinue.gameObject.SetActive(false);
            }

            CancelInvoke("ChangeBackground");
        }

        public void SetSong()
        {
            Reset();
            score = 0;
            ScoreUpdate(score);

            Generate();
            MakeBoundary();
        }

        public void OnTap()
        {
            if (!isStared || isDead || !isTap) return;

            float clickPosX = Input.mousePosition.x;
            float clickPosY = Input.mousePosition.y;


            for (int i = 0; i < actvieNodes.Count; i++)
            {
                if (actvieNodes[i].transform.localPosition.y < clickPosY &&
                    clickPosY < (actvieNodes[i].height + actvieNodes[i].transform.localPosition.y))
                {
                    deadNode = actvieNodes[i];
                    int index = (int)(clickPosX / devidedValue);
                    Vector3 deadPos = new Vector3(index * nodeWidth, actvieNodes[i].transform.localPosition.y, 0);
                    speed = 0;

                    Node dn = DeadNodeGenerate(deadPos);
                    StartCoroutine(IDeadNode(dn, true));
                    return;
                }
            }
        }

        public void Dead(Node deadNode)
        {
            this.deadNode = deadNode;
            StartCoroutine(IDeadNode(deadNode, false));
        }

        public void OnLeave(Node node, bool isSucceed)
        {
            if (!isSucceed)
            {
                deadNode = node;

                StartCoroutine(IDeadNode(node, false));
            }
            else if (isSucceed)
            {
                if (nodeIndex < activeSong.totalNote)
                    GenerateSingle(nodeIndex);
                else
                {
                    nodeIndex = 0;
                }
            }
        }

        public void StartGame()
        {
            isStared = true;
            activeSong = GameManager.Instance._songDataManager.GetSongData(SoundTrackEnum.Chopin_Preludes);
            speed = activeSong.tempo * screenHeight / 1280f;
            Invoke("ChangeBackground", intervalForBg);
        }

        public void OpenGameView()
        {
            Setup(true);
            SetSong();
            sliderStarController.ResetProgress();
            _scoreCalculate.ResetScore();
        }

        public void ScoreUpdate(int amount)
        {
            score += amount;
            txScore.text = score.ToString();
            sliderStarController.UpdateStarsSlider(score);
        }

        public void GetScore(RectTransform nodeRectTransform)
        {
            HighlightBackground();
            float distanceToScoreLine =
                Mathf.Abs(nodeRectTransform.anchoredPosition.y - scoreLineRectTransform.anchoredPosition.y);
            ScoreUpdate(_scoreCalculate.CompareToRank(distanceToScoreLine));
        }

        private void HighlightBackground()
        {
            //decorateImage.transform.localScale = Vector3.one;

            Sequence sequence = DOTween.Sequence();
            //sequence.Kill();
            sequence.Append(decorateImage.DOFade(1f, 0.5f).SetEase(Ease.OutBack))
                //.Join(decorateImage.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack))
                .Append(decorateImage.DOFade(0.7f, 0.25f).SetEase(Ease.OutBack));
            //.Join(decorateImage.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        }

        public bool isMoveable()
        {
            return !isDead && isStared;
        }

        #endregion

        #region Private Methods

        void ChangeBackground()
        {
            if (isDead || !isStared) return;

            if (ienChangeBg != null)
                StopCoroutine(ienChangeBg);
            ienChangeBg = IChangeBackground(backgroundImgList[bgCount]);
            StartCoroutine(ienChangeBg);
            bgCount++;
            if (bgCount >= backgroundImgList.Count)
            {
                bgCount = 0;
            }
        }

        IEnumerator IChangeBackground(Sprite img)
        {
            Color c = background.color;

            while (c.a > 0.8f)
            {
                c.a -= 0.01f;
                yield return new WaitForSeconds(0.2f);
                background.color = c;
            }

            background.sprite = img;
            yield return new WaitForSeconds(1f);
            while (c.a < 1f)
            {
                c.a += 0.01f;
                yield return new WaitForSeconds(0.2f);
                background.color = c;
            }

            CancelInvoke("ChangeBackground");
            Invoke("ChangeBackground", intervalForBg);
        }

        IEnumerator IDeadNode(Node dn, bool isWrongTap)
        {
            isTap = false;
            if (isWrongTap)
            {
                speed = 0;
            }

            else
            {
                speed = -speed;
                yield return new WaitForSeconds(0.5f);
                speed = 0;
                dn.gameObject.SetActive(false);

                dn = DeadNodeGenerate(dn.transform.localPosition);
            }

            isDead = true;

            for (int i = 0; i < 4; i++)
            {
                dn.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);

                dn.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1f);

            isStared = false;
            Setup(false);

            //Post event lose
            EventDispatcher.Instance.PostEvent(EventID.Lose);
        }

        private void MakeBoundary()
        {
            if (boundaryHolder == null)
            {
                boundaryHolder = new GameObject("BoundaryHolder").transform;
                boundaryHolder.parent = nodeHook;
                boundaryHolder.localScale = Vector3.one;
            }

            for (int i = 1; i <= 3; i++)
            {
                GameObject go = Instantiate(boundary);
                go.transform.SetParent(boundaryHolder);
                RectTransform rect = go.GetComponent<RectTransform>();

                rect.pivot = Vector2.zero;
                rect.localPosition = new Vector2(200f * i, 0);
                rect.sizeDelta = new Vector2(1, Screen.height);
                rect.localScale = new Vector3(2f, 2f, 0f);
            }
        }

        private Node DeadNodeGenerate(Vector3 pos)
        {
            MusicManager.Instance.Stop();
            GameObject go = Instantiate(nodePrefab);
            go.transform.SetParent(nodeHolder);
            go.GetComponent<RectTransform>().localPosition = pos;
            go.GetComponent<RectTransform>().localScale = Vector3.one;


            Node n = go.GetComponent<Node>();
            n.Init(deadNode.type, deadNode.nodeIndex, true);

            return n;
        }

        private void Generate()
        {
            if (nodeHolder == null)
            {
                nodeHolder = new GameObject("NodeHolder").transform;
                nodeHolder.parent = nodeHook;
                nodeHolder.localScale = Vector3.one;
            }

            for (int i = -1; i < 8; i++)
            {
                GenerateSingle(i);
            }
        }

        private void Reset()
        {
            screenHeight = Screen.height;
            nodeHeight = 300;
            nodeWidth = 200;


            score = 0;
            nodeIndex = 0;
            lastMixedNodePosY = 0;
            isStared = false;
            isDead = false;
            isTap = true;


            if (nodeHolder != null)
                Destroy(nodeHolder.gameObject);

            nodeHolder = null;
            actvieNodes.Clear();
            Color c = background.color;
            c.a = 1;
            background.color = c;
        }

        private Node.Type GetNoteType()
        {
            Node.Type type = Node.Type.NORMAL;

            int v = Random.Range(0, 4);
            if (v == 0)
            {
                type = Node.Type.NORMAL;
            }
            else if (v == 1)
            {
                type = Node.Type.LONG;
            }
            else if (v == 2)
            {
                type = Node.Type.LONG2;
            }
            else if (v == 3)
            {
                type = Node.Type.LONG3;
            }

            return type;
        }

        private void GenerateSingle(int index)
        {
            Node.Type activeType = index == -1 ? Node.Type.START : GetNoteType();


            float xPos = nodeWidth * GetUniqueNote();
            float yPos = 0;

            if (activeType == Node.Type.START)
            {
                yPos = transBestScore.GetComponent<RectTransform>().sizeDelta.y;
            }
            else if (activeType == Node.Type.MIXED)
            {
                mixedNodeCount++;
                if (mixedNodeCount >= 2)
                {
                    yPos = lastMixedNodePosY;
                    mixedNodeCount = 0;
                }
                else
                {
                    yPos = lastNode.transform.localPosition.y + lastNode.height;
                    lastMixedNodePosY = yPos;
                }
            }
            else
            {
                yPos = lastNode.transform.localPosition.y + lastNode.height;
            }

            Vector3 pos = new Vector3(xPos, yPos, 0);
            GameObject go = Instantiate(nodePrefab);
            go.SetActive(true);
            go.transform.SetParent(nodeHolder);
            go.GetComponent<RectTransform>().localPosition = pos;
            go.GetComponent<RectTransform>().localScale = Vector3.one;

            lastNode = go.GetComponent<Node>();
            lastNode.Init(activeType, index, false);
            lastNode.OnLeave += OnLeave;
            actvieNodes.Add(lastNode);

            nodeIndex++;
        }

        private int GetUniqueNote()
        {
            int tmpIndex = Random.Range(0, 4);
            while (noteIndex == tmpIndex)
            {
                tmpIndex = Random.Range(0, 4);
            }

            noteIndex = tmpIndex;
            return noteIndex;
        }

        #endregion
    }
}