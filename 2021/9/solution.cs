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
    
    static Dictionary<int, List<int>> movements = new Dictionary<int, List<int>>() {
        // direction, list of { x, y }
        { mLeft, new List<int>() { 1, 0 } },
        { mRight, new List<int>() { -1, 0 } },
        { mUp, new List<int>() { 0, -1 } },
        { mDown, new List<int>() { 0, 1 } }
    };

    static Dictionary<int, int> opposites = new Dictionary<int, int>() {
        { mLeft, mRight },
        { mRight, mLeft },
        { mUp, mDown },
        { mDown, mUp }
    };

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
        // point is guaranteed to be within range because this function is always called
        // either from a low point in SolveProblem(), or recursively AFTER all the validation checks
        // have been made here.
        int point = heightMap[y][x];
        int packedPoint = PackPoint(x, y);
        if (!basin.Contains(packedPoint)) {
            basin.Add(packedPoint);
        }
        
        /* Restart the recursion:
         * We remove the opposite direction of travel (so if this point was reached by travelling left, mRight is removed,
         * because it makes no sense to travel backwards to a previously checked point).
         * After that we find the value of each adjacent point in directions that are valid for travelling,
         * check that it doesn't overflow the heightMap list's boundaries, that it's not 9, and that it hasn't been previously checked.
         * If all these validation checks are passed for the examined adjacent point, the recursion chain continues with it.
         * If none of the checks pass for any direction, the recursion ends, because there is nothing left to do, and the function exits. */

        List<int> directions = new List<int>() { mLeft, mRight, mDown, mUp };
        int opposite = opposites[direction];
        directions.Remove(opposite);
        foreach(int newDirection in directions) {
            int nextX = x+movements[newDirection][0];
            int nextY = y+movements[newDirection][1];
            if ((nextX < 0) || (nextX == heightMap[0].Count)) {
                // Skip this
                continue;
            }
            if ((nextY < 0) || (nextY == heightMap.Count)) {
                // Skip
                continue;
            }
            // Valid next point
            packedPoint = PackPoint(nextX, nextY);
            if ((basin.Contains(packedPoint)) || (heightMap[nextY][nextX] == 9)) {
                // The next point is either a 9, or has already been checked previously. Skip
                continue;
            }
            // The next point is valid, not 9, and not previously checked. Restart the recursion.
            FindBasin(heightMap, basin, newDirection, nextX, nextY);
        }
    }

    private static Output SolveProblem(List<List<int>> heightMap) {
        int rows = heightMap.Count; // y
        int columns = heightMap[0].Count; // x
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
        int largestBasinsSize = basins[0] * basins[1] * basins[2];
        return new Output(sum, largestBasinsSize);
    }
}
