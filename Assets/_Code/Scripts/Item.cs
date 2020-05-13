using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void Jump()
    {
        Vector3 jumpValue = new Vector3(-1, transform.position.y, 2);
        transform.DOJump(transform.position + jumpValue, 3, 1, 0.1f, false);

        Vector3 rotationValue = new Vector3(90, 0, 45);
        transform.DORotate(rotationValue, 0.1f, RotateMode.Fast);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Jump();
    }
}
