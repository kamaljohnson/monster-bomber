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
    Contagious,
    Dead
}

public class Person : MonoBehaviour
{
    // after the infection duration the peron will get killed
    public float infectionToDeathDuration;
    public float infectionActivationDuration;

    public Material infectedMaterial;
    public Material deadMaterial;

    public List<Object> listOfObjectsToBeCleanedAfterDeath;

    private static int _personCash = 10;
    private const float PersonCashMultiplier = 0.5f;

    public InfectedDeathBar deathBar;

    private void Start()
    {
        GetPersonCashFromPref();
    }

    public void TriggerInfection()
    {
        if(!gameObject.CompareTag(GetTag(PersonTags.Healthy))) return;

        gameObject.tag = GetTag(PersonTags.Contagious);
        
        gameObject.GetComponent<PersonMovementController>().TriggerChasingMode();
        gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;

        CashManager.AddOrRemoveCash(_personCash);

        // this starts the death timer, after which the person dies due to the infection
        deathBar.barLifeTime = infectionToDeathDuration;
        deathBar.Start();
        
        StartCoroutine(TriggerDeathTimer());
    }

    public void TriggerDeath()
    {
        gameObject.tag = GetTag(PersonTags.Dead);
        gameObject.GetComponent<MeshRenderer>().material = deadMaterial;
        GameManager.ReportPersonDead();
        CleanupShit();
    }

// the collider is enabled if the person is infected
    public void OnTriggerStay(Collider other)
    {
        var otherTag = other.tag;
        if (otherTag != GetTag(PersonTags.Infected) && otherTag != GetTag(PersonTags.Contagious)) return;
        
        StartCoroutine(TriggerInfectionTimer());
    }
    
    IEnumerator TriggerInfectionTimer()
    {
        gameObject.tag = GetTag(PersonTags.Infected);
        yield return new WaitForSeconds(infectionActivationDuration);
 
        // Code to execute after the delay
        TriggerInfection();
    }
    
    IEnumerator TriggerDeathTimer()
    {
        yield return new WaitForSeconds(infectionToDeathDuration);
 
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
            case PersonTags.Contagious:
                return "Contagious Person";
            case PersonTags.Dead:
                return "Dead Person";
            default:
                return "None";
        }
    }

    private static void GetPersonCashFromPref()
    {
        if (PlayerPrefs.HasKey("PersonCash"))
        {
            _personCash = PlayerPrefs.GetInt("PersonCash");
        }
        else
        {
            SetPersonCashToPref();
        }
    }

    private static void SetPersonCashToPref()
    {
        PlayerPrefs.SetInt("PersonCash", _personCash);
    }

    public static void UpdatePersonCash()
    {
        _personCash += (int) (_personCash * PersonCashMultiplier);

        SetPersonCashToPref();
    }
}