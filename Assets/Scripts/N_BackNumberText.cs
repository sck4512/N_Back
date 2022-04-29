using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class N_BackNumberText : MonoBehaviour
{
    Graphic graphic;
    Color startColor;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
        startColor = graphic.color;
    }

    private void OnEnable()
    {
        graphic.color = startColor;
    }

    public void ChangeColor(Color _ChangeColor)
    {
        StartCoroutine(ChangeColorRoutine(_ChangeColor));
    }

    IEnumerator ChangeColorRoutine(Color _ChangeColor)
    {
        graphic.color = _ChangeColor;

        while (MJ.MyTool.GetMagnitude(graphic.color - startColor) > 0.2f)
        {
            graphic.color = Color.Lerp(graphic.color, startColor, Time.deltaTime * 8f);
            yield return null;
        }

        graphic.color = startColor;
    }
}
