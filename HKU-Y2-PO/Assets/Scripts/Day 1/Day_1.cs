using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Day_1 : MonoBehaviour
{
    private int resultingFrequency = 0;
    private HashSet<int> foundFrequencys = new HashSet<int>();

    private bool foundDoubbleFreq = false;

    // Start is called before the first frame update
    void Start()
    {
        // It starts at 0
        foundFrequencys.Add(0);

        PrintPuzzleInput();
        print("Resulting frequency is: " + resultingFrequency);
    }

    private void PrintPuzzleInput()
    {
        System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
        t.Start();

        var sr = new StreamReader("Assets/Resources/day_1_puzzle_input.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        print("Amount of lines: " + lines.Length);

        // Convert string into int array
        int[] lineArray = new int[lines.Length];
        for(int i = 0; i < lines.Length; i++)
        {
            lineArray[i] = int.Parse(lines[i]);
        }

        int infExit = 9999; // To prevent infinite loop if something is wrong
        // While there hasnt been a doubble freq keep looping trough it
        while(foundDoubbleFreq == false)
        {
            for(int i = 0; i < lineArray.Length; i++)
            {
                AddToFrequency(lineArray[i]);
                if(foundDoubbleFreq) break;
            }

            infExit--;
            if(infExit <= 0)
            {
                Debug.LogWarning("Takes 2 long");
                break;
            }
        }

        t.Stop();
        Debug.Log("This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds/1000 + " seconds.");
    }

    private void AddToFrequency(int v)
    {
        resultingFrequency += v;
        // Check if frequency is already reached
        if(!foundFrequencys.Contains(resultingFrequency))
        {
            foundFrequencys.Add(resultingFrequency);
        }
        else
        {
            Debug.LogWarning("This frequency is already here: " + resultingFrequency);
            foundDoubbleFreq = true;
        }
    }
}

/*  Analyze of day 1
 *  
 *  Its just adding and subtracting.
 *  First need a way to read the puzzle input
 *  
 *  Cant just put it in a array, need commas so we need to read the txt file.
 *  
 *  Allright that worked. (PrintPuzzleInput)
 *  
 *  Now just convert it to ints and add it to a number
 *  Got awnser 538.
 *  That is correct! up to the next part.
 *  
 *  So we need to keep track of the frequencys we got.
 *  First thing that comes to mind is a list so lets try that
 *  
 *  List.Contains seems to not be working.. so lets try a dictionary,
 *  butt since we only need 1 value we can use a HashSet
 *  
 *  Seems I have a problem with just using .Contains(int) dont know whyy.
 *  
 *  Nevermind I just didnt read it correctly. .Contains works but i need to loop multiple times trough the puzzle input -_-.
 *  I got number 77271.
 *  And thats correct! Day 1 completed. Now comes the fun part, optimizing.
 *  After adding System.diagonstics I found out my spagetti takes 15 seconds to complete and thats waaay to long.
 *  
 *  First lets remove some debug lines, surly that should remove some ms.
 *  Then first just convert the string into a int array instead of covering it every loop into a int.
 *  BOOM! Just 27ms! what a difference. So instead of having to convert the strings to ints every time we just do it once and that saves about ~15 seconds.
 
     
     */