using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    public GameBehavior gameManager;
    public Player player;
    private float speedBoost = 2f;
    private float seconds = 5f;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "DisguisedPlayer")
        {
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Speed Pickup collected!");
            player.SpeedBoost(speedBoost, seconds);
        }
    }
}
