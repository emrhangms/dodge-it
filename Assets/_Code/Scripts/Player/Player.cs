using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player ins;

    public GameObject MSH_Player;
    public GameObject DeadParticles;
    public TrailRenderer[] tireMarks;
    public bool tireMarksFlag;

    public float speed;
    public float damage;
    public float rotationPower;

    private Vector2 mouseOldPosition;
    private Vector3 diffVector;
    public Sequence moveForward;
    public Sequence rotateMesh;

    public GameObject PrefWinParticle;
    public GameObject PrefAllFruitExplosions;
    public GameObject PrefBoxPileExplosion;
    public GameObject PrefCoinExplosion;

    public bool win = false;
    public bool diedOnce;
    public int score;

    private void Awake() => ins = this;

    void Start()
    {
        rotationPower = 10;

        MoveForward();
        RotateMesh();
    }

    public Timer scoreTimer = new Timer();

    void Update()
    {
        Move();
        CheckDrift();

        if (scoreTimer.IsFinished().Item2)
        {
            scoreTimer.StartCountDown();
            score += 100;
        }
    }

    public void Move()
    {
        Vector2 curViewPort = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            mouseOldPosition = curViewPort;
        }
        else if (Input.GetMouseButton(0))
        {
            diffVector = -1 * Time.deltaTime * new Vector3(mouseOldPosition.x - curViewPort.x, 0, 0);
            transform.position += diffVector;

            mouseOldPosition = curViewPort;
        }

        if (Input.GetMouseButtonUp(0))
            diffVector = Vector3.zero;
    }

    public void MoveForward()
    {
        moveForward = DOTween.Sequence()
            .Append(transform.DOMoveZ(transform.position.z + speed, 1, false).SetEase(Ease.Linear))
            .AppendCallback(MoveForward);
    }

    public void RotateMesh()
    {
        Vector3 rotation = new Vector3(0, rotationPower, 0);

        rotateMesh = DOTween.Sequence()
          .Append(MSH_Player.transform.DORotate(rotation * diffVector.normalized.x, 0.07f, RotateMode.Fast))
          .AppendCallback(RotateMesh);
    }

    #region ------------------| Car Wheels Trail Effect
    public void CheckDrift()
    {
        if (diffVector.x >= 0.2 || diffVector.x <= -0.2)
            StartEmitter();
        else
            StopEmitter();
    }

    public void StartEmitter()
    {
        if (tireMarksFlag) return;
        foreach (var T in tireMarks)
            T.emitting = true;

        tireMarksFlag = true;
    }

    public void StopEmitter()
    {
        if (!tireMarksFlag) return;
        foreach (var T in tireMarks)
            T.emitting = false;

        tireMarksFlag = false;
    }
    #endregion

    public void WinGame(bool winValue)
    {
        if (!win)
        {
            win = winValue;
            moveForward.Kill();
            speed = 100;
            MoveForward();

            Instantiate(PrefWinParticle, transform.position, Quaternion.identity);

            CameraController.ins.VCam.Follow = null;
        }
    }

    public void Dead()
    {
        if (!diedOnce)
        {
            CanvasManager.ins.LoseGame();

            diedOnce = true;
            moveForward.Kill();
            rotateMesh.Kill();

            speed = 0;
            DeadParticles.SetActive(true);
            MSH_Player.transform.DORotate(new Vector3(0, 0, 120), 0.5f, RotateMode.Fast);

            Vector3 jumpValue = new Vector3(0, 1, -1);
            MSH_Player.transform.DOJump(transform.position + jumpValue, 3, 1, 0.4f, false);
            transform.DOJump(transform.position + jumpValue, 3, 1, 0.4f, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Police"))
            other.GetComponent<Police>().Push(damage, transform.position);

        if (other.CompareTag("Fruit"))
        {
            Destroy(other.gameObject);
            Instantiate(PrefAllFruitExplosions, other.transform.position, Quaternion.identity);
            CameraController.ins.ShakeCamera();
            score -= 500;
        }

        if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            Instantiate(PrefBoxPileExplosion, other.transform.position, Quaternion.identity);
            CameraController.ins.ShakeCamera();
            score -= 1000;
        }

        if (other.CompareTag("Gold"))
        {
            Destroy(other.gameObject);
            Instantiate(PrefCoinExplosion, other.transform.position, Quaternion.Euler(-90, 0, 0));
            score += 500;
        }

        if (other.CompareTag("KillTrigger"))
        {
            Dead();

            foreach (var police in Police.Polices)
                police.moveSequance.Kill();
        }

        if (other.CompareTag("Truck"))
        {
            Dead();

            foreach (var police in Police.Polices)
                police.moveSequance.Kill();
        }
    }
}
