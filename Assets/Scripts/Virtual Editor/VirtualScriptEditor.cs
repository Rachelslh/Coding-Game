using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VirtualScriptEditor : MonoBehaviour
{

    [Header("Task Info")]

    public TMP_Text WTD;
    public TMP_Text editText;
    public TMP_Text task_text;
    public TMP_Text hints_text;

    [Header("Code Editor")]
    public SyntaxTheme syntaxTheme;

    [Header("Errors UI")]
    public TMP_Text errorsUI;

    [Header("Script Assets")]
    // Script loading vars
    string sourceFolder = Application.streamingAssetsPath;
    public List<ScriptAsset> scriptAssets;

    DeferredSynchronizeInvoke synchronizedInvoke;
    CSharpCompiler.ScriptBundleLoader loader;

    private static VirtualScriptEditor _instance;

    public static VirtualScriptEditor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VirtualScriptEditor>();
            }
            return _instance;
        }
    }

    // Properties
    public bool ExecuteCode
    {
        get { return executeCode; }
        set
        {
            executeCode = value;

            StopCoroutine("RunTask");

            if (value)
                StartCoroutine("RunTask");
        }
    }

    private bool executeCode = false;

    public bool isThereErrors = false;
    public string errors = "0 Errors.";

    public int Counter
    {
        get { return counter; }
        set
        {
            counter = value;
            UpdateUI();
        }
    }

    private int counter = -1;


    private bool initMode = true;
    private const string indentString = "    ";
    string legalChars = "abcdefghijklmnopqrstuvwxyz 0.123456789+-/*%=<>()[]{},&|\"\"\'\';";
    
    public ScriptAsset selectedScript;

    [Header("Caret")]
    public Vector3 initialPos;
    public float cursorDelay = 0.3f;
    private float _timer = 0f;
    string stopChar = ".";


    public IEnumerator StartEditor()
    {
        TaskManager.Instance.Init();
        
        synchronizedInvoke = new DeferredSynchronizeInvoke();

        loader = new CSharpCompiler.ScriptBundleLoader(synchronizedInvoke);
        loader.logWriter = new CSharpCompiler.UnityLogTextWriter();

        loader.createInstance = (Type t) =>
        {
            if (typeof(Component).IsAssignableFrom(t))
            {
                Component comp = this.gameObject.AddComponent(t);
                ExecuteCode = true;
                return comp;
            }
            else
            {
                object obj = System.Activator.CreateInstance(t);
                ExecuteCode = true;
                return obj;
            }
        };

        loader.destroyInstance = (object instance) =>
        {
            if (instance is Component) Destroy(instance as Component);
        };
        
        UpdateEditor();

        // Register input keys to authorize the continued input press
        CustomInput.instance.RegisterKey(KeyCode.Backspace);
        CustomInput.instance.RegisterKey(KeyCode.LeftArrow);
        CustomInput.instance.RegisterKey(KeyCode.RightArrow);
        CustomInput.instance.RegisterKey(KeyCode.UpArrow);
        CustomInput.instance.RegisterKey(KeyCode.DownArrow);

        yield return new WaitForSeconds(2f);

        editText.text = "Press[Ctrl + E] To Launch Edit Mode";

        Counter += 1;

        Camera.main.GetComponent<Animator>().enabled = false ;
    }


    void Update()
    {
        synchronizedInvoke.ProcessQueue();

        if (!SceneHandler.Instance.Active)
            return;

        if (selectedScript != null && selectedScript.codeUI != null && selectedScript.caret != null)
        {
            HandleTextInput();
            HandleSpecialInput();

            BlinkCaret();
            setCaret();

            UpdateEditor();
        }

        if (Input.GetKeyDown(KeyCode.S) && SceneHandler.ControlOperatorDown())
            SaveAndClose();
    }
    

    void BlinkCaret()
    {
        // Blink
        _timer += Time.deltaTime;
        if (_timer > cursorDelay)
        {
            _timer = 0f;
            selectedScript.caret.enabled = (!selectedScript.caret.enabled);
        }
    }


    void setCaret()
    {

        // Get single line height, and height of code up to charIndex
        selectedScript.codeUI.text = stopChar;
        float stopCharWidth = selectedScript.codeUI.preferredWidth;
        float singleLineHeight = selectedScript.codeUI.preferredHeight;
        
        string codeUpToCharIndex = selectedScript.GetCode().Substring(0, selectedScript.charIndex);
        selectedScript.codeUI.text = codeUpToCharIndex + stopChar;
        float height = selectedScript.codeUI.preferredHeight - singleLineHeight;

        // Get indent level
        int indentLevel = GetIndentLevel();

        // Get string from start of current line up to caret
        string textUpToCaretOnCurrentLine = "";
        for (int i = selectedScript.charIndex - 1; i >= 0; i--)
        {
            if (selectedScript.GetCode()[i] == '\n' || i == 0)
            {
                textUpToCaretOnCurrentLine = selectedScript.GetCode().Substring(i, selectedScript.charIndex - i);
                break;
            }
        }
        textUpToCaretOnCurrentLine = textUpToCaretOnCurrentLine.Replace("\n", "");
        for (int i = 0; i < indentLevel; i++)
        {
            textUpToCaretOnCurrentLine = indentString + textUpToCaretOnCurrentLine;
        }

        selectedScript.codeUI.text = textUpToCaretOnCurrentLine + stopChar;
        float width = selectedScript.codeUI.preferredWidth - stopCharWidth;

        selectedScript.caret.rectTransform.anchoredPosition = initialPos;
        selectedScript.caret.rectTransform.anchoredPosition += Vector2.right * (width + selectedScript.caret.rectTransform.rect.width / 2f);
        selectedScript.caret.rectTransform.anchoredPosition += Vector2.down * (selectedScript.caret.rectTransform.rect.height / 2 + height);
    }


    int GetIndentLevel()
    {
        // Get indent level
        int indentLevel = 0;
        string[] lines = selectedScript.GetCode().Split('\n');
        bool startIndentationNextLine = false;
        // lineIndex
        for (int i = 0; i <= selectedScript.lineIndex; i++)
        {
            if (startIndentationNextLine)
            {
                startIndentationNextLine = false;
                indentLevel++;
            }
            if (lines[i].Contains("{"))
            {
                startIndentationNextLine = true;
            }
            if (lines[i].Contains("}"))
            {
                if (startIndentationNextLine)
                {
                    startIndentationNextLine = false;
                }
                else
                {
                    indentLevel--;
                }
            }
        }

        return indentLevel;
    }


    void HandleTextInput()
    {
        string input = Input.inputString;

        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftCommand))
        {
            foreach (char c in input)
            {
                if (legalChars.Contains(c.ToString().ToLower()))
                {
                    //lastInputTime = Time.time;
                    if (string.IsNullOrEmpty(selectedScript.GetCode()) ||
                        selectedScript.charIndex == selectedScript.GetCode().Length)
                        selectedScript.AddCode(c);
                    else
                        selectedScript.InsertCode(c, selectedScript.charIndex);
                    
                    selectedScript.charIndex++;
                }
            }
        }
    }


    void HandleSpecialInput()
    {
        // New line
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //lastInputTime = Time.time;
            if (string.IsNullOrEmpty(selectedScript.GetCode()) || selectedScript.charIndex == selectedScript.GetCode().Length)
                selectedScript.AddCode('\n');
            else
                selectedScript.InsertCode('\n', selectedScript.charIndex);

            selectedScript.charIndex++;
            selectedScript.lineIndex++;
        }

        // Delete
        if (CustomInput.instance.GetKeyPress(KeyCode.Backspace))
        {
            if (selectedScript.charIndex > 0)
            {
                //lastInputTime = Time.time;
                char deletedChar = selectedScript.GetCode()[selectedScript.charIndex - 1];
                string start = selectedScript.GetCode().Substring(0, selectedScript.charIndex - 1);
                string end = selectedScript.GetCode().Substring(selectedScript.charIndex, 
                    selectedScript.GetCode().Length - selectedScript.charIndex);
                selectedScript.SetCode(start + end);

                selectedScript.charIndex--;

                if (deletedChar == '\n')
                    selectedScript.lineIndex--;
            }
        }

        if (CustomInput.instance.GetKeyPress(KeyCode.LeftArrow))
        {
            //lastInputTime = Time.time;
            if (selectedScript.GetCode().Length > 0 && selectedScript.charIndex > 0)
            {
                if (selectedScript.GetCode()[selectedScript.charIndex - 1] == '\n')
                    selectedScript.lineIndex--;
            }
            selectedScript.charIndex = Mathf.Max(0, selectedScript.charIndex - 1);
        }

        if (CustomInput.instance.GetKeyPress(KeyCode.RightArrow))
        {
            //lastInputTime = Time.time;
            if (selectedScript.GetCode().Length > selectedScript.charIndex)
            {
                if (selectedScript.GetCode()[selectedScript.charIndex] == '\n')
                    selectedScript.lineIndex++;
            }
            selectedScript.charIndex = Mathf.Min(selectedScript.GetCode().Length, selectedScript.charIndex + 1);
        }

        if (CustomInput.instance.GetKeyPress(KeyCode.UpArrow))
        {
            if (selectedScript.lineIndex > 0)
            {
                //lastInputTime = Time.time;
                string[] lines = selectedScript.GetCode().Split('\n');
                int numCharsInPreviousLines = 0;
                for (int i = 0; i < selectedScript.lineIndex; i++)
                    numCharsInPreviousLines += lines[i].Length + 1;

                selectedScript.charIndex = numCharsInPreviousLines - 1;
                selectedScript.lineIndex--;
            }
        }

        if (CustomInput.instance.GetKeyPress(KeyCode.DownArrow))
        {
            string[] lines = selectedScript.GetCode().Split('\n');

            if (selectedScript.lineIndex < lines.Length - 1)
            {
                //lastInputTime = Time.time;

                int numCharsInPreviousLines = lines[0].Length;

                for (int i = 1; i <= selectedScript.lineIndex + 1; i++)
                    numCharsInPreviousLines += lines[i].Length + 1;

                selectedScript.charIndex = numCharsInPreviousLines;
                selectedScript.lineIndex++;
            }
        }
    }


    void UpdateEditor()
    {
        string formattedCode = FormatIndenting();
        selectedScript.codeUI.text = SyntaxHighlighter.HighlightCode(formattedCode, syntaxTheme);
        selectedScript.SetLineNumbers(selectedScript.numLines);

        errorsUI.text = errors;
    }


    string FormatIndenting()
    {
        string formattedCode = "";
        string[] lines = selectedScript.GetCode().Split('\n');

        int indentLevel = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Contains("}"))
            {
                indentLevel--;
            }

            for (int j = 0; j < indentLevel; j++)
            {
                line = indentString + line;
            }

            formattedCode += line;
            if (i < lines.Length - 1)
            {
                formattedCode += "\n";
            }

            if (line.Contains("{"))
            {
                indentLevel++;
            }
        }

        return formattedCode;
    }


    private IEnumerator RunTask()
    {
        yield return new WaitForSeconds(1f);

        if (Counter < TaskManager.Instance.taskList.Count)
        {
            yield return new WaitUntil(() => isThereErrors == false);
            errors = "0 Errors.";

            TaskManager.Instance.getCurrentTask().ExecuteTest = true;
        }
    }


    void SaveAndClose()
    {
        isThereErrors = false;

        foreach (ScriptAsset script in scriptAssets)
        {
            script.UpdateFile();
        }

        if (initMode)
        {
            foreach (ScriptAsset script in scriptAssets)
            {
                loader.LoadAndWatchScriptsBundle(new[] { script.GetFullPath() });
            }

            initMode = false;
        }

        SceneHandler.Instance.Active = false;
    }


    private void UpdateUI()
    {
        if (Counter < TaskManager.Instance.taskList.Count)
        {
            WTD.SetText(TaskManager.Instance.getCurrentTask().getDescription());
            task_text.SetText(TaskManager.Instance.getCurrentTask().getDescription());
            hints_text.SetText(TaskManager.Instance.getCurrentTask().getHints());
        } else
        {
            WTD.SetText("");
            task_text.SetText("Have fun adding extra functionnalities to the game.");
            hints_text.SetText("No hints.");
        }
    }

}