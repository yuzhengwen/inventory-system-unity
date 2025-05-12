using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInteractible : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = Color.green; // Change color to indicate interaction
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = Color.white; // Reset color when not interacting
        }
    }
}