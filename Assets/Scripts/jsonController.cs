using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class jsonController : MonoBehaviour
{
    public string jsonURL;

    public Sprite[] tiles;
    public GameObject tile;

    jsonMapData jsonData;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getData());

        CreateMap();
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
            
        }        
    }

    private void processJsonData(string _url)
    {
        jsonData = JsonUtility.FromJson<jsonMapData>(_url);        
        Debug.Log(jsonData.map_height);
        Debug.Log(jsonData.tiles[0].type);

    }

    public void CreateMap()
    {
        Vector3 pos;

        for (int x = 0; x < 16; x++)
        {
            for(int y = 0; y < 16; y++)
            {
                pos.x = x;
                pos.y = y;
                pos.z = 0;

                Instantiate(tile, pos, Quaternion.identity);
                SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = tiles[Random.Range(0, 5)];
            }
        }
    }
}
