
    using UnityEngine;

    public class UnlockMissionReward : MissionReward
    {
        [SerializeField] private MissionData missionData;
        public override string Display => null;
        public override void ClaimReward()
        {
            missionData.CurrentStatus = MissionAvailabilityStatus.Available;
        }
    }
