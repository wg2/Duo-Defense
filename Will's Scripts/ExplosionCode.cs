using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCode : MonoBehaviour
{
    private float startTime;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().SetTrigger("PlayAnimation");
        startTime = Time.deltaTime;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !gameObject.GetComponent<Animator>().IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            try
            {
                collision.gameObject.GetComponent<Enemy>().DealDamage(10 * gameManager.TempDamageMonitor());
            }
            catch
            {

            }
        }
    }
}
