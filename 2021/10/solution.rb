sample = ["[({(<(())[]>[[{[]{<()<>>",
          "[(()[<>])]({[<{<<[]>>(",
          "{([(<{}[<>[]}>{[]{[(<()>",
          "(((({<>}<{<{<>}{[]{[]{}",
          "[[<[([]))<([[{}[[()]]]",
          "[{[{({}]{}}([{[{{{}}([]",
          "{<[[]]>}<{[{[{[]{()[[[]",
          "[<(<(<(<{}))><([]([]()",
          "<{([([[(<>()){}]>(<<{{",
          "<{([{{}}[<[[[<>{}]]]>[]]",
]

def read_input(filename)
  fh = open(filename)
  data = fh.readlines()
  fh.close()
  return data
end

def calculate_line(line)
  score = { ")" => 3, "]" => 57, "}" => 1197, ">" => 25137 }
  open = ["(", "[", "{", "<"]
  close = [")", "]", "}", ">"]
  expecting = [] # Closing chars we're expecting to see
  line.each_char { |character|
    if open.include? character
      i = open.index(character)
      closer = close[i]
      expecting.unshift(closer)
    elsif close.include? character
      if character == expecting[0]
        # valid closer, remove from our stack
        expecting.shift()
      else
        # invalid closing character, this line is corrupted.
        return [score[character], 0]
      end
    end
  }
  # If we reached here, the line is incomplete, not corrupted
  expecting_score = { ")" => 1, "]" => 2, "}" => 3, ">" => 4 }
  expecting_sum = 0
  for c in expecting
    expecting_sum *= 5
    expecting_sum += expecting_score[c]
  end
  return [0, expecting_sum]
end

def solve_problem(input)
  corrupted = 0
  incomplete = []
  for line in input
    p1, p2 = calculate_line(line)
    corrupted += p1
    if p2 > 0
      incomplete.push(p2)
    end
  end
  incomplete = incomplete.sort()
  middle = incomplete[incomplete.length() / 2]
  puts "Corrupted: %d, middle: %d" %[corrupted, middle]
end

solve_problem(sample)
solve_problem(read_input("input.txt"))
