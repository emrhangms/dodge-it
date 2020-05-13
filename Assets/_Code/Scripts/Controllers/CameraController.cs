using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController ins;

    private Quaternion m_rotation;
    public CinemachineVirtualCamera VCam;

    private void Awake() => ins = this;

    void Start()
    {
        m_rotation = transform.rotation;
    }

    void Update()
    {

    }

    public void ShakeCamera(float time = 0.1f)
    {
        DOTween.Sequence()
            .Append(transform.DOShakeRotation(time, 6f, 7, 50f, true))
            .AppendCallback(() => transform.DORotate(m_rotation.eulerAngles, 0.1f, RotateMode.Fast));
    }
}
