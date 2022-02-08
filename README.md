# Kui-Lang

Relational, Object Oriented, General Purpose Programming Language.

## Various thougts and rational:

### What is the difference between a object oriented database and a so called "relational database".
In a relational database, (AKA table oriented), a foreign key is simply a reference to another table entry.  
A primary key is simply the object reference, `this`.  
This is a raw reference which is not typed to what it represent, and to me, this is the difference: the OO database is simply fully typed, while the table oriented database represent everything within tables, being more "weakly typed" (it still have types, but you can't extends it to make your system more fool-proof). 

### The most well 'optimised' piece of code in the everyday of a webdev life is a DB query written in 10 minutes.  
Take any serious DB, write a non trivial query, and it will get magically parallelised over all your CPU cores, with fully asynchronous IO.

### A lot of hardware innovation does not happen because of software
- We still rely a lot on a lot of software that compile to a single static binary, so it cannot adapt to the machine.
- Most JIT or interpreters compile a procedural language that does not provide enough flexibility.
Note: Mosts JITs can still profit a bit from small to medium changes in hardware.

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
