using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace Kacia
{
    public class FusionItem : MonoBehaviour
    {
        public string key;
        public int damage = 3;

        private void Awake()
        {
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }
            
            if (GetComponent<XRGrabInteractable>() == null)
            {
                gameObject.AddComponent<XRGrabInteractable>();
            }
            
            if (GetComponent<XRGeneralGrabTransformer>() == null)
            {
                gameObject.AddComponent<XRGeneralGrabTransformer>();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter {this} {collision} other={collision}", this);
            
            FusionItem otherItem = collision.gameObject.GetComponent<FusionItem>();
            if (otherItem != null)
            {
                FusionManager.Instance.TryFusion(new List<FusionItem>{ this, otherItem });
                return;
            }
            
            Ennemy ennemy = collision.gameObject.GetComponent<Ennemy>();
            if (ennemy != null)
            {
                Debug.Log($" Script Ennemy trouvé sur {ennemy.name}");

                ennemy.TakeDamage(damage, collision);

                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;
                
                transform
                    .DOScale(0, 0.3f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>  Destroy(this.gameObject));
            }
        }
    }
}