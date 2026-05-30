using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace ReadingInAFile
{
    public class InitFile : MonoBehaviour
    {
        #region Custom Object Classes
        //the first two elements of each line create this object
        public class DialogueNode
        {
            public string prompt;
            public string NodeID;
            public ArrayList ResponseOptions;                  //An ArrayList of ResponseOption objects 

            public DialogueNode(string pstrNodeID, string pstrPrompt, ArrayList parrDialogueOptions)
            {
                prompt = pstrPrompt;
                NodeID = pstrNodeID;
                ResponseOptions = parrDialogueOptions;
            }
        }

        //the remaining elements for each line of the dialogue text file create these objects
        public class ResponseOption
        {
            public string nextNodeID;
            public string AnswerText;

            public ResponseOption(string pstrnextNodeID, string pstrAnswerText)
            {
                nextNodeID = pstrnextNodeID;
                AnswerText = pstrAnswerText;
            }
        }
        #endregion

        #region Constants/Hardcoded Values
        const string cstrFileLocation = @"Assets/Text Files/DialogueFileExample.txt";
        const string cstrFileDelimiter = "|";
        const string cstrFileIgnoreLineSymbol = "//";
        const string cstrFileNewLineSymbol = "~";
        #endregion

        #region Global Variables
        public ArrayList AllDialogues;
        #endregion

        #region Execute
        public void Main(out ArrayList AllDialogues)
        {
            AllDialogues = new();
            if (File.Exists(cstrFileLocation))
            {
                // Store each line in array of strings
                string[] arrLines = File.ReadAllLines(cstrFileLocation);
                InitializeLinkedNodes(arrLines);
                /*
                 *to confirm the file was read into the object correctly
                foreach (DialogueNode convertedRow in AllDialogues)
                {
                    Console.WriteLine(convertedRow.prompt + "\n\n");
                    foreach (ResponseOption response in convertedRow.ResponseOptions)
                    {
                        Console.WriteLine(response.nextNodeID + ": " + response.AnswerText);
                    }

                }
                */
            }
            else
            {
                throw new Exception("We couldn't find the text file at " + cstrFileLocation);
            }
        }
        #endregion
        #region Helper Functions

        private void InitializeLinkedNodes(string[] parrLines)
        {
            AllDialogues = new();
            foreach (string strLine in parrLines)
            {
                if (strLine.StartsWith(cstrFileIgnoreLineSymbol) || string.IsNullOrEmpty(strLine))//check if we ignore this line of the file
                {
                    continue;
                }

                string strFormattedLine = strLine.Replace(cstrFileNewLineSymbol, "\n");
                if (strFormattedLine.EndsWith(cstrFileDelimiter))
                {
                    strFormattedLine = strFormattedLine.Substring(0, strFormattedLine.Length - cstrFileDelimiter.Length);
                }

                string[] arrLineParts = strFormattedLine.Split(cstrFileDelimiter);

                string strRowID = arrLineParts[0].Trim();
                string strRowPrompt = arrLineParts[1].Trim();
                ArrayList arrDialogueOptions = new();

                for (int i = 0; i < arrLineParts.Length - 2; i += 2)
                {
                    string nextRowID = "";
                    string strAnswerText = arrLineParts[i + 2].Trim();
                    if (arrLineParts.Length > i + 3) //a response may not have a node that it points to if its the end of a conversation
                    {
                        nextRowID = arrLineParts[i + 3].Trim();
                    }
                    arrDialogueOptions.Add(new ResponseOption(nextRowID, strAnswerText));
                }

                AllDialogues.Add(new DialogueNode(strRowID, strRowPrompt, arrDialogueOptions));
            }
        }
        #endregion
    }
}