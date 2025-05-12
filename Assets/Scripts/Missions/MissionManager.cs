using UnityEngine;
using UnityEngine.Playables;

public class MissionManager : Singleton<MissionManager>
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private MissionData[] missions;
}