# Kui-Lang #TODO: find a better name

Declarative, Object Oriented, General Purpose Programming Language.

<details>
<summary>
    Why another language ? <sub>[Expand]</sub>
</summary>
Right now, most popular general purpose languages are procedural.  

They force the developer to choose the data structures implementations (DoublyLinkedList vs ArrayList).  
They also for you to choose the implementation of all the common operations you do.
For example, if you want to calculate a sum: 
```js
//pseudocode
number sum(numbers: number[]){
    var sum = 0;
    foreach(var number in numbers) {
        sum += number
    }
}

```

There, by accident, you specified that:  
- The numbers are in a contiguous espace of memory.  
- You must loop, in order on the numbers.  
- You must loop sequentially on the numbers.  

Will you run the same function on 40 thousands, millions, billions items ?  
This example is for the sum example, but now, replace it with any business app. "This important logic was designed to run once and now is called thousands of times in a loop but we don't have the time to optimise it" is a too common scenario.  

What I want, is that the logic and the implementation is decoupled.  

And something we know well does that: SQL Databases.  
In SQL DBs, you write your schema structure, queries, and the DB engine implement it.  
You painlessly write highparalised code, doing async IO, that's can run and adapt without any work to your tiny laptop or on your production clusters of machines with hundreds of cores available.

Sadly, SQL has a lot of issues, [but a lot are due to the language itself.](https://www.scattered-thoughts.net/writing/against-sql).  

Finally, software thats require high performance begin to adopt more and more a database-like architecture.

Games Engine adopt the ECS patterns: https://en.wikipedia.org/wiki/Entity_component_system 
Compilers start to be query based: https://rustc-dev-guide.rust-lang.org/query.html 
</details>

## Ideal high level architecture

The code is compiled to an intermediate language (to avoid parsing text at runtime).
The optimizer run a pass on the intermediate language to run some optimisations.
The runtime is like a JIT, Profile Guided Optimisation can help a lot (like statistics in DBs) to help choose the correct data structures, and to choose the best implementations to use.

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
