import java.util.*;
import java.io.*;

class Solution {
    public static void main(String args[]) {
        int[] sample = {16, 1, 2, 0, 4, 2, 7, 1, 2, 14};
        //System.out.format("%d\n", solvePartOne(sample));
        //System.out.format("%d\n", solvePartTwo(sample));
        int[] answers = solveProblem(parseInput("input.txt"));
        System.out.format("%d\n%d\n", answers[0], answers[1]);
    }

    private static int[] parseInput(String filename) {
        // In this case, we know it's only one line,
        // so we'll forgo error checking
        String line = "";
        try {
            File inputFile = new File(filename);
            Scanner reader = new Scanner(inputFile);
            line = reader.nextLine();
            reader.close();
        }
        catch(FileNotFoundException e) {
            System.out.println("Error reading file");
            return null;
        }
        String[] digits = line.split(",");
        int[] numbers = new int[digits.length];
        for(int i = 0; i < digits.length; i++) {
            numbers[i] = Integer.parseInt(digits[i]);
        }
        return numbers;
    }

    private static HashMap<Integer, Integer> groupCrabs(int[] positions) {
        // Organizes the crabs into groups based on their current position
        HashMap<Integer, Integer> groups = new HashMap<Integer, Integer>();
        for(int pos : positions) {
            if (groups.containsKey(pos)) {
                groups.put(pos, groups.get(pos)+1);
            }
            else {
                groups.put(pos, 1);
            }
        }
        return groups;
    }

    private static int[] solveProblem(int[] positions) {
        // First organize the crabs into groups
        HashMap<Integer, Integer> groups = groupCrabs(positions);
        // Brute force approach
        int max = 0;
        for(int i : groups.keySet()) {
            if (i > max) { max = i; }
        }
        int normalLeastFuel = (int)Math.pow(2, 32); // Part 1, "normal fuel"
        int expensiveLeastFuel = (int)Math.pow(2, 32); // Part 2, "expensive fuel"
        for(int i = 0; i < max; i++) {
            int normalFuel = 0;
            int expensiveFuel = 0;
            for(int pos : groups.keySet()) {
                int n = (pos > i) ? pos-i : i-pos;
                normalFuel += n*groups.get(pos);
                expensiveFuel += (n*(n+1)/2)*groups.get(pos);
            }
            if (normalFuel < normalLeastFuel) {
                normalLeastFuel = normalFuel;
            }
            if (expensiveFuel < expensiveLeastFuel) {
                expensiveLeastFuel = expensiveFuel;
            }
        }
        int[] results = {normalLeastFuel, expensiveLeastFuel};
        return results;
    }
}
