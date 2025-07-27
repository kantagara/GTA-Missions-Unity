
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

public class OnStepStarted : IEvent
{
    public MissionStep Step { get; set; }
}

public class OnStepFinished : IEvent
{
    public MissionStep Step { get; set; }
}

public class OnMissionStatusChanged : IEvent
{
    public MissionData Mission { get; set; }
    public MissionAvailabilityStatus PreviousStatus { get; set; }
}


public class OnMissionStarting : IEvent
{
    public MissionData Mission { get; set; }
}

public class OnMissionPrerequisiteNotSatisfied : IEvent
{
    public string Text { get; set; }
}