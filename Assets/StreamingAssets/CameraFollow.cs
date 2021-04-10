using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraFollow : MonoBehaviour 
{ 
private GameObject mainCamera; 
private GameObject player; 

float smoothSpeed = 0.125f;
Vector3 offset;
 
// Start is called before the first frame update 
void Start() 
{ 
offset = new Vector3(0f, 0f, -5);
player = GameObject.FindGameObjectWithTag("Player"); 
mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
}
 
// Update is called once per frame 
void Update() 
{ 
Vector3 desiredPos = player.transform.position + offset;
Vector3 smoothedPos = Vector3.Lerp(mainCamera.transform.position, desiredPos, smoothSpeed);
mainCamera.transform.position = smoothedPos;

mainCamera.transform.LookAt(player.transform.position);
} 
}