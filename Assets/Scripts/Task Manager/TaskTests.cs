using System.Collections;
using UnityEngine;
using TMPro;


public class TaskTests : MonoBehaviour
{
    public GameObject player;
    public TMP_Text testText;
    public GameObject spawner;

    private static TaskTests _instance;

    public static TaskTests Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TaskTests>();
            return _instance;
        }
    }
    
    // Verify rigidbody existence
    public IEnumerator Test0()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        //player.transform.rotation = Quaternion.identity;
        TaskManager.Instance.getCurrentTask().TestPassed = 
            (rb != null) ? true : false;

        yield return null;
    }

    // Verify disable rb
    public IEnumerator Test1()
    {
        player.transform.rotation = Quaternion.identity;
        TaskManager.Instance.getCurrentTask().TestPassed =
            (player.GetComponent<Rigidbody>() == null) ? true : false;

        yield return null;
    }

    // Verify character movement
    public IEnumerator Test2()
    {
        float rightV = 0, leftV = 0, forwardV = 0, backwardV = 0;
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            testText.text = "Press [ D ]";

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D) == true);

            yield return new WaitForFixedUpdate();

            rightV = cc.velocity.normalized.x;

            testText.text = "Press [ A ]";

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) == true);

            yield return new WaitForFixedUpdate();

            leftV = cc.velocity.normalized.x;

            testText.text = "Press [ W ]";

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W) == true);

            yield return new WaitForFixedUpdate();

            forwardV = cc.velocity.normalized.z;


            testText.text = "Press [ S ]";

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.S) == true);

            yield return new WaitForFixedUpdate();

            backwardV = cc.velocity.normalized.z;

            testText.text = "";

            TaskManager.Instance.getCurrentTask().TestPassed =
                (leftV < 0 && rightV > 0 && forwardV > 0 && backwardV < 0) ? true : false;
        }
        else
            TaskManager.Instance.getCurrentTask().TestPassed = false;

    }


    // Verify direction change
    public IEnumerator Test3()
    {
        bool rightD, leftD, forwardD, backwardD;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        testText.text = "Press [ D ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D) == true);

        yield return new WaitForFixedUpdate();

        rightD = (player.transform.rotation.eulerAngles.y == 90);

        testText.text = "Press [ A ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) == true);

        yield return new WaitForFixedUpdate();

        leftD = (player.transform.rotation.eulerAngles.y == 270);

        testText.text = "Press [ W ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W) == true);

        yield return new WaitForFixedUpdate();

        forwardD = (player.transform.rotation.eulerAngles.y == 0);


        testText.text = "Press [ S ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.S) == true);

        yield return new WaitForFixedUpdate();

        backwardD = (player.transform.rotation.eulerAngles.y == 180);

        testText.text = "";

        TaskManager.Instance.getCurrentTask().TestPassed =
            (leftD && rightD && forwardD && backwardD) ? true : false;
    }


    // Verify gradual direction change
    public IEnumerator Test4()
    {
        bool rightD, leftD, forwardD, backwardD;
        float R, L, F, B;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Transform transform = player.transform;

        testText.text = "Press [ D ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D) == true);

        yield return new WaitForFixedUpdate();

        R = transform.rotation.eulerAngles.y;
        rightD = (R > 0 && R <= 90) ||
            (R >= 90 && R < 180);

        testText.text = "Press [ A ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) == true);

        yield return new WaitForFixedUpdate();

        L = transform.rotation.eulerAngles.y;
        leftD = (L < R);

        testText.text = "Press [ W ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W) == true);

        yield return new WaitForFixedUpdate();

        F = transform.rotation.eulerAngles.y;
        forwardD = (F > R);


        testText.text = "Press [ S ]";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.S) == true);

        yield return new WaitForFixedUpdate();

        B = transform.rotation.eulerAngles.y;
        backwardD = (B > F);

        testText.text = "";

        TaskManager.Instance.getCurrentTask().TestPassed =
            (leftD && rightD && forwardD && backwardD) ? true : false;
    }

    // Verify the camera follow - Not done
    public IEnumerator Test5()
    {
        TaskManager.Instance.getCurrentTask().TestPassed = true;

        yield return null;
    }

    // Verify the camera follow's rotation - Not done
    public IEnumerator Test6()
    {
        TaskManager.Instance.getCurrentTask().TestPassed = true;

        yield return null;
    }

    // Verify the collisions - Not done
    public IEnumerator Test7()
    {
        spawner.SetActive(true);
        TaskManager.Instance.getCurrentTask().TestPassed = true;

        yield return null;
    }
}
