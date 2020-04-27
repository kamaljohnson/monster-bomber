using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PersonHealth
{
    Healthy,
    Infected,
    Dead
}

public class Person : MonoBehaviour
{
    public PersonHealth personHealth;


    public float infectedPersonSpeed;
    // after the infection duration the peron will get killed
    public float infectionToDeathDuration;
    
    public Collider infectionCollider;

    public Material infectedMaterial;
    public Material deadMaterial;

    public List<Object> listOfObjectsToBeCleanedAfterDeath;
    
    public void TriggerInfection()
    {
        // do noting if the person is not healthy 
        if (personHealth != PersonHealth.Healthy)
            return;

        personHealth = PersonHealth.Infected;
        infectionCollider.enabled = true;
        gameObject.GetComponent<PersonMovementController>().agent.speed = infectedPersonSpeed;
        gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;

        // this starts the death timer, after which the person dies due to the infection
        StartCoroutine(TriggerDeathTimer(infectionToDeathDuration));
    }

    public void TriggerDeath()
    {
        infectionCollider.enabled = false;
        personHealth = PersonHealth.Dead;
        gameObject.GetComponent<MeshRenderer>().material = deadMaterial;
        CleanupShit();
    }

// the collider is enabled if the person is infected
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Person") && personHealth == PersonHealth.Infected)
        {
            Debug.Log("the infection is transmitted");
            other.gameObject.GetComponent<Person>().TriggerInfection();
        }
    }
    
    IEnumerator TriggerDeathTimer(float time)
    {
        yield return new WaitForSeconds(time);
 
        // Code to execute after the delay
        TriggerDeath();
    }

    private void CleanupShit()
    {
        listOfObjectsToBeCleanedAfterDeath.ForEach(Destroy);
    }
}