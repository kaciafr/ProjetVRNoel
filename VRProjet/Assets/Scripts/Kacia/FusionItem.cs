using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace Kacia
{
    public class FusionItem : MonoBehaviour
    {
        public string key;

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
            
            /*Collider[] colliders = GetComponents<Collider>();
            foreach (Collider c in colliders)
                c.isTrigger = true;*/
        }

        private void OnCollisionEnter(Collision other)
        {
            FusionItem otherItem = other.gameObject.GetComponent<FusionItem>();

            Debug.Log($"OnTriggerEnter {this} {other} otherItem={otherItem}", this);
            
            if (otherItem == null) return;
            
            FusionManager.Instance.TryFusion(new List<FusionItem>{ this, otherItem });
        }
    }
}