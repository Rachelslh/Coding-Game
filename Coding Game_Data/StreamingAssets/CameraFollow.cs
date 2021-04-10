using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraFollow : MonoBehaviour 
{ 
private GameObject mainCamera; 
private GameObject player; 

// Start is called before the first frame update 
void Start() 
{
player = GameObject.FindGameObjectWithTag("Player"); 
mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
}
 
// Update is called once per frame 
void Update() 
{ 
} 
}