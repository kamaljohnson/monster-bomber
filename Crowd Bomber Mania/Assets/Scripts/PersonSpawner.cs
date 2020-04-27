using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{

    public int initSpawnCount;

    public Vector2 groundDiamention;

    public List<GameObject> listOfPersons;
    
    private void Start()
    {
        SpawnPerson(initSpawnCount);
    }
    
    public void SpawnPerson(int count) {
        for (var i = 0; i < count; i++)
        {
            var person = Instantiate(listOfPersons[Random.Range(0, listOfPersons.Count)]);
            var z = groundDiamention.y;
            var x = groundDiamention.x;
            var y = transform.position.y;

            person.transform.position = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        }
    }
}
