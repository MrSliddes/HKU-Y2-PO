using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class Day_4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SortGuards();
    }

    private void SortGuards()
    {
        System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
        t.Start();

        // First read it from txt file
        var sr = new StreamReader("Assets/Resources/day_4_puzzle_input.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        print("Amount of lines: " + lines.Length);

        // Convert string into int array
        string[] lineArray = new string[lines.Length];
               
        // Initialize lineArray
        for(int i = 0; i < lines.Length; i++)
        {
            lineArray[i] = string.Concat(lines[i].Where(c => !char.IsWhiteSpace(c)));            
        }

        // Sort array only on date format
        System.Array.Sort(lineArray, delegate (string twt1, string twt2)
        {
            if(twt1 == null && twt2 == null) return 0;
            if(twt1 == null) return -1;
            if(twt2 == null) return 1;
            return twt1.Substring(0, 16).CompareTo(twt2.Substring(0, 16));
        });

        // Test feedback
        for(int i = 0; i < lineArray.Length; i++)
        {
            //print(lineArray[i]);
        }

        // Now make list of guard id followed by minutes sleeped
        int currentGuardIdList = 0;
        List<Vector2> guards = new List<Vector2>(); // x = id, y = timeasleepminutes
        for(int i = 0; i < lineArray.Length; i++)
        {
            string s = lineArray[i];
            s = s.ToLower();
            int startTimeFallsAsleep = 0;
            // Check for guard difference
            if(s.IndexOf("guard") != -1)
            {
                // new guard                
                guards.Add(new Vector2(GetGuardIDFromString(s), 0));
                currentGuardIdList = guards.Count - 1;
            }
            else
            {
                // same guard, get action
                if(s.IndexOf("falls") != -1)
                {
                    // Get the start minute
                    string time = s.Substring(s.IndexOf("]") - 2, 2);
                    startTimeFallsAsleep = int.Parse(time);
                }
                else if(s.IndexOf("wakes") != -1)
                {
                    // add time to start minute
                    string time = s.Substring(s.IndexOf("]") - 2, 2);
                    guards[currentGuardIdList] = new Vector2(guards[currentGuardIdList].x, guards[currentGuardIdList].y + (int.Parse(time) - startTimeFallsAsleep));
                }
            }
        }

        // Show guards (debug)
        for(int i = 0; i < guards.Count; i++)
        {
            print("guard: " + guards[i]);
        }

        // Get the heighest min asleep guard
        int heighest = 0;
        for(int i = 0; i < guards.Count; i++)
        {
            if(guards[i].y > guards[heighest].y) heighest = i;
        }

        Debug.Log("The guard with id: " + guards[heighest].x + " has the heighest sleep in minutes: " + guards[heighest].y + ". That makes part 1 awnser: " + guards[heighest].x * guards[heighest].y);

        t.Stop();
        Debug.Log("(Part 1) This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds / 1000 + " seconds.");
    }

    private int GetGuardIDFromString(string s)
    {
        // String comes in, check for # then continue in string and check if char is Parsable
        int startIndex = s.IndexOf("#");
        string id = "";
        int result;
        for(int i = startIndex + 1; i < s.Length; i++)
        {
            if(int.TryParse(s[i].ToString(), out result))
            {
                id += s[i];
            }
            else
            {
                break;
            }
        }

        return int.Parse(id);
    }
}

/* Day 4 analyze
 * Gotta sort that stuff first
 * Got awnser 123083, that was wrong, first time i got it wrong when trying the full code.
 * Thatss because i read the question wrong... I need to find the minute the guard is most asleep on. not just id * allminutessleeped
 * Thats a job for future me cause it is 00:58 right now and i wanna sleep. 
 */
