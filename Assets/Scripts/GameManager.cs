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
        // If Left mouse button is clicked and Tile Name Panel is not active then Raycast and get tile information and display it
        if (Input.GetMouseButtonDown(0) &&  tileNamePanel.activeSelf == false)
        {
            // Get the mouse position on the screen
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Set the mouse position to the Vector2
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Raycast on the current clicked mouse position
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            // If there is a raycast hit show tile info
            if (hit.collider != null)
            {
                // Get tile component of the raycast hit collider
                Tile tile = hit.collider.GetComponent<Tile>();
                // Set the text info to the clicked tile info
                tileNameText.text = "Tile name: " + tile.tileName;
                // Toggle Tile Info Panel on the mouse position
                ToggleTileNamePanel(mousePos2D);
            }
        }
        // Else Remove the Tile panel if it is active
        else if (Input.GetMouseButtonDown(0) && tileNamePanel.activeSelf == true)
        {
            RemoveTileNamePanel();
        }

        // If Right mouse button is clicked and tiles is a building, 'demolish' it
        if (Input.GetMouseButtonDown(1))
        {
            // Get the mouse position on the screen
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Set the mouse position to the Vector2
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Raycast on the current clicked mouse position
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            // If there is a raycast hit change the tile 
            if (hit.collider != null)
            {
                // Get tile and sprite renderer component of the raycast hit collider
                Tile tile = hit.collider.GetComponent<Tile>();
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

                // If the Sprite of the clicked tile is a building 'demolish' it
                if (spriteRenderer.sprite == jsonController.tiles[5] || spriteRenderer.sprite == jsonController.tiles[6])
                {
                    // Set the random number to correspond to the empty tiles
                    randomNumber = Random.Range(0, 4);

                    // Change the Sprite of the clicked tile to the random empty tile
                    spriteRenderer.sprite = jsonController.tiles[randomNumber];

                    // Change the information of the newly changed tile to the correct information
                    tile.tileType = jsonController.tileList[randomNumber].type;
                    tile.tileName = jsonController.tileList[randomNumber].name;

                    // Decrease the number of the current houses on the map
                    jsonController.currentNumberOfHouses--;
                    
                    // Update the current number of the houses on the map on UI
                    UpdateNumberOfHouses();
                }               
            }
        }

        
    }

    void LateUpdate()
    {
        // If there is a middle mouse click and you hold the middle mouse, set the drag to true
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

        // If there is a mouse drag from the original position, change the camera position to the current mouse position while dragging the mouse
        if (Drag == true)
        {
            Camera.main.transform.position = Origin - Difference;
        }

        // Scroll Camera Bounds so you can't scroll way off the map
        if (scrollCameraBounds)
        {            
            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, minCameraPosition.x - (startingOrthoSize - orthoSize + 1), maxCameraPosition.x + (startingOrthoSize - orthoSize + 1)), 
                Mathf.Clamp(Camera.main.transform.position.y, minCameraPosition.y - (startingOrthoSize - orthoSize), maxCameraPosition.y + (startingOrthoSize - orthoSize)), 
                Mathf.Clamp(Camera.main.transform.position.z, minCameraPosition.z, maxCameraPosition.z));
           
        }

        // Zoom functionality - If you use the scroll wheel, adjust the camera size accordingly
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        orthoSize = Camera.main.orthographicSize;

        // Zoom Camera Bounds so you can scroll around the map while zoomed in
        if (zoomCameraBounds)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);
        }       
    }

    // Method for Toggling the Tile Info Panel
    public void ToggleTileNamePanel(Vector2 mousePos2D)
    {
        tileNamePanel.SetActive(true);
        tileNamePanel.transform.position = mousePos2D + tilePanelShift;
    }

    // Method for Removing the Tile Info Panel
    public void RemoveTileNamePanel()
    {
        tileNamePanel.SetActive(false);
    }

    // Method for Updating the Current Number of Houses on the map
    public void UpdateNumberOfHouses()
    {
        numberOfHousesText.text = "Current Number of Houses: " + jsonController.currentNumberOfHouses.ToString();
    }
}
