# Kui-Lang #TODO: find a better name

Declarative, Object Oriented, General Purpose Programming Language.

<details>
<summary>
    Why another language ? <sub>[Expand]</sub>
</summary>
    <p><ul>
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
</ul></p>
</details>

## Ideal, High Level architecture

The source code is compiled to an intermediate language, to avoid the costly text parsing at runtime.  
Ideally, the optimiser can run a pass on the outputed binary.  
The runtime is like a JIT, except because it's up to the optimiser to choose how to implement the program, Profile Guided Optimisation will be very important (like statistics in SQL DBs), to choose the right data structures.   

Because the optimiser does the job of implementing the program, the problems of async IO, and maybe, most of the concurrency issues should be gone.  
This also make it possible to offload work to something else than the OS CPU, or another machine, even if the code was not designed for it.
