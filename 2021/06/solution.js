// JavaScript with NodeJS

/* I'm proud of this one, because after my initial naive brute force solution only solved part one,
 * I came up with a much better approach that solves both parts, and even much larger inputs, instantly. */

fs = require('fs');

let sample = [3, 4, 3, 1, 2]; // Sample input from AoC

function solveProblem(numbers, lastDay) {
    // We create an array of 9 elements.
    // The elements' index numbers represent the days.
    // So all fishes with 0 days will be grouped at index 0,
    // 1 day grouped at index 1, 2 days group at index 2, etc until index 8, which houses the number of new fishes.
    fishes = [];
    for(let i = 0; i < 9; i++) {
        fishes.push(0);
    }
    // Organize the fishes array based on the initial state
    // each n is treated as an index number of the day it corresponds to
    // So if we encounter 3, index 3 gets one more fish in it.
    // If we encounter 0, index 0 gets one more fish, and so forth.
    for(let n of numbers) {
        fishes[n]++;
    }
    // Start the loop
    for(let day = 0; day < lastDay; day++) {
        // Everyday all the fishes move one day downwards in the array, until they reach index 0.
        // Once they reach 0, they create new fishes, one per each of them.
        const created = fishes[0]; // How many were created today
        for(let i = 1; i < fishes.length; i++) {
            fishes[i-1] = fishes[i];
        }
        // The fishes that moved down from index 7 need to be
        // combined with the fishes who reached 0 and have now recycled.
        fishes[6] += created;
        fishes[8] = created;
    }
    let sum = 0;
    for(let fish of fishes) { sum += fish; }
    return sum;
}

function readInput(filename) {
    // Read the input file in a synchronous manner,
    // otherwise the solution function starts executing before
    // it has the actual data it needs to operate on.
    try {
        let numbers = [];
        const data = fs.readFileSync(filename, "utf8");
        const digits = data.split(",");
        for(let digit of digits) {
            numbers.push(Number(digit));
        }
        return numbers;
    }
    catch(err) {
        console.log("Error reading file %s: %s", filename, err);
    }
}

const inputNumbers = readInput("input.txt");

// Test

const cases = { 80: 5934, 256: 26984457539 }

for(let test in cases) {
    console.log("Testing with %d", test);
    const result = solveProblem(sample, test);
    if (result == cases[test]) {
        console.log("...passed");
    }
    else {
        console.log("...failed. Expected: %d, actual: %d", cases[test], result);
    }
}

console.log(solveProblem(inputNumbers, 80)); // Part one is up to 80
console.log(solveProblem(inputNumbers, 256)); // Part two is 256 days
