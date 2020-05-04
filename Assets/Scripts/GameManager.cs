using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Houses Stuff")]
    public Text numberOfHousesText;

    [Header("Tile Stuff")]
    public Text tileNameText;
    public GameObject tileNamePanel;
    public Vector2 tilePanelShift;

    [Header("Camera Stuff")]
    public float orthoSize;
    public float startingOrthoSize = 5;
    public bool scrollCameraBounds = true;
    public bool zoomCameraBounds = true;
    public float minCameraZoom = 3;
    public float maxCameraZoom = 5;
    public Vector3 minCameraPosition;
    public Vector3 maxCameraPosition;
    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Difference;
    private bool Drag = false;


    private int randomNumber;

    jsonController jsonController;

    // Start is called before the first frame update
    void Start()
    {       
        jsonController = GetComponent<jsonController>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&  tileNamePanel.activeSelf == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                tileNameText.text = "Tile name: " + tile.tileName;
                ToggleTileNamePanel(mousePos2D);
            }
        }
        else if (Input.GetMouseButtonDown(0) && tileNamePanel.activeSelf == true)
        {
            RemoveTileNamePanel();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                if (spriteRenderer.sprite == jsonController.tiles[5] || spriteRenderer.sprite == jsonController.tiles[6])
                {
                    randomNumber = Random.Range(0, 4);
                    spriteRenderer.sprite = jsonController.tiles[randomNumber];
                    tile.tileType = jsonController.tileList[randomNumber].type;
                    tile.tileName = jsonController.tileList[randomNumber].name;
                    jsonController.currentNumberOfHouses--;
                    UpdateNumberOfHouses();
                }               
            }
        }

        
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(2))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }

        if (Drag == true)
        {
            Camera.main.transform.position = Origin - Difference;
        }

        if (scrollCameraBounds)
        {            
            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, minCameraPosition.x - (startingOrthoSize - orthoSize + 1), maxCameraPosition.x + (startingOrthoSize - orthoSize + 1)), 
                Mathf.Clamp(Camera.main.transform.position.y, minCameraPosition.y - (startingOrthoSize - orthoSize), maxCameraPosition.y + (startingOrthoSize - orthoSize)), 
                Mathf.Clamp(Camera.main.transform.position.z, minCameraPosition.z, maxCameraPosition.z));
           
        }

        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        orthoSize = Camera.main.orthographicSize;

        if (zoomCameraBounds)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);
        }       
    }


    public void ToggleTileNamePanel(Vector2 mousePos2D)
    {
        tileNamePanel.SetActive(true);
        tileNamePanel.transform.position = mousePos2D + tilePanelShift;
    }

    public void RemoveTileNamePanel()
    {
        tileNamePanel.SetActive(false);
    }

    public void UpdateNumberOfHouses()
    {
        numberOfHousesText.text = "Current Number of Houses: " + jsonController.currentNumberOfHouses.ToString();
    }
}
