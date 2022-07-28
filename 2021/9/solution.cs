using System;
using System.IO;
using System.Collections.Generic;

public class Output {
    public int Basin { get; }
    public int Sum { get; }
    public Output(int sum, int basin) {
        Sum = sum;
        Basin = basin;
    }
}

public class Solution {
    
    // Movement direction values
    static int mLeft = 0;
    static int mRight  = 1;
    static int mUp = 2;
    static int mDown = 3;

    private static string[] _sample = {
        "2199943210",
        "3987894921",
        "9856789892",
        "8767896789",
        "9899965678",
    };

    public static void Main(string[] args) {
        List<List<int>> sample = ParseInput(_sample);
        Output sampleSol = SolveProblem(sample);
        Console.WriteLine("Sum: {0}, basins: {1}", sampleSol.Sum, sampleSol.Basin);
        List<List<int>> input = ParseInput(ReadInput("input.txt"));
        Output realSol = SolveProblem(input);
        Console.WriteLine("Sum: {0}, basins: {1}", realSol.Sum, realSol.Basin);
    }

    private static string[] ReadInput(string filename) {
        string[] lines = File.ReadAllLines(filename);
        return lines;
    }

    private static List<List<int>> ParseInput(string[] input) {
        List<List<int>> output = new List<List<int>>();
        foreach(string line in input) {
            List<int> numbers = new List<int>();
            for(int i = 0; i < line.Length; i++) {
                numbers.Add(int.Parse(line[i].ToString()));
            }
            output.Add(numbers);
        }
        return output;
    }

    private static int PackPoint(int x, int y) {
        // Pack x,y into a single integer.
        // The values of x and y are guaranteed to fit in a byte
        int packed = (x << 8) ^ y;
        return packed;
    }

    private static void FindBasin(List<List<int>> heightMap, HashSet<int> basin, int direction, int x, int y) {
        // Move in all directions EXCEPT opposite of <direction>
        // This means that if <direction> is right, we can only move right, up, or down.
        // End conditions for recursion:
        // if encountered a wall, 9, or all adjacent positions have already been encountered.
        if (heightMap[y][x] == 9) { return; }
        int packedPoint = PackPoint(x, y);
        if (!basin.Contains(packedPoint)) {
            // We haven't seen this one yet, add it
            basin.Add(packedPoint);
        }
        // Now check, do we continue our recursion, or not?
        // If a wall is encountered, set the next value in its direction to 9, to signify that we're done here.
        int up = 1, down = 1, left = 1, right = 1;
        if (y == 0) { 
            // Top wall reached
            up = 9;
            down = heightMap[y+1][x];
        }
        else if (y+1 == heightMap.Count) {
            // Bottom wall
            down = 9;
            up = heightMap[y-1][x];
        }
        if (x == 0) {
            // Left wall
            left = 9;
            right = heightMap[y][x+1];
        }
        else if (x+1 == heightMap[y].Count) {
            // Right wall
            right = 9;
            left = heightMap[y][x-1];
        }

        // If the adjacent position in the direction of movement is neither 9, nor found in our basin set, we start a new recursion there.
        if (direction == mLeft) {
            // y+1/-1, x-1
            if ((left != 9) && (!basin.Contains(PackPoint(x-1, y)))) {
                FindBasin(heightMap, basin, mLeft, x-1, y);
            }
            if ((up != 9) && (!basin.Contains(PackPoint(x, y-1)))) {
                FindBasin(heightMap, basin, mUp, x, y-1);
            }
            if ((down != 9) && (!basin.Contains(PackPoint(x, y+1)))) {
                FindBasin(heightMap, basin, mDown, x, y+1);
            }
        }
        else if (direction == mRight) {
            // y+1/-1, x+1
            if ((right != 9) && (!basin.Contains(PackPoint(x+1, y)))) {
                FindBasin(heightMap, basin, mRight, x+1, y);
            }
            if ((up != 9) && (!basin.Contains(PackPoint(x, y-1)))) {
                FindBasin(heightMap, basin, mUp, x, y-1);
            }
            if ((down != 9) && (!basin.Contains(PackPoint(x, y+1)))) {
                FindBasin(heightMap, basin, mDown, x, y+1);
            }
        }
        else if (direction == mUp) {
            // y+1/-1, x-1
            if ((left != 9) && (!basin.Contains(PackPoint(x-1, y)))) {
                FindBasin(heightMap, basin, mLeft, x-1, y);
            }
            if ((right != 9) && (!basin.Contains(PackPoint(x+1, y)))) {
                FindBasin(heightMap, basin, mRight, x+1, y);
            }
            if ((up != 9) && (!basin.Contains(PackPoint(x, y-1)))) {
                FindBasin(heightMap, basin, mDown, x, y-1);
            }
        }
        else if (direction == mDown) {
            // y+1/-1, x-1
            if ((left != 9) && (!basin.Contains(PackPoint(x-1, y)))) {
                FindBasin(heightMap, basin, mLeft, x-1, y);
            }
            if ((right != 9) && (!basin.Contains(PackPoint(x+1, y)))) {
                FindBasin(heightMap, basin, mRight, x+1, y);
            }
            if ((down != 9) && (!basin.Contains(PackPoint(x, y+1)))) {
                FindBasin(heightMap, basin, mDown, x, y+1);
            }
        }
    }

    private static Output SolveProblem(List<List<int>> heightMap) {
        int rows = (int)heightMap.Count; // y
        int columns = (int)heightMap[0].Count; // x
        int left, right, above, below;
        int sum = 0;
        List<int> basins = new List<int>();
        for(int y = 0; y < rows; y++) {
            for(int x = 0; x < columns; x++) {
                int point = heightMap[y][x];
                try {
                    below = heightMap[y+1][x];
                    if (below <= point) { continue; }
                }
                catch (ArgumentOutOfRangeException) {}
                try {
                    left = heightMap[y][x-1];
                    if (left <= point) { continue; }
                }
                catch (ArgumentOutOfRangeException) {}
                try {
                    right = heightMap[y][x+1];
                    if (right <= point) { continue; }
                }
                catch (ArgumentOutOfRangeException) {}
                try {
                    above = heightMap[y-1][x];
                    if (above <= point) { continue; }
                }
                catch (ArgumentOutOfRangeException) {}
                // Since one of the conditions would have to cause the continue operation to run,
                // it means that if we reached here, no condition was true, and thus this point is the lowest.
                sum += point+1;
                HashSet<int> basin = new HashSet<int>();
                List<int> directions = new List<int>() { mRight, mUp, mDown, mLeft };
                foreach(int direction in directions) {
                    FindBasin(heightMap, basin, direction, x, y);
                }
                basins.Add(basin.Count);
            }
        }
        basins.Sort();
        basins.Reverse();
        //Console.WriteLine(string.Join(", ", basins));
        int largestBasinsSize = basins[0] * basins[1] * basins[2];
        return new Output(sum, largestBasinsSize);
    }
}
