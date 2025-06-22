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
            EventSystem<OnMissionStarting>.Invoke(new OnMissionStarting()
            {
                Mission = missionData
            });
        }
    }
}