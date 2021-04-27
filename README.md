# Workshop: Building a scalable backend with Akka&#46;NET
This repository will be the guideline throughout the workshop. You can find all relevant resources for the workshop here:
* The **source** for this workshop: [/src](./src/)
* The **slides** for this workshop: [/slides](./slides/)

## Content
_If you’re developing something that has the potential to explode in usage, you’re going to need to be able to scale the resources of your backend when your business takes off. It doesn’t matter if it’s an IoT device, a smartphone app, an Alexa skill or a video game. And if Amdahl’s law teaches us anything, it is that to do that successfully in a stateful system, you are going to need high degrees of parallelization._

Akka&#46;NET, and its implementation of the actor model, promise you just that: highly parallelizable code that scales beautifully as machines are added to the cluster. But there are more advantages than just scaling. Actor models can make your backend very robust, because of their self-healing nature. The code is also easy to reason about, and easy to test.

But the patterns for designing such a system are different from what you might be used to writing object-oriented code. And there are a few pitfalls that you might want to avoid altogether.

During this workshop, you will build your own backend for dealing with the meter readings coming from an electricity meter. Everything from ingesting large quantities of data, over shaping and cleaning your data, storing it, and generating alerts under certain conditions. You will not need any knowledge about Akka&#46;NET to attend this workshop. All concepts are handled during the course.

All the concepts you will encounter will be just as valuable in building other types of backend systems as well.

## Covered Topics

During this workshop, you will learn about:

* The Akka&#46;NET actor model
* Using Akka.Remoting to send messages to your central ActorSystem
* Designing an actor hierarchy without bottlenecks
* Data normalization techniques and what they can do for your codebase
* Using Akka.Persistence to recover state
* Designing for recovery:
  * Using supervision
  * Restarting the actor system
* Common Actor design patterns
* An introduction to Akka.Cluster

## Target audience

To attend this workshop, you should be a developer with:

* Working knowledge of the C# programming language
* An understanding of basic threading concepts
* An understanding of LINQ
* A basic knowledge of how to use a SQL Server database from code

You will spend a lot of time coding during this workshop. I will present concepts for the whole class, which you will then apply in code on your own. This workshop will be very hands-on. If you prefer watching slide decks for most of the day, this workshop might not be for you.

## Laptop setup

You will need: 
* Your .NET IDE of choice (Visual Studio or Rider)
* .NET 5 SDK
* A working SQL server instance (LocalDB will do just fine)
* A working internet connection

## Doubting if this workshop is for you?

Feel free to get in touch through [Twitter](https://twitter.com/hannes_lowette) or [LinkedIn](https://linkedin.com/in/hanneslowette).
