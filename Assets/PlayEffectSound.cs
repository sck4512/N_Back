using UnityEngine;
using UnityEngine.EventSystems;

public class PlayEffectSound : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] AudioClip effectSound;
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlayEffectSound(effectSound);
    }
}
