using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralArrow : MonoBehaviour
{

    public int remaining;
    public bool infinite;
    public float damage;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boost()
    {
        damage *= 2;
    }

    IEnumerator boostTimer()
    {
        yield return new WaitForSeconds(5);

        damage /= 2;
    }

}
