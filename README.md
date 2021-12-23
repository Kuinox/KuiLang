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
