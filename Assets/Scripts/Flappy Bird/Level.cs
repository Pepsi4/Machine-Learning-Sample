using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Level;

namespace FlappyBird
{
    public class Level : MonoBehaviour
    {
        private static int _score;
        public static int Score { get { return _score; } set { _score = value; OnScoreChanged?.Invoke(_score); } }
        public static event Action<int> OnScoreChanged;

        [SerializeField] GameObject pipeHeadPrefab;
        [SerializeField] GameObject pipeBodyPrefab;
        [SerializeField] GameObject scoreTrigger;
        [SerializeField] Bird bird;
        [SerializeField] float delay = 3f;
        private float gapSize = 60f;

        private const float BIRD_X_POSITION = 0f;
        private const float CAMERA_ORTHO_SIZE = 50f;
        private const float PIPE_WIDTH = 8;
        private const float PIPE_HEAD_HEIGHT = 3.75f;
        private const float SPAWN_X = 100f;
        private List<Pipe> pipeList;
        private List<PipeComplete> pipeCompleteList;

        private void Awake()
        {
            pipeList = new List<Pipe>();
            pipeCompleteList = new List<PipeComplete>();
        }
        void Start()
        {
            StartCoroutine(SpawnPipeWithDelay());
        }

        public void Restart()
        {
            Debug.Log("<color=green> Restart...  </color>");


            Score = 0;
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            bird.Reset();
            pipeList.Clear();
            pipeCompleteList.Clear();
            //Destroy();
        }

        IEnumerator SpawnPipeWithDelay()
        {
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);

            CreateGapPipes(height, gapSize, SPAWN_X);
            yield return new WaitForSeconds(delay);
            StartCoroutine(SpawnPipeWithDelay());
        }

        void SpawnScoreTrigger(Transform parent)
        {
            var trigger = GameObject.Instantiate(scoreTrigger, parent);
            trigger.transform.localPosition = new Vector3(-3f, 0, 0); ;
        }

        void CreateGapPipes(float gapY, float gapSize, float xPos)
        {

            var pipeTop = SpawnPipe(CAMERA_ORTHO_SIZE * 2 - gapY - gapSize * .5f, xPos, false);
            var pipeBottom = SpawnPipe(gapY - gapSize * .5f, xPos, true);
            SpawnScoreTrigger(pipeBottom.pipeBodyTransform);


            pipeCompleteList.Add(new PipeComplete
            {
                pipeTop = pipeTop,
                pipeBottom = pipeBottom,
                gapY = gapY,
                gapSize = gapSize,
            });
        }

        private PipeComplete GetPipeCompleteWithPipe(Pipe pipe)
        {
            for (int i = 0; i < pipeCompleteList.Count; i++)
            {
                PipeComplete pipeComplete = pipeCompleteList[i];
                if (pipeComplete.pipeBottom == pipe || pipeComplete.pipeTop == pipe)
                {
                    return pipeComplete;
                }
            }
            return null;
        }

        public PipeComplete GetNextPipeComplete()
        {
            for (int i = 0; i < pipeList.Count; i++)
            {
                Pipe pipe = pipeList[i];
                if (pipe.pipeBodyTransform != null && pipe.GetXPosition() > BIRD_X_POSITION && pipe.IsBottom())
                {
                    PipeComplete pipeComplete = GetPipeCompleteWithPipe(pipe);
                    return pipeComplete;
                }
            }
            return null;
        }

        Pipe SpawnPipe(float height, float x, bool createBottom)
        {
            Transform pipeHead = Instantiate(pipeHeadPrefab, this.transform).transform;

            float pipeHeadYPoistion;
            if (createBottom)
            {
                pipeHeadYPoistion = -CAMERA_ORTHO_SIZE + height + PIPE_HEAD_HEIGHT * .5f;
            }
            else
            {
                pipeHeadYPoistion = CAMERA_ORTHO_SIZE - height - PIPE_HEAD_HEIGHT * .5f;
            }
            pipeHead.position = new Vector3(x, pipeHeadYPoistion);

            Transform pipeBody = Instantiate(pipeBodyPrefab, pipeHead).transform;

            float pipeBodyYPosition;

            SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
            pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

            if (createBottom)
            {
                pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
            }
            else
            {
                pipeBodyYPosition = CAMERA_ORTHO_SIZE;
                pipeBody.localScale = new Vector3(1, -1, 1);
                pipeBodySpriteRenderer.flipX = true;
            }
            pipeBody.position = new Vector3(x, pipeBodyYPosition);

            Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
            pipeList.Add(pipe);

            BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
            pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
            pipeBodyBoxCollider.offset = new Vector2(0, height * .5f);

            return pipe;
        }


        public class PipeComplete
        {

            public Pipe pipeBottom;
            public Pipe pipeTop;
            public float gapY;
            public float gapSize;

        }

        public class Pipe
        {
            private Transform pipeHeadTransform;
            public Transform pipeBodyTransform;
            private bool isBottom;

            public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
            {
                this.pipeHeadTransform = pipeHeadTransform;
                this.pipeBodyTransform = pipeBodyTransform;
                this.isBottom = isBottom;
            }

            public float GetXPosition()
            {
                return pipeHeadTransform.position.x;
            }

            public bool IsBottom()
            {
                return isBottom;
            }

            public void DestroySelf()
            {
                Destroy(pipeHeadTransform.gameObject);
                Destroy(pipeBodyTransform.gameObject);
            }
        }
    }
}