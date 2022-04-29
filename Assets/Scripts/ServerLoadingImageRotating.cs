using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerLoadingImageRotating : MonoBehaviour
{
    float rotatingSpeed = 330f;
    Transform myTransform;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        myTransform.rotation = Quaternion.identity;
        StartCoroutine(RotatingRoutine());
    }

    void OnDisable()
    {
        StopCoroutine(RotatingRoutine());
    }

    //�ð�������� ��� ȸ��
    IEnumerator RotatingRoutine()
    {
        while (true)
        {
            myTransform.Rotate(Vector3.forward, -0.02f * rotatingSpeed);
            yield return YieldContainer.GetWaitForSecondsRealtime(0.02f);
        }
    }
}
