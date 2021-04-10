using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitionner : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            LoadMainScene();
    }


    public void LoadMainScene()
    {
        StartCoroutine(MakeTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator MakeTransition( int sceneIndex )
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
   
    }
}
