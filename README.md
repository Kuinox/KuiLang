Determine identifier location at compile time.
Need to create static instance.


# KuiLang

Declarative, Object Oriented, General Purpose Programming Language.

<details>
<summary>
    Why another language ? <sub>[Expand]</sub>
</summary>
    <p><ul>
Right now, most popular general purpose languages are procedural.  

They force the developer to choose the data structures implementations (DoublyLinkedList vs ArrayList).  
They also force you to choose the implementation of all the common operations you do.
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
- The numbers are in a contiguous space of memory.  
- You must loop, in order on the numbers.  
- You must loop sequentially on the numbers.  

Will you run the same function on 40 thousands, millions, billions items ?  
In this example, we only compute a sum, but now, replace it with any business app. "This important logic was designed to run once and now is called thousands of times in a loop but we don't have the time to optimise it" is a too common scenario.  

What I want, is that the logic and implementation to be decoupled.  

And something we know well does that: SQL Databases.  
In SQL DBs, you write your schema structure, queries, and the DB engine implement it.  
You painlessly write highparalised code, doing async IO, that can run and adapt without any work, from your tiny laptop to your production clusters of machines with hundreds of cores available.  

Sadly, SQL has a lot of issues, [but a lot are due to the language itself](https://www.scattered-thoughts.net/writing/against-sql), not declarative programming.  

Finally, software thats require high performance begin to adopt more and more a database-like architecture.  

Games Engine adopt the ECS patterns: https://en.wikipedia.org/wiki/Entity_component_system  
Compilers start to be query based: https://rustc-dev-guide.rust-lang.org/query.html  
</ul></p>
</details>
