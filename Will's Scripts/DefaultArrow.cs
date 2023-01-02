using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultArrow : MonoBehaviour
{
    public float damage;
    public float speed;
    private KeyboardPlayerController keyboardPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
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
        //if (transform.position)
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.collider.gameObject;
        if (hit.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Enemy>().DealDamage(damage);
        }
        Destroy(gameObject);
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Arrow collided with " + collision.gameObject);
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Enemy"))
        {
            try 
            {
                hit.GetComponent<Enemy>().DealDamage(damage * keyboardPlayer.damageX);
            } catch {}
        }
        if (!hit.gameObject.CompareTag("Money") && !hit.gameObject.CompareTag("Explosive Arrow Drop") && !hit.gameObject.CompareTag("Multi Arrow Drop") && !hit.gameObject.CompareTag("Basic Health Jug") && !hit.gameObject.CompareTag("Explosion") && !hit.gameObject.CompareTag("WallTexture")&& !hit.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
    }



}
