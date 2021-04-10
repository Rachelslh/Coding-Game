using UnityEngine;
using System.Collections;

public class InfiniteTerrain : MonoBehaviour
{
	public GameObject PlayerObject;
    public GameObject terrainPrefab;
    
	private GameObject[,] _terrainGrid = new GameObject[3,3];
	
	void Start ()
	{
		GameObject linkedTerrain = terrainPrefab;
        linkedTerrain.transform.position = gameObject.transform.position;

        _terrainGrid[0, 0] = Instantiate(terrainPrefab);
		_terrainGrid[0,1] = Instantiate(terrainPrefab);
		_terrainGrid[0,2] = Instantiate(terrainPrefab);
		_terrainGrid[1,0] = Instantiate(terrainPrefab);
        _terrainGrid[1,1] = linkedTerrain;
		_terrainGrid[1,2] = Instantiate(terrainPrefab);
		_terrainGrid[2,0] = Instantiate(terrainPrefab);
		_terrainGrid[2,1] = Instantiate(terrainPrefab);
		_terrainGrid[2,2] = Instantiate(terrainPrefab);

        UpdateTerrainPositionsAndNeighbors();
	}
	
	private void UpdateTerrainPositionsAndNeighbors()
	{
		_terrainGrid[0,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
		_terrainGrid[0,1].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z);
		_terrainGrid[0,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
		
		_terrainGrid[1,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
		_terrainGrid[1,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
		
		_terrainGrid[2,0].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
		_terrainGrid[2,1].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z);
		_terrainGrid[2,2].transform.position = new Vector3(
			_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.x,
			_terrainGrid[1,1].transform.position.y,
			_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].GetComponent<Renderer>().bounds.size.z);
	}
	
	void Update ()
	{
		Vector3 playerPosition = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
		GameObject playerTerrain = null;
		int xOffset = 0;
		int yOffset = 0;
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				if ((playerPosition.x >= _terrainGrid[x,y].transform.position.x) &&
					(playerPosition.x <= (_terrainGrid[x,y].transform.position.x + _terrainGrid[x,y].GetComponent<Renderer>().bounds.size.x)) &&
					(playerPosition.z >= _terrainGrid[x,y].transform.position.z) &&
					(playerPosition.z <= (_terrainGrid[x,y].transform.position.z + _terrainGrid[x,y].GetComponent<Renderer>().bounds.size.z)))
				{
					playerTerrain = _terrainGrid[x,y];
					xOffset = 1 - x;
					yOffset = 1 - y;
					break;
				}
			}
			if (playerTerrain != null)
				break;
		}
		
		if (playerTerrain != _terrainGrid[1,1])
		{
			GameObject[,] newTerrainGrid = new GameObject[3,3];
			for (int x = 0; x < 3; x++)
				for (int y = 0; y < 3; y++)
				{
					int newX = x + xOffset;
					if (newX < 0)
						newX = 2;
					else if (newX > 2)
						newX = 0;
					int newY = y + yOffset;
					if (newY < 0)
						newY = 2;
					else if (newY > 2)
						newY = 0;
					newTerrainGrid[newX, newY] = _terrainGrid[x,y];
				}
			_terrainGrid = newTerrainGrid;
			UpdateTerrainPositionsAndNeighbors();
		}
	}
}
