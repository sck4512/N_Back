using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool IsShaking { get; private set; } = false;


    public void Shake(float _Time, float _Range)
    {
        IsShaking = true;
        StartCoroutine(ShakeRoutine(_Time, _Range));
    }

    IEnumerator ShakeRoutine(float _Time, float _Range)
    {
        //������ ������ �����Ǿ� �ִ� ��� = ���� OK

        //�ٸ� ��ġ�� �����Ǿ� �ִ� ��� = �ణ�� �ڵ�������� ����

        //�����̴� ���


        float time = 0f;
        var startPos = transform.position;
        while (time < _Time)
        {
            time += Time.fixedDeltaTime;
            var randomPos = new Vector3(Random.Range(-1, 1) * _Range, Random.Range(-1f, 1f) * _Range, transform.position.z);
            transform.position = randomPos;
            yield return new WaitForFixedUpdate();
        }
        transform.position = startPos;
        IsShaking = false;
    }
}
