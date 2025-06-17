using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum PickupType { DoubleJump, Dash}

    [Header("Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject DJCard;
    [SerializeField] private GameObject dashCard;
    [SerializeField] private PickupType cardType;

    public PlayerMove Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(Player == null)
            {
                Debug.LogWarning("Player component not found!");
                return;
            }

            switch (cardType)
            {
                case PickupType.DoubleJump:
                    Player.UnlockDoubleJump();
                    gameObject.SetActive(false);
                    break;
                case PickupType.Dash:
                    Player.UnlockDash();
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
