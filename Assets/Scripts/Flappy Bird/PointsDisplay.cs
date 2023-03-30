using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlappyBird
{
    public class PointsDisplay : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text;

        private void Start()
        {
            Level.OnScoreChanged += UpdateText;
        }

        void UpdateText(int score)
        {
            text.text = score.ToString();
        }
    }
}