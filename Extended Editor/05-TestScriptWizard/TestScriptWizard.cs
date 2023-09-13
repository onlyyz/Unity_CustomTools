using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestScriptWizard: ScriptableWizard 
{

    [MenuItem("CustomEditorTutorial/TestScriptWizard")]
    private static void MenuEntryCall() 
    {
        DisplayWizard<TestScriptWizard>("Title");
    }

    private void OnWizardCreate() 
    {

    }
}
