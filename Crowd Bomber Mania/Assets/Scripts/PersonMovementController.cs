using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PersonMovementController : MonoBehaviour
{
    public NavMeshAgent agent;

    private const float AgentWalkSpeed = 2f;
    private const float HealthyAgentRunSpeed = 2.5f;
    private static float _infectedAgentRunSpeed = 3f;
    
    public Vector2 groundDimention;
    
    private Vector3 _destination;

    public Person targetPerson;
    public Person chasingPerson;

    private bool _noHealthyPersonNearby;

    public bool isChasedByInfectedPerson;

    // Start is called before the first frame update
    private void Start()
    {
        agent.speed = AgentWalkSpeed;
        _destination = transform.position;
        
        GetInfectedAgentSpeedFromPref();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CompareTag(Person.GetTag(PersonTags.Contagious)) && !_noHealthyPersonNearby)
        {
            ChaseNearestHealtyPerson();
            return;
        }

        if (CompareTag(Person.GetTag(PersonTags.Healthy)) && isChasedByInfectedPerson)
        {
            MoveAwayFromInfectedPerson();
        }
        
        if (CheckDestinationReached())
        {
            Move();
        }
    }

    private void ChaseNearestHealtyPerson()
    {
        if (targetPerson == null)
        {
            targetPerson = gameObject.GetComponent<Person>().GetNearestHealthyPerson();
        } 
        else if (!targetPerson.CompareTag(Person.GetTag(PersonTags.Healthy)))
        {
            targetPerson = gameObject.GetComponent<Person>().GetNearestHealthyPerson();
        }

        if (targetPerson == null)
        {
            _noHealthyPersonNearby = true;
            TriggerNormalMode();
        }
        else
        {
            _destination = targetPerson.transform.position;
            agent.SetDestination(_destination);
            TriggerChasingMode();
        }
    }

    private void MoveAwayFromInfectedPerson()
    {
        if (chasingPerson == null)
        {
            TriggerNormalMode();
            return;
        }
        _destination = chasingPerson.transform.position * -1;
    }
    
    private void Move()
    {
        var z = groundDimention.y;
        var x = groundDimention.x;
        var y = transform.position.y;

        _destination = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        agent.SetDestination(_destination);
    }

    // Healthy Person
    public void TriggerChasingMode(Person chasingPerson)
    {
        isChasedByInfectedPerson = true;
        this.chasingPerson = chasingPerson;
        agent.speed = HealthyAgentRunSpeed;
    }

    // Infected Person
    public void TriggerChasingMode()
    {
        agent.speed = _infectedAgentRunSpeed;
    }

    
    public void TriggerNormalMode()
    {
        isChasedByInfectedPerson = false;
        agent.speed = AgentWalkSpeed;
    }
    
    private bool CheckDestinationReached()
    {
        const float tolerance = 1f;
        return agent.remainingDistance <= tolerance;
    }

    public static void UpdatePersonSpeed()
    {
        // this ensures a less increase rate
        _infectedAgentRunSpeed += 0.05f;
        SetInfectedSpeedToPref();
    }
    
    private static void SetInfectedSpeedToPref()
    {
        PlayerPrefs.SetFloat("InfectedAgentRunSpeed", _infectedAgentRunSpeed);
    }
    
    private static void GetInfectedAgentSpeedFromPref()
    {
        if (PlayerPrefs.HasKey("InfectedAgentRunSpeed"))
        {
            _infectedAgentRunSpeed = PlayerPrefs.GetFloat("InfectedAgentRunSpeed");
        }
        else
        {
            SetInfectedSpeedToPref();
        }
    }
}
