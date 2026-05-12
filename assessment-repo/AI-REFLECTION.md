# AI Reflection

## What did AI handle well?
AI accelerated the implementation of Unit Tests, Rate Limiting, Health Checks, and Validators by helping with test generation, creating validators using FluentValidation, and reducing boilerplate. Without it, research and implementation would have taken significantly longer.
<!-- Where did AI accelerate you? What would have taken
significantly longer without it? -->
## Where did you override AI?
AI suggested using Domain-Driven Design (DDD) with external mapping libraries like AutoMapper or Mapster for object mapping, but I chose to use a simpler Layered Architecture (Presentation, Application, Infrastructure) instead. I also opted for manual mapping because it is easier to debug, more readable, and keeps the behavior explicit rather than hidden.
<!-- Any suggestions you rejected or substantially rewrote.
Why were they wrong? -->
## What did you write without AI?
I designed and implemented the core architecture and conventions without AI, including creating the mappers, defining normalized entities, separating queries and commands, designing the database schema, defining DTOs and contracts, creating custom exception classes, and implementing the exception-handling middleware.
<!-- Which parts did you intentionally write yourself, and why? -->
