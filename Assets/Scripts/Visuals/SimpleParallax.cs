using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

namespace Visuals {
    [ExecuteAlways, DisallowMultipleComponent]
    public class SimpleParallax : MonoBehaviour
    {
        [Range(-2, 1)] public float distance = 0.1f;

        [SerializeField] private bool updateScale = true;

        [SerializeField] private bool updateColor = true;
        public Color tintColor = Color.white;

        [SerializeField] private bool updateOrder = true;
        public int maxLayer = 100;

        public bool updateInEditMode = false;

        [HideInInspector, SerializeField] private Vector3 originalPosition;
        [HideInInspector, SerializeField] private Vector3 originalScale;
        [HideInInspector, SerializeField] private int originalOrder;
        [HideInInspector, SerializeField] private Color originalColor;

        private Renderer rend;
        private SortingGroup sortingGroup;

        private List<SpriteRenderer> spriteRenderers;
        private Tilemap tilemap;

        [HideInInspector, SerializeField] private bool firstInit = true;
        private bool loaded = true;

        void OnEnable () {
            rend = GetComponent<Renderer> ();
            sortingGroup = GetComponent<SortingGroup> ();
            spriteRenderers = new List<SpriteRenderer> ();
            GetComponentsInChildren<SpriteRenderer> (spriteRenderers);
            tilemap = GetComponent<Tilemap> ();

            if (loaded && !firstInit) return; //Object was saved in the enabled state, return
            loaded = true;
            firstInit = false;

            originalPosition = transform.position;
            originalScale = transform.localScale;
            originalColor = GetColor ();
            originalOrder = GetOrder ();
        }

        void OnDisable () {
            loaded = false;
            transform.position = originalPosition;
            transform.localScale = originalScale;
            SetColor (originalColor);
            SetOrder (originalOrder);
        }


        void LateUpdate () {
            if (!Application.isPlaying && !updateInEditMode) return;

            Transform cameraTransform = Camera.main.transform;
            transform.position = Vector3.LerpUnclamped (originalPosition, cameraTransform.position, distance);
            if (updateScale) transform.localScale = originalScale * (1 - distance);

            if (updateColor) SetColor (Color.LerpUnclamped (originalColor, tintColor, Mathf.Abs(distance)));
            if (updateOrder) SetOrder (-2 * Mathf.RoundToInt (maxLayer * distance));
        }

        Color GetColor () {
            if (spriteRenderers.Count > 0) return spriteRenderers[0].color;
            if (tilemap != null) return tilemap.color;
            return Color.white;
        }

        void SetColor (Color color) {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.color = color;
            } if (tilemap != null) {
                tilemap.color = color;
            }
        }

        int GetOrder () {
            if (rend != null) return rend.sortingOrder;
            if (sortingGroup != null) return sortingGroup.sortingOrder;
            return 0;
        }

        void SetOrder (int order) {
            if (rend != null) {
                rend.sortingOrder = order;
            } if (sortingGroup != null) {
                sortingGroup.sortingOrder = order;
            }
        }

        void OnValidate () {
            maxLayer = Mathf.Max (maxLayer, 1);
        }
    }
}