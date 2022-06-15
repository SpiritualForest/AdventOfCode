def solvePartOne(inputList: list[str]) -> int:
    uniqueSegments = {2, 4, 3, 7} # The amount of segments used by the digits 1, 4, 7, 8 respectively
    count = 0
    for line in inputList:
        signals, digits = line.split("|")
        values = digits.strip().split()
        for value in values:
            if len(value) in uniqueSegments:
                count += 1
    return count

"""
Shape indices
 0000
5    1
5    1
 6666
4    2
4    2
 3333

Algorithm explanation:

First we parse the input, line by line, into the signal patterns and output digits.
Then we analyze the signal patterns.
We know that 1, 4 and 7 are made up of a unique number of letters.
The first letter whose position we can confirm is the one who's part of 7, but not part of 1.
After that we confirm the index positions for 1 and 2, then 5 and 6, and finally 3 and 4.
1 and 2 are compared against the number 6, because they are mutually exclusive there.
5 and 6 are compared against 0, because they are mutually exclusive there.
3 and 4 are compared against 9, because they are mutually exclusive there.
Based on this mutual exclusivity, we know that if one letter of the possible two in each unconfirmed position
appears in our signal pattern, we can confirm the position that both these letters should go to.
Only numbers whose shape use 6 segments provide this ability to determine the exclusivity, that's why we must use them.
After we've determined the positions, we move on to determine which number each remaining pattern we haven't examined, corresponds to,
based on what letters exist in it.
"""

def solvePartTwo(inputList: list[str]) -> int:
    finalSum = 0
    for line in inputList:
        signals, digits = line.split("|")
        # Remove the trailing spaces
        signals, digits = signals.strip(), digits.strip()
        signalPatterns = sorted(signals.split(), key=len)
        remainingLetters = list("abcdefg")
        # Our goal is to confirm the index position of each letter.
        # First, we need to find 1, 4, 7, as they're the information patterns we need
        # in order to determine the initial indices.
        informationPatterns = []
        for pattern in signalPatterns[:3]:
            informationPatterns.append(pattern)
            for letter in pattern:
                try:
                    # Remove this letter from the remaining letters
                    remainingLetters.remove(letter)
                except ValueError:
                    pass

        one, two = informationPatterns[0] # Letters
        for i, pattern in enumerate(informationPatterns[1:]):
            for letter in informationPatterns[0]:
                pattern = pattern.replace(letter, "")
            informationPatterns[i+1] = pattern
        zero = informationPatterns[1]
        five, six = informationPatterns[2] # Letters
        # Now put the remaining letters in positions 3 and 4
        three, four = remainingLetters
        # Now it's time to analyze the rest of the signal patterns
        # We try to make a shape based on the curretly known placements of each letter
        # Examine shape 6 to confirm indices 1 and 2, shape 0 to confirm indices 5 and 6,
        # and shape 9 to confirm indices 3 and 4.
        # First, examine the letters for indices 1 and 2
        onetwo, zeroonetwo, onetwofivesix = ["".join(sorted(pattern)) for pattern in (one+two, zero+one+two, one+two+five+six)]
        confirmedPatterns = {onetwo: 1, zeroonetwo: 7, onetwofivesix: 4}
        indices = dict()
        for pattern in signalPatterns[6:9]:
            # 6 first
            pattern = "".join(sorted(pattern))
            if one in pattern and two not in pattern:
                # "one" is in index 2
                indices[2], indices[1] = one, two
                sixPattern = pattern
            elif two in pattern and one not in pattern:
                # "two" is in index 2
                indices[1], indices[2] = one, two
                sixPattern = pattern
            # 0 second
            elif five in pattern and six not in pattern:
                # five is index 5, six is index 6
                indices[5], indices[6] = five, six
                zeroPattern = pattern
            elif six in pattern and five not in pattern:
                # six is index 5, five is index 6
                indices[6], indices[5] = five, six
                zeroPattern = pattern
            # Now 9
            elif three in pattern and four not in pattern:
                indices[3], indices[4] = three, four
                ninePattern = pattern
            else:
                indices[4], indices[3] = three, four
                ninePattern = pattern
        
        # pattern str -> number
        confirmedPatterns.update(dict(zip((zeroPattern, sixPattern, ninePattern, "abcdefg"), (0, 6, 9, 8))))
        # After confirming all the positions, we can examine the final three patterns
        for pattern in signalPatterns[3:6]:
            pattern = "".join(sorted(pattern))
            # the shape for 3 doesn't contain indices 4 and 5
            if indices[4] not in pattern and indices[5] not in pattern:
                # It's 3
                confirmedPatterns[pattern] = 3
            # The shape for 5 doesn't contain indices 1 and 4
            elif indices[1] not in pattern and indices[4] not in pattern:
                confirmedPatterns[pattern] = 5
            # The shape for 2 doesn't contain indices 5 and 2
            elif indices[2] not in pattern and indices[5] not in pattern:
                confirmedPatterns[pattern] = 2
       
        # Now we decode the digits
        digitSum = ""
        for digit in digits.split():
            digitSum += str(confirmedPatterns["".join(sorted(digit))])
        finalSum += int(digitSum)
    return finalSum

def readInput(filename: str) -> list[str]:
    fh = open(filename)
    lines = fh.read()
    fh.close()
    lines = lines.strip().split("\n")
    return lines

sampleLines = readInput("sample.txt")
realLines = readInput("input.txt")
print(solvePartOne(sampleLines))
print(solvePartOne(realLines))
print(solvePartTwo(sampleLines))
print(solvePartTwo(realLines))
