# Kui-Lang

Relational, Object Oriented, General Purpose Programming Language.

## Various thougts and rational:

### What is the difference between a object oriented database and a so called "relational database".
In a relational database, (AKA table oriented), a foreign key is simply a reference to another table entry.  
A primary key is simply the "object reference, 'this'".  
The reference is typed, and to me, this is the difference: the OO database is simply typed, while the table oriented database represent everything withing tables
The 'types' come from the constraint on the keys.  

### The most well 'optimised' piece of code the average webdev will write is a DB query written in 10 minutes.  
Take any major DB, write a non trivial query, and it will get magically parallelised over all your CPU cores.

### A lot of hardware innovation does not happen because of software
- We still rely a lot on a lot of software that compile to a static binary, so it cannot adapt to the machine.
- Most JIT or interpreters compile a procedural language that does not provide enough flexibility.
Note: Mosts JITs can still profit a bit from small to medium changes in hardware.

## Main principle, philosophy

1. Contributing to the langage, and core library should be simple. It mean that, we should try to avoid using other programming language, like C, as much as possible in the core library.
2. While writing code, users should not think about "does this need a Stack or a Queue, or a List, or a HashSet". This is an implementation detail, which the compilator should choose.
3. The language should not get in the way of the advanced users. If an user have nicely represented it's data, but the compiler implementation is poor, we should give a way for the advanced user to write it's own implementation.


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
