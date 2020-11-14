using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Day_3 : MonoBehaviour
{
    //private Dictionary<Vector2, int> fabricGrid = new Dictionary<Vector2, int>();
    private int[,] fabricGrid = new int[1000, 1000];
    private List<int[]> cleanClaim = new List<int[]>();

    // Start is called before the first frame update
    void Start()
    {

        CalcOverlap();

        /*
        string s = "#1 @ 16,576: 17x14";
        s = s.Remove(0, 5);
        print(s);
        s = s.Replace(",", "-").Replace(":", "-").Replace(" ", "-").Replace("x", "-");
        print(s);
        int numberIndex = 0;
        string x, y, width, height;
        x = y = width = height = "";
        bool isNumber = false;
        foreach(char c in s)
        {
            if(c.Equals('-'))
            {
                if(isNumber == true)
                {
                    numberIndex++;
                }
                isNumber = false;
                continue;
            }
            isNumber = true;

            switch(numberIndex)
            {
                case 0:                    
                    x += c;
                    break;
                case 1:
                    y += c;
                    break;
                case 2:
                    width += c;
                    break;
                case 3:
                    height += c;
                    break;
                default: Debug.LogError("wrong");
                    break;
            }
        }
        print(int.Parse(x));
        print(int.Parse(y));
        print(int.Parse(width));
        print(int.Parse(height));*/
    }

    private void CalcOverlap()
    {
        System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
        t.Start();

        // First read it from txt file
        var sr = new StreamReader("Assets/Resources/day_3_puzzle_input.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        print("Amount of lines: " + lines.Length);

        // Convert string into int array
        string[] lineArray = new string[lines.Length];

        for(int i = 0; i < lines.Length; i++)
        {
            // Convert string to numbers
            // first number starts at char 6
            lineArray[i] = string.Concat(lines[i].Where(c => !char.IsWhiteSpace(c)));
            string s = lines[i];
            // Remove junk from string
            int atIndex = s.IndexOf("@");
            // Get id int
            int id = int.Parse(s.Substring(1, atIndex - 2));
            s = s.Remove(0, atIndex+1); // Can remove first 6 chars, but as #number increases it takes longer
            s = s.Replace(",", "-").Replace(":", "-").Replace(" ", "-").Replace("x", "-");
            int numberIndex = 0;
            string x, y, width, height;
            x = y = width = height = "0";
            bool isNumber = false;
            //print(s);
            foreach(char c in s)
            {
                if(c.Equals('-'))
                {
                    if(isNumber == true)
                    {
                        numberIndex++;
                    }
                    isNumber = false;
                    continue;
                }
                isNumber = true;

                switch(numberIndex)
                {
                    case 0:
                        x += c;
                        break;
                    case 1:
                        y += c;
                        break;
                    case 2:
                        width += c;
                        break;
                    case 3:
                        height += c;
                        break;
                    default:
                        Debug.LogError("wrong");
                        break;
                }
            }

            //print(x);
            //print(y);
            //print(width);
            //print(height);
            //print(int.Parse(x));
            //print(int.Parse(y));
            //print(int.Parse(width));
            //print(int.Parse(height));
            PositionClaim(int.Parse(x), int.Parse(y), int.Parse(width), int.Parse(height), id);
        }

        // Loop trough grid and get inches coverd
        int coverdInchesSquare = 0;
        for(int x = 0; x < fabricGrid.GetLength(0); x++)
        {
            for(int y = 0; y < fabricGrid.GetLength(1); y++)
            {
                if(fabricGrid[x, y] > 1) coverdInchesSquare++;
            }
        }
        print("Coverd inches in square: " + coverdInchesSquare);

        t.Stop();
        Debug.Log("(Part 1) This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds / 1000 + " seconds.");
        t.Start();

        // Dont loop trough a million grid, takes too long
        // Loop trough cleanClaim positions and check if they are still clean
        print("Claims that seem clean: " + cleanClaim.Count);
        for(int i = 0; i < cleanClaim.Count; i++)
        {
            bool stillClean = true;
            for(int x = cleanClaim[i][0]; x < cleanClaim[i][0] + cleanClaim[i][2]; x++)
            {
                for(int y = cleanClaim[i][1]; y < cleanClaim[i][1] + cleanClaim[i][3]; y++)
                {
                    if(fabricGrid[x, y] > 1) stillClean = false;
                }
            }
            if(stillClean)
            {
                Debug.Log("Claim that doesnt overlap is #" + cleanClaim[i][4]);
                break;
            }
        }

        t.Stop();
        Debug.Log("(Part 2) This spagetti was cooked in: " + t.ElapsedMilliseconds + "ms, or " + t.ElapsedMilliseconds / 1000 + " seconds.");
    }

    private void PositionClaim(int posX, int posY, int width, int height, int iD)
    {
        // Add claim to array
        bool claimWasUnclaimed = true;
        for(int x = posX; x < posX + width; x++)
        {
            for(int y = posY; y < posY + height; y++)
            {
                if(fabricGrid[x, y] > 0) claimWasUnclaimed = false;
                fabricGrid[x, y]++;
            }
        }

        // P2
        if(claimWasUnclaimed)
        {
            cleanClaim.Add(new int[] { posX, posY, width, height, iD});
        }
    }
}


/* Analyze of day 2 
 * 
 * Looks like a hard assignment. See u in 2 hours
 * 
 * Went faster than expected. First code does it in 171ms (part 1) awnser was 111630 square inches
 * Now part 2
 * 
 * it is not fast to loop trough a million grid.
 * Hour later and we got it! awnser was id #724. completed it in 34ms
 * Optimized part 1 (removed junk) and that runs in 31ms
 
     
     */
