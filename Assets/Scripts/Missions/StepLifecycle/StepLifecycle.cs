using UnityEngine;

namespace Missions
{
    public abstract class StepLifecycleEvent : ScriptableObject
    {
        public abstract void Invoke(MissionData mission);
    }

}