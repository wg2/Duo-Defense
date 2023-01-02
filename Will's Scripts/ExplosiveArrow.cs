using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : MonoBehaviour
{
    public int damage;
    public float speed;
    public GameObject explosion;
    public List<GameObject> explosions;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position += transform.up * 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 10 || transform.position.x < -10 || transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(gameObject);
        }
        transform.position += transform.up * Time.deltaTime * speed;
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.collider.gameObject;
        if (hit.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Enemy>().DealDamage(damage);
        }
        Explode();
        Destroy(gameObject);
    }*/
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (!hit.gameObject.CompareTag("Money") && !hit.gameObject.CompareTag("Explosive Arrow Drop") && !hit.gameObject.CompareTag("Multi Arrow Drop") && !hit.gameObject.CompareTag("Basic Health Jug") && !hit.gameObject.CompareTag("Player") && !hit.gameObject.CompareTag("WallTexture")) // explodes when colliding with all objects except the following
        {
            if (hit.CompareTag("Enemy"))
            {
                try 
                {
                    hit.GetComponent<Enemy>().DealDamage(damage);
                } catch {}
            }
            Explode();
            Destroy(gameObject);
        }
    }
    private void Explode()
    {
        Instantiate(explosions[Random.Range(0, explosions.Count)], gameObject.transform.position, gameObject.transform.rotation);
    }
}
