using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals {
    [ExecuteAlways]
    public class Fog : MonoBehaviour
    {
        public Color fogColor = Color.white;
        public Sprite layerSprite;
        [Range (0, 1)] public float maxDistance = 1;
        public int layerCount = 10;

        private bool refresh = false;

        void Update () {
            if (!refresh) return;
            refresh = false;
            DestroyChildren ();
            InstantiateLayers ();
        }

        void DestroyChildren () {
            for (int i = transform.childCount - 1; i >= 0; i--) {
                Transform child = transform.GetChild (i);
                if (Application.isPlaying) {
                    Destroy (child.gameObject);
                } else {
                    DestroyImmediate (child.gameObject);
                }
            }
        }

        void InstantiateLayers () {
            Color layerColor = fogColor;
            layerColor.a = 1 - Mathf.Pow (1 - fogColor.a, 1f / layerCount);

            for (int i = 0; i < layerCount; i++) {
                int order = -2 * Mathf.RoundToInt (100 * maxDistance * i / (layerCount - 1)) - 1;
                CreateLayer (layerColor, order);
            }
        }

        void CreateLayer (Color color, int order) {
            GameObject layer = new GameObject ("FogLayer");
            layer.transform.SetParent (transform, false);
            SpriteRenderer spriteRenderer = layer.AddComponent<SpriteRenderer> ();
            spriteRenderer.sprite = layerSprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = order;
        }

        void OnValidate () {
            refresh = true;
            layerCount = Mathf.Max (layerCount, 2);
        }
    }
}