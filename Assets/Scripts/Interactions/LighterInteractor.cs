using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LighterInteractor : MonoBehaviour
{
    [Header("XR")]
    public XRGrabInteractable grabInteractable;

    [Header("Flame")]
    public GameObject lighterFlame;

    [Header("Ray")]
    public float igniteDistance = 0.15f;

    [Header("Scripts ref")]
    public Scene1_UIManager uiManager;
    public CandleLightEffect candleLightEffect;


    private bool lighterPicked = false;

    private void Awake()
    {
        grabInteractable.activated.AddListener(OnTriggerPressed);
        grabInteractable.deactivated.AddListener(OnTriggerReleased);
        grabInteractable.selectEntered.AddListener(OnPickedUp);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grabInteractable.activated.RemoveListener(OnTriggerPressed);
        grabInteractable.deactivated.RemoveListener(OnTriggerReleased);
        grabInteractable.selectEntered.RemoveListener(OnPickedUp);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    // =========================
    // GRAB EVENTS
    // =========================
    private void OnPickedUp(SelectEnterEventArgs args)
    {
        if (lighterPicked) return;

        lighterPicked = true;

        if (uiManager != null)
            uiManager.OnLighterPicked();
        
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (lighterFlame != null)
            lighterFlame.SetActive(false);
    }

    // =========================
    // TRIGGER EVENTS
    // =========================
    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if (lighterFlame == null) return;

        AudioManager.Instance.PlaylighterClick();
        StartCoroutine(EnableFlameAfterDelay(1f));
    }

    IEnumerator EnableFlameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lighterFlame.SetActive(true);
    }


    private void OnTriggerReleased(DeactivateEventArgs args)
    {
        if (lighterFlame.activeInHierarchy== true)
            lighterFlame.SetActive(false);
    }

    // =========================
    // IGNITION CHECK
    // =========================
    private void Update()
    {
        if (lighterFlame == null || !lighterFlame.activeSelf)
            return;

        Ray ray = new Ray(lighterFlame.transform.position, lighterFlame.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, igniteDistance))
        {
            if (hit.collider.gameObject.name == "Wick" )
            {
                hit.collider.transform.GetChild(0).gameObject.SetActive(true);

                    if (uiManager != null)
                        uiManager.OnCandleLit();
               
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (lighterFlame == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(
            lighterFlame.transform.position,
            lighterFlame.transform.forward * igniteDistance
        );
    }
#endif
}
