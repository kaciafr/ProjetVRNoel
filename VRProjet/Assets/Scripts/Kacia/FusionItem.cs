using System.Collections.Generic;
using UnityEngine;

namespace Kacia
{
    public class FusionItem : MonoBehaviour
    {
        public string key;

        void OnCollisionEnter(Collision collision)
        {
            FusionItem other = collision.gameObject.GetComponent<FusionItem>();

            if (other == null) return;
            
            FusionManager.Instance.TryFusion(new List<FusionItem>{ this, other });
        }
    }
}