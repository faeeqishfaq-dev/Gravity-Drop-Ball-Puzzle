using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase11_Trigger
    {
        static Phase11_Trigger()
        {
            if (!SessionState.GetBool("Phase11_Done", false))
            {
                SessionState.SetBool("Phase11_Done", true);
                EditorApplication.delayCall += Phase11_FixAndFlowSetup.BuildFullFlow;
            }
        }
    }
}
