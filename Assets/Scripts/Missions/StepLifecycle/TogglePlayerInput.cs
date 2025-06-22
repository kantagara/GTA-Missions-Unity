using UnityEngine;

namespace Missions
{
    [CreateAssetMenu(menuName = "Mission/Steps/Lifecycle Event/Toggle Player Input")]
    public class TogglePlayerInput : StepLifecycleEvent
    {
        [SerializeField] private bool enableInput;
        public override void Invoke(MissionData mission)
        {
            PlayerMovement.Instance.InputEnabled = enableInput;
        }
    }
}