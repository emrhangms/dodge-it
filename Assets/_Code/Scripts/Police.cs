using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Police : MonoBehaviour
{
    public static List<Police> Polices = new List<Police>();

    public float speed = 45;
    public float health = 100;
    public float pushDistance = 1;

    public GameObject MSH_Police;

    public GameObject PrefExplosion;
    public GameObject PrefSpark;
    public GameObject DeadParticles;
    public GameObject ParticlePosition;
    public GameObject PrefAllFruitExplosions;
    public GameObject PrefBoxPileExplosion;

    public GameObject HealthBarIndicator;
    public GameObject HealthBar;

    private Quaternion m_rotation;
    public float rotationPower;
    public Sequence moveSequance;

    public Timer pushTimer = new Timer();

    public int id;

    public bool pressed;
    public bool deadOnce;

    private void OnEnable() => Polices.Add(this);

    private void OnDestroy() => Polices.Remove(this);

    void Start()
    {
        health = 100;
        rotationPower = 15;

        m_rotation = transform.rotation;

        MoveForward();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && id == 1)
        {
            pressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && id == 0)
        {
            pressed = true;
        }

        if (Input.GetKey(KeyCode.R) && id == 1)
        {
            MoveToPlayer();
        }
        else if (Input.GetKey(KeyCode.E) && id == 0)
        {
            MoveToPlayer();
        }
    }

    public void MoveToPlayer()
    {
        Vector3 direction = -(transform.position - Player.ins.transform.position).normalized;
        transform.DOMoveX(transform.position.x + (direction.x * 0.3f), 0.2f, false);
    }

    public void MoveForward()
    {
        if (!pressed)
        {
            moveSequance = DOTween.Sequence()
                .Append(transform.DOMoveZ(transform.position.z + speed, 1, false).SetEase(Ease.Linear))
                .AppendCallback(MoveForward);
        }
        else
        {
            moveSequance = DOTween.Sequence()
                .Append(transform.DOMoveZ(Player.ins.transform.position.z + speed, 1, false).SetEase(Ease.Linear))
                .AppendCallback(MoveForward);
        }
    }

    public void Push(float damage, Vector3 playerPosition)
    {
        if (pushTimer.IsFinished().Item2)
        {
            pushTimer.StartCountDown();

            CameraController.ins.ShakeCamera();

            GetDamage(damage);


            Vector3 direction = (transform.position - playerPosition).normalized;
            Vector3 rotation = new Vector3(0, rotationPower, 0);

            DOTween.Sequence()
              .Append(transform.DOMoveX(transform.position.x + (direction.x * pushDistance), 0.1f, false))
              .Append(MSH_Police.transform.DORotate(rotation * direction.normalized.x, 0.07f, RotateMode.Fast))
              .AppendCallback(() => MSH_Police.transform.DORotate(m_rotation.eulerAngles, 0.1f, RotateMode.Fast));
            ;

            Instantiate(PrefSpark, transform.position, Quaternion.Euler(-80, 30 * direction.normalized.x, 0), transform);
        }
    }

    public void GetDamage(float damage)
    {
        if (health - damage <= 0)
            Dead();
        else
            health -= damage;

        UpdateHealthBar(health);
    }

    public void UpdateHealthBar(float health)
    {
        Vector3 healthBarLocalScale = HealthBar.transform.localScale;
        HealthBar.transform.localScale = new Vector3((health / 100) / 0.9f, healthBarLocalScale.y, healthBarLocalScale.z);

        if (health <= 40)
            HealthBar.transform.GetComponent<MeshRenderer>().material.DOColor(Color.red, 0.6f);
    }

    public void Dead()
    {
        if (!deadOnce)
        {
            deadOnce = true;
            Instantiate(PrefExplosion, transform.position, Quaternion.identity);

            DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, 0, -120), 0.5f, RotateMode.Fast))
                .Join(transform.DOJump(transform.position + new Vector3(0, 6, 0), 3, 1, 0.5f, false))
                .AppendCallback(() =>
                {
                    DeadParticles.SetActive(true);
                    speed = 0;
                    moveSequance.Kill();
                    Player.ins.score += 100;
                });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            Dead();
        }

        if (other.CompareTag("Fruit"))
        {
            Destroy(other.gameObject);
            Instantiate(PrefAllFruitExplosions, other.transform.position, Quaternion.identity);
            CameraController.ins.ShakeCamera(0.01f);
        }

        if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            Instantiate(PrefBoxPileExplosion, other.transform.position, Quaternion.identity);
            CameraController.ins.ShakeCamera(0.01f);
        }

    }
}
