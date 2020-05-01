using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class jsonMapData
{
    public int map_width;
    public int map_height;
    public int number_of_houses;
    public List<tiles> tiles;
}

[Serializable]
public class tiles
{
    public string type;
    public string name;
}

