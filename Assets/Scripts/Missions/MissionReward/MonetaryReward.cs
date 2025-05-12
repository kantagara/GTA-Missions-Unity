using UnityEngine;

public class MonetaryReward : MissionReward
{
    [SerializeField] private int amount;
    public override string Display => $"{amount}$";
    
    public override void ClaimReward()
    {
                
    }
}
