using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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

    public List<Object> listOfObjectsToBeCleanedAfterDeath;

    private static ulong _personCash = 10;
    private const float PersonCashMultiplier = 0.5f;

    public InfectedDeathBar deathBar;

    public GameObject personCashNotificationObject;

    public GameObject healthPersonModel;
    public GameObject infectedPersonModel;

    public GameObject currentPersonModel;
    
    private void Start()
    {
        LoadPersonCashFromPref();
        UpdatePersonModel();
    }

    public void TriggerInfection()
    {
        if(!gameObject.CompareTag(GetTag(PersonTags.Healthy))) return;

        gameObject.tag = GetTag(PersonTags.Infected);

        GameProgressManager.UpdateProgress();

        gameObject.GetComponent<PersonMovementController>().TriggerChasingMode();
        UpdatePersonModel();
        
        var notificationObj = Instantiate(personCashNotificationObject, transform.position, transform.rotation);
        notificationObj.GetComponent<PersonCashNotification>().SetCashAmount(_personCash);
        Destroy(notificationObj, 0.5f);
        
        CashManager.AddOrRemoveCash(_personCash);

        // this starts the death timer, after which the person dies due to the infection
        deathBar.barLifeTime = infectionToDeathDuration;
        deathBar.Start();
        
        StartCoroutine(TriggerDeathTimer());
    }

    public void TriggerDeath()
    {
        gameObject.tag = GetTag(PersonTags.Dead);
        currentPersonModel.GetComponent<PersonAnimationController>().PlayAnimation(AnimationType.Die);
        
        GameManager.ReportPersonDead();
        CleanupShit();
    }

// the collider is enabled if the person is infected
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GetTag(PersonTags.Infected)) || other.CompareTag("CannonBall"))
        {
            TriggerInfection();
        };
        
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
            case PersonTags.Dead:
                return "Dead Person";
            default:
                return "None";
        }
    }

    private static void LoadPersonCashFromPref()
    {
        if (PlayerPrefs.HasKey("PersonCash"))
        {
            _personCash = Convert.ToUInt64(PlayerPrefs.GetString("PersonCash"));
        }
        else
        {
            SetPersonCashToPref();
        }
    }

    private static void SetPersonCashToPref()
    {
        PlayerPrefs.SetString("PersonCash", "" + _personCash);
    }

    public static void UpdatePersonCash()
    {
        _personCash += (ulong) (_personCash * PersonCashMultiplier);
        SetPersonCashToPref();
    }

    public void UpdatePersonModel()
    {
        if (gameObject.CompareTag(GetTag(PersonTags.Healthy)))
        {
            currentPersonModel = healthPersonModel;
            infectedPersonModel.SetActive(false);
        }
        else
        {
            currentPersonModel = infectedPersonModel;
            healthPersonModel.SetActive(false);
        }
        currentPersonModel.SetActive(true);
    }
}