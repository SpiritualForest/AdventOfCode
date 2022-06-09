fh = open("input.txt")
data = fh.read().strip().split("\n")
fh.close()

def partOne(data):
    horizontal = 0
    depth = 0

    for line in data:
        command, value = line.split()
        value = int(value)
        if command == "forward":
            horizontal += value
        elif command == "down":
            depth += value
        elif command == "up":
            depth -= value

    return horizontal*depth

print(partOne(data))

def partTwo(data):
    horizontal = 0
    depth = 0
    aim = 0

    for line in data:
        command, value = line.split()
        value = int(value)
        if command == "down":
            aim += value
        elif command == "up":
            aim -= value
        else:
            # forward
            horizontal += value
            if aim > 0:
                depth += aim*value

    return horizontal*depth

print(partTwo(data))
