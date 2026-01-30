using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("[TestInteractable] Interactable object has been interacted with: " + gameObject.name);
    }
}