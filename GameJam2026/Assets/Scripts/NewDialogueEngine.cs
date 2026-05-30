using ReadingInAFile;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static ReadingInAFile.InitFile;
using System.Collections;

public class NewDialogueEngine : MonoBehaviour
{
    #region Global Variables
    /// string that keeps track of which line of dialogue we're having
    /// references the Row ID from the dialogue text file
    private string strRowID;

    ///GameObjects we will turn on and off
    [SerializeField] private GameObject dialogueEngine; //Set to active to enable dialogue
    [SerializeField] private Text PromptText;
    [SerializeField] private Text ResponseOptionsText;
    [SerializeField] private Image CurrentImage;

    //class that reads in the conversation file and the object it translates it into
    private ArrayList allDialogues;
    private InitFile.DialogueNode currentDialogueNode;


    //Input Actions
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here
    private InputAction toggleDialogue;
    private InputAction dialogueOption1;
    private InputAction dialogueOption2;
    private InputAction dialogueOption3;
    private InputAction dialogueOption4;
    private InputAction dialogueOption5;
    private InputAction dialogueOption6;
    private InputAction dialogueOption7;
    private InputAction dialogueOption8;
    private InputAction dialogueOption9;
    #endregion


    private void OnEnable()
    {
        //go into your InputActions asset and enable the "Dialogue" action map
        InputActions.FindActionMap("Dialogue").Enable();

        //Read in the conversations text file and convert it into an arraylist of DialogueNode objects

        InitFile InitFile = new();
        InitFile.Main(out allDialogues);
        strRowID = "";
    }

    private void OnDisable()
    {
        //Disable the Dialogue actionmap when you're not in Dialogue
        InputActions.FindActionMap("Dialogue").Disable();
    }

    private void Awake()
    {
        //point toggleDialogue to the correct action in the actionmap
        toggleDialogue = InputSystem.actions.FindAction("StartDialogue");
        dialogueOption1 = InputSystem.actions.FindAction("Response1");
        dialogueOption2 = InputSystem.actions.FindAction("Response2");
        dialogueOption3 = InputSystem.actions.FindAction("Response3");
        dialogueOption4 = InputSystem.actions.FindAction("Response4");
        dialogueOption5 = InputSystem.actions.FindAction("Response5");
        dialogueOption6 = InputSystem.actions.FindAction("Response6");
        dialogueOption7 = InputSystem.actions.FindAction("Response7");
        dialogueOption8 = InputSystem.actions.FindAction("Response8");
        dialogueOption9 = InputSystem.actions.FindAction("Response9");
    }

    // Update is called once per frame
    void Update()
    {
        CheckActionTaken(); //The function that keeps track of which conversation we're on

        if (toggleDialogue.WasPressedThisFrame())//this checks if the spacebar is pressed, then switches to the next convo if so
        {
            dialogueEngine.SetActive(true);
            //in a real game, we should set this field to the first Row ID of a dialogue when it is 
            //triggered, like when we approach a character and the cut scene starts
            //for demo purposes we will just switch back and forth
            if (strRowID.StartsWith("john"))
            {
                strRowID = "jane000";
                UpdateCurrentLine();
            }
            else
            {
                strRowID = "john000";
                UpdateCurrentLine();
            }
        }

    }

    void CheckActionTaken()//The function that keeps track of which conversation we're on
    {
        ///depending on input, switch which dialogue is active
        if (dialogueOption1.WasPressedThisFrame())
        {
            ValidateOption(1);
        }

        if (dialogueOption2.WasPressedThisFrame())
        {
            ValidateOption(2);
        }

        if (dialogueOption3.WasPressedThisFrame())
        {
            ValidateOption(3);
        }

        if (dialogueOption4.WasPressedThisFrame())
        {
            ValidateOption(4);
        }

        if (dialogueOption5.WasPressedThisFrame())
        {
            ValidateOption(5);
        }

        if (dialogueOption6.WasPressedThisFrame())
        {
            ValidateOption(6);
        }

        if (dialogueOption7.WasPressedThisFrame())
        {
            ValidateOption(7);
        }

        if (dialogueOption8.WasPressedThisFrame())
        {
            ValidateOption(8);
        }

        if (dialogueOption9.WasPressedThisFrame())
        {
            ValidateOption(9);
        }
    }

    private void ValidateOption(int pintReponseSelected)
    {
        if (currentDialogueNode.ResponseOptions.Count >= pintReponseSelected)
        {
            ResponseOption respSelected = (ResponseOption)currentDialogueNode.ResponseOptions[pintReponseSelected - 1];
            strRowID = respSelected.nextNodeID;
            UpdateCurrentLine();
        }
        else
        {
            bool blnIsEndOfDialogue = true;
            foreach (ResponseOption response in currentDialogueNode.ResponseOptions)
            {
                if (!string.IsNullOrEmpty(response.nextNodeID))
                {
                    blnIsEndOfDialogue = false;
                }
            }
            if (blnIsEndOfDialogue)
            {
                dialogueEngine.SetActive(false);
            }
        }
    }

    private void UpdateCurrentLine()
    {
        currentDialogueNode = FindDialogueNodeByNodeID(strRowID);
        if (currentDialogueNode == null) //prompt with that RowID not found
        {
            dialogueEngine.SetActive(false);
        }

        string strPrompt = currentDialogueNode.prompt;
        PromptText.text = strPrompt;

        string strResponses = "";
        for (int i = 0; i < currentDialogueNode.ResponseOptions.Count; i++)
        {
            ResponseOption respOption = (ResponseOption)currentDialogueNode.ResponseOptions[i];
            strResponses += $"{i + 1}: {respOption.AnswerText}\n";
        }
        ResponseOptionsText.text = strResponses;

        var image = GameObject.Find(strRowID);
        //CurrentImage.sprite = image;
        //CurrentImage.image = image;
        //this doesnt work but the idea is there will be an image for each frame and the title of 
        //the image in the asset folder is the same as the strRowID for the prompt
        //so we find the image that matches the prompt and update the current image to that
    }

    public DialogueNode FindDialogueNodeByNodeID(string pstrNodeID)
    {
        foreach (DialogueNode convertedRow in allDialogues)
        {
            if (convertedRow.NodeID.Equals(pstrNodeID))
            {
                return convertedRow;
            }
        }
        return null;
    }
}