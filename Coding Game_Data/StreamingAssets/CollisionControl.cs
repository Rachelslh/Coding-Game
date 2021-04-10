using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CollisionControl : MonoBehaviour 
{ 
private GameObject player; 
private CharacterController controller;

void Start() 
{ 
player = GameObject.FindGameObjectWithTag("Player"); 
controller = player.GetComponent<CharacterController>();
} 

// OnCollisionEnter is the builtin function called whenever a collision happens  
void OnCollisionEnter(Collision collisionInfo) 
{ 
if (collisionInfo.collider.tag == "Obstacle") 
{ 
} 
} 

} 
