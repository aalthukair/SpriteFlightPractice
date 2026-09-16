using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    
    public float minSize = 1f;
    public float maxSize = 2.5f;
    
    public float minSpeed = 100f;
    public float maxSpeed = 200f;

    public float maxSpinSpeed = 7f;
    Rigidbody2D rb;
    
    public GameObject bounceEffectPrefab;
    
    void Start()
    {
        float randScale = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randScale, randScale, 1);
        
        rb = GetComponent<Rigidbody2D>();
        
        float randSpeed = Random.Range(minSpeed, maxSpeed) / randScale;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randSpeed);
        
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point; 
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
        
        Destroy(bounceEffect, 1f);
    }
    
    void Update()
    {
        if (rb.linearVelocity.magnitude > 20)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * 10;
        }
    }
}
