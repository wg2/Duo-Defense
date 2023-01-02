using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTexture : MonoBehaviour
{
    public int numberOfWallsTouching;
    public Sprite fullHealth;
    public Sprite threeQuartersHealth;
    public Sprite halfHealth;
    public Sprite oneQuarterHealth;
    private SpriteRenderer wallImage;
    private PlayerWall wallScript;
    public GameObject explosion;
    private int wallType;
    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.IgnoreCollision(GameObject.Find("KeyboardPlayer").GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        wallImage = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(6,6,1);
        //transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        try 
        {
            wallScript = gameObject.transform.parent.GetComponent<PlayerWall>();
        } catch {} 

        wallType = wallScript.wallType;
        if (wallType == 1) // get alternate textures if applicable
        {
            fullHealth = Resources.Load<Sprite>("explosive_75-100");
            threeQuartersHealth = Resources.Load<Sprite>("explosive_50-75");
            halfHealth = Resources.Load<Sprite>("explosive_25-50");
            oneQuarterHealth = Resources.Load<Sprite>("explosive_0-25");
            wallImage.sprite = fullHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Wall collided with " + collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
            transform.parent.GetComponent<PlayerWall>().damageWall();
    }

    private void OnTriggerEnter2D(Collider2D collision) // wall explodes when in contact with explosive wall
    {
        //Debug.Log("Wall triggered with " + collision.gameObject);
        if (transform.parent.GetComponent<PlayerWall>().getWallType() == 1 && collision.gameObject.CompareTag("Explosion") && transform.parent.GetComponent<PlayerWall>().getExplosionStatus() == false)
        {
            transform.parent.GetComponent<PlayerWall>().WallExplodes();
        }
    }

    public void ChangeSprite()
    {
        gameObject.SetActive(true);
        float wallHealth = wallScript.getWallHealth();
        if (wallHealth > 0.75)
        {
            wallImage.sprite = fullHealth;
        } else if (wallHealth > 0.5)
        {
            wallImage.sprite = threeQuartersHealth;
        } else if (wallHealth > 0.25)
        {
            wallImage.sprite = halfHealth;
        } else 
        {
            wallImage.sprite = oneQuarterHealth;
        }
    }
}
