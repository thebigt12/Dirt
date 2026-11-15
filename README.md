# Dirt

A custom statically-typed programming language with its own interpreter,
written in C#.

I started this project because I wanted to really understand how programming
languages work on the inside, a learning project for lexers, parsers,
interpreters, and type systems.

You write code in a `.dirt` file and Dirt runs it. Dirt is its own language
with its own rules.

## Example

```dirt
x: int = 5
y: int = 3

add(a: int, b: int): int {
    stop a + b
}

out(add(x, y))      // prints 8
```

## What it does

- Variables with types: `int`, `dec`, `string`, `bool`
- Output with `out("hello")`
- Branching with `when (x > 5) { } otherwise { }`
- Loops with `when (x > 0) repeat { }` or `repeat 5 { }`
- Pattern matching with `match (x) { 1 -> { } _ -> { } }`
- Functions with `add(a: int): int { stop a }`

Two ideas make Dirt its own thing. First, `when`, `repeat`, and `match` are
building blocks you combine, instead of fixed constructs like `if`, `while`,
and `for` — `when (...) repeat` is a conditional loop without a separate
`while` keyword. Second, `stop` does two jobs: it breaks out of a loop and it
exits a function with a return value, one word instead of `break` and
`return`. There are no semicolons — a line ending ends a statement.

## How it works

When Dirt runs a file, the code passes through four stages, each making the
next one's job easier:

```
Your code  →  Lexer  →  Parser  →  TypeChecker  →  Interpreter  →  Output
```

**1. Lexer — splits the text.** It cuts the raw text into small pieces
("tokens"). It doesn't understand meaning yet, it just recognizes words and
symbols: `x: int = 10` becomes `[x] [:] [int] [=] [10]`.

**2. Parser — builds a tree.** It turns the flat list of tokens into a tree
that captures the structure. The tree makes the order unambiguous. For
`1 + 2 * 3`, the `*` ends up deeper in the tree than the `+`, and the rule
when running is that whatever sits deeper gets calculated first. So
"multiplication before addition" falls out of the tree's shape on its own —
nothing extra to program.

**3. TypeChecker — catches errors.** It walks the tree and checks that the
types fit together before anything runs. `x: int = "hello"` is reported as an
error before the program even starts. That's what makes Dirt statically
typed.

**4. Interpreter — runs it.** It walks the checked tree and does what it says:
calculates, stores variables, prints. For `out(1 + 2 * 3)` it works from the
bottom up — first `2 * 3 = 6`, then `1 + 6 = 7` — and prints `7`.

## Project structure

Each part only knows the part it gets its input from — a clean chain from
text to output.

```
Dirt/
├── Dirt.Lexer/        text        → tokens
├── Dirt.Parser/       tokens      → tree (AST)
├── Dirt.TypeChecker/  tree        → checked (or error)
├── Dirt.Interpreter/  tree        → execution
└── Dirt.CLI/          reads the .dirt file and starts everything
```

## Status

The core language works end to end. The Lexer, Parser, TypeChecker, and
tree-walking Interpreter are all complete — variables, expressions,
`when/otherwise`, `repeat`, `match`, and functions all run and get type-checked.

## Running

```
cd Dirt.CLI
dotnet run
```

Requires the .NET SDK (8.0 or newer).

## Planned extensions

- `and` / `or` / `not` as logical operators
- calculating with decimals in the interpreter
- implicit return — using the last line of a block instead of `stop`
- `when` / `match` / `repeat` as expressions that return a value
- optional type inference (`x = 5` instead of `x: int = 5`)
- result-style error handling, and destructuring in pattern matching
