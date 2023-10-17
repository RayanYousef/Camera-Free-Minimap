using UnityEditor;

namespace CameraFreeMinimap
{

    [InitializeOnLoad]
    public static class ScriptExecutionOrderChanger
    {
        static ScriptExecutionOrderChanger()
        {
            // The script base type you want to modify the execution order for.
            System.Type baseScriptType = typeof(CFM_MinimapManagerBaseClass);

            // The desired execution order (lower values execute earlier).
            int desiredExecutionOrder = -10;

            MonoScript[] scripts = MonoImporter.GetAllRuntimeMonoScripts();

            foreach (MonoScript script in scripts)
            {
                System.Type scriptType = script.GetClass();

                if (scriptType != null && baseScriptType.IsAssignableFrom(scriptType))
                {
                    int currentExecutionOrder = MonoImporter.GetExecutionOrder(script);

                    if (currentExecutionOrder != desiredExecutionOrder)
                    {
                        MonoImporter.SetExecutionOrder(script, desiredExecutionOrder);
                    }
                }
            }
        }
    }

}

