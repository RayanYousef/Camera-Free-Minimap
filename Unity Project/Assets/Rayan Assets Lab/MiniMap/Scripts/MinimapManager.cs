using UnityEngine;
using UnityEngine.UI;


namespace CameraFreeMinimap
{
    public class MinimapManager : MonoBehaviour
    {

        [Header("Minimap UI in canvas/Minimap sprite in world)")]
        //Minimap UI in Canvas: This is the image (UI element) that represents the minimap in the canvas
        [SerializeField] Image minimapUIInCanvas;
        //Minimap Sprite in the World: A sprite of the minimap in the world,
        //it is used to make sure that the drawn map is alligned with the world elements (the aspect ratio of this sprite will be applied to the minimap in the canvas).
        [SerializeField] SpriteRenderer minimapSpriteRendererInTheWorld;

        [Header("Minimap Sprite (Image)")]
        [SerializeField] Sprite _minimapSprite;

        [Header("Player transform and player icon in canvas")]
        // Player transform, this is the transform of the player that moves and rotates in the world
        [SerializeField] Transform playerTransformInWorld;
        // Player Icon In Minimap: The icon of the player in the minimap usually an arrow.
        [SerializeField] RectTransform playerIconInMinimap;

        [Header("Camera transform and camera FOV in canvas")]
        //Camera transform it represnets the player camera in the world
        [SerializeField] Transform playerCameraInWorld;
        // Field Of View: It represents the FOV of the camera in the minimap (you can ignore it in some games)
        [SerializeField] RectTransform cameraFieldOfViewInMinimap;


        [Header("Mini Map World Boundaries")]
        // The variables below are the boundaries of the spirite in the world.
        float minXinWorld, maxXInWorld, minZInWorld, maxZInWorld;
        float xLengthInWorld, zLengthInWorld;

        [Header("Mini Map Canvas Boundaries")]
        // The variables below are the boundaries of the spirite in the canvas.
        float minXInCanvas, maxXInCanvas, minYInCanvas, maxYInCanvas;
        float xTotalLengthC, yTotalLengthC;

        void Start()
        {
            // Sets the sprite of the minimap in the UI and in the world as well as fix aspect ratio issues in the canvas
            SetMinimapSpriteAndAspectRatio();

            //Gets the max/min x and z boundaries of the sprite in the world
            GetWorldMinimapSpriteBoundaries();

            //Gets the max/min x and y boundaries of UI image in the canvas
            GetCanvasMinimapUIBoundaries();


            minimapSpriteRendererInTheWorld.GetComponent<SpriteRenderer>().enabled = false;

        }
        private void FixedUpdate()
        {

            UpdatePlayerPositionOnMinimap();
            UpdatePlayerRotationOnMinimap();

            if (playerCameraInWorld != null)
                UpdateCameraRotationOnMinimap();
        }

        #region On Start Methods
        private void GetWorldMinimapSpriteBoundaries()
        {
            minXinWorld = minimapSpriteRendererInTheWorld.bounds.min.x;
            maxXInWorld = minimapSpriteRendererInTheWorld.bounds.max.x;

            minZInWorld = minimapSpriteRendererInTheWorld.bounds.min.z;
            maxZInWorld = minimapSpriteRendererInTheWorld.bounds.max.z;

            xLengthInWorld = minimapSpriteRendererInTheWorld.bounds.size.x;
            zLengthInWorld = minimapSpriteRendererInTheWorld.bounds.size.z;
        }
        private void GetCanvasMinimapUIBoundaries()
        {
            minXInCanvas = minimapUIInCanvas.rectTransform.position.x - minimapUIInCanvas.rectTransform.rect.xMax;
            maxXInCanvas = minimapUIInCanvas.rectTransform.rect.xMin + minimapUIInCanvas.rectTransform.position.x;

            minYInCanvas = minimapUIInCanvas.rectTransform.position.y - minimapUIInCanvas.rectTransform.rect.yMin;
            maxYInCanvas = minimapUIInCanvas.rectTransform.rect.yMax + minimapUIInCanvas.rectTransform.position.y;

            xTotalLengthC = minimapUIInCanvas.rectTransform.rect.size.x;
            yTotalLengthC = minimapUIInCanvas.rectTransform.rect.size.y;
        }

        public void SetMinimapSpriteAndAspectRatio()
        {
            TryGetComponent<SpriteRenderer>(out minimapSpriteRendererInTheWorld);

            // Sets the sprite of the (Minimap Sprite Renderer in The World) with the sprite of the minimap we assigned in the inspector
            minimapSpriteRendererInTheWorld.sprite = _minimapSprite;
            // Calculates the aspect ratio of the sprite in the world.
            float aspectRatio = minimapSpriteRendererInTheWorld.bounds.size.x / minimapSpriteRendererInTheWorld.bounds.size.z;

            // Sets the sprite of the (Minimap UI in Canvas) with the sprite of the minimap we assigned in the inspector
            if (minimapUIInCanvas != null)
            {
                minimapUIInCanvas.sprite = _minimapSprite;

                // Matches the aspect ratio of the world minimap to the minimap in canvas
                minimapUIInCanvas.rectTransform.sizeDelta =
                    new Vector2(minimapUIInCanvas.rectTransform.sizeDelta.y * aspectRatio,
                    minimapUIInCanvas.rectTransform.sizeDelta.y);
            }
        }
        #endregion

        #region Update Methods
        private void UpdatePlayerPositionOnMinimap()
        {
            float playerXPosInWorldRatioToMinimap = Mathf.Abs((playerTransformInWorld.position.x - minXinWorld) / xLengthInWorld);
            float playerZPosInWorldRatioToMinimap = Mathf.Abs((playerTransformInWorld.position.z - maxZInWorld) / zLengthInWorld);

            float playerXPosOnMinimap = minXInCanvas + (playerXPosInWorldRatioToMinimap * xTotalLengthC);
            float playerYposOnMinimap = maxYInCanvas - (playerZPosInWorldRatioToMinimap * yTotalLengthC);
            playerIconInMinimap.position = new Vector3(playerXPosOnMinimap, playerYposOnMinimap);
        }

        private void UpdateCameraRotationOnMinimap()
        {
            float cameraRotationAroundY = playerCameraInWorld.eulerAngles.y;
            cameraFieldOfViewInMinimap.rotation = Quaternion.Euler(0, 0, -cameraRotationAroundY);
        }

        private void UpdatePlayerRotationOnMinimap()
        {
            float playerRotationAroundY = playerTransformInWorld.eulerAngles.y;
            playerIconInMinimap.rotation = Quaternion.Euler(0, 0, -playerRotationAroundY);

        }
        #endregion

#if UNITY_EDITOR
        public void UpdateInEditorMode()
        {
            bool canUpdateInEditor = playerTransformInWorld != null && playerIconInMinimap != null;

            if (canUpdateInEditor)
            {

                SetMinimapSpriteAndAspectRatio();

                //Gets the max/min x and z boundaries of the sprite in the world
                GetWorldMinimapSpriteBoundaries();

                //Gets the max/min x and y boundaries of UI image in the canvas
                GetCanvasMinimapUIBoundaries();

                UpdatePlayerPositionOnMinimap();
                UpdatePlayerRotationOnMinimap();

                if (playerCameraInWorld != null && cameraFieldOfViewInMinimap != null)
                    UpdateCameraRotationOnMinimap();

            }

        }

#endif
    }
}
