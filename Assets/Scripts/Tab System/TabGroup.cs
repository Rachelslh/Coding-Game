using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;

    /*
    public Color scriptIdle;
    public Color scriptHover;
    public Color scriptActive;
    */
    public TabButton selectedTab;
    public List<GameObject> scriptsToSwap;


    private void Start()
    {
        if(selectedTab != null)
        {
            OnTabSelected(selectedTab);
        }
    }


    public void Subscribe(TabButton tabButton)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabButton tabButton)
    {
        ResetTabs();
        /*if (selectedTab != null && tabButton != selectedTab)
        {
            tabButton.background.color = scriptHover;
        }*/
    }

    public void OnTabExit(TabButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton)
    {
        selectedTab = tabButton;
        ResetTabs();
        //tabButton.background.color = scriptActive;
        tabButton.line.enabled = true;

        int index = tabButton.transform.GetSiblingIndex();
        for (int i = 0; i < scriptsToSwap.Count; i++)
        {
            if (i == index)
            {
                scriptsToSwap[i].SetActive(true);
                VirtualScriptEditor.Instance.selectedScript = scriptsToSwap[i].GetComponent<ScriptAsset>();
            }
            else
            {
                scriptsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton tabButton in tabButtons)
        {
            if (selectedTab != null && tabButton == selectedTab) { continue; }
            //tabButton.background.color = scriptIdle;
            tabButton.line.enabled = false;
        }
    }
}
