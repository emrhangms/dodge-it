using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CrewPerson : MonoBehaviour
{

    public Police closestTarget;
    public Timer hitTimer = new Timer();
    public float damage;

    public bool canAttack = true;

    public GameObject gun;

    void Start()
    {

    }

    void Update()
    {

        if (Police.Polices.Count > 0 && CanvasManager.ins.started && canAttack && !Player.ins.diedOnce)
        {
            closestTarget = FindClosestTarget();
            transform.LookAt(closestTarget.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            HitTarget();
        }

        if (Player.ins.diedOnce)
            CloseGun();
    }

    public void HitTarget()
    {
        if (hitTimer.IsFinished().Item2)
        {
            hitTimer.StartCountDown();
            closestTarget.GetDamage(damage);
        }
    }

    public void CloseGun() => gun.SetActive(false);

    public Police FindClosestTarget()
    {
        Vector3 position = transform.position;
        return Police.Polices.OrderBy(go => (position - go.transform.position).sqrMagnitude).First();
    }
}
