using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneateHitCollider : MonoBehaviour
{
    public GameObject GeneratePoint;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.ins.CreateRoad(GeneratePoint.transform.position);
        }
    }
}
