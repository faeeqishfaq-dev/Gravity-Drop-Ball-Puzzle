using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDraw.Level
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "PhysicsDraw/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        public List<LevelData> allLevels = new List<LevelData>();

        public LevelData GetLevel(int id)
        {
            return allLevels.Find(l => l.levelID == id);
        }
    }
}
