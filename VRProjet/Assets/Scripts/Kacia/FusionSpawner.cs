using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Kacia
{
    public class FusionSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        public int max = 5;
        public GameObject prefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;

        [Header("Randomization")]
        public float randRotation = 5f;
        public float randPosition = 0.2f;

        private readonly List<GameObject> _objects = new List<GameObject>();

        void Start()
        {
            InvokeRepeating(nameof(CheckAndRespawn), 0.5f, 0.5f);
        }

        void CheckAndRespawn()
        {
            _objects.RemoveAll(obj => obj == null);
            if (_objects.Count < max) SpawnObject();
        }

        private void SpawnObject()
        {
            if (prefab == null)
            {
                Debug.LogWarning("Prefab not assigned!");
                return;
            }

            // Random offset
            Vector3 randomOffset = new Vector3(
                Random.Range(-randPosition, randPosition),
                Random.Range(-randPosition, randPosition),
                Random.Range(-randPosition, randPosition)
            );
            Quaternion randomRot = Quaternion.Euler(
                Random.Range(-randRotation, randRotation),
                Random.Range(-randRotation, randRotation),
                Random.Range(-randRotation, randRotation)
            );

            GameObject newObj = Instantiate(
                prefab, 
                transform.position + spawnPosition + randomOffset, 
                transform.rotation * Quaternion.Euler(spawnRotation) * randomRot
            );
            _objects.Add(newObj);

            // Disable physics
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }

            // Spawn animation
            newObj.transform.localScale = Vector3.zero;
            newObj.transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => 
                {
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.detectCollisions = true;
                    }
                });
        }

        void OnDestroy()
        {
            CancelInvoke(nameof(CheckAndRespawn));
        }
    }
}
