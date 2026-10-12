using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using PhysicsDraw.EditorScripts;

public static class RestoreProject
{
    [MenuItem("PhysicsDraw/CRITICAL RESTORE (Run Once)")]
    public static void RunFullRestore()
    {
        Debug.Log("--- STARTING CRITICAL RESTORE ---");

        // 1. Rebuild UI and Scenes (Phase 14)
        Phase14_Master.RunAll();

        // 2. Fix Progression, Camera, and add Fail UI/DeathZone (Phase 15)
        PhysicsDraw.EditorScripts.Phase15_FixProgression.FixAll();
        PhysicsDraw.EditorScripts.Phase15_2_FailUIAndDeathZone.BuildFailUI();

        // 3. Rebuild Levels 1-10 for portrait (Phase 16)
        Phase16_FixLevelLayouts.RebuildForPortrait();

        // 4. Create Levels 11 and 12 (Phase 17)
        Phase17_CreateLevels11_12.CreateLevels();

        // 5. Create Level 13 (Phase 18)
        Phase18_CreateLevel13.CreateLevel13();

        // Fix missing references one last time
        FixMissingManagers.Fix();

        // Explicitly Save All Open Scenes to force serialization
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();

        Debug.Log("--- CRITICAL RESTORE COMPLETE! ALL CHANGES PERMANENTLY SAVED. ---");
    }
}
