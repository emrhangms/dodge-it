using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController ins;

    public GameObject starterRoad;

    [Header("Creation of Road")]
    public GameObject parentObj;
    public List<GameObject> roads = new List<GameObject>();

    [Header("After Creation of Road")]
    public List<GameObject> activeRoads = new List<GameObject>();
    public int maxRoadCount = 3;

    private void Awake() => ins = this;

    void Start()
    {
        activeRoads.Add(starterRoad);
    }

    void Update()
    {

    }

    public void CreateRoad(Vector3 position)
    {
        DeleteRoad();

        int rand = Random.Range(0, roads.Count);

        GameObject road = Instantiate(roads[rand], position, Quaternion.identity);
        road.transform.parent = parentObj.transform;
        activeRoads.Add(road);
    }

    public void DeleteRoad()
    {
        if (activeRoads.Count >= maxRoadCount)
        {
            GameObject deleteRoad = activeRoads[0].gameObject;
            activeRoads.RemoveAt(0);
            Destroy(deleteRoad);
        }
    }
}
