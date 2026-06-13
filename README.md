# Dirt



A custom statically-typed programming language with interpreter, written in C#.

I started this Project because I really wanted to have a better understanding of programming languages — a learning project for lexers, parsers, interpreters, and type systems.

It's called Dirt because it is pretty useless afterall.



## What it does



Dirt is a small programming language with its own lexer, parser, and (planned) interpreter and type checker. It features:



- `name: type` syntax, types: `int`, `dec`, `string`, `bool`

- `when/otherwise`, `repeat`, `match` — orthogonal, combinable control flow constructs

- `stop` = break + early return

- `out()` for output, `and/or/not` as keywords

- Implicit return, no semicolons



\*\*Example:\*\*



```dirt

x: int = 10



when (x > 5) {

&#x20;   out("big")

} otherwise {

&#x20;   out("small")

}

```



## Status



Lexer — complete, tested

Parser — variables, expressions, `when/otherwise`, `repeat`, `match` done; functions next

Interpreter \& TypeChecker — planned

Other extensions — planned for when the initial tasks are funcioning



## Running



cd Dirt.CLI

dotnet run



## Requirements



- .NET SDK (8.0 or newer)



## Sidenotes



\- Architecture: `Dirt.Lexer` → `Dirt.Parser` → `Dirt.Interpreter` → `Dirt.TypeChecker` → `Dirt.CLI`

\- Planned extensions: implicit return for `when`/`match`/`repeat` as expressions, optional type inference, result-style error handling, destructuring in pattern matching

