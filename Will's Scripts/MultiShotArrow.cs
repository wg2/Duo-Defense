using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotArrow : MonoBehaviour
{
    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = transform.position;
        Quaternion rotation = transform.rotation;

        Vector3 angles = transform.rotation.eulerAngles;

        Instantiate(arrow, location, rotation);
        Instantiate(arrow, location, Quaternion.Euler(angles.x, angles.y, angles.z + 15));
        Instantiate(arrow, location, Quaternion.Euler(angles.x, angles.y, angles.z - 15));
        Instantiate(arrow, location, Quaternion.Euler(angles.x, angles.y, angles.z + 30));
        Instantiate(arrow, location, Quaternion.Euler(angles.x, angles.y, angles.z - 30));
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
