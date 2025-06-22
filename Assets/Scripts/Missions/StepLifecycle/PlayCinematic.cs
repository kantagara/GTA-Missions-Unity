using UnityEngine;
using UnityEngine.Playables;

namespace Missions
{
    [CreateAssetMenu(menuName = "Mission/Steps/Lifecycle Event/Play Cinematic")]
    public class PlayCinematic : StepLifecycleEvent
    {
        [SerializeField] private PlayableAsset playableAsset;
        public override void Invoke(MissionData mission)
        {
            MissionCinematicUIController.Instance.PlayCinematic(mission, playableAsset);
        }
        
    }
}