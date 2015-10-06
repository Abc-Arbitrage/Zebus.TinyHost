## About Zebus.TinyHost
**Zebus.TinyHost** is a lightweight host used to run [Zebus](https://github.com/Abc-Arbitrage/Zebus) enabled services.

### Introduction
As a peer to peer service bus, the main goal of Zebus is to allow applications to communicate with each other.
Any type of application can use Zebus, it can be a console application, a service, a WPF or Winform application. 

Using **Zebus.TinyHost** you can quickly get started and create a small console application ready to send and receive messages on the bus. 
Then you can create another one, which will communicate with it. If your applications are so cool that you want to let them run all day long, then you just have to launch them as windows services. 
Without realizing it, you just started your own [microservice architecture](http://martinfowler.com/articles/microservices.html)!

### Features
- You can launch Zebus.TinyHost as a console application or as a service
- You can define host initializers to run code before and after start/stop
- Reads Zebus settings from App.config 

### Copyright

Copyright © 2015 Abc Arbitrage Asset Management

### License

Zebus.TinyHost is licensed under MIT, refer to [LICENSE.md](https://github.com/Abc-Arbitrage/Zebus.TinyHost/blob/master/LICENSE.md) for more information. 
