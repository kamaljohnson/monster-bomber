using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{

    public int initialPersonCount;

    public Vector2 groundDiamention;

    public List<GameObject> listOfPersons;

    private static PersonSpawner _spawner;
    
    private void Start()
    {
        _spawner = this;
        
        GetPersonSpawnCountFromPref();
        SpawnPersons(initialPersonCount);
    }
    
    public static void SpawnExtraPersons(int count)
    {
        _spawner.initialPersonCount += count;
        SetPersonSpawnCountToPref();
        SpawnPersons(count);
    }

    private static void GetPersonSpawnCountFromPref()
    {
        if (PlayerPrefs.HasKey("InitialPersonCount"))
        {
            _spawner.initialPersonCount = PlayerPrefs.GetInt("InitialPersonCount");
        }
        else
        {    
            SetPersonSpawnCountToPref();
        }
    }

    private static void SetPersonSpawnCountToPref()
    {
        
    }
    
    public static void SpawnPersons(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var person = Instantiate(_spawner.listOfPersons[Random.Range(0, _spawner.listOfPersons.Count)]);
            var z = _spawner.groundDiamention.y;
            var x = _spawner.groundDiamention.x;
            var y = _spawner.transform.position.y;

            person.transform.position = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        }        
    }

    public static void Reset()
    {
        var deadPersons = GameObject.FindGameObjectsWithTag(Person.GetTag(PersonTags.Dead));
        foreach (var deadPerson in deadPersons)
        {
            Destroy(deadPerson.gameObject);
            SpawnPersons(1);
        }
    }
}
