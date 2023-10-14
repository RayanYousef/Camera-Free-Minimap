using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CameraFreeMinimap
{
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class CFM_MinimapCanvasElement : MonoBehaviour
    {

        #region Fields
        // List of all canvas elements: This list serves as a central registry to store instances of CFM_MinimapCanvasElement.
        // It keeps track of all canvas elements available in the application.
        public static List<CFM_MinimapCanvasElement> listOfCanvasElements = new List<CFM_MinimapCanvasElement>();

        // worldElement: This field stores a reference to a CFM_MinimapWorldElement associated with this canvas element.
        // It allows this canvas element to track its corresponding world element.
        [SerializeField] CFM_MinimapWorldElement worldElement;

        // worldTransform: This field stores a reference to the transform component of the world element.
        [SerializeField] Transform worldTransform;

        // instanceID: This field holds a unique identifier (Instance ID) associated with this canvas element.
        // It is used for identifying and searching for this specific canvas element, making it easier for the creator
        // to locate and manipulate it within the application.
        [SerializeField] int instanceID;
        #endregion

        #region Properties
        public Transform WorldTransform => worldTransform;
        public int InstanceID => instanceID;
        #endregion

        #region Constructor
        // Constructor: This constructor is called when a new instance of CFM_MinimapCanvasElement is created.
        // Its primary purpose is to add the newly created canvas element to the listOfCanvasElements,
        // which serves as a centralized repository for tracking all canvas elements in the application.
        // Additionally, it includes a loop to remove any null references from the list, ensuring it only
        // contains valid canvas elements.
        public CFM_MinimapCanvasElement()
        {
            // Add this canvas element instance to the listOfCanvasElements.
            listOfCanvasElements.Add(this);

            // Iterate through the listOfCanvasElements in reverse order.
            for (int i = listOfCanvasElements.Count - 1; i >= 0; i--)
            {
                // Check if the canvas element reference is null.
                if (listOfCanvasElements[i] == null)
                {
                    // If null, remove it from the list to maintain a clean list of canvas elements.
                    listOfCanvasElements.Remove(listOfCanvasElements[i]);
                }
            }
        }
        #endregion

        #region Unity
        private void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorApplication.update += EnsureAlignmentWithWorldElement;
#endif
        }

        private void OnDestroy()
        {
            listOfCanvasElements.Remove(this);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorApplication.update -= EnsureAlignmentWithWorldElement;
#endif
        }

        #endregion

        #region Methods
        // TryAssociateCanvasElement: This function attempts to associate a canvas element with a specific CFM_MinimapWorldElement.
        // It checks if a canvas element with a matching Instance ID exists in the list of canvas elements. If found, it associates
        // the canvas transform with the world element. If not found, it returns false.

        public static bool TryAssociateCanvasElement(CFM_MinimapWorldElement worldElement)
        {
            foreach (CFM_MinimapCanvasElement instance in listOfCanvasElements)
            {
                if (worldElement.GetHashCode() == instance.InstanceID)
                {
                    // Associate the canvas transform with the world element.
                    worldElement.CanvasTransform = instance.transform;
                    return true;
                }
            }
            return false;
        }

        // SetID: This function allows setting the unique identifier (Instance ID) of this canvas element.
        // It's typically used by the creator of this canvas element to establish a connection between
        // the canvas element and its associated world element.
        public void SetID(int instanceID)
        {
            this.instanceID = instanceID;
        }

        // SetWorldElement: This function is responsible for associating a CFM_MinimapWorldElement with this canvas element.
        // It sets the worldTransform property to the transform of the associated world element.
        // It is used for validation purposes. If the world transform of the associated world element is not
        // the same as this canvas element's world transform, this function helps ensure that the canvas element is correctly
        // aligned with its associated world element.
        public void SetWorldElement(CFM_MinimapWorldElement worldElement)
        {
            // Set the worldTransform to the transform of the associated world element.
            this.worldTransform = worldElement.WorldTransform;

            // Store a reference to the associated world element.
            this.worldElement = worldElement;
        }
        #endregion

#if UNITY_EDITOR
        // EnsureAlignmentWithWorldElement: This function checks if the canvas element is correctly aligned with its associated
        // world element by comparing their respective world transforms. If they do not match, it indicates a mismatch, and this
        // function ensures the canvas element's removal from the scene. The removal method varies based on whether the application
        // is running or in edit mode. In play mode, the canvas element is destroyed using the "Destroy" method to adhere to runtime
        // behavior. In edit mode, the "DestroyImmediate" method is used for immediate removal, accommodating design-time adjustments.
        public void EnsureAlignmentWithWorldElement()
        {

            // Check if either the world transform or world element reference is null, and if so, exit the function.
            if (worldTransform == null || worldElement == null) return;

            // Compare the world transform of the canvas element with that of the associated world element.
            if (worldTransform != worldElement.WorldTransform)
            {
                // Check if the application is in play mode (runtime) or edit mode.
                if (Application.isPlaying)
                    Destroy(gameObject); // Destroy the canvas element during play mode.
                else
                    DestroyImmediate(gameObject); // Destroy the canvas element immediately in edit mode.
            }
        }
    }
}

#endif
    

