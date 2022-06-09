// C#

using System;
using System.IO; // For File
using System.Collections.Generic;

public struct BingoNumber {
    public BingoNumber(int x, int y, int n, bool isMarked) {
        Number = n;
        IsMarked = isMarked;
        X = x;
        Y = y;
    }
    public int Number { get; }
    public bool IsMarked { get; set; }
    public int X { get; }
    public int Y { get; }
}

public class Solution {

    public static void Main(string[] args) {
        string inputFile = "input.txt";
        SolveProblem(inputFile);
    }

    static void SolveProblem(string inputFile) {
        /* Read input first */
        string[] lines = File.ReadAllLines(inputFile);
        
        // The randomly generated bingo numbers
        List<int> upcomingNumbers = new List<int>();
        foreach(string digit in lines[0].Split(",")) {
            upcomingNumbers.Add(int.Parse(digit));
        }

        // Now starting at line 2, read 5 lines at a time
        Dictionary<int, BingoNumber> positions = new Dictionary<int, BingoNumber>();
        int i = 2;
        int leastSteps = upcomingNumbers.Count; // The least steps taken so far in order to win - indicates first winner
        int mostSteps = 0; // Most steps taken so far to win - indicates last winnner after all grids have been processed
        int firstWinnerSum = 0, lastWinnerSum = 0;
        while(i < lines.Length) {
            List<List<BingoNumber>> grid = new List<List<BingoNumber>>();
            int y = 0;
            for(int iPlusFive = i+5; i < iPlusFive; i++, y++) {
                List<BingoNumber> row = new List<BingoNumber>();
                int x = 0;
                foreach(string digit in lines[i].Split(" ")) {
                    try {
                        int n = int.Parse(digit);
                        // Create a new BingoNumber struct for this n and set it as not marked.
                        BingoNumber bingoNumber = new BingoNumber(x, y, n, false);
                        row.Add(bingoNumber);
                        // Add it to the positions dictionary for easy retrieval later
                        positions.Add(n, bingoNumber);
                        x++;
                    }
                    catch(System.FormatException) {
                        // Some whitespace was encountered, skip it
                        continue;
                    }
                }
                grid.Add(row);
            }
            // Skip the empty line
            i++;

            // Now process this grid
            for(int s = 0; s < upcomingNumbers.Count; s++) {
                int num = upcomingNumbers[s];
                if (positions.ContainsKey(num)) {
                    BingoNumber bingoNumber = positions[num];
                    bingoNumber.IsMarked = true;
                    // Copy the modified structure back into the dictionary and list of lists
                    positions[num] = bingoNumber;
                    grid[bingoNumber.Y][bingoNumber.X] = bingoNumber;
                // Now check, based on where num is located, whether a full row or column has been marked
                    if (isWinner(grid, bingoNumber.Y, bingoNumber.X)) {
                        // Compute the sum
                        int sum = 0;
                        for(y = 0; y < grid.Count; y++) {
                            for(int x = 0; x < grid[y].Count; x++) {
                                if (!grid[y][x].IsMarked) { sum += grid[y][x].Number; }
                            }
                        }
                        sum *= num;
                        // Determine if it's the first winner
                        if (s < leastSteps) {
                            leastSteps = s;
                            firstWinnerSum = sum;
                        }
                        else if (s > mostSteps) {
                            mostSteps = s;
                            lastWinnerSum = sum;
                        }
                        break;
                    }
                }
            }
            // Clear the positions dictionary
            positions = new Dictionary<int, BingoNumber>();
        }
        Console.WriteLine("Fastest winner sum: {0}", firstWinnerSum);
        Console.WriteLine("Last winner sum: {0}", lastWinnerSum);
    }
    private static bool isWinner(List<List<BingoNumber>> grid, int ny, int nx) {
        bool allMarked = true;
        for(int y = 0; y < 5; y++) {
            BingoNumber num = grid[y][nx];
            if (!num.IsMarked) {
                allMarked = false;
                break;
            }
        }
        if (allMarked) { return true; }
        // Now x
        allMarked = true;
        for(int x = 0; x < 5; x++) {
            BingoNumber num = grid[ny][x];
            if (!num.IsMarked) {
                return false;
            }
        }
        return true;
    }
}
