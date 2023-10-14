using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CameraFreeMinimap
{
    public struct ObjectBounds
    {
        [SerializeField] float minX, maxX, minY,maxY, minZ, maxZ, xTotalSize, yTotalSize, zTotalSize;

        public float MinX { get => minX; set => minX = value; }
        public float MaxX { get => maxX; set => maxX = value; }

        public float MinY { get => minY; set => minY = value; }
        public float MaxY { get => maxY; set => maxY = value; }

        public float MinZ { get => minZ; set => minZ = value; }
        public float MaxZ { get => maxZ; set => maxZ = value; }

        public float XTotalSize { get => xTotalSize; set => xTotalSize = value; }
        public float YTotalSize { get => yTotalSize; set => yTotalSize = value; }
        public float ZTotalSize { get => zTotalSize; set => zTotalSize = value; }
    }
    public static class CFM_ExtensionMethods
    {
        #region Extension Methods
        public static void GetBounds(this SpriteRenderer spriteRenderer, ref ObjectBounds bounds)
        {

            bounds.MinX = spriteRenderer.bounds.min.x;
            bounds.MaxX = spriteRenderer.bounds.max.x;

            bounds.MinY = spriteRenderer.bounds.min.y;
            bounds.MaxY = spriteRenderer.bounds.max.y;

            bounds.MinZ = spriteRenderer.bounds.min.z;
            bounds.MaxZ = spriteRenderer.bounds.max.z;

            bounds.XTotalSize = spriteRenderer.bounds.size.x;
            bounds.YTotalSize = spriteRenderer.bounds.size.y;
            bounds.ZTotalSize = spriteRenderer.bounds.size.z;

        }

        public static void GetBounds(this Image _minimap, ref ObjectBounds bounds)
        {
            Vector3[] corners = new Vector3[4];
            _minimap.rectTransform.GetWorldCorners(corners);

            bounds.MinZ = corners[0].z;
            bounds.MaxZ = corners[1].z;
            bounds.MinY = corners[0].y;
            bounds.MaxY = corners[1].y;
            bounds.MinX = corners[1].x;
            bounds.MaxX = corners[2].x;

            // Calculate boundaries
            foreach (Vector3 corner in corners)
            {
                bounds.MinX = Math.Min(bounds.MinX, corner.x);
                bounds.MaxX = Math.Max(bounds.MaxX, corner.x);

                bounds.MinY = Math.Min(bounds.MinY, corner.y);
                bounds.MaxY = Math.Max(bounds.MaxY, corner.y);

                bounds.MinZ = Math.Min(bounds.MinZ, corner.z);
                bounds.MaxZ = Math.Max(bounds.MaxZ, corner.z);
            }

            bounds.XTotalSize = MathF.Abs(bounds.MinX - bounds.MaxX);
            bounds.YTotalSize = MathF.Abs(bounds.MinY - bounds.MaxY);
            bounds.ZTotalSize = MathF.Abs(bounds.MinZ - bounds.MaxZ);

        }
        #endregion

        #region Unused
        private static Vector2 CalculateScreenResolutionFromAnyCamera()
        {
            Vector2 screenResolution = Vector2.zero;

            if (Camera.allCamerasCount > 0)
            {
                Vector2 Resolution;
                float xDivisionValue, yDivisionValue;
                Camera camera = Camera.allCameras[0];


                xDivisionValue =
                    Camera.main.rect.min.x > 0 ?
                    Mathf.Min(1 - camera.rect.min.x, camera.rect.width) : camera.rect.width + camera.rect.min.x;

                yDivisionValue =
                    camera.rect.min.y > 0 ?
                    Mathf.Min(1 - camera.rect.min.y, camera.rect.height) : camera.rect.height + camera.rect.min.y;

                if (camera.rect.width + camera.rect.min.x > 1 && camera.rect.min.x < 0) xDivisionValue = 1;
                if (camera.rect.height + camera.rect.min.y > 1 && camera.rect.min.y < 0) yDivisionValue = 1;

                Resolution = new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight);

                screenResolution = new Vector2(Resolution.x / xDivisionValue, Resolution.y / yDivisionValue);
            }
            return screenResolution;
        } 
        #endregion
    }

}