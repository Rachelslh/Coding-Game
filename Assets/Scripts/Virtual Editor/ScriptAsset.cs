using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptAsset : MonoBehaviour
{
    [Header("Code Editor")]
    public TMP_Text codeUI;
    public TMP_Text lineNumbersUI;
    public Image caret;

    public int charIndex = 0, lineIndex = 0, numLines = 0;

    private string code { get; set; }

    private string sourceFolder = Application.streamingAssetsPath;

    public string fileName;


    private void Awake()
    {
        // Copy file content to the input field
        string fileContent = File.ReadAllText(GetFullPath());
        fileContent = fileContent.Replace('\r', ' ');
        code = fileContent;
    }


    public string GetCode()
    {
        return code;
    }

    public void AddCode(char c)
    {
        code += c;
        Debug.Log("HERE");
    }

    public void InsertCode(char c, int charIndex)
    {
        code = code.Insert(charIndex, c.ToString());
    }

    public void SetCode(string newCode)
    {
        code = newCode;
    }

    public void UpdateFile()
    {
        File.WriteAllText(GetFullPath(), code);
    }

    public void SetLineNumbers(int numLines)
    {
        string numbers = "";

        numLines = code.Split('\n').Length;
        for (int i = 0; i < numLines; i++)
        {
            numbers += (i + 1) + "\n";
        }

        lineNumbersUI.text = numbers;
    }

    public string GetFullPath()
    {
        return sourceFolder + '/' + fileName;
    }

}
