using System.Collections;
using System.Collections.Generic;
using MagicTiles3.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MagicTiles3
{
    public class Node : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        public enum Type
        {
            DEAD,
            START,
            NORMAL,
            BOMB,
            LONG,
            LONG2,
            LONG3,
            MIXED
        };

        public Image longImg2;
        public Sprite normalImg, clickedImg, startImg, longImg, deadImg;
        public Text txtStart;
        public Type type;

        public System.Action<int> OnClicked;
        public System.Action<Node, bool> OnLeave;

        public float height = 300f, width = 200f;

        [SerializeField] bool isClicked, isLongType, isThumps, isChecked;
        bool isBomb;
        RectTransform rectTrans, longRect;

        public Transform longB, bomb;

        public Transform longTrans;

        //public string note = "a2";
        public int nodeIndex;
        int toneIdex;

        float normalHeight = 300f;
        float longHeight = 700f;
        float long2Height = 1100f;
        float long3Height = 1400f;

        #endregion


        #region LifeCycle

        void Update()
        {
            if (!GameplayController.Instance.isMoveable()) return;
            Move();
            LongRectSize();
            CheckPos();
            Disable();
        }

        void Start()
        {
            rectTrans = GetComponent<RectTransform>();
        }

        #endregion

        float GetHeight(Type type)
        {
            float _height = 0;
            switch (type)
            {
                case Node.Type.START:
                    _height = normalHeight;
                    break;
                case Node.Type.NORMAL:
                    _height = normalHeight;
                    break;
                case Node.Type.BOMB:
                    _height = normalHeight;
                    break;
                case Node.Type.MIXED:
                    _height = normalHeight;
                    break;
                case Node.Type.LONG:
                    _height = longHeight;
                    break;
                case Node.Type.LONG2:
                    _height = long2Height;
                    break;
                case Node.Type.LONG3:
                    _height = long3Height;
                    break;
            }

            return _height;
        }

        public void Init(Type type, int nodeIndex, bool isDead)
        {
            this.type = type;
            this.nodeIndex = nodeIndex;
            this.toneIdex = nodeIndex;
            isBomb = false;

            height = GetHeight(type);


            rectTrans = GetComponent<RectTransform>();
            switch (type)
            {
                case Type.START:
                    longTrans.gameObject.SetActive(false);
                    GetComponent<Image>().sprite = startImg;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

                    longB.gameObject.SetActive(false);
                    txtStart.gameObject.SetActive(true);


                    break;
                case Type.NORMAL:
                    longTrans.gameObject.SetActive(false);
                    GetComponent<Image>().sprite = normalImg;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    txtStart.gameObject.SetActive(false);
                    longB.gameObject.SetActive(false);
                    bomb.gameObject.SetActive(false);
                    break;
                case Type.BOMB:
                    longTrans.gameObject.SetActive(false);
                    GetComponent<Image>().sprite = normalImg;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    txtStart.gameObject.SetActive(false);
                    longB.gameObject.SetActive(false);
                    bomb.gameObject.SetActive(true);
                    isBomb = true;
                    break;

                case Type.MIXED:
                    longTrans.gameObject.SetActive(false);
                    GetComponent<Image>().sprite = normalImg;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    txtStart.gameObject.SetActive(false);
                    longB.gameObject.SetActive(false);
                    break;
                case Type.LONG:
                    longTrans.gameObject.SetActive(true);
                    longB.gameObject.SetActive(true);
                    GetComponent<Image>().sprite = longImg;
                    longRect = longTrans.GetComponent<RectTransform>();
                    txtStart.gameObject.SetActive(false);
                    isLongType = true;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    longRect.sizeDelta = new Vector2(width, height / 4);
                    break;
                case Type.LONG2:
                    longTrans.gameObject.SetActive(true);
                    longB.gameObject.SetActive(true);
                    GetComponent<Image>().sprite = longImg;
                    longRect = longTrans.GetComponent<RectTransform>();
                    txtStart.gameObject.SetActive(false);
                    isLongType = true;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    longRect.sizeDelta = new Vector2(width, height / 4);

                    longImg2.gameObject.SetActive(true);
                    longImg2.GetComponent<RectTransform>().localPosition = new Vector2(width / 2f, height / 1.5f);
                    break;
                case Type.LONG3:
                    longTrans.gameObject.SetActive(true);
                    longB.gameObject.SetActive(true);
                    GetComponent<Image>().sprite = longImg;
                    longRect = longTrans.GetComponent<RectTransform>();
                    txtStart.gameObject.SetActive(false);
                    isLongType = true;
                    GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    longRect.sizeDelta = new Vector2(width, height / 4);

                    longImg2.gameObject.SetActive(true);
                    longImg2.GetComponent<RectTransform>().localPosition = new Vector2(width / 2, height / 2);
                    break;
            }

            if (isDead)
            {
                longTrans.gameObject.SetActive(false);
                GetComponent<Image>().sprite = deadImg;
                GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

                longB.gameObject.SetActive(false);
                txtStart.gameObject.SetActive(false);
            }
        }

        void Move()
        {
            transform.Translate(new Vector3(0, -1, 0) * GameplayController.Instance.getSpeed() * Time.deltaTime);
        }

        public void OnClickDown()
        {
            if (isClicked) return;


            switch (type)
            {
                case Type.START:
                    ClickedStart();

                    break;
                case Type.NORMAL:
                    ClickedNormal();
                    break;
                case Type.BOMB:
                    ClickedNormal();
                    break;
                case Type.LONG:
                    ClickedLong();
                    break;
                case Type.LONG2:
                    ClickedLong();
                    break;
                case Type.LONG3:
                    ClickedLong();
                    break;
                case Type.MIXED:
                    ClickedNormal();
                    break;
            }

            if (!GameplayController.Instance.isStared) return;

            isClicked = true;

            if (OnClicked != null)
            {
                OnClicked(toneIdex);
            }
        }

        public void OnLongImage()
        {
            toneIdex++;

            if (OnClicked != null)
            {
                OnClicked(toneIdex);
            }
        }

        public void OnClickUp()
        {
            if (!GameplayController.Instance.isStared) return;

            isThumps = false;
        }

        void ClickedStart()
        {
            isThumps = true;
            txtStart.gameObject.SetActive(false);
            GetComponent<Image>().sprite = clickedImg;
            GameplayController.Instance.StartGame();
            MusicManager.Instance.PlayLevelMusic();
        }

        void ClickedNormal()
        {
            if (!GameplayController.Instance.isStared) return;
            if (isBomb)
            {
                GameplayController.Instance.Dead(this);
            }

            isThumps = true;
            if (!isClicked)
            {
                GetComponent<Image>().sprite = clickedImg;
                GameplayController.Instance.GetScore(this.rectTrans);
            }
        }

        void ClickedLong()
        {
            GameplayController.Instance.GetScore(this.rectTrans);

            if (!GameplayController.Instance.isStared) return;

            isThumps = true;
        }

        void LongRectSize()
        {
            if (isLongType && isThumps && longRect.sizeDelta.y < height)
            {
                longRect.sizeDelta = new Vector2(longRect.sizeDelta.x,
                    longRect.sizeDelta.y + GameplayController.Instance.speed / 70f);
            }
        }

        void CheckPos()
        {
            if (isBomb && !isClicked && !isChecked && transform.position.y < -150f)
            {
                if (OnLeave != null)
                {
                    OnLeave(this, true);
                    isChecked = true;

                    return;
                }
            }
            else if (isClicked && !isChecked && transform.position.y < -150f)
            {
                if (OnLeave != null)
                {
                    OnLeave(this, true);
                    isChecked = true;
                    return;
                }
            }
            else if (OnLeave != null && !isClicked && !isChecked && transform.position.y < -150f)
            {
                OnLeave(this, false);
                isChecked = true;
            }
        }

        void Disable()
        {
            if (!isClicked) return;
            if (transform.position.y < -2 * height)
            {
                GameplayController.Instance.actvieNodes.Remove(this);
                Destroy(gameObject);
            }
        }


        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }
}