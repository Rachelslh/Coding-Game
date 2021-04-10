using System.Collections;
using System.Reflection;
using UnityEngine;


public class Task
{
    [Header("Code")]
    private string description;
    private string hints;
    private MethodInfo testMethod;

    private bool executeTest = false;
    public bool ExecuteTest
    {
        get { return executeTest; }
        set
        {
            executeTest = value;
            
            if (value)
                TaskTests.Instance.StartCoroutine("Test" + VirtualScriptEditor.Instance.Counter);
        }
    }

    private bool testPassed = false;
    public bool TestPassed
    {
        get { return testPassed; }
        set
        {
            testPassed = value;
            Debug.Log(value);
            if (value) {

                VirtualScriptEditor.Instance.Counter += 1;
            }
        }
    }

    public Task(string description, string hints)
    {
        this.description = description;
        this.hints = hints;
    }

    // add listener

    public string getDescription()
    {
        return description;
    }

    public string getHints()
    {
        return hints;
    }

    /*private void setTestFunc()
    {
        testMethod = typeof(TaskTests).GetMethod("Test" + VirtualScriptEditor.Instance.Counter);
    }

    public void RunTest()
    {
        setTestFunc();
        testMethod.Invoke(TaskTests.Instance, null);
    }*/
}
