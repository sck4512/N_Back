using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MessageBoxAction : MonoBehaviour
{
    [SerializeField] float biggingTime = 1f;
    [SerializeField] Image[] images;
    RectTransform rectTransform;
    Vector3 startScale;

    void Awake()
    {
        Application.targetFrameRate = 45;

        rectTransform = GetComponent<RectTransform>();

        startScale = rectTransform.localScale;
        //images = GetComponentsInChildren<Image>();
    }

    void OnEnable()
    {
        //rectTransform.localScale *= 0.01f;
        ////컬러 알파값 0.3으로 낮춤
        //for (int i = 0; i < images.Length; i++)
        //{
        //    var color = images[i].color;
        //    color.a = 0.1f;
        //    images[i].color = color;
        //}

        //StartCoroutine(BiggingEvent(biggingTime, 0.1f));

        StartCoroutine(DoSizeEffectRoutine());
    }


    //커지면서 알파값 올림
    IEnumerator BiggingEvent(float _BiggingTime, float _SmallBiggingTime)
    {
        float time = 0f;
        Vector3 addVec = startScale - rectTransform.localScale;
        addVec *= (1 / _BiggingTime);
        float addColorAlpha = 1 - images[0].color.a;
        addColorAlpha *= (1 / _BiggingTime);
        addColorAlpha *= 0.68f; //너무 빨라서 약간 맞춰줌

        while (time < _BiggingTime + _SmallBiggingTime)
        {
            time += Time.deltaTime;
            if (rectTransform.localScale.x < startScale.x + 0.1f)
                rectTransform.localScale += addVec * Time.deltaTime;
            for (int i = 0; i < images.Length; i++)
            {
                var color = images[i].color;
                color.a += addColorAlpha * Time.deltaTime;
                images[i].color = color;
            }

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < images.Length; i++)
        {
            var color = images[i].color;
            color.a = 1f;
            images[i].color = color;
        }

        time = 0f;
        while (time < _SmallBiggingTime)
        {
            time += Time.deltaTime;

            if (rectTransform.localScale.x > startScale.x && rectTransform.localScale.y > startScale.y)
                rectTransform.localScale -= addVec * Time.deltaTime * 0.3f;
            else
                break;
            yield return new WaitForEndOfFrame();
        }

        rectTransform.localScale = startScale;



        //StartCoroutine(DoSizeEffectRoutine());
    }



    IEnumerator DoSizeEffectRoutine()
    {
        var originPos = transform.localScale;
        transform.localScale = new Vector3(originPos.x * 0.98f, originPos.y * 0.98f, 1f);
        float time = 0f;
        while (time < 0.1f)
        {
            time += Time.fixedDeltaTime;
            transform.localScale += Vector3.right * Time.fixedDeltaTime * 0.46f + Vector3.up * Time.fixedDeltaTime * 0.46f;
            yield return new WaitForFixedUpdate();
        }


        time = 0f;
        while (time < 0.03f)
        {
            time += Time.fixedDeltaTime;
            transform.localScale += Vector3.left * Time.fixedDeltaTime * 0.7f + Vector3.down * Time.fixedDeltaTime * 0.7f;
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = originPos;
    }
}
