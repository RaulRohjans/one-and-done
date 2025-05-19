using UnityEngine;

public enum DropType { Carrot, EvilCarrot, Shield, DoubleJump }

public class Drop : MonoBehaviour
{
    private float maxFallSpeed = -3f;
    public DropType type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Remove if it hits the ground
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<MainCharacterScript>().Pickup(this);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // Make the items fall a bit slower so the player has time to catch them
        if (GetComponent<Rigidbody2D>().linearVelocity.y < maxFallSpeed)
        {
            var v = GetComponent<Rigidbody2D>().linearVelocity;
            v.y = maxFallSpeed;
            GetComponent<Rigidbody2D>().linearVelocity = v;
        }
    }
}
