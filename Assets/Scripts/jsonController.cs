using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class jsonController : MonoBehaviour
{
    // Set the jsonURL in the inspector
    [Header("Json URL")]
    public string jsonURL;

    // Set the sprites of tiles and tile prefab in the inspector
    [Header("Tile Stuff")]
    public Sprite[] tiles;
    public GameObject tile;      

    private int randomNumber;

    [Header("Json File Data")]
    public int mapWidth;
    public int mapHeight;
    public int numberOfHouses;
    public List<tiles> tileList;

    jsonMapData jsonData;
    GameManager gm;

    [Header("Debug")]
    // current number of houses
    public int currentNumberOfHouses = 0;


    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();

        // Start the coroutine to Get Data and to create the map
        StartCoroutine(getData());
    }

    // Get the data from the Json File and Create the map and Update the number of houses
    IEnumerator getData()
    {
        UnityWebRequest www = UnityWebRequest.Get(jsonURL);

        // Request and wait for the desired page.
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Process Json Data
            processJsonData(www.downloadHandler.text);
            // Create the map
            CreateMap();
            // Update current number of houses on the map
            gm.UpdateNumberOfHouses();
        }        
    }

    // Process Json Data from the online URL and put the information into local variables and lists
    private void processJsonData(string _url)
    {
        jsonData = JsonUtility.FromJson<jsonMapData>(_url);
        mapHeight = jsonData.map_height;
        mapWidth = jsonData.map_width;
        numberOfHouses = jsonData.number_of_houses;
        tileList = jsonData.tiles;
    }

    // Create the map with randomized tiles 
    public void CreateMap()
    {
        Vector3 pos;

        // 24x24 tiles map
        for (int y = 0; y < 24; y++)
        {
            for(int x = 0; x < 24; x++)
            {
                // Set the random number from 0-6
                randomNumber = Random.Range(0, 7);

                // Set the position of the (to be created) tile to the X and Y in the matrix
                pos.x = x;
                pos.y = y;              
                pos.z = 0;

                // Instantiate the Tile on the current position in the matrix
                GameObject tileObject = Instantiate(tile, pos, Quaternion.identity);
                // Parent the tiles to the TilesContainer empty object
                tileObject.transform.parent = GameObject.Find("TilesContainer").transform;

                // Get the components for Sprite Renderer and current Tile 
                SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer>();
                Tile currentTile = tileObject.GetComponent<Tile>();

                // Set the Sprite of the tile to the random tile
                spriteRenderer.sprite = tiles[randomNumber];
                if (randomNumber == 5 || randomNumber == 6)
                {
                    // Count the current number of houses if the random number is 5 or 6
                    currentNumberOfHouses++;                 
                }

                // Set the information for the corresponding current tile
                currentTile.tileType = tileList[randomNumber].type;
                currentTile.tileName = tileList[randomNumber].name;
                
            }
        }
    }
}
