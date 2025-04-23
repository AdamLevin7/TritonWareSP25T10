using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiHoverSound);
        FMODUnity.RuntimeManager.GetBus("bus:/SFX").getVolume(out float volume);
    }
}
