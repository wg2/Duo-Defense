using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    public int health;
    public int maxHealth;
    private GameObject wall;
    private GameObject currentWall;
    private LineRenderer lineRenderer;
    //private EdgeCollider2D edgeCollider;
    private GameObject[] wallTextures;
    public int wallType;
    public GameObject explosion;
    public bool explosionStatus;
    // Start is called before the first frame update
    void Start()
    {
        explosionStatus = false;
        gameObject.tag = "Wall"; // sets the tag to "wall". Helps enemies identify that they've hit a wall and should bounce back as a result
        lineRenderer = GetComponent<LineRenderer>();
        maxHealth = lineRenderer.positionCount; // saves the max health for opacity purposes
        health = lineRenderer.positionCount; // gets the number of vertices in the line and uses that as the health
        Vector3 previousPointPos = new Vector3(1000, 1000, 1000);
        float previousRotation = 0;
        wall = GameObject.Find("Wall");
        GetComponent<Renderer>().enabled = false; // hides black line
        wallTextures = new GameObject[health - 1]; // creates list of GameObjects to keep track of all child wall textures
        if (wallType == 1) // if explosive wall, get all animations
        {
            explosion = Resources.Load<GameObject>("Explosion 1");
            maxHealth /= 2; // explosive walls have a lot less health
            health = maxHealth;
            //Debug.Log("Explosion resource loaded");
        }
        for (int i = 0; i < lineRenderer.positionCount - 1; i++) // spawns in wall textures over top the points of the linerenderer
        { 
            // get the difference in x and y-values between the two points and each as a float
            float changeInY = (lineRenderer.GetPosition(i).y - previousPointPos.y);
            float changeInX = (lineRenderer.GetPosition(i).x - previousPointPos.x);

            // get the angle difference by using arctan of (difference_of_x/difference_of_y) and save as a float called rotation
            float rotation = Mathf.Abs(Mathf.Atan(changeInX / changeInY) * Mathf.Rad2Deg - 90);
            //Debug.Log("Current rotation:" + rotation + "\tPrevious rotation" + previousRotation);
            if (Mathf.Abs(rotation - previousRotation) > 100) // if the difference in wall rotation is too great, add 180
            {
                rotation += 180;
            }

            // instantiate the currentwall using the found rotation along the z-axis
            wallTextures[i] = Instantiate(wall, lineRenderer.GetPosition(i), Quaternion.Euler(0,0, rotation));
            wallTextures[i].transform.SetParent(transform); // sets the wall's parent object to the line. That way once the line disappears, so do the wall textures
            previousPointPos = lineRenderer.GetPosition(i);
            previousRotation = rotation;
        }
        try {
            Destroy(wallTextures[0]);
        } catch {}
        GetComponent<EdgeCollider2D>().enabled = false; // disable edge collider for line once wall textures are all in position
        //Debug.Log("Wall added with type " + wallType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damageWall()
    {
        Debug.Log("Damage Wall");
        health--;
        if (health <= 0) { // destroy self if health hits 0
            if (wallType == 1) // if explosive wall, spawn in an explosion along every node
            {
                WallExplodes();
            }
            Destroy(gameObject);
        }
        for (int i = 0; i < wallTextures.Length - 1; i++)
        {
            WallTexture wallTextureScript = gameObject.transform.GetChild(i).GetComponent<WallTexture>();
            wallTextureScript.ChangeSprite();
        }
    }

    public float getWallHealth() 
    {
        return (float) health / (float) maxHealth;
    }

    public void setWallType(int input) // assigns wallType to the newly instantiated wall
    {
        wallType = input;
    }

    public int getWallType()
    {
        return wallType; 
    }

    public void WallExplodes()
    {
        explosionStatus = true; // necessary so that method is not called many times by different wall textures
        for (int i = 0; i < lineRenderer.positionCount; i++) 
        {
            Instantiate(explosion, lineRenderer.GetPosition(i), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public bool getExplosionStatus() // informs wall textures in the same line if an explosion has started
    {
        return explosionStatus;
    }
}
