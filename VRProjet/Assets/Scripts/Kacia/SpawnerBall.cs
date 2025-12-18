using UnityEngine;
using System.Collections.Generic;

namespace Kacia
{
    public class BallSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class BallConfig
        {
            public GameObject ballPrefab;
            public Vector3 spawnPosition;
        }

        public List<BallConfig> balls = new List<BallConfig>();
        public float spawnInterval = 2f;

        private float _timer = 0f;
        private int _currentBall = 0;

        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= spawnInterval)
            {
                SpawnBall();
                _timer = 0f;
            }
        }

        public void SpawnBall()
        {
            if (balls.Count == 0) return;

            BallConfig config = balls[_currentBall];
            GameObject ball = Instantiate(config.ballPrefab, config.spawnPosition, Quaternion.identity);
            ball.transform.localScale = Vector3.one * 0.2f;
        }
    }
}
