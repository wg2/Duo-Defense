
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAItem : MonoBehaviour
{

    private KeyboardPlayerController playerControl;
    public GeneralItem genItem;

    // Start is called before the first frame update
    void Awake()
    {
        playerControl = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        genItem = gameObject.GetComponent<GeneralItem>();
        genItem.itemIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBuy()
    {
        playerControl.numOfExplosiveArrows += 10;
    }

}
