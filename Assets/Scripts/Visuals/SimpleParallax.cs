using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals {
    public class SimpleParallax : MonoBehaviour
    {
        public float modifier = 0.5f;

        private Vector3 originalPosition;
        private Vector3 originalScale;

        void Start () {
            originalPosition = transform.position;
            originalScale = transform.localScale;
        }


        void LateUpdate () {
            Transform cameraTransform = Camera.main.transform;
            transform.position = Vector3.LerpUnclamped (originalPosition, cameraTransform.position, modifier);
            transform.localScale = originalScale * (1 - modifier);
        }
    }
}