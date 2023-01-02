using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardShelf : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public GameObject item;
    private GeneralItem itemScript;
    public GameObject notEnoughMoney;
    public GameObject shelfOutline;
    public bool selected;
    private TowerController towerControl;
    private GameObject mouseWaveButton;
    private GameObject keyboardWaveButton;
    

    // Start is called before the first frame update
    void Awake()
    {
        towerControl = GameObject.Find("Tower").GetComponent<TowerController>();
        mouseWaveButton = towerControl.mouseNextWave;
        keyboardWaveButton = towerControl.keyboardNextWave;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            shelfOutline.SetActive(true);
        } else if (!selected)
        {
            shelfOutline.SetActive(false);
        }

        if (towerControl.mShop && itemScript.mItem || towerControl.kShop && itemScript.kItem)
        {
            costText.enabled = true;
        } else
        {
            costText.enabled = false;
        }

        costText.text = itemScript.cost.ToString();

    }

    public void setScript()
    {
        itemScript = item.GetComponent<GeneralItem>();
    }

    public void OnClick()
    {
        itemScript.OnBuy();
        Debug.Log("clicked");
    }

    public void outlineOn()
    {
        shelfOutline.SetActive(true);
        selected = true;
        towerControl.toolTipBox.SetActive(true);
        towerControl.toolTipText.text = itemScript.toolTip;
        towerControl.toolTipText.fontSize = itemScript.textSize;
        towerControl.titleText.text = itemScript.title;
        mouseWaveButton.transform.position = new Vector2(mouseWaveButton.transform.position.x, -2.5f);
        keyboardWaveButton.transform.position = new Vector2(keyboardWaveButton.transform.position.x, -2.5f);
    }
    public void outlineOff()
    {
        shelfOutline.SetActive(false);
        selected = false;
        towerControl.toolTipBox.SetActive(false);
        mouseWaveButton.transform.position = new Vector2(mouseWaveButton.transform.position.x, -1);
        keyboardWaveButton.transform.position = new Vector2(keyboardWaveButton.transform.position.x, -1);
    }
}
