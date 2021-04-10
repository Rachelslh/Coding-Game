using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using TMPro; 
 
public class Movement : MonoBehaviour 
{ 
private GameObject player; 
 
private Rigidbody rb;
private CharacterController controller; 
private float speed = 2f; 
    
 
private float smoothTime = 0.1f; 
private float smoothVelocity; 
 
public Vector3 offset;      // A variable that allows us to offset the position (x, y, z) 
 
 
void Start() 
{ 
player = GameObject.FindGameObjectWithTag("Player"); 

rb = player.GetComponent<Rigidbody>();
//if (rb == null) {
//rb = player.AddComponent<Rigidbody>();
//}
//Destroy(rb);

// Attach the controller to the player 
if (player.GetComponent<CharacterController>() == null) 
{ 
controller = player.AddComponent<CharacterController>(); 
} 
else 
{ 
controller = player.GetComponent<CharacterController>(); 
} 
} 
 
void Update() 
{ 
//Movement Part 
float horizontal = Input.GetAxisRaw("Horizontal"); 
float vertical = Input.GetAxisRaw("Vertical"); 
Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; 
 
if(direction.magnitude >= 0.1f) 
{ 
float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; 
float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime); 
player.transform.rotation = Quaternion.Euler(0f, angle, 0f); 
controller.Move(direction * speed * Time.deltaTime); 
} 
 
} 
 
}