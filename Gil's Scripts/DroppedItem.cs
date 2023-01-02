using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public GameManager gameManager;
    public KeyboardPlayerController keyboardPlayerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        keyboardPlayerControllerScript = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) // pickup arrows, money, and health packs when touching mouse. Then delete self
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (gameObject.CompareTag("Money"))
            {
                gameManager.UpdateMoney(1);
            }
            else if (gameObject.CompareTag("Explosive Arrow Drop"))
            {
                keyboardPlayerControllerScript.numOfExplosiveArrows++;
                keyboardPlayerControllerScript.arrowCounter.text = keyboardPlayerControllerScript.numOfExplosiveArrows.ToString();
            }
            else if (gameObject.CompareTag("Multi Arrow Drop"))
            {
                keyboardPlayerControllerScript.numOfMultiArrows++;
                keyboardPlayerControllerScript.multiArrowCounter.text = keyboardPlayerControllerScript.numOfMultiArrows.ToString();
            }
            else if (gameObject.CompareTag("Basic Health Jug"))
            {
                gameManager.adjustHealthValue(+3);
            }
            Destroy(gameObject);
        }
    }
}
