 using UnityEngine;
 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;
 using TMPro;

public class PlayerMouseController : MonoBehaviour
{  
    [SerializeField]
    private float lineSeperationDistance = 0.2f;
    [SerializeField]
    private float lineWidth = 0.1f;
    [SerializeField]
    private Color lineColor = Color.black;
    [SerializeField]
    private int lineCapVertices = 5;
    public GameManager gameManager;
    private GameObject currentLineObject;
    private List<GameObject> lines;
    private List<Vector2> currentLine;
    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentLineEdgeCollider;
    public TextMeshProUGUI wallTypeInfoText; // text that appears when right clicking to change wall type
    private bool wallTypeInfoActive = false;
    public GameObject mouseIcon;
    private readonly int standardWallCost = 1;
    private readonly int explosiveWallCost = 6;

    private EdgeCollider2D currentLineEdgeTrigger;
    private bool drawing = false;
    public int typeOfWall; // type of wall, default is 0
    private readonly int typeOfWallLength = 1; // max length of wallType, used to rollback to 0 after right clicking past every option

    private Camera mainCamera;

    private void Awake() 
    {
        mainCamera = Camera.main;
        StartCoroutine(CreatePolygonCollider());
    }

    private void Update()
    {   if (!gameManager.playersStunned) // mouse cannot move if health is recharging after it reached 0
        {
            try 
            {
                mouseIcon.GetComponent<PolygonCollider2D>().enabled = true;
            } catch {}
            mouseIcon.transform.position = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, -1);
        }
        else
        {
            mouseIcon.GetComponent<PolygonCollider2D>().enabled = false;
        }
        // if the mouse is pressed down and there exists some draw value, then create a new brush instance
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameManager.getDrawBarValue() > 0)
        {
            OnStartDraw();
        }
        else if (!Input.GetKey(KeyCode.Mouse0) && drawing) // disables drawing function if currently drawing
        {
            OnEndDraw();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) // when right clicking, change the type of drawing material
        {
            typeOfWall++;
            if (typeOfWall > typeOfWallLength) {
                typeOfWall = 0;
            }
            StartCoroutine(displayWallTypeInfo());
            //Debug.Log("Type of wall updated to " + typeOfWall);
        }
        if (wallTypeInfoActive) // to set wall info text next to mouse player when right clicking
        {
            wallTypeInfoText.transform.position = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        }
        else if (gameManager.getDrawBarValue() == 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(displayWallTypeInfo());
        }
        else
        {
             wallTypeInfoText.transform.position = new Vector3(500,500,-1);
        }    
    }

    private void OnStartDraw() 
    {
        StartCoroutine("Drawing");
    }

    private void OnEndDraw() 
    {
        drawing = false;
        // allows for collisions only once the line is complete
        currentLineObject.AddComponent<PlayerWall>();
        currentLineObject.GetComponent<PlayerWall>().setWallType(typeOfWall); // assign the type of wall to the newly instantiated wall
    }

    IEnumerator Drawing()
    {
        drawing = true;
        StartLine();
        while(drawing && gameManager.getDrawBarValue() > 0) {
            AddPoint(GetCurrentWorldPoint());
            yield return null;
        }
        EndLine();
    }

    private void StartLine() 
    {
        // instantiates a new line
        currentLine = new List<Vector2>();
        currentLineObject = new GameObject();
        currentLineObject.name = "Line";
        currentLineObject.transform.parent = transform;
        currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
        currentLineEdgeCollider = currentLineObject.AddComponent<EdgeCollider2D>();

        
        

        // set settings
        currentLineRenderer.positionCount = 0;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        currentLineRenderer.numCapVertices = lineCapVertices;
        currentLineRenderer.material = new Material (Shader.Find("Particles/Standard Unlit"));
        currentLineRenderer.startColor = lineColor;
        currentLineRenderer.endColor = lineColor;
        currentLineEdgeCollider.edgeRadius = 0.1f;
    }

    private void EndLine() 
    {
        if (currentLine.Count == 1) 
        {
            Destroy(currentLineObject);
            if (typeOfWall == 0 && gameManager.getDrawBarValue() > standardWallCost / 2)    
                gameManager.adjustDrawBarValue(standardWallCost); // adds back draw bar value when clicking
            else if (typeOfWall == 1 && gameManager.getDrawBarValue() > explosiveWallCost / 2)
                gameManager.adjustDrawBarValue(explosiveWallCost);
        }
        else 
        {
            currentLineEdgeCollider.SetPoints(currentLine);
            //Debug.Log("GetKeyDown: " + Input.GetKeyDown(KeyCode.Mouse0) + "\nGetKey: " + Input.GetKey(KeyCode.Mouse0));
        }
        

    }

    private Vector2 GetCurrentWorldPoint()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void AddPoint(Vector2 point) 
    {
        if (PlacePoint(point)) {
            currentLine.Add(point);
            switch (typeOfWall) // reduces draw bar by certain amounts depending on wall type
            {
                case 0:
                    gameManager.adjustDrawBarValue(-standardWallCost);
                    break;
                case 1:
                    gameManager.adjustDrawBarValue(-explosiveWallCost);
                    break;
            }
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, point);
            
        }
    }

    private bool PlacePoint(Vector2 point) // places point only if certain conditions are met
    {
        if (currentLine.Count == 0) return true;
        if (Vector2.Distance(point, currentLine[currentLine.Count - 1]) < lineSeperationDistance) return false; // does not allow another point to be placed if the previous is too close
        return true;
    }

    private IEnumerator displayWallTypeInfo() // enables wall text info for a short period of time
    {
        wallTypeInfoActive = true;
        if (gameManager.getDrawBarValue() == 0) // out of draw bar indicator
        {
            wallTypeInfoText.text = "Draw bar empty";
        }
        else
        {
        switch (typeOfWall) //informs user of type of wall
        {
            case 0:
                wallTypeInfoText.text = "Standard";
                break;
            case 1:
                wallTypeInfoText.text = "Explosive";
                break;
        }
        }
        yield return new WaitForSeconds(2);
        wallTypeInfoActive = false;
        wallTypeInfoText.text = "";
    }

    private IEnumerator CreatePolygonCollider()
    {
        yield return new WaitForSeconds(1);
        mouseIcon.AddComponent<PolygonCollider2D>();
    }
}