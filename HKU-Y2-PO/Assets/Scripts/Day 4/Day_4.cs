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
        System.Array.Sort(lineArray, delegate (string twt1, string twt2) //////// <<<<<<<<<<<<<<<<< This is wrong, he sorts it as :00 first and then :23 -_-
        {
            if(twt1 == null && twt2 == null) return 0;
            if(twt1 == null) return -1;
            if(twt2 == null) return 1;
            if(twt1.Substring(1, 9) == twt2.Substring(1, 9))
            { 
                if(int.Parse(twt1.Substring(12, 1)) < int.Parse(twt2.Substring(12, 1)))
                {
                    // if 00 < 23
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return twt1.Substring(0, 16).CompareTo(twt2.Substring(0, 16));
        });

        // Test feedback
        for(int i = 0; i < lineArray.Length; i++)
        {
            print(lineArray[i]);
        }
        return;

        /// ^^^^^^^^^^ good?


        // Now make list of guard id followed by minutes sleeped
        int currentGuardIdList = 0;
        List<Guard> guards = new List<Guard>(); // x = id, y = timeasleepminutes
        int startTimeFallsAsleep = 0;
        for(int i = 0; i < lineArray.Length; i++)
        {
            string s = lineArray[i];
            s = s.ToLower();
            
            // Check for guard difference
            if(s.IndexOf("guard") != -1)
            {
                // new guard
                int guardId = GetGuardIDFromString(s);

                // Check if guard already exists
                int oldGuardIndex = -1;
                if(GuardExists(guards, guardId, out oldGuardIndex))
                {
                    currentGuardIdList = oldGuardIndex;
                }
                else
                {
                    guards.Add(new Guard(guardId, 0));
                    currentGuardIdList = guards.Count -1;
                }
            }
            else
            {
                // same guard, get action
                if(s.IndexOf("falls") != -1)
                {
                    // Get the start minute
                    string time = s.Substring(s.IndexOf("]") - 2, 2);
                    startTimeFallsAsleep = int.Parse(time); // 05
                    if(guards[currentGuardIdList].id == 643) print("startime 643 sleeping: " + startTimeFallsAsleep);
                }
                else if(s.IndexOf("wakes") != -1)
                {
                    // add time to start minute
                    string time = s.Substring(s.IndexOf("]") - 2, 2);
                    int timeInt = int.Parse(time);
                    guards[currentGuardIdList].sleepTimeMinutes += (timeInt - startTimeFallsAsleep); // = new Vector2(guards[currentGuardIdList].x, guards[currentGuardIdList].y + (int.Parse(time) - startTimeFallsAsleep));

                    if(guards[currentGuardIdList].id == 643) print(s.Substring(0, 11) + " startime 643 woke up at: " + timeInt + "so he has been sleeping for: " + (timeInt - startTimeFallsAsleep));

                    // Calculate minuteIndexes
                    for(int j = startTimeFallsAsleep; j < startTimeFallsAsleep + (timeInt - startTimeFallsAsleep) - 1; j++)
                    {
                        //guards[currentGuardIdList].minutesIndexes[j]++;
                    }

                    string hash = "";
                    for(int j = 0; j < 60; j++)
                    {
                        if(j >= startTimeFallsAsleep - 1 && j <= startTimeFallsAsleep + (timeInt - startTimeFallsAsleep) - 2)
                        {
                            hash += "#";
                            //print("before " + guards[currentGuardIdList].minutesIndexes[j]);
                            guards[currentGuardIdList].minutesIndexes[j] = guards[currentGuardIdList].minutesIndexes[j] + 1;
                            //print("after " + guards[currentGuardIdList].minutesIndexes[j]);
                        }
                        else
                        {
                            hash += "o";
                        }
                    }
                    if(guards[currentGuardIdList].id == 643) print(hash);
                }
            }
        }

        // Show guards (debug)
        // Sort array only on sleep time
        //System.Array.Sort(guards, delegate (Guard twt1, Guard twt2)
        //{
        //    if(twt1 == null && twt2 == null) return 0;
        //    if(twt1 == null) return -1;
        //    if(twt2 == null) return 1;
        //    return twt1.sleepTimeMinutes.CompareTo(twt2.sleepTimeMinutes);
        //});
        guards = guards.OrderBy(c => c.sleepTimeMinutes).ToList();
        guards.Reverse();
        for(int i = 0; i < guards.Count; i++)
        {
            print("guard: " + guards[i].id + " slept for" + guards[i].sleepTimeMinutes);
        }

        // Get the heighest min asleep guard
        //int heighest = 0;
        //for(int i = 0; i < guards.Count; i++)
        //{
        //    if(guards[i].sleepTimeMinutes > guards[heighest].sleepTimeMinutes) heighest = i;
        //}
        // Doesnt need to anymore, just sorted array
        print("Guard with heighest sleep is guard: " + guards[0].id);

        // Now get the heighest minute that the guard sleeps on
        int heighestMinute = 0;
        int index = 0;
        string minIndex = "";
        for(int i = 0; i < 60; i++)
        {
            if(guards[0].minutesIndexes[i] > heighestMinute)
            {
                heighestMinute = guards[0].minutesIndexes[i];
                index = i;
                //print(guards[0].minutesIndexes[i] + " " + heighestMinute);
            }
            minIndex += guards[0].minutesIndexes[i].ToString() + "|";
            //print(guards[heighest].minutesIndexes[i]);
        }
        Debug.Log("yo: " + minIndex);
        Debug.Log("HeighestMin is: " + heighestMinute + " at index: " + index);

        Debug.Log("The guard with id: " + guards[0].id + " has the heighest sleep on minute: " + index + ". That makes part 1 awnser: " + guards[0].id * index);
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

    private bool GuardExists(List<Guard> g, int id, out int index)
    {
        index = -1;
        for(int i = 0; i < g.Count; i++)
        {
            if(g[i].id == id)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
}

public class Guard
{
    public int id;
    public int sleepTimeMinutes = 0;
    public int[] minutesIndexes = new int[60];

    public Guard(int id, int sleepTimeMinutes)
    {
        this.id = id;
        this.sleepTimeMinutes = sleepTimeMinutes;
    }
}

/* Day 4 analyze
 * Gotta sort that stuff first
 * Got awnser 123083, that was wrong, first time i got it wrong when trying the full code.
 * Thatss because i read the question wrong... I need to find the minute the guard is most asleep on. not just id * allminutessleeped
 * Thats a job for future me cause it is 00:58 right now and i wanna sleep. 
 * 
 * Future me here, i got a new awnser of 2433, which apperently is also wrong
 * 
 * Now i got an id of 643 but the most minute asleep isnt just 1 number its several...
 * 
 * couple of hours later.. (16 hour total btw for 10 days) i got 69369, still wrong
 * 
 * Now trying 47463, also wrong..
 */
