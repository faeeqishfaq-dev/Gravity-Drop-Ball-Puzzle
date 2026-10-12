using UnityEngine;

namespace PhysicsDraw.Core
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "PhysicsDraw/Settings/Gameplay Settings")]
    public class GameplaySettings : ScriptableObject
    {
        [Header("Goal Settings")]
        [Tooltip("How long the ball must remain inside the goal trigger to win.")]
        public float goalValidationTime = 0.3f;

        [Header("UI Settings")]
        public float defaultTransitionDuration = 0.5f;
    }
}
