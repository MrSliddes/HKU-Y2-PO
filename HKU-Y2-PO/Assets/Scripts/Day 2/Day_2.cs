using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Day_2 : MonoBehaviour
{
    public int appear_2_Times = 0;
    public int appear_3_Times = 0;

    // Start is called before the first frame update
    void Start()
    {
        ScanBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScanBoxes()
    {
        System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
        t.Start();

        // First read it from txt file
        var sr = new StreamReader("Assets/Resources/day_2_puzzle_input.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        print("Amount of lines: " + lines.Length);

        // Convert string into int array
        string[] lineArray = new string[lines.Length];
        for(int i = 0; i < lines.Length; i++)
        {
            lineArray[i] = string.Concat(lines[i].Where(c => !char.IsWhiteSpace(c)));
            CheckId(lines[i]);
        }

        // Checksum
        print("2 times appeared: " + appear_2_Times + " times. 3 times appeared: " + appear_3_Times + " times. So the checksum of box ids list is: " + appear_2_Times + " * " + appear_3_Times + " = " + (appear_2_Times * appear_3_Times));

        t.Stop();
        Debug.Log("(Part 1) This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds / 1000 + " seconds.");
        t.Start();

        CompareIds(lineArray);

        t.Stop();
        Debug.Log("(Part 2) This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds / 1000 + " seconds.");
    }

    /// <summary>
    /// Used for part 1
    /// </summary>
    /// <param name="id"></param>
    private void CheckId(string id)
    {
        Dictionary<char, int> letters = new Dictionary<char, int>();
        //print(id);
        // Loop trough string, check if letter is in dict, if not add, if it is upp the int count
        foreach(char c in id)
        {
            // Does dict contain letter
            if(letters.ContainsKey(c))
            {
                // Up the value
                letters[c]++;//+= 1; // make sure to only add 1 -_-
            }
            else
            {
                // add it
                letters.Add(c, 1);
            }
        }

        // Now check for count_2 and count_3
        // Loop trough dict
        bool contains2 = false;
        bool contains3 = false;
        //print(letters.Count);
        foreach(KeyValuePair<char, int> item in letters)
        {
            //print(item.Value);
            if(item.Value == 2)
            {
                contains2 = true;
            }
            else if(item.Value == 3)
            {
                contains3 = true;
            }
            if(contains2 && contains3) break;
        }

        if(contains2) appear_2_Times++;
        if(contains3) appear_3_Times++;
    }

    private void CompareIds(string[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(i + 1 == array.Length) break; // last one, doesnt need compare
            // Compare the array string downwards
            for(int j = i + 1; j < array.Length; j++)
            {
                string newS = "";
                string s = array[i];
                string s1 = array[j];

                for(int c = 0; c < s.Length; c++)
                {
                    if(s[c].Equals(s1[c])) newS += s[c];
                }
                if(newS.Length == 25)
                {
                    print(newS);
                }
            }
        }
    }
}

/* Analyze of day 2
 * 
 * Allright, question was a bit hard to understand but i get it. Time to write some code and see if it works
 * 
 * Wauw, it didnt work, and then i added some comments, and then suddenly it does work (my brain is going to explode), but i got 6150, and that is the correct awnser so hey, it works
 * It completed it in 781ms
 * 
 * Lets see if it can go any faster
 * Changing += 1 to ++ already made it to 760ms
 * Adding a break on when to check if it contains 2 or 3 made it to 724ms
 * Woops part 2 is first
 * 
 * After some breakdowns and finally finding out that my txt file has white spaces on all except the last line i finally got 1 string: rteotyxzbodglnpkudawhijsc
 * And its correct, fck yea.
 * 
 * Now some more optimizing, i removed some print lines from part 1 so that went down from 724ms to 6ms. Wauw.
 * Part 2 runs in 179ms, placing some code better (instead of creating it in every for loop) should make it a bit faster
 * 
 * Wierd, I rand it 2 times with the change and the first one was 204ms and the second 184ms. Seems to be 184ms, so thats 5ms slower?? wierd.
 * Placing it back now gives around 183ms, it isnt very consistend..
 * I think 1 of the major things is comparing each char with the other string, but i dont really know how to change that and this took me way longer then I wanted.
 
     */
