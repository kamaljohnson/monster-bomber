using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public enum PersonTags
{
    Healthy,
    Infected,
    Dead
}

public class Person : MonoBehaviour
{
    // after the infection duration the peron will get killed
    public float infectionToDeathDuration;

    public Material infectedMaterial;
    public Material deadMaterial;

    public List<Object> listOfObjectsToBeCleanedAfterDeath;

    public int personCash;
    public float personCashMultiplier;

    public InfectedDeathBar deathBar;

    private void Start()
    {
        GetPersonCash();
    }

    public void TriggerInfection()
    {
        if(!gameObject.CompareTag(GetTag(PersonTags.Healthy))) return;
        
        gameObject.tag = GetTag(PersonTags.Infected);
        
        gameObject.GetComponent<PersonMovementController>().TriggerChasingMode();
        gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;

        CashManager.AddOrRemoveCash(personCash);

        // this starts the death timer, after which the person dies due to the infection
        deathBar.barLifeTime = infectionToDeathDuration;
        deathBar.Start();
        
        StartCoroutine(TriggerDeathTimer(infectionToDeathDuration));
    }

    public void TriggerDeath()
    {
        gameObject.tag = GetTag(PersonTags.Dead);
        gameObject.GetComponent<MeshRenderer>().material = deadMaterial;
        GameManager.ReportPersonDead();
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

        if (targetPerson != null)
        {
            targetPerson.GetComponent<PersonMovementController>().TriggerChasingMode(this);
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

    private void GetPersonCash()
    {
        if (PlayerPrefs.HasKey("PersonCash"))
        {
            personCash = PlayerPrefs.GetInt("PlayerCash");
        }
        else
        {
            PlayerPrefs.SetInt("PersonCash", personCash);
        }
    }

    public static void UpdatePersonCash()
    {
        var currentPersonCash = PlayerPrefs.GetInt("PersonCash");
        currentPersonCash = (int) (currentPersonCash * 0.5f);
        PlayerPrefs.SetInt("PersonCash", currentPersonCash);
    }
}