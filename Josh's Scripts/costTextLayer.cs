using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostTextLayer : MonoBehaviour
{

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text.GetComponent<Renderer>().sortingLayerName = "CostText";
        text.GetComponent<Renderer>().sortingOrder = 1;

    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Renderer>().sortingLayerName = "CostText";
    }
}
