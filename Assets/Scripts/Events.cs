
using UnityEngine;

public class OnDamageableDestroyed : IEvent
{
    public  GameObject DamageableDestroyed { get; set; }
}

public class OnItemPickedUp : IEvent
{
    public GameObject ObjectPickedUp { get; set; }
}

public class OnMissionCompleted : IEvent
{
    public MissionData Mission { get; set; }
}

public class OnMissionFailed : IEvent
{
    public MissionData Mission { get; set; }
    public string Reason { get; set; }
}