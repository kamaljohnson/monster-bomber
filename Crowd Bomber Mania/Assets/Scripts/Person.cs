using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum PersonTags
{
    Healthy,
    Infected,
    Dead
}

public class Person : MonoBehaviour
{
    public float infectedPersonSpeed;
    // after the infection duration the peron will get killed
    public float infectionToDeathDuration;

    public Material infectedMaterial;
    public Material deadMaterial;

    public List<Object> listOfObjectsToBeCleanedAfterDeath;

    public void TriggerInfection()
    {
        gameObject.tag = GetTag(PersonTags.Infected);
        
        gameObject.GetComponent<PersonMovementController>().agent.speed = infectedPersonSpeed;
        gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;

        // this starts the death timer, after which the person dies due to the infection
        StartCoroutine(TriggerDeathTimer(infectionToDeathDuration));
    }

    public void TriggerDeath()
    {
        gameObject.tag = GetTag(PersonTags.Dead);
        gameObject.GetComponent<MeshRenderer>().material = deadMaterial;
        CleanupShit();
    }

// the collider is enabled if the person is infected
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(GetTag(PersonTags.Infected))) return;
        
        TriggerInfection();
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

    public Person GetNearestHealthyPerson()
    {
        var closeDistance = 100f;
        Person targetPerson = null;
        var healthyPersons = GameObject.FindGameObjectsWithTag(GetTag(PersonTags.Healthy));
 
        foreach (var healthyPerson in healthyPersons)
        {
            var distance = Vector3.Distance(transform.position, healthyPerson.transform.position);
            if(distance <= closeDistance)
            {
                closeDistance = distance;
                targetPerson = healthyPerson.GetComponent<Person>();
            }
        }
        return targetPerson;
    }

    public static string GetTag(PersonTags personTags)
    {
        switch (personTags)
        {
            case PersonTags.Healthy:
                return "Healthy Person";
            case PersonTags.Infected:
                return "Infected Person";
            case PersonTags.Dead:
                return "Dead Person";
            default:
                return "None";
        }
    }
}