fh = open("input.txt")
data = fh.read().strip().split("\n")
fh.close()

def compareBits(bitsList: list[str], position: int):
    ones = 0
    zeroes = 0
    for bitSequence in bitsList:
        if bitSequence[position] == "1":
            ones += 1
        else:
            zeroes += 1
    
    return (ones, zeroes)

def partOne(data):
    epsilon = []
    gamma = []
    for x in range(len(data[0])):
        ones, zeroes = compareBits(data, x)
        if zeroes > ones:
            gamma.append("0")
            epsilon.append("1")
        elif ones > zeroes:
            gamma.append("1")
            epsilon.append("0")

    gammaValue = int("".join(gamma), 2)
    epsilonValue = int("".join(epsilon), 2)

    return gammaValue*epsilonValue

def partTwo(data):
    oxygenValue = findOxygen(data, 0)
    scrubberValue = findScrubber(data, 0)
    return oxygenValue*scrubberValue

def findOxygen(data, position):
    if len(data) == 1:
        return int(data.pop(), 2)
    results = []
    ones, zeroes = compareBits(data, position)
    for bits in data:
        if ones >= zeroes:
            # Oxygen keeps the more common bit, or 1 if equal
            if bits[position] == "1":
                results.append(bits)
        else:
            # Zeroes more common
            if bits[position] == "0":
                results.append(bits)
    return findOxygen(results, position+1)

def findScrubber(data, position):
    # Scrubber keeps the less common bit, or 0 if equal
    if len(data) == 1:
        return int(data.pop(), 2)
    results = []
    ones, zeroes = compareBits(data, position)
    for bits in data:
        if zeroes <= ones:
            if bits[position] == "0":
                results.append(bits)
        else:
            # Ones less common
            if bits[position] == "1":
                results.append(bits)
    return findScrubber(results, position+1)

testData = [
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010",
        ]

testData2 = [
        "001101",
        "100010",
        "101000",
        "011101"
        ]

print(partOne(data))
print(partTwo(data))
