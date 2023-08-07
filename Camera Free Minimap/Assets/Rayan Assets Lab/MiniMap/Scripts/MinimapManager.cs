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
        float xMapTotalLengthInCanvas, yMapTotalLengthInCanvas;

        [Header("Resolution Variables")]
        Vector2 scalerResolution, screenResolution, screenResToScalerRes;

        void Start()
        {
            // Sets the sprite of the minimap in the UI and in the world as well as fix aspect ratio issues in the canvas
            SetMinimapSpriteAndAspectRatio();

            //Gets the max/min x and z boundaries of the sprite in the world
            GetWorldMinimapSpriteBoundaries();

            // Calculates Screen To Scaler Resolution which affect the player position in the minimap if the resolution changes
            CalculateScreenResToScalerRes();

            //Gets the max/min x and y boundaries of UI image in the canvas
            CalculateMinimapBoundariesInCanvas();

            // Disables world sprite renderer
            minimapSpriteRendererInTheWorld.GetComponent<SpriteRenderer>().enabled = false;

        }
        private void FixedUpdate()
        {

            //Calculates Screen Resolution/ Scaler Resolution, which is used to make sure the player position in minimap will be correct despite the size of the "Screen" or the "Canvas Scaler".
            CalculateScreenResToScalerRes();

            //Calculates the boundaries of the minimap in the screen.
            CalculateMinimapBoundariesInCanvas();

            //Updates the player position on the screen using its position in real world.
            UpdatePlayerPositionOnMinimap();

            //Updates the rotation of the player icon. (Not his FOV)
            UpdatePlayerRotationOnMinimap();

            //Updates the Camera FOV in the minimap.
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

        public void SetMinimapSpriteAndAspectRatio()
        {
            if (minimapSpriteRendererInTheWorld != null)
                TryGetComponent<SpriteRenderer>(out minimapSpriteRendererInTheWorld);

            // Sets the sprite of the (Minimap Sprite Renderer in The World) with the sprite of the minimap we assigned in the inspector
            minimapSpriteRendererInTheWorld.sprite = _minimapSprite;
            // Calculates the aspect ratio of the sprite in the world.
            float aspectRatio = minimapSpriteRendererInTheWorld.bounds.size.x / minimapSpriteRendererInTheWorld.bounds.size.z;

            // Sets the sprite of the (Minimap UI in Canvas) with the sprite of the minimap we assigned in the inspector
            if (minimapUIInCanvas != null)
            {
                // Changes the sprite of the minimap to the sprite you have set in the Minimap Manager
                minimapUIInCanvas.sprite = _minimapSprite;

                // Matches the aspect ratio of the world minimap to the minimap in canvas
                minimapUIInCanvas.rectTransform.sizeDelta =
                    new Vector2(minimapUIInCanvas.rectTransform.sizeDelta.y * aspectRatio,
                    minimapUIInCanvas.rectTransform.sizeDelta.y);
            }
        }
        #endregion

        #region Update Methods
        private void CalculateScreenResToScalerRes()
        {
            switch (minimapUIInCanvas.GetComponentInParent<Canvas>().GetComponent<CanvasScaler>().uiScaleMode)
            {
                case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                    {
                        // Gets the resolution of the screen and the resolution of the scaler
                        // they are used to make sure that the minimap works even if the user has changed the resolution
                        scalerResolution = minimapUIInCanvas.GetComponentInParent<Canvas>().GetComponent<CanvasScaler>().referenceResolution;
                        screenResolution = new Vector2(Screen.width, Screen.height);
                        screenResToScalerRes = new Vector2(screenResolution.x / scalerResolution.x,
                        screenResolution.y / scalerResolution.y);
                        break;
                    }
                case CanvasScaler.ScaleMode.ConstantPixelSize:
                    {
                        screenResToScalerRes = Vector2.one * minimapUIInCanvas.GetComponentInParent<Canvas>().GetComponent<CanvasScaler>().scaleFactor;
                        break;
                    }
            }
        }

        private void CalculateMinimapBoundariesInCanvas()
        {

            // The position of the minimap in x coordinates can be added to the xMin or xMax to calculate the position of the boundaries of the minimap in x and y axis.s
            minXInCanvas = minimapUIInCanvas.rectTransform.position.x + minimapUIInCanvas.rectTransform.rect.xMin * screenResToScalerRes.x;
            maxXInCanvas = minimapUIInCanvas.rectTransform.position.x + minimapUIInCanvas.rectTransform.rect.xMax * screenResToScalerRes.x;

            minYInCanvas = minimapUIInCanvas.rectTransform.position.y + minimapUIInCanvas.rectTransform.rect.yMin * screenResToScalerRes.y;
            maxYInCanvas = minimapUIInCanvas.rectTransform.position.y + minimapUIInCanvas.rectTransform.rect.yMax * screenResToScalerRes.y;

            xMapTotalLengthInCanvas = minimapUIInCanvas.rectTransform.rect.size.x * screenResToScalerRes.x;
            yMapTotalLengthInCanvas = minimapUIInCanvas.rectTransform.rect.size.y * screenResToScalerRes.y;
        }

        private void UpdatePlayerPositionOnMinimap()
        {
            float playerXPosInWorldRatioToMinimap = Mathf.Abs((playerTransformInWorld.position.x - minXinWorld) / xLengthInWorld);
            float playerZPosInWorldRatioToMinimap = Mathf.Abs((playerTransformInWorld.position.z - maxZInWorld) / zLengthInWorld);

            float playerXPosOnMinimap = minXInCanvas + (playerXPosInWorldRatioToMinimap * xMapTotalLengthInCanvas);
            float playerYposOnMinimap = maxYInCanvas - (playerZPosInWorldRatioToMinimap * yMapTotalLengthInCanvas);

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

                // Calculates Screen To Scaler Resolution which affect the player position in the minimap if the resolution changes
                CalculateScreenResToScalerRes();

                //Gets the max/min x and y boundaries of UI image in the canvas
                CalculateMinimapBoundariesInCanvas();

                UpdatePlayerPositionOnMinimap();
                UpdatePlayerRotationOnMinimap();

                if (playerCameraInWorld != null && cameraFieldOfViewInMinimap != null)
                    UpdateCameraRotationOnMinimap();

            }

        }

#endif
    }
}
