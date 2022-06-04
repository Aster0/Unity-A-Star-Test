using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{

    [SerializeField] private GameObject gridPrefab;


    public GameObject destinationObj, playerObj;

    [SerializeField] private int x, y;

    private int nodeIndex = 1;

    public int GridStraightCost { get; set; }= 10; 
    public int GridDiagonalCost { get; set; }= 14;


    private string[] booleans = { "true", "false" };
    
    
    private int unwalkableCount = 0;
    void Awake()
    {
        for (int xPos = 0; xPos < x; xPos++)
        {
            for (int yPos = 0; yPos < y; yPos++)
            {
            
                GameObject grid = Instantiate(gridPrefab, new Vector3(xPos * gridPrefab.transform.localScale.x,
                    yPos * gridPrefab.transform.localScale.y, 1), Quaternion.identity);
                
                grid.transform.SetParent(this.gameObject.transform);


                grid.GetComponent<NodeManager>().number.text = nodeIndex.ToString();

                bool unwalkable = Boolean.Parse(booleans[Random.Range(0, 2)]);


                if (unwalkable && (Vector2) grid.transform.position != (Vector2) destinationObj.transform.position 
                               && unwalkableCount <= 4 && (Vector2) grid.transform.position != (Vector2) playerObj.transform.position )
                {
                    grid.tag = "unwalkable";

                    unwalkableCount++;
                    grid.GetComponent<Renderer>().material.color = Color.black;
                    
                }
                


                nodeIndex++;
            }
        }
    }


    public GameObject FindNode(Vector2 pos)
    {
      
 
        foreach (Transform child in this.transform)
        {
        
            
            if ((Vector2) child.TransformPoint(new Vector2(0,0)) == pos && !child.gameObject.CompareTag("unwalkable"))
            {
                
                child.gameObject.GetComponent<Renderer>().material.color = Color.red;
                return child.gameObject;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
