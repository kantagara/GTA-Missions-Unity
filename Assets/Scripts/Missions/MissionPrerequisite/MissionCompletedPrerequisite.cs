using UnityEngine;

namespace Missions.MissionPrerequisite
{
    public class MissionCompletedPrerequisite : MissionPrerequisite
    {
        [SerializeField] private MissionData missionCompleted;
        public override string PrerequisiteFailText => $"Mission {missionCompleted.MissionName} must be completed ";
        public override bool IsPrerequisiteSatisfied()
        {
            return missionCompleted.CurrentStatus == MissionAvailabilityStatus.Completed;
        }
    }
}