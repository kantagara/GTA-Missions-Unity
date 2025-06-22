
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerTrigger : MonoBehaviour
{
    public ITriggerAction TriggerAction { get; set; }
    
    public void OnTriggerEnter(Collider other)
    {
        Assert.IsTrue(TriggerAction != null, $"Trigger object is not set for {gameObject.name}!");
        if (other.CompareTag("Player"))
            TriggerAction.Execute(gameObject);
    }
}

public interface ITriggerAction
{
    public void Execute(GameObject go);
}
