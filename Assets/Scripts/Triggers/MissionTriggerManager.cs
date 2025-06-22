using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class MissionTriggerManager : Singleton<MissionTriggerManager>
    {
        [SerializeField] private PlayerTrigger missionTrigger;

        private readonly Dictionary<MissionData, PlayerTrigger> triggers = new();

        protected override void Awake()
        {
            base.Awake();
            EventSystem<OnMissionBecameAvailable>.Subscribe(MissionBecameAvailable);
            EventSystem<OnMissionCompleted>.Subscribe(MissionCompleted);
            EventSystem<OnMissionStarting>.Subscribe(MissionStarting);
            EventSystem<OnMissionFailed>.Subscribe(MissionFailed);
        }

        private void OnDestroy()
        {
            EventSystem<OnMissionBecameAvailable>.Unsubscribe(MissionBecameAvailable);
        }

        private void MissionStarting(OnMissionStarting obj)
        {
            ToggleTriggers(false);
        }

        private void MissionFailed(OnMissionFailed obj)
        {
            ToggleTriggers(true);
        }

        private void MissionCompleted(OnMissionCompleted obj)
        {
            Destroy(triggers[obj.Mission].gameObject);
            triggers.Remove(obj.Mission);
            ToggleTriggers(true);
        }

        private void ToggleTriggers(bool isOn)
        {
            foreach (var trigger in triggers.Values) trigger.gameObject.SetActive(isOn);
        }

        private void MissionBecameAvailable(OnMissionBecameAvailable obj)
        {
            var trigger = Instantiate(missionTrigger, obj.Mission.MissionPosition, Quaternion.identity);
            trigger.TriggerAction = new StartMissionTrigger(obj.Mission);
            triggers.Add(obj.Mission, trigger);
        }
    }
}