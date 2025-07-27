using UnityEngine;

namespace Triggers
{
    public class StartMissionTrigger : ITriggerAction
    {
        private readonly MissionData missionData;

        public StartMissionTrigger(MissionData data)
        {
            missionData = data;
        }

        public void Execute(GameObject go)
        {
            if (missionData.MissionPrerequisite != null && !missionData.MissionPrerequisite.IsPrerequisiteSatisfied())
            {
                EventSystem<OnMissionPrerequisiteNotSatisfied>.Invoke(new OnMissionPrerequisiteNotSatisfied()
                {
                    Text = missionData.MissionPrerequisite.PrerequisiteFailText
                });
                return;
            }
            
            EventSystem<OnMissionStarting>.Invoke(new OnMissionStarting()
            {
                Mission = missionData
            });
        }
    }
}