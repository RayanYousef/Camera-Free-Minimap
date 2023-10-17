using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraFreeMinimap
{
    [AddComponentMenu("CFM Minimap/CFM Minimap Manager (Follow Target)")]
    public class CFM_MinimapManager_FollowTarget : CFM_MinimapManagerBaseClass
    {
        [SerializeField] CFM_MinimapWorldElement targetToFollow;

        [Header("Center Positions")]
        [Tooltip("The center position of the minimap on the canvas (Parent's position is used as the center).")]
        Vector3 minimapCenterPos;

        [Tooltip("The center position of the game world (This (Minimap Manager) gameObject's position is used as the center).")]
        Vector3 worldCenterPos;


        #region Unity

        // Override the FixedUpdate method from the base class.
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            SetWorldCenterPosition();
            SetMinimapCenterPosition();

            FollowTarget();
        }
        #endregion

        #region Methods
        // Set the target that the minimap will follow.
        public void SetTargetToFollow(CFM_MinimapWorldElement target)
        {
            targetToFollow = target;
        }

        private void SetWorldCenterPosition()
        {
            // Set the center of the world to the position of this gameObject.
            worldCenterPos = transform.position;
        }

        private void SetMinimapCenterPosition()
        {
            // Set the center of the minimap.
            minimapCenterPos = minimapImage.rectTransform.parent.position;
        }

        private void FollowTarget()
        {
            // Ensure there's a target to follow.
            if (targetToFollow == null) return;

            // Calculate the distance between the world center and the target's position.
            Vector3 DistanceOfWorldCenter = worldCenterPos - targetToFollow.WorldTransform.position;

            // Calculate ratios based on the distances.
            float xRatio = ((DistanceOfWorldCenter.x) / worldBounds.XTotalSize);
            float zRatio = ((DistanceOfWorldCenter.z) / worldBounds.ZTotalSize);

            // Calculate shifts on the minimap based on the ratios.
            float xMinimapShift = minimapBounds.XTotalSize * xRatio;
            float yMinimapShift = minimapBounds.YTotalSize * zRatio;

            // Calculate the new position for the minimap image.
            Vector3 newMinimapPosition = new Vector3(minimapCenterPos.x + xMinimapShift, minimapCenterPos.y + yMinimapShift, 0);

            // Set the minimap image's position.
            minimapImage.rectTransform.position = newMinimapPosition;
        }
        #endregion
    }
}
