# [Collision Callback](./Assets/Scripts/CollisionCallback.cs) 

## Introduction

In Unity, only objects with a collider component can use `OnTriggerEnter()` and `OnCollisionEnter()` to detect collisions. However, in many scenarios, developers may need to implement program logic for collisions in one game object (Let say "A"), while another game object (Let say "B") is used for collision detection.

One common solution for this situation is to attach a script to object "B" and implement the collision logic within that script. Then, object "A" can establish a reference to object "B". However, this solution has two drawbacks:
+ If there are many objects like "B" in the project, developers would need to create scripts for each of these objects. As the project grows in size, the complex dependencies and massive logic flow can make maintenance difficult.
+ Some variables and data needed by the collision logic may be stored in object "A", and object "B" does not have direct access to them.

`CollisionCallback` is a solution to address problems. It allows users to implement the collision logic in object A and register the logic as callback actions in object B, where the *CollisionCallback* script should be attached. The registered callbacks are stored in a dictionary within *CollisionCallback*, allowing for easy management.

When registering a callback with *CollisionCallback*, users need to specify the type or tag of the callback invoker. This ensures that when *CollisionCallback* is hit by objects with the specified types or tags, only the corresponding callbacks will be invoked. This provides a flexible and scalable approach to handling collisions, allowing for decoupled logic and efficient communication between objects involved in collisions.


## Instructions

To use `CollisionCallback`, simply attach this component to the object that will be involved in the collision, whether it is the collision initiator or receiver. Additionally, a collider component must be attached to both the collision initiator and receiver objects to detect the collision. It is important to note that the current implementation of *CollisionCallback* only supports `Box Collider`, `Sphere Collider`, and `Capsule Collider` for collision detection. 
Once *CollisionCallback* is attached, any objects that wish to register callbacks should establish an explicit reference to the object with *CollisionCallback* attached. *CollisionCallback* provides an API for users to register their desired callbacks.


## API

```csharp

/// <summary>
/// Register a callback logic that will be invoked when then objects with specified type or tag collide with this instance. 
/// </summary>
/// <param name="callback"> The callback that need to be invoked. </param>
/// <param name="isCallbackToSelf"> If true, the callback will be applied to this instance. Otherwise, apply to the incoming object. </param>
/// <param name="targetType"> Objects with this type will invoke the callback when collison occurs. </param>
/// <param name="targetTag"> Objects with this tag will invoke the callback when collision occurs. </param>
void AddCallback(Action<GameObject> callback, bool isCallbackToSelf, Type targetType = null, string targetTag = null);

```


