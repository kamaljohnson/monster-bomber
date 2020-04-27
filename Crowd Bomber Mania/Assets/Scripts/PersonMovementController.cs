using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PersonMovementController : MonoBehaviour
{
    public NavMeshAgent agent;

    public float agentSpeed;
    
    public Vector2 groundDimention;
    
    private Vector3 _destination;

    public Person targetPerson;

    private bool _noHealthyPersonNearby = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        agent.speed = agentSpeed;
        _destination = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CompareTag(Person.GetTag(PersonTags.Infected)) && !_noHealthyPersonNearby)
        {
            MoveToNearestHealthyPerson();   
            return;
        }
        
        if (CheckDestinationReached())
        {
            Move();
        }
    }

    private void MoveToNearestHealthyPerson()
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
        }
        else
        {
            _destination = targetPerson.transform.position;
            agent.SetDestination(_destination);
        }
    }
    
    private void Move()
    {
        var z = groundDimention.y;
        var x = groundDimention.x;
        var y = transform.position.y;

        _destination = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        agent.SetDestination(_destination);
    }

    private bool CheckDestinationReached()
    {
        const float tolerance = 1f;
        return agent.remainingDistance <= tolerance;
    }
}
