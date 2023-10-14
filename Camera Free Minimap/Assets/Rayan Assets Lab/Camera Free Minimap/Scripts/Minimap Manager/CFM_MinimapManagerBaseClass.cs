using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace CameraFreeMinimap
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class CFM_MinimapManagerBaseClass : MonoBehaviour
    {
        #region Fields
        [Header("Required References")]

        [Tooltip("The Image component in the Canvas representing the minimap.")]
        [SerializeField] protected Image minimapImage;

        [Tooltip("The SpriteRenderer of the minimap in the world (Minimap Manager). It determines the world's size and bounds, so ensure it aligns accurately with your world.")]
        protected SpriteRenderer worldSpriteRenderer;

        [Header("Game Boundaries")]
        [Tooltip("Boundaries of the minimap's sprite in the world.")]
        protected ObjectBounds worldBounds = new ObjectBounds();

        [Tooltip("Boundaries of the minimap's Image in the Canvas.")]
        protected ObjectBounds minimapBounds = new ObjectBounds();

        [Header("Element Layers in Hierarchy")]
        protected Transform[] parentTransformLayers = new Transform[5];

        #endregion

        #region Properties
        public Transform[] ParentTransformLayers => parentTransformLayers;
        public Image MinimapImage => minimapImage;
        public ObjectBounds WorldBounds => worldBounds;
        public ObjectBounds MinimapBounds => minimapBounds;

        #endregion

        #region Unity
        protected virtual void Awake()
        {

            worldSpriteRenderer = GetComponent<SpriteRenderer>();

            if (minimapImage == null)
            {
                Debug.LogError($"Camera Free Minimap: Please set minimap reference for {name} Minimap Manager Component");
                return;
            }

            CreateParentsForElementsSorting();

            SetWorldSpriteRendererSpriteToMinimapSprite();

            SetMinimapAspectRatiosToWorldAspectRatios();

            GetWorldBounds();

            GetMinimapBounds();

            // Disables world sprite renderer on playing mode
            if (Application.isPlaying)
                worldSpriteRenderer.enabled = false;

        }
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
                EditorApplication.update += EditorUpdate;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdate;
#endif
        }

        protected virtual void FixedUpdate()
        {
            GetMinimapBounds();
        }

        #endregion

        #region Minimap Methods

        // CreateParentsForElementsSorting: This function is responsible for creating and managing parent transforms within the minimap's hierarchy.
        // These parent transforms are used for sorting minimap elements on top of each other based on their rendering order.
        public void CreateParentsForElementsSorting()
        {
            // Loop through each available rendering layer.
            for (int i = 0; i < parentTransformLayers.Length; i++)
            {
                // Check if the parent transform for this layer is not assigned.
                if (parentTransformLayers[i] == null)
                {
                    // If a corresponding parent transform is found in the minimap image's hierarchy, use it.
                    if (minimapImage.transform.Find(Enum.GetName(typeof(MinimapElementRenderOrder), i)) != null)
                    {
                        parentTransformLayers[i] = minimapImage.transform.Find(Enum.GetName(typeof(MinimapElementRenderOrder), i));
                        parentTransformLayers[i].SetSiblingIndex(i); // Set the sibling index for proper layering.
                    }
                    else
                    {
                        // If no corresponding parent transform is found, create a new one.
                        parentTransformLayers[i] = new GameObject().transform;
                        parentTransformLayers[i].parent = minimapImage.transform; // Set the minimap image as the parent.
                        parentTransformLayers[i].name = Enum.GetName(typeof(MinimapElementRenderOrder), i);
                        parentTransformLayers[i].localPosition = Vector3.zero;
                        parentTransformLayers[i].SetSiblingIndex(i); // Set the sibling index for proper layering.
                    }
                }

                // Ensure the local scale of the parent transform is set to its default value.
                parentTransformLayers[i].localScale = Vector3.one;
            }
        }


        // SetWorldSpriteRendererSpriteToMinimapSprite: This function sets the sprite of the world sprite renderer to match the sprite of the minimap image in the UI.
        protected void SetWorldSpriteRendererSpriteToMinimapSprite()
        {
            worldSpriteRenderer.sprite = minimapImage.sprite;
        }

        // SetMinimapAspectRatiosToWorldAspectRatios: This function adjusts the canvas minimap's size to match the aspect ratio of the world sprite image. 
        protected void SetMinimapAspectRatiosToWorldAspectRatios()
        {
            // Calculate the aspect ratio of the sprite in the world.
            float aspectRatio = worldSpriteRenderer.bounds.size.x / worldSpriteRenderer.bounds.size.z;

            // Adjust the aspect ratios of the minimap to match the world bounds.
            minimapImage.rectTransform.sizeDelta =
                new Vector2(minimapImage.rectTransform.sizeDelta.y * aspectRatio,
                            minimapImage.rectTransform.sizeDelta.y);
        }

        // GetWorldBounds: This function retrieves the bounds of the game world by utilizing the worldSpriteRenderer.
        protected void GetWorldBounds()
        {
            worldSpriteRenderer.GetBounds(ref worldBounds);
        }

        // GetMinimapBounds: This function retrieves the bounds of the minimap within the canvas.
        protected void GetMinimapBounds()
        {
            if (minimapImage == null)
                return;

            minimapImage.GetBounds(ref minimapBounds);
        }

        #endregion



#if UNITY_EDITOR
        // EditorUpdate: This function is used in the Unity Editor to update various aspects of the minimap element when changes are made. It checks for specific conditions before performing updates.
        public void EditorUpdate()
        {

            // If the application is running (not in edit mode), exit the function.
            if (Application.isPlaying) return;

            // If the minimapImage reference is missing, log an error message and exit the function.
            if (minimapImage == null)
            {
                Debug.LogError($"Camera Free Minimap: Please set minimap reference for {name} Minimap Manager Component");
                return;
            }

            // Create or update the parent elements for sorting minimap elements.
            CreateParentsForElementsSorting();

            // Synchronize the sprite of the world sprite renderer with the minimap sprite.
            SetWorldSpriteRendererSpriteToMinimapSprite();

            // Adjust the aspect ratios of the minimap to match the aspect ratio of the world sprite.
            SetMinimapAspectRatiosToWorldAspectRatios();

            // Get the maximum and minimum x and z boundaries of the sprite in the world.
            GetWorldBounds();

            // Get the maximum and minimum x and y boundaries of the UI image in the canvas (minimap).
            GetMinimapBounds();
        }
#endif


    }
}
