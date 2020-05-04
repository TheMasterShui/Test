using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class jsonController : MonoBehaviour
{
    public string jsonURL;

    public Sprite[] tiles;
    public GameObject tile;

    public int currentNumberOfHouses = 0;

    private int randomNumber;

    public int mapWidth;
    public int mapHeight;
    public int numberOfHouses;
    public List<tiles> tileList;

    jsonMapData jsonData;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();

        StartCoroutine(getData());
    }

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
            processJsonData(www.downloadHandler.text);
            CreateMap();
            gm.UpdateNumberOfHouses();
        }        
    }

    private void processJsonData(string _url)
    {
        jsonData = JsonUtility.FromJson<jsonMapData>(_url);
        mapHeight = jsonData.map_height;
        mapWidth = jsonData.map_width;
        numberOfHouses = jsonData.number_of_houses;
        tileList = jsonData.tiles;
    }

    public void CreateMap()
    {
        Vector3 pos;

        for (int y = 0; y < 24; y++)
        {
            for(int x = 0; x < 24; x++)
            {
                randomNumber = Random.Range(0, 7);

                pos.x = x;
                pos.y = y;              
                pos.z = 0;

                GameObject tileObject = Instantiate(tile, pos, Quaternion.identity);
                tileObject.transform.parent = GameObject.Find("TilesContainer").transform;
                SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer>();
                Tile currentTile = tileObject.GetComponent<Tile>();

                spriteRenderer.sprite = tiles[randomNumber];
                if (randomNumber == 5 || randomNumber == 6)
                {
                    currentNumberOfHouses++;                 
                }          

                currentTile.tileType = tileList[randomNumber].type;
                currentTile.tileName = tileList[randomNumber].name;
                
            }
        }
    }
}
