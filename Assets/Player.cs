using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{

    public class Node
    {
        public float G, H, F;

        public Vector3 pos;

        public GameObject gameObject;

        private NodeManager _nodeManager;


        public Node fromNode;
        
        public int Index { get; set; }

        public Node(GameObject gameObject, Node currentNode, float G)
        {
            this.pos = pos;


            this.gameObject = gameObject;
            this.pos = gameObject.transform.position;
            
            if (currentNode != null)
            {
                this.G = G += currentNode.G;


            }
            
            H = CalculateH(Destination.position);

            F = this.G + H;

            fromNode = currentNode;

            _nodeManager = gameObject.GetComponent<NodeManager>();

            _nodeManager.fCost.gameObject.SetActive(true);
            _nodeManager.gCost.gameObject.SetActive(true);
            _nodeManager.hCost.gameObject.SetActive(true);
            
            _nodeManager.fCost.text = Math.Round(F).ToString();
            _nodeManager.gCost.text = this.G.ToString();
            _nodeManager.hCost.text = Math.Round(H).ToString();

         

          

            this.Index = Int32.Parse(_nodeManager.number.text) ;


        }


        public float CalculateH(Vector2 destination)
        {
            // Euclidean


            return Mathf.Sqrt(
                Mathf.Abs((destination.x - pos.x) * (destination.x - pos.x)) +
                Mathf.Abs((destination.y - pos.y) * (destination.y - pos.y)));

        }

    }
    
    
    [SerializeField]
    private GridManager gridManager;


    private List<Node> openList = new List<Node>();
    private List<Node> closedList = new List<Node>();

    private int gridDCost, gridScost;

    private Node currentNode;

    private bool reachDestination = false;

    public static Transform Destination { get; set; }


    private int nodeIndex = 1;

    private Node startNode;
    
    private void OnPathInitialize()
    {
        
        Debug.Log(gridManager.FindNode(transform.position));
        currentNode = new Node(gridManager.FindNode(transform.position), null, 0);
        
        

   
    }


    private void CalculateCurrentNode(Vector2 nodePos)
    {

        Node node;

        // Calculate Neighbours


        // find LEFT

        node = MakeNode(nodePos + new Vector2(-1, 0), gridScost);

        
        // find D-LEFT


        node = MakeNode(nodePos + new Vector2(-1, 1), gridDCost);

        
        // find RIGHT

 
        node = MakeNode(nodePos + new Vector2(1, 0), gridScost);
  
        
        // find D-RIGHT

     
        node = MakeNode(nodePos + new Vector2(1, 1), gridDCost);

        
        // find UP

   
        node = MakeNode(nodePos + new Vector2(0, 1), gridScost);
     
        
        // find DOWN

    
        node = MakeNode(nodePos + new Vector2(0, -1), gridScost);
 
      


  

    }

    private Node MakeNode(Vector2 nodePos, float gcost)
    {
        Node node = null;


        
        try
        {
            foreach (Node node1 in closedList)
            {
                if ((Vector2) node1.pos == nodePos)
                {
            
                
                    return null;
                
         
                }
            }
            
            
            nodeIndex++;
            node = FindNode(nodePos, gcost);
            
            
            bool add = true;



            if(add && node != null)
                openList.Add(node);

            openList = openList.OrderBy(n => n.F).ToList();

           




        }
        catch (NullReferenceException noNode)
        {
          
        }


        return node;

    }


    private void StepNode()
    {




        
        Node smallestFNode = null;
        try
        {
            smallestFNode = openList[0];
        }
        catch (Exception e)
        {
            
        }


        openList.Remove(smallestFNode);
        
        if(smallestFNode != null)
            currentNode = smallestFNode;



        bool add = true;


        foreach (Node node in closedList) // dont step into a closed node.
        {
            if (node.Index == currentNode.Index)
            {
                add = false;
                break;
            }
        }
        
        
        if(add)
            closedList.Add(currentNode);


  
       
        if ((Vector2) Destination.transform.position == (Vector2) currentNode.pos)
        {
            Debug.Log("True!");
            reachDestination = true;


            StartCoroutine(GetDestination());

            
            return;
        }
        
      
        CalculateCurrentNode(currentNode.pos);
    }

    private IEnumerator GetDestination()
    {



        Node endNode = null; 
        foreach (Node node in closedList)
        {
            if ((Vector2)node.pos == (Vector2)Destination.transform.position)
            {
                endNode = node;
                break;
                
            }
        }

        List<Node> destinationNodes = new List<Node>();
        
        Node currentNodeHere = endNode;
        
        destinationNodes.Add(endNode);


        while (currentNodeHere != startNode)
        {
            destinationNodes.Add(currentNodeHere.fromNode);

            currentNodeHere = currentNodeHere.fromNode;
        }

        destinationNodes.OrderBy(n => n.Index);
        destinationNodes.Reverse();
        
        foreach (Node node in destinationNodes)
        {
            transform.position = node.pos;

            yield return new WaitForSeconds(1f);


        }
    }



    private Node FindNode(Vector2 pos, float gcost)
    {
        GameObject gameObject = gridManager.FindNode(pos);

        foreach (Node node1 in openList)
        {
            if ((Vector2) node1.pos == pos)
            {
                
      
                return null;
                
         
            }
        }
        

        




        return new Node(gameObject, currentNode, gcost);
    }
    // Start is called before the first frame update
    void Start()
    {
        gridScost = gridManager.GridStraightCost;
        gridDCost = gridManager.GridDiagonalCost;

        Destination = GameObject.Find("Destination").transform;
        

        OnPathInitialize();
        //CalculateCurrentNode(transform.position);

        startNode = currentNode;


    }

    // Update is called once per frame
    void Update()
    {
        
        if (!reachDestination)
        {
            StepNode();
        }
      

    }



    
    
}
