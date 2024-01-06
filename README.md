# What is Kalan My Money?

According to AULEX dictionary, Kalan means guardian or protector in Maya language,
and it's an appropriate name for a personal finance manager system (maybe not for commercial uses hehe), so that is Kalan My Money. 

Front End Application: https://github.com/xChiro/KalanMyWeb/tree/main

# Arquitecture

Kalan My Money follows Clean Architecture principles; you can find more about that on 
the next page: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

As you can see in the following image, clean architecture has 4 layers (not necessary).

![CleanArchitecture](https://user-images.githubusercontent.com/3914477/177056418-fa3c875d-87ff-4ce9-994a-dfe40ce946c4.jpg)

This system follows those principles, but we are free to add any layers that the systems needs, but we need to keep good 
practices in the two inner layers, entities and use case we called this Domain; the domain needs to follow the SOLID and 
Clean Architecture principles strictly. 

# TDD and Unit Testing

Also, we are following the three TDD rules:

- You are not allowed to write any production code unless it is to make a failing unit test pass.
- You are not allowed to write any more of a unit test than is sufficient to fail; and compilation failures are failures.
- You are not allowed to write any more production code than is sufficient to pass the one failing unit test.

but we want to keep and order in or unit test suit so, for that we following the Arrange Act and Assert patterns in our TDD, 
yeah, I know, maybe it's strange, but it works fine if we are disciplined. 

So remember Red, Gren and Blue!.

# Use Cases and Desinging

In Documents folder, you can find our uses cases and some diagrams; this is very important, we use TDD as a tool to confirm our design 
and protect our code from bugs, it is normal when you are working with TDD following design, 
ending with a completely different one, and for that, we are using TDD in this project!, to confirm our design. 


# How to contribute? 

im working on it!

# Licence

https://github.com/xChiro/KalanMyMoney/blob/main/LICENSE.md
