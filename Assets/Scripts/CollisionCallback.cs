using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component provides an indirect control methods for the objects that are involved in the collison, both collision invokers and collision receivers.
/// </summary>
public class CollisionCallback : MonoBehaviour
{
    private class CallbackConfig
    {
        /* Will pass the gameobject of the invader back to the callback function */
        public List<Action<GameObject>> callbackToOther = new List<Action<GameObject>>();
        /* Will pass the gameobject of this script back to the callback function */
        public List<Action<GameObject>> callbackToSelf = new List<Action<GameObject>>();

        public void AddCallback(Action<GameObject> callback, bool isCallbackToSelf)
        {
            if (isCallbackToSelf)
                callbackToSelf.Add(callback);
            else
                callbackToOther.Add(callback);
        }
    }

    private Dictionary<Type, CallbackConfig> typeCallbackDict = new Dictionary<Type, CallbackConfig>();
    private Dictionary<string, CallbackConfig> tagCallbackDict = new Dictionary<string, CallbackConfig>();


    private void Awake()
    {
        CheckValidation();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning("OnTriggerEnter!!");
        InvokeCallback(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.LogWarning("OnCollisionEnter");
        InvokeCallback(collision.gameObject);
    }

    /// <summary>
    /// To utilize this component, the self instance must possess either a 'BoxCollider', a 
    /// 'CapsuleCollider', or a 'SphereCollider'.
    /// </summary>
    private void CheckValidation()
    {
        if (TryGetComponent<BoxCollider>(out _) == false &&
            TryGetComponent<CapsuleCollider>(out _) == false &&
            TryGetComponent<SphereCollider>(out _) == false)
        {
            Debug.LogError("CollisionCallback: collider component is required to use this script!");
        }
    }

    /// <summary>
    /// When a collision occurs, iterate through all types and tags to check if the incoming object 
    /// contains components or tags that are designated to execute callback logic
    /// </summary>
    private void InvokeCallback(GameObject other)
    {
        foreach (var pair in typeCallbackDict)
        {
            Type type = pair.Key;
            if (other.TryGetComponent(type, out _) == true)
            {
                foreach (Action<GameObject> callback in pair.Value.callbackToSelf)
                    callback.Invoke(gameObject);
                foreach (Action<GameObject> callback in pair.Value.callbackToOther)
                    callback.Invoke(other);
            }
        }

        foreach (var pair in tagCallbackDict)
        {
            string tag = pair.Key;
            if (other.CompareTag(tag) == true)
            {
                foreach (Action<GameObject> callback in pair.Value.callbackToSelf)
                    callback.Invoke(gameObject);
                foreach (Action<GameObject> callback in pair.Value.callbackToOther)
                    callback.Invoke(other);
            }
        }
    }

    /// <summary>
    /// Register a callback logic that will be invoked when then objects with specified type or tag 
    /// collide with this instance. 
    /// </summary>
    /// <param name="callback"> The callback that need to be invoked. </param>
    /// <param name="isCallbackToSelf"> If true, the callback will be applied to this instance. Otherwise, apply to the incoming object. </param>
    /// <param name="targetType"> Objects with this type will invoke the callback when collison occurs. </param>
    /// <param name="targetTag"> Objects with this tag will invoke the callback when collision occurs. </param>
    public void AddCallback(Action<GameObject> callback, bool isCallbackToSelf, Type targetType = null, string targetTag = null)
    {
        if (targetType != null)
        {
            if (typeCallbackDict.ContainsKey(targetType) == false)
                typeCallbackDict[targetType] = new CallbackConfig();
            typeCallbackDict[targetType].AddCallback(callback, isCallbackToSelf);
        }

        if (targetTag != null)
        {
            if (tagCallbackDict.ContainsKey(targetTag) == false)
                tagCallbackDict[targetTag] = new CallbackConfig();
            tagCallbackDict[targetTag].AddCallback(callback, isCallbackToSelf);
        }
    }

}