using UnityEngine;

namespace Missions.MissionPrerequisite
{
    public abstract class MissionPrerequisite : ScriptableObject
    {
        [field: SerializeField] public abstract string PrerequisiteFailText { get;  }
        public abstract bool IsPrerequisiteSatisfied();
    }
}