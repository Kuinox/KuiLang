# Kui-Lang

Relational, Object Oriented, General Purpose Programming Language.

## Why current languages are not enough:

Current procedural languages make you:
 - Choose the data structure you have to use.
 - Implement how the calculation is done.

On the other hand, in procedural OOP languages, you don't really care about the data structure, or the way the calculation is made.
You care about 2 things: 
1. Neatly representating your data & encapsulate it to reduce complexity: You don't care that if it's a LinkedList or an ArrayList, what you care about is thats it's a collection you can add item to, and expose a readonly view of it. 
2. Representing the logic in comprehensible format.
3. Getting your feature done in a reasonable time

Which conflict with:
- Having the right data structure for the job: Getting the perfect data structure for each workload is time consuming, a plain array is often faster than an HashSet.
- Expressing your logic so it can be parallelized or simply processed faster, makes the code less readable, and takes more time to implement.

It means the logic and the implementation must be decoupled.
One way so, is that the programmer write the logic, and a magic compiler figure out how to implement it.

And we already have multiple languages that does this:  
SQL: you write your tables structures, relations, queries, and the DB engine implement it.
Prolog: you write facts and rules, query, and prolog implement it.

But these languages have weakness:

No sane person write an entier app in SQL. SQL is made to store or process data, it's not "general purpose".
SQL has several problems: https://www.scattered-thoughts.net/writing/against-sql

Q: C# has linq, why you don't simply use it ?  
First: you are still relying on a certain implementation of a data structure.
Second: I believe thats the more your program is written in this unrelational way, the more the optimiser will have room to improve your program.

## The language itself  

Model definition:
Fully typed. Heavy inspirations from typescripts.
Number: Little experience I want to do:
What if everything is a number ?
A boolean is 0 or 1. Etc...
All terminal data type is simply a number with a constraint.
Collections & constraint

Executions: 
Concurrency with a keyword like go.
Any line can "await"

Iterators: OnAll, Iterate, Lag, etc...

//TODO: field update lead to events ?

## Various thougts and rational:

### What is the difference between a object oriented database and a so called "relational database".
In a relational database, (AKA table oriented), a foreign key is simply a "reference" to another table entry.  
A primary key is simply the object reference, `this`.  
This is a raw reference which is not typed to what it represent, and to me, this is the difference: the OO database is simply fully typed, while the table oriented database represent everything within tables, being more "weakly typed" (it still have types, but you can't extends it to make your system more fool-proof). 

### The most efficient piece of code in the everyday of a webdev life is a DB query written in 10 minutes.  
Take any serious DB, write a non trivial query, and it will get magically parallelised over all your CPU cores, with fully asynchronous IO.

### Future is highly parallelised, and asynchronous







## Main principle, philosophy

- Contributing to the langage, and core library should be simple. It mean that, we should try to avoid using other programming language, like C, as much as possible in the core library.
- While writing code, users should not think about "does this need a Stack or a Queue, or a List, or a HashSet". This is an implementation detail, which the compiler/query engine should choose. What the user should thing is "this has only unique entries, of the type XYZ".
- The language should not get in the way of the advanced users. If an user have nicely represented it's data, but the compiler implementation is poor, we should give a way for the advanced user to write it's own implementation.


### Some ideas

point of entry is the mains, any external events + self emitted (shouldn't be abused)

### Current decisions:

No indirect side effect.
```js
foo(A a, B b)
{
  Foo foo = new(a, b); // Indirect side effect: allocation.
  foo.MutateAAndB(); // Direct side effect, A and B were given as argument to Foo constructor.
  console.log("Mutated a&b"); // Indirect side effect: writing to console.
}

correctFoo(A a, B b, Allocator new, Console console)
{
  Foo foo = new(a, b);
  foo.MutateAAndB();
  console.log("Mutated a&b");
}
```

Scope are opened and closed by `{}`.

Typescript like typesystem: 
Anonymous interfaces.
Union types & interface grouping.

Fields backed by a relation (Auto Properties)

I want it to be self hosted ASAP.  
It mean, when the language will be specified, I will rush an interpreter, to write it's internal composant with it.
The compiler: 
Transform the source to an intermediary representation.

The JIT/Query Engine.
Do some optimisations at compile time, and at runtime. Write statistics in a file to reuse previous run data in next runs. 
