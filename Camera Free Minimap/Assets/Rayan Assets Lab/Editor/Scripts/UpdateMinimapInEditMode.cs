using CameraFreeMinimap;
using UnityEditor;
using UnityEngine;


namespace CameraFreeMinimapEditor
{
    [CustomEditor(typeof(MinimapManager))]
    public class UpdateMinimapInEditMode : Editor
    {
        private MinimapManager _minimapManager;

        private void OnEnable()
        {
            _minimapManager = (MinimapManager)target;

            EditorApplication.update += UpdateFunctionInEditor;
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateFunctionInEditor;
        }

        private void UpdateFunctionInEditor()
        {
            _minimapManager.SetMinimapSpriteAndAspectRatio();
            _minimapManager.UpdateInEditorMode();
        }
    }

}