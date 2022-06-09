fh = open("input.txt")
data = fh.read().strip().split("\n")
data = list(map(int, data))
fh.close()

def partOne(data):
    increases = 0
    previous = data[0]

    for number in data:
        if number > previous:
            increases += 1
        previous = number

    return increases

print(partOne(data))

# Part 2

def partTwo(data):
    increases = 0
    previousSum = sum(data[0:3])
    i = 1
    while i < len(data):
        currentSum = sum(data[i:i+3])
        if currentSum > previousSum:
            increases += 1
        previousSum = currentSum
        i += 1

    return increases

print(partTwo(data))
