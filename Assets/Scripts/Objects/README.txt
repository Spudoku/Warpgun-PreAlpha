The Objects folder encapsulates all behaviors related to all GameObjects, especially Actors.
In this build, Actors are defined as any GameObject that is dynamic in some way. This includes (but is not limited to):
1.  Objects that move
2.  Objects that can take damage/die/be destroyed
3.  Objects with AI
4.  Objects that recieve player input

This module is intended to be self-contained, such that any Actor Object should only require a single component in order 
to function as expected.