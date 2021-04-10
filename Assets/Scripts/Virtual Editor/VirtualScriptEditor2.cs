using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Needs more work: 
// - TMP input fields mess up the caret position when rich text tags are added
// - Include Backspace key action
public class VirtualScriptEditor2 : MonoBehaviour {

    [Header("Components")]

    public TMP_Text WTD;
    public TMP_Text task_text;
    public TMP_Text hints_text;

    public SyntaxTheme syntaxTheme;
    public TMP_InputField codeUI;
    public TMP_Text lineNumbersUI;

    // Script loading vars
    private Component fileComponent;
    private string filePath = Application.streamingAssetsPath + "/TestScript.cs";

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
    public bool ExecuteFile
    {
        get { return executeFile; }
        set
        {
            executeFile = value;

            StopCoroutine("RunTask");

            if (value)
                StartCoroutine("RunTask");
        }
    }

    private bool executeFile = false;
    public bool isThereErrors = false;


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


    private bool initiMode = true;
    private const string indentString = "    ";
    string legalChars = "abcdefghijklmnopqrstuvwxyz 0.123456789+-/*=<>()[]{},";

    private string code;
    private string Code {
        get { return code; }
        set
        {
            code = value;
            OnValueChanged();
        }
    }

    private int charIndex = 0;
    private int numLines = 0;

    // Start is called before the first frame update
    void Awake()
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
                fileComponent = comp;
                ExecuteFile = true;
                return comp;
            }
            else
            {
                object obj = System.Activator.CreateInstance(t);
                ExecuteFile = true;
                return obj;
            }
        };

        loader.destroyInstance = (object instance) =>
        {
            if (instance is Component) Destroy(instance as Component);
        };

        // Copy file content to the input field
        string fileContent = File.ReadAllText(filePath);
        code = fileContent;
        code = code.Replace("\r", "");
        Code = FormatIndenting();
        Debug.Log(Code);
        codeUI.onValidateInput += delegate (string input, int charIndex, char addedChar) { return OnValidateChar(addedChar); };

        Counter += 1;
    }


    void Update () {

        if (!SceneHandler.Instance.Active)
            return;

        synchronizedInvoke.ProcessQueue();

        if (Input.GetKeyDown(KeyCode.S) && SceneHandler.ControlOperatorDown())
            SaveAndClose();
    }


    string FormatIndenting()
    {
        string formattedCode = "";
        string[] lines = Code.Split('\n');

        int indentLevel = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Contains("}"))
            {
                indentLevel--;
            }

            //int originalLineLength = line.Length;
            //line = line.TrimStart (' ');

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
        // print ("originalCharIndex: " + originalCharIndex + "  new: " + charIndex);
        //print (code + "  " + formattedCode);
        return formattedCode;
    }

    char OnValidateChar(char charToValidate)
    {
        charIndex = codeUI.caretPosition;

        if (legalChars.Contains(charToValidate.ToString().ToLower()) || charToValidate == '\n')
         {
            if (string.IsNullOrEmpty(code) || charIndex == code.Length)
                Code += charToValidate;
            else
                Code = Code.Insert(charIndex, charToValidate.ToString());
           

            codeUI.caretPosition++;
            Debug.Log(Code);
            //return charToValidate;
        }

        return '\0';
    }


    void OnValueChanged()
    {
        //string formattedCode = FormatIndenting();
        string formattedCode = Code;
        codeUI.text = SyntaxHighlighter.HighlightCode(formattedCode, syntaxTheme);
        SetLineNumbers();
    }


    private IEnumerator RunTask()
    {
        /*if (!isThereErrors)
        {
            yield return fileComponent;

            TaskManager.Instance.getCurrentTask().RunTest();
            if (result)
                Counter += 1;
        }
        */
        yield return null;
    }


    void SaveAndClose()
    {
        File.WriteAllText(filePath, Code);

        if (initiMode)
        {
            // Pass file to the CSharp Compiler
            loader.LoadAndWatchScriptsBundle(new[] { filePath });

            initiMode = false;
        }

        SceneHandler.Instance.Active = false;
    }


    private void UpdateUI()
    {
        WTD.SetText(TaskManager.Instance.getCurrentTask().getDescription());
        task_text.SetText(TaskManager.Instance.getCurrentTask().getDescription());
        hints_text.SetText(TaskManager.Instance.getCurrentTask().getHints());
    }


    void SetLineNumbers () {
        string numbers = "";

        numLines = Code.Split ('\n').Length;
        for (int i = 0; i < numLines; i++) {
            numbers += (i + 1) + "\n";
        }
        
        lineNumbersUI.text = numbers;
    }

    string RemoveTags()
    {
        string[] lines = codeUI.text.Split('\n');
        string newCode = "";

        for (int i = 0; i < lines.Length; i++)
        {
            newCode += StripTagsCharArray(lines[i]);
            if (i < lines.Length - 1)
                newCode += '\n';
        }

        //codeUI.Select();
        //codeUI.text = "";

        return newCode;
    }


    public static string StripTagsCharArray(string source)
    {
        char[] array = new char[source.Length];
        int arrayIndex = 0;
        bool inside = false;

        for (int i = 0; i < source.Length; i++)
        {
            char let = source[i];

            if (let == '<')
            {
                if ((i + 6) < source.Length)
                {
                    if (source.Substring(i + 1, 6).Equals("/color"))
                    {
                        inside = true;
                        continue;
                    }
                }
                if ((i + 7) < source.Length)
                {
                    if (source.Substring(i + 1, 7).Equals("color=#"))
                    {
                        inside = true;
                        continue;
                    }
                }
            }
            else if (let == '>')
            {
                if ((i - 7) >= 0)
                {
                    if (source.Substring(i - 7, 7).Equals("</color"))
                    {
                        inside = false;
                        continue;
                    }
                }
                if ((i - 13) >= 0)
                {
                    if (source.Substring(i - 13, 7).Equals("color=#"))
                    {
                        inside = false;
                        continue;
                    }
                }
            }

            if (!inside)
            {
                array[arrayIndex] = let;
                arrayIndex++;
            }
        }
        return new string(array, 0, arrayIndex);
    }
}