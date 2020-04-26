using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PersonMovementController : MonoBehaviour
{
    public NavMeshAgent agent;

    public float agentSpeed;
    
    public Person person;
    
    public Vector2 groundDimention;
    
    private Vector3 _destination;
    private bool _destinationReached;

    // Start is called before the first frame update
    private void Start()
    {
        _destination = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CheckDestinationReached())
        {
            Move();
        }
    }
    
    private void Move()
    {
        // if the person is infected the speed will be updated
        agent.speed = agentSpeed;
        
        var z = groundDimention.y;
        var x = groundDimention.x;
        var y = transform.position.y;

        _destinationReached = false;
        
        _destination = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        agent.SetDestination(_destination);
    }

    private bool CheckDestinationReached()
    {
        const float tolerance = 0.5f;
        return Mathf.Abs(Vector3.Distance(transform.position, _destination)) <= tolerance;
    }
}
