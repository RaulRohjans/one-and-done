using UnityEngine;

public class Drop : MonoBehaviour
{
    private float maxFallSpeed = -3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Remove object if it hits the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
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
