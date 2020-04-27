using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PersonSpawner : MonoBehaviour
{

    public int initialPersonCount;

    public Vector2 groundDiamention;

    public List<GameObject> listOfPersons;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("InitialPersonCount"))
        {
            initialPersonCount = PlayerPrefs.GetInt("InitialPersonCount");
        }
        else
        {
            PlayerPrefs.SetInt("InitialPersonCount", initialPersonCount);
        }
        
        SpawnPersons(initialPersonCount);
    }
    
    public void SpawnExtraPersons(int count)
    {
        initialPersonCount += count;
        PlayerPrefs.SetInt("InitialPersonCount", initialPersonCount);
        SpawnPersons(count);
    }

    public void SpawnPersons(int count)
    {
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
