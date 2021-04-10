using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneHandler : MonoBehaviour
{
    public TMP_Text wtdText;
    public Animator animator;
    public TMP_Text editText;

    private bool active = false;

    public bool Active
    {
        get { return active; }
        set
        {
            active = value;

            if (active)
            {
                wtdText.gameObject.SetActive(false);
                animator.SetTrigger("Run");
                animator.ResetTrigger("Run2");
                editText.SetText("Press [ Ctrl + S ] To Save and Close");
    
            } else
            {
                wtdText.gameObject.SetActive(true);
                animator.SetTrigger("Run2");
                animator.ResetTrigger("Run");
                editText.SetText("Press [ Ctrl + E ] To Launch Edit Mode");

            }
        }
    }

    // TM var
    private static SceneHandler _instance;

    public static SceneHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneHandler>();
            }
            return _instance;
        }
    }


    private void Start()
    {
        VirtualScriptEditor.Instance.StartCoroutine(VirtualScriptEditor.Instance.StartEditor());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ControlOperatorDown() && !active)
            Active = true;
    }


    public static bool ControlOperatorDown()
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        bool cmd = Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
        return ctrl || cmd;
    }

}
