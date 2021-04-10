using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{

    public List<Task> taskList = new List<Task>();
 

    // TM var
    private static TaskManager _instance;

    public static TaskManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TaskManager();
            }
            return _instance;
        }
    }

    private string defaultHint = "The GameObject of intrest is tagged by \"Player\", " +
        "try using the builtin function <color=#D49861>GameObject.FindGameObjectWithTag" +
        "(<color=#E0E0DF>string Tag</color>);</color>\n";


    public void Init()
    {
        taskList.Add(new Task("The player is staying still. Enable gravity",
            defaultHint + "\nTry adding a RigidBody component to the player, gravity will be enabled by default"+
            "\nTry using the builting function <color=#D49861>gameObject.AddComponent<Type>();</color>" +
            "\nThink of using an If statement to verify the non-existance of the Rigidbody component first"));

        taskList.Add(new Task("Ignore the physics system", defaultHint + "\nSimply destroy the rigidbody component using " +
            "the <color=#D49861>Destroy(component)</color> function." +
            "\nDon't forget to take off the rigidbody part code as we no longer need it."));
            
        taskList.Add(new Task("The scene seems a bit boring. Add some movement.",
            defaultHint + "\nEnable movement towards all directions, triggered by the input arrow keys by adding a <color=#D49861>CharacterController</color> component to the player.\n" +
            "\nDefine a speed variable " +
            "\nGet both horizontal and vertical directions using <color=#E0E0DF>Input.GetAxisRaw(direction);</color>" +
            "\nThe player should not be able to move on the y-axis." +
            "\nTry using the normalized direction vector and check the magnitude before affecting the movement to the controller."));
        
        taskList.Add(new Task("Make the player face the direction he's moving into.",
                    defaultHint +
                    "\nGet the target angle by using <color=#D49861>Mathf.Atan2(direction.x, direction.z)</color> according to the " +
                    "unity rotation system that increases clockWise." +
                    "\nConvert the output from Radians to Degrees using <color=#D49861>Mathf.Rad2Deg</color>." +
                    "\nAffect the final target angle to the player's transform on the y-axis through <color=#D49861>Quaternion.Euler(x, y, z)</color>."));

        taskList.Add(new Task("Try to make the movement smoother!!",
                    defaultHint + "\nAdd a Smooth Time float variable, set to 0.1f by default." +
                    "\nFor a gradual change of direction, implement the <color=#D49861>Mathf.SmoothDampAngle" +
                    "(currentValue, targetValue, ref smoothVelocity, smoothTime)</color>." +
                    "\nNote: smoothVelocity is just a private float variable you need to add, represents the velocity that keeps changing each time the function is called."));

        taskList.Add(new Task("How about a camera follow ?",
            defaultHint + "\nMainly, you should bind the player's position with the camera's." +
            "\nInitialize an offset Vector3 variable to quantify the distance between the camera and the player, you can tweak it later for better results." +
            "\nFor smoother effects, add a speed float set to 0.125f by default, use interpolation methods " +
            "(such as <color=#D49861>Vector3.Lerp(currentPos, desiredPos, speed)</color>)" +
            "\nTo make the smoothign occur at the same speed no matter the frame rate, consider multiplying the speed by <color=#D49861>Time.deltaTime</color>."));

        taskList.Add(new Task("Make the camera look at the player",
            defaultHint +
            "\nConsider changing the <color=#D49861>Update()</color> method to <color=#D49861>FixedUpdate()</color> in order to make the change simultaneaous with the physics engine" +
            "\nRotate the camera in order tolook at the player simply by calling <color=#D49861>mainCamera.transform.LookAt(player.transform)</color>."));

        taskList.Add(new Task("Rocks falling everywhere, have fun with collisions",
            defaultHint + "\nHere we make use of the <color=#D49861>OnCollisionEnter()</color> method." +
            "\nYou may trigger a rotation on the y-axis whenever the player collides with the objects tagged <color=#D49861>Obstacle</color> or any other behaviour."));

    }


    public Task getCurrentTask()
    {
        return taskList[VirtualScriptEditor.Instance.Counter];
    }

}