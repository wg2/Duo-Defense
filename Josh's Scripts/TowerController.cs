
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerController : MonoBehaviour
{
    private GameManager gameManager;
    private KeyboardPlayerController keyboardPlayer;

    public float health = 100;
    public GameObject healthBar;

    public GameObject gameManagerObj;
    public GameObject keyboardShop;
    public bool kShop = false;
    public GameObject mouseShop;
    public bool mShop = false;
    public GameObject[,] keyboardItems;
    public GameObject[,] mouseItems;
    public int shopSize = 2;

    public Animator cantAffordAnimator1;
    public Animator cantAffordAnimator2;
    public Animator cantAffordAnimator3;
    public Animator cantAffordAnimator4;

    public Animator cantAffordAnimatorM1;
    public Animator cantAffordAnimatorM2;
    public Animator cantAffordAnimatorM3;
    public Animator cantAffordAnimatorM4;

    public TextMeshProUGUI toolTipText;
    public TextMeshProUGUI titleText;
    public GameObject toolTipBox;

    public int kItemsCap;
    public int mItemsCap;

    public GameObject mouseCostText;
    public GameObject keyboardCostText;

    public List<GameObject> kItems = new List<GameObject>();
    public List<GameObject> mItems = new List<GameObject>();

    public GameObject[,] keyboardShelves = new GameObject[2, 2];
    public GameObject[,] mouseShelves = new GameObject[2, 2];

    public GameObject keyboardItem;

    public GameObject selectedShelf;

    public GameObject mouseNextWave;
    public GameObject keyboardNextWave;

    private bool canReset = true;
    public bool DestroyItems = false;
    public bool hideItems = true;

    public bool hideItemsK = true;
    public bool hideItemsM = true;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        keyboardPlayer = GameObject.Find("KeyboardPlayer").GetComponent<KeyboardPlayerController>();
        keyboardItems = new GameObject[shopSize, shopSize];
        mouseItems = new GameObject[shopSize, shopSize];

        keyboardShelves[0, 0] = GameObject.Find("/Shop/KeyboardShop/KShelf");
        keyboardShelves[0, 1] = GameObject.Find("/Shop/KeyboardShop/KShelf 2");
        keyboardShelves[1, 0] = GameObject.Find("/Shop/KeyboardShop/KShelf 3");
        keyboardShelves[1, 1] = GameObject.Find("/Shop/KeyboardShop/KShelf 4");

        selectedShelf = mouseShelves[0, 0] = GameObject.Find("/Shop/MouseShop/MShelf");
        mouseShelves[0, 1] = GameObject.Find("/Shop/MouseShop/MShelf 2");
        mouseShelves[1, 0] = GameObject.Find("/Shop/MouseShop/MShelf 3");
        mouseShelves[1, 1] = GameObject.Find("/Shop/MouseShop/MShelf 4");

        keyboardShop.SetActive(false);
        mouseShop.SetActive(false);
        keyboardCostText.SetActive(false);
        mouseCostText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !kShop && !mShop)
        {
            keyboardShop.SetActive(true);
            kShop = true;
            hideItemsK = false;
            keyboardCostText.SetActive(true);
        } else if (Input.GetMouseButtonDown(2) && kShop)
        {
            keyboardShop.SetActive(false);
            kShop = false;
            hideItemsK = true;
            keyboardCostText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && !mShop && !kShop)
        {
            mouseShop.SetActive(true);
            mShop = true;
            hideItemsM = false;
            mouseCostText.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.E) && mShop)
        {
            mouseShop.SetActive(false);
            mShop = false;
            hideItemsM = true;
            mouseCostText.SetActive(false);
        }

        if (Input.GetKey(KeyCode.R) && canReset)
        {
            ShopReset();
            canReset = false;
        }
        if (Input.GetKey(KeyCode.M) && !canReset)
        {
            canReset = true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            keyboardPlayer.numOfExplosiveArrows += 1;
        }

        if (mShop)
        {
            MouseShopControl();
        }

        if(!kShop && !mShop)
        {
            toolTipBox.SetActive(false);
        } 

        if (gameManager.enemiesRemaining == 0)
        {
            keyboardNextWave.SetActive(true);
            mouseNextWave.SetActive(true);
        }
    }

    public void RemoveNonComsumableK(int index)
    {
        if (index == 1)
        {
            kItems.RemoveAt(1);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.transform.localScale = new Vector3(health, 7.5f, 1);
        if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

    public Vector3 FindPosition()
    {
        return transform.position;
    }
    /*public void DealDamage(float damage)
    {
        health = health - damage;
        /*if (health < 0) {
        endgame();
        */
    //}

    public void ShopReset()
    {
        ShopClear();
        StartCoroutine(ShopFill());
    }

    private void ShopClear()
    {
        DestroyItems = true;
    }

    // fills the shop with items
    IEnumerator ShopFill()
    {
        Debug.Log("fill");
        yield return new WaitForSeconds(0.0001f);

        // does not destroy the items
        DestroyItems = false;
        int shelfPosNumK = 0;
        int shelfPosNumM = 0;
        // declarations
        GameObject nextItemK;
        GameObject nextItemM;
        GameObject currentShelfK;
        GameObject currentShelfM;
        // loops through each shelf for the keyboard shop
        for (int i = 0; i < shopSize; i++)
        {
            for (int k = 0; k < shopSize; k++)
            {
                shelfPosNumK += 1;
                Debug.Log("ItemSpawned");
                // choses random item 
                keyboardItems[i, k] = kItems[Random.Range(0, kItemsCap)];
                int spawnX = i + 1;
                int spawnY = k;
                Vector3 spawnPos = new Vector3(spawnX, spawnY, 2);
                Quaternion spawnRot = new Quaternion(0, 0, 0, 0);

                // spawns item on shelf
                nextItemK = Instantiate(keyboardItems[i, k], spawnPos, spawnRot);

                // links item to shelf
                currentShelfK = keyboardShelves[i, k];
                currentShelfK.GetComponent<KeyboardShelf>().item = nextItemK;
                currentShelfK.GetComponent<KeyboardShelf>().item.GetComponent<GeneralItem>().shelfNum = shelfPosNumK;
                currentShelfK.GetComponent<KeyboardShelf>().setScript();

                // sets cost text
                TextMeshProUGUI costText = currentShelfK.GetComponent<KeyboardShelf>().costText;
                costText.text = currentShelfK.GetComponent<KeyboardShelf>().item.GetComponent<GeneralItem>().cost.ToString();

            }
        }

        // loops through each shelf for the mouse shop
        for (int i = 0; i < shopSize; i++)
        {
            for (int m = 0; m < shopSize; m++)
            {
                shelfPosNumM += 1;
                Debug.Log("ItemSpawnedMouse");
                // choses random item 
                mouseItems[i, m] = mItems[Random.Range(0, mItemsCap)];
                int spawnX = i + 1 - 3;
                int spawnY = m;
                Vector3 spawnPos = new Vector3(spawnX, spawnY, 2);
                Quaternion spawnRot = new Quaternion(0, 0, 0, 0);

                // spawns item on shelf
                nextItemM = Instantiate(mouseItems[i, m], spawnPos, spawnRot);

                // links item to shelf
                currentShelfM = mouseShelves[i, m];
                Debug.Log("set shelf mouse");
                currentShelfM.GetComponent<KeyboardShelf>().item = nextItemM;
                Debug.Log("set item mouse");
                currentShelfM.GetComponent<KeyboardShelf>().item.GetComponent<GeneralItem>().shelfNum = shelfPosNumM;
                currentShelfM.GetComponent<KeyboardShelf>().setScript();

                // sets cost text
                TextMeshProUGUI costText = currentShelfM.GetComponent<KeyboardShelf>().costText;
                costText.text = currentShelfM.GetComponent<KeyboardShelf>().item.GetComponent<GeneralItem>().cost.ToString();
                Debug.Log("sucessful mouse loop");
            }
        }
    }

    // manages cant afford animations for each shelf
    public void CantAfford(int shelf, bool kItem)
    {
        if (kItem)
        {
            if (shelf == 1)
            {
                cantAffordAnimator1.SetTrigger("cantAfford");
                Debug.Log("cantAfford1");
            }
            else if (shelf == 2)
            {
                cantAffordAnimator2.SetTrigger("cantAfford");
                Debug.Log("cantAfford2");
            }
            else if (shelf == 3)
            {
                cantAffordAnimator3.SetTrigger("cantAfford");
            }
            else if (shelf == 4)
            {
                cantAffordAnimator4.SetTrigger("cantAfford");
            }
        } else
        {
            if (shelf == 1)
            {
                cantAffordAnimatorM1.SetTrigger("cantAfford");
            }
            else if (shelf == 2)
            {
                cantAffordAnimatorM2.SetTrigger("cantAfford");
            }
            else if (shelf == 3)
            {
                cantAffordAnimatorM3.SetTrigger("cantAfford");
            }
            else if (shelf == 4)
            {
                cantAffordAnimatorM4.SetTrigger("cantAfford");
            }
        }
        
        
    }

    // controls mouse shop
    void MouseShopControl()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedShelf == mouseShelves[0, 0] || selectedShelf == mouseShelves[0, 1])
            {
                if (selectedShelf == mouseShelves[0, 0])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[1, 0];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
                else if (selectedShelf == mouseShelves[0, 1])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[1, 1];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectedShelf == mouseShelves[1, 0] || selectedShelf == mouseShelves[1, 1])
            {
                if (selectedShelf == mouseShelves[1, 0])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[0, 0];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
                else if (selectedShelf == mouseShelves[1, 1])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[0, 1];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectedShelf == mouseShelves[0, 0] || selectedShelf == mouseShelves[1, 0])
            {
                if (selectedShelf == mouseShelves[0, 0])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[0, 1];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
                else if (selectedShelf == mouseShelves[1, 0])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[1, 1];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectedShelf == mouseShelves[0, 1] || selectedShelf == mouseShelves[1, 1])
            {
                if (selectedShelf == mouseShelves[0, 1])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[0, 0];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
                else if (selectedShelf == mouseShelves[1, 1])
                {
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOff();
                    selectedShelf = mouseShelves[1, 0];
                    selectedShelf.GetComponent<KeyboardShelf>().outlineOn();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            selectedShelf.GetComponent<KeyboardShelf>().OnClick();
            Debug.Log("pressed");
        }

    }   

}


