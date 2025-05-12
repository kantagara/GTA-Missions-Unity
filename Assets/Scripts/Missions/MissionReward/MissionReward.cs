
using UnityEngine;

public abstract class MissionReward : ScriptableObject
{
    public abstract string Display { get; }
    public abstract void ClaimReward();
}
