
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CameraFreeMinimap
{

    // Enum to define the sorting layers of elements in the minimap.
    public enum MinimapElementRenderOrder
    {
        [Tooltip("Render this element as the bottom-most layer on the minimap, it will be below everything.")]
        BottomMost,

        [Tooltip("Render this element as the second layer from the bottom on the minimap.")]
        SecondLayer,

        [Tooltip("Render this element as the third layer from the bottom on the minimap.")]
        ThirdLayer,

        [Tooltip("Render this element as the fourth layer from the bottom on the minimap.")]
        FourthLayer,

        [Tooltip("Render this element as the top-most layer on the minimap, it will be above everything.")]
        TopMost
    }


    [ExecuteInEditMode]
    [AddComponentMenu("CFM Minimap/CFM Minimap World Element")]
    public class CFM_MinimapWorldElement : MonoBehaviour
    {

        [Header("Element Dependencies/References")]
        [Tooltip("The minimap manager controlling this object's display on the minimap.")]
        [SerializeField] CFM_MinimapManagerBaseClass minimapManager;

        [Tooltip("The world transform this object will track for its position on the minimap. It is automatically set to this object's transform, but you can change it if needed.")]
        [SerializeField] Transform trackedWorldTransform;

        [Tooltip("The UI transform in the canvas that moves to represent the position of this object in the game world.")]
        [SerializeField] Transform minimapCanvasTransform;

        [Header("Element State on Canvas")]
        [Tooltip("If enabled, this element updates only once upon creation (OnEnable).")]
        [SerializeField] bool isStaticElement;

        [Tooltip("Determines the rendering order of this element in the minimap canvas. Elements with higher values will be above others.")]
        [SerializeField] MinimapElementRenderOrder renderOrder;

        [Header("Element Image Data")]
        [Tooltip("The image representing this element in the minimap.")]
        [SerializeField] Sprite minimapIcon;
        [Tooltip("The color of this element on the minimap.")]
        [SerializeField] Color minimapIconColour = Color.white;

        [Header("Element Transform Properties")]
        //[Header("Element Offset in Minimap")]
        [Tooltip("Element position offset on x axis")]
        [SerializeField][Range(-200f, 200f)] float xPositionOffset;
        [Tooltip("Element position offset on y axis")]
        [SerializeField][Range(-200f, 200f)] float yPositionOffset;

        //[Header("Element Rotation Offset")]
        [SerializeField][Range(-180f, 180f)] float rotationOffset;

        //[Header("Element Width and Height in Minimap")]
        [Tooltip("Height of the element on the minimap. 100% means it matches the minimap's height, 1% means 1% of the minimap's height.")]
        [SerializeField][Range(0.1f, 100f)] float heightPercentage = 5f;
        [Tooltip("Width of the element on the minimap. 100% means it matches the minimap's width, 1% means 1% of the minimap's width.")]
        [SerializeField][Range(0.1f, 100f)] float widthPercentage = 5f;

        //-----------------------------------------------------------------------------------------------------------

        #region Properties
        // Properties to access private fields.
        public Transform CanvasTransform { get => minimapCanvasTransform; set => minimapCanvasTransform = value; }
        public Transform WorldTransform => trackedWorldTransform;
        public CFM_MinimapManagerBaseClass MiniMapManager => minimapManager;

        public MinimapElementRenderOrder ElementLayer => renderOrder;

        public bool IsStatic => isStaticElement;

        public Color MinimapIconColour { get => minimapIconColour; set => minimapIconColour = value; }

        #endregion

        #region Unity

        public void Awake()
        {
            SetMinimapManager();

            SetWorldTransformToThisTransform();

            CreateMinimapCanvasTransform();

            SetCanvasTransformParent();
        }

        public void OnEnable()
        {
            if (minimapCanvasTransform != null)
                minimapCanvasTransform.gameObject.SetActive(true);

            UpdateElementTransformOnMinimap();

#if UNITY_EDITOR
            if (Application.isPlaying == false)
                EditorApplication.update += EditorUpdate;
#endif
        }

        public void OnDisable()
        {
            if (minimapCanvasTransform != null)
                minimapCanvasTransform.gameObject.SetActive(false);

#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdate;
#endif
        }

        public void OnDestroy()
        {
#if UNITY_EDITOR
            if (minimapCanvasTransform != null)
                if (Application.isPlaying)
                    Destroy(minimapCanvasTransform.gameObject);
                else
                    DestroyImmediate(minimapCanvasTransform.gameObject);
#else
            if (canvasTransform != null)
                    Destroy(canvasTransform.gameObject);
#endif
        }

        private void FixedUpdate()
        {
            if (!isStaticElement)
                UpdateElementTransformOnMinimap();
#if UNITY_EDITOR
            UpdateElementSprite();
            UpdateElementColour();
            UpdateElementSize();
            UpdateElementOffset();
#endif
        }

        #endregion

        #region Element Methods

        // UpdateElementTransformOnMinimap: This function recalculates and updates the position and rotation of the element on the minimap canvas
        // based on the world position of the tracked object. It ensures the element is correctly positioned and oriented on the minimap.
        public void UpdateElementTransformOnMinimap()
        {
            if (minimapManager == null)
            {
                // No minimap manager assigned, so we cannot update the transform.
                return;
            }

            if( minimapCanvasTransform == null)
            {
                return;
            }

            if (trackedWorldTransform.position.x > minimapManager.WorldBounds.MaxX || trackedWorldTransform.position.x < minimapManager.WorldBounds.MinX)
            {
                minimapCanvasTransform.gameObject.SetActive(false);
                return;
            }

            if (trackedWorldTransform.position.z > minimapManager.WorldBounds.MaxZ || trackedWorldTransform.position.z < minimapManager.WorldBounds.MinZ)
            {
                minimapCanvasTransform.gameObject.SetActive(false);
                return;
            }

            minimapCanvasTransform.gameObject.SetActive(true);
            // Calculate the position and rotation.
            Vector3 newPosition = CalculatePositionOnMinimap();
            Quaternion newRotation = CalculateRotationOnMinimap();

            // Apply the new position and rotation.
            minimapCanvasTransform.position = newPosition;
            minimapCanvasTransform.rotation = newRotation;
        }

        private Vector3 CalculatePositionOnMinimap()
        {
            // Calculate position on the minimap based on the world position.
            float transformXPosInWorldRatioToMinimap = Mathf.Clamp01((minimapManager.WorldBounds.MaxX - trackedWorldTransform.position.x) / minimapManager.WorldBounds.XTotalSize);
            float transformZPosInWorldRatioToMinimap = Mathf.Clamp01((minimapManager.WorldBounds.MaxZ - trackedWorldTransform.position.z) / minimapManager.WorldBounds.ZTotalSize);

            // Calculate the exact position on the minimap based on ratios.
            float transformXPosOnMinimap = Mathf.RoundToInt(minimapManager.MinimapBounds.MaxX - (transformXPosInWorldRatioToMinimap * minimapManager.MinimapBounds.XTotalSize));
            float transformYPosOnMinimap = Mathf.RoundToInt(minimapManager.MinimapBounds.MaxY - (transformZPosInWorldRatioToMinimap * minimapManager.MinimapBounds.YTotalSize));

            return new Vector3(transformXPosOnMinimap, transformYPosOnMinimap, 0);
        }

        private Quaternion CalculateRotationOnMinimap()
        {
            // Calculate the rotation based on the tracked object's rotation and any additional rotation offset.
            float trackedWorldRotationAroundY = trackedWorldTransform.eulerAngles.y;
            return Quaternion.Euler(0, 0, -trackedWorldRotationAroundY - rotationOffset);
        }

        // SetMinimapManager: Attempts to set the minimap manager if it's not already assigned.
        public void SetMinimapManager()
        {
            if (minimapManager != null) // If the minimap manager is already assigned, exit the function.
                return;

            minimapManager = FindObjectOfType<CFM_MinimapManagerBaseClass>(); // Find the minimap manager in the scene.
        }

        // SetWorldTransformToThisTransform: Automatically sets the world transform to this object's transform if not already assigned.
        public void SetWorldTransformToThisTransform()
        {
            if (trackedWorldTransform != null) // If the world transform is already assigned, exit the function.
                return;

            trackedWorldTransform = transform; // Set the world transform to this object's transform.
        }

        // SetCanvasTransformParent: Sets the parent of the canvas transform to the appropriate layer within the minimap canvas hierarchy.
        public void SetCanvasTransformParent()
        {
            if (minimapManager == null) // If the minimap manager is not assigned, exit the function.
                return;

            if (minimapManager.MinimapImage == null) // If the minimap image is not assigned in the minimap manager, exit the function.
                return;

            if (minimapCanvasTransform == null) // If the minimap canvas transform is not assigned, exit the function.
                return;

            minimapManager.CreateParentsForElementsSorting(); // Ensure that the parent layers for element sorting are created.

            // Set the parent of the minimap canvas transform to the appropriate rendering layer within the minimap canvas hierarchy.
            minimapCanvasTransform.parent = minimapManager.ParentTransformLayers[(int)renderOrder];
        }


        // CreateMinimapCanvasTransform: This function is responsible for creating a canvas transform
        // to represent this minimap element. It's called during initialization to set up the visual representation
        // of the element on the minimap.
        private void CreateMinimapCanvasTransform()
        {
            // Check if the minimap manager is not assigned, and if so, return.
            if (minimapManager == null)
                return;

            // Check if the minimap image is not assigned, and if so, return.
            if (minimapManager.MinimapImage == null)
                return;

            // Check if the minimap canvas transform already exists, and if so, return.
            if (minimapCanvasTransform != null)
                return;

            // Attempt to associate this element with an existing MinimapCanvasElement, if successful, return.
            if (CFM_MinimapCanvasElement.TryAssociateCanvasElement(this))
            {
                return;
            }

            // Create a new canvas gameObject to represent this minimap element.
            minimapCanvasTransform = new GameObject().transform;

            // Set the name of the canvas gameObject to include the name of the tracked world transform.
            string canvasGameObjectName = trackedWorldTransform.name + " Minimap Element Canvas";
            minimapCanvasTransform.name = canvasGameObjectName;

            // Add a MinimapCanvasElement component to the canvas gameObject.
            // This component assigns a unique ID to this element, which is crucial for identification and tracking.
            CFM_MinimapCanvasElement canvasElement = minimapCanvasTransform.gameObject.AddComponent<CFM_MinimapCanvasElement>();
            canvasElement.SetID(GetInstanceID());
            canvasElement.SetWorldElement(this);

            // Create a child game object named "Image" within the canvas.
            GameObject elementWithImageComponent = new GameObject();
            elementWithImageComponent.name = "Image";

            // Add an Image component to the child game object, which will display the element's image on the minimap.
            elementWithImageComponent.AddComponent<Image>();

            // Set the child game object's parent to the minimap canvas, making it a part of the minimap's visual hierarchy.
            elementWithImageComponent.transform.SetParent(minimapCanvasTransform);

            // Update the element's Image component in the minimap based on the element's attributes.
            UpdateElementSprite();
            UpdateElementColour();
            UpdateElementSize();
            UpdateElementOffset();
        }


        #endregion

        #region Set Element Features
        public void SetElementSprite(Sprite newSprite)
        {
            minimapIcon = newSprite;
            UpdateElementSprite();
        }
        public void SetElementColour(Color newColour)
        {
            minimapIconColour = newColour;
            UpdateElementColour();
        }
        public void SetElementSize(float width, float height)
        {
            this.widthPercentage = width;
            this.heightPercentage = height;
            UpdateElementSize();
        }

        public void SetElementRotationOffset(float rotationOffset)
        {
            this.rotationOffset = rotationOffset;
        }

        public void SetIconPositionOffset(float xOffset, float yOffset)
        {
            this.xPositionOffset = xOffset;
            this.yPositionOffset = yOffset;
            UpdateElementOffset();
        }
        #endregion

        #region Update Element Features
        protected void UpdateElementSize()
        {
            if (minimapManager == null) 
                return;

            // Get the UI Image
            Image elementImage = minimapCanvasTransform.GetComponentInChildren<Image>();

            // Resize the created element using the given height and width, and it is dependent on the size of the Minimap width and height
            Vector2 MinimapSize = minimapManager.MinimapImage.GetComponent<RectTransform>().sizeDelta;
            elementImage.GetComponent<RectTransform>().sizeDelta = new Vector2(MinimapSize.x * (widthPercentage / 100), MinimapSize.y * (heightPercentage / 100));
            elementImage.GetComponent<RectTransform>().localScale = Vector3.one;
            minimapCanvasTransform.localScale = Vector3.one;
        }
        protected void UpdateElementOffset()
        {
            if (minimapCanvasTransform == null) return;

            // Setting the position of the image gameObject
            Vector3 offsetValue = new Vector3(xPositionOffset, yPositionOffset, 0);
            Transform uIGameObject = minimapCanvasTransform.GetComponentInChildren<Image>().transform;
            uIGameObject.localPosition = Vector3.zero + offsetValue;
        }
        protected void UpdateElementSprite()
        {
            if (minimapCanvasTransform == null) return;

            Image elementImage = minimapCanvasTransform.GetComponentInChildren<Image>();
            elementImage.sprite = minimapIcon;
        }
        protected void UpdateElementColour()
        {
            if(minimapCanvasTransform == null) return;

            Image elementImage = minimapCanvasTransform.GetComponentInChildren<Image>();
            elementImage.color = minimapIconColour;
        }
        #endregion



#if UNITY_EDITOR
        // EditorUpdate: This function is used exclusively in the Unity Editor to update the minimap element's properties and appearance.
        // It is called when the scene is being edited and not during gameplay.
        public void EditorUpdate()
        {
            // Check if this component has been deleted, and if so, return.
            if (this == null)
                return;

            // Ensure that this function is not executed during play mode.
            if (Application.isPlaying)
                return;

            // Set up the minimap manager reference.
            SetMinimapManager();

            // Ensure the tracked world transform is correctly assigned to this object's transform.
            SetWorldTransformToThisTransform();

            // Create the minimap canvas transform if it doesn't exist.
            CreateMinimapCanvasTransform();

            // Set the parent of the canvas transform to the appropriate layer within the minimap.
            SetCanvasTransformParent();

            // If the minimap manager is not assigned, return.
            if (minimapManager == null)
                return;

            // If the minimap image is not assigned, return.
            if (minimapManager.MinimapImage == null)
                return;

            // If the minimap canvas transform is not assigned, return.
            if (minimapCanvasTransform == null)
                return;

            // Update the element's Image component in the minimap based on the element's attributes.
            UpdateElementSprite();
            UpdateElementColour();
            UpdateElementSize();
            UpdateElementOffset();

            // Update the position and rotation of the element on the minimap.
            UpdateElementTransformOnMinimap();
        }
#endif


    }

}
