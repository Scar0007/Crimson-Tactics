using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CubeGrid : MonoBehaviour
{
	public GameObject selectedUnit;

	public CubeType[] tileTypes;

	int[,] tiles;
	Node[,] graph;


	int mapSizeX = 10;
	int mapSizeY = 10;

	void Start() {
		// Setup the selectedUnit's variable
		selectedUnit.GetComponent<PlayerAI>().tileX = (int)selectedUnit.transform.position.x;
		selectedUnit.GetComponent<PlayerAI>().tileY = (int)selectedUnit.transform.position.y;
		selectedUnit.GetComponent<PlayerAI>().map = this;

		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}

	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[mapSizeX,mapSizeY];
		
		int x,y;
		
		// Initialize our map tiles
		for(x=0; x < mapSizeX; x++) {
			for(y=0; y < mapSizeX; y++) {
				tiles[x,y] = 0;
			}
		}
		//Obstacles
		tiles[4, 4] = 1;
		tiles[5, 4] = 1;
		tiles[6, 5] = 1;
		tiles[7, 4] = 1;
		tiles[8, 4] = 1;

		tiles[4, 7] = 1;
		tiles[4, 6] = 1;
		tiles[8, 4] = 1;
		tiles[8, 6] = 1;

	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) 
    {

		CubeType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) 
        {
			cost += 0.001f;
		}

		return cost;

	}

	void GeneratePathfindingGraph() 
    {
		// Initialize the array
		graph = new Node[mapSizeX,mapSizeY];

		// Initialize a Node for each spot in the array
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) 
            {
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their neighbours
		for(int x=0; x < mapSizeX; x++) 
        {
			for(int y=0; y < mapSizeX; y++) 
            {

				// Check left
				if(x > 0) 
                {
					graph[x,y].neighbours.Add( graph[x-1, y] );
					if(y > 0)
						graph[x,y].neighbours.Add( graph[x-1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].neighbours.Add( graph[x-1, y+1] );
				}

				// Check Right
				if(x < mapSizeX-1) 
                {
					graph[x,y].neighbours.Add( graph[x+1, y] );
					if(y > 0)
						graph[x,y].neighbours.Add( graph[x+1, y-1] );
					if(y < mapSizeY-1)
						graph[x,y].neighbours.Add( graph[x+1, y+1] );
				}

				// Check straight up and down
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );
			}
		}
	}

	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeX; y++) {
				CubeType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity );

				OnClick ct = go.GetComponent<OnClick>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) {
		return new Vector3(x, 1.5f, y);
	}

	public bool UnitCanEnterTile(int x, int y) 
    {
		return tileTypes[ tiles[x,y] ].isWalkable;
	}

	public void GeneratePathTo(int x, int y) 
    {
		// Clear out our unit's old path.
		selectedUnit.GetComponent<PlayerAI>().currentPath = null;

		if( UnitCanEnterTile(x,y) == false ) 
        {
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    selectedUnit.GetComponent<PlayerAI>().tileX, 
		                    selectedUnit.GetComponent<PlayerAI>().tileY
		                    ];
		
		Node target = graph[
		                    x, 
		                    y
		                    ];
		
		dist[source] = 0;
		prev[source] = null;
        foreach(Node v in graph) 
        {
			if(v != source) 
            {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) 
        {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) 
            {
				if(u == null || dist[possibleU] < dist[u]) 
                {
					u = possibleU;
				}
			}

			if(u == target) 
            {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) 
            {
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) 
                {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		if(prev[target] == null) {
			// No route between our target and the source
			return;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// So we need to invert it!

		currentPath.Reverse();

		selectedUnit.GetComponent<PlayerAI>().currentPath = currentPath;
	}
}
