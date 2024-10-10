using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionClass
{
    public static Enum GetRandomEnumValue(this Type t)
    {
        return Enum.GetValues(t) // get values from Type provided
            .OfType<Enum>() // casts to Enum
            .OrderBy(e => Guid.NewGuid()) // mess with order of results
            .FirstOrDefault(); // take first item in result
    }

    public static WaitForFixedUpdate WaitForFixedUpdate =
        new WaitForFixedUpdate();

    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    static Dictionary<float, WaitForSeconds> _cachedWaits
        = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (!_cachedWaits.ContainsKey(seconds))
            _cachedWaits.Add(seconds, new WaitForSeconds(seconds));
        return _cachedWaits[seconds];
    }

    static Dictionary<float, WaitForSecondsRealtime> _cachedWaitsRealtime
        = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(
        float seconds)
    {
        if (!_cachedWaitsRealtime.ContainsKey(seconds))
            _cachedWaitsRealtime.Add(seconds, new WaitForSecondsRealtime(seconds));
        return _cachedWaitsRealtime[seconds];
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static string TrimLower(this string str)
    {
        return str.ToLower().Trim();
    }

    /// <summary> 
    /// Rotates the current transform toward the target transform just by Y axis 
    /// </summary>
    public static void LookAtByY(this Transform currentTransform, Transform targetTransform)
    {
        currentTransform.LookAt(new Vector3(targetTransform.position.x, currentTransform.position.y,
            targetTransform.position.z));
    }

    /// <summary>
    /// Reset a tranform to default values.
    /// </summary>
    public static void ResetTransform(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Set active an object.
    /// </summary>
    public static void SetActive(this Transform transform, bool enable)
        => transform.gameObject.SetActive(enable);

    /// <summary> 
    /// Destroy children of an object. 
    /// </summary>
    public static void DestroyAllChildren(this GameObject gameObject)
    {
        foreach (var child in gameObject.transform.Cast<Transform>()) UnityEngine.Object.Destroy(child.gameObject);
    }

    public static void DestroyAllChildren(this Transform gameObject)
    {
        foreach (var child in gameObject.transform.Cast<Transform>()) UnityEngine.Object.Destroy(child.gameObject);
    }

    /// <summary>
    /// Destroy child at index
    /// </summary>
    public static void DestroyChildAt(this Transform transform, int index)
    {
        UnityEngine.Object.Destroy(transform.Cast<Transform>().ToList()[index]);
    }

    /// <summary>
    /// Set active all children of an object.
    /// </summary>
    public static void SetActiveAllChildren(this GameObject gameObject, bool enable)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(enable);
        }
    }

    public static void SetActiveAllChildren(this Transform gameObject, bool enable)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(enable);
        }
    }

    public static void SetActiveGameObjects(this MonoBehaviour monoBehaviour, GameObject[] gameObjects,
        bool setActive)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(setActive);
        }
    }

    /// <summary>
    /// Change index of a child.
    /// </summary>
    public static void ChangeChildIndex(this Transform transform, int from, int to)
        => transform.GetChild(from).SetSiblingIndex(to);

    public static void MoveChildToFirst(this Transform transform, int index)
        => transform.GetChild(index).SetAsFirstSibling();

    public static void MoveChildToLast(this Transform transform, int index)
        => transform.GetChild(index).SetAsLastSibling();


    public static IEnumerator WaitAndAction(float duration, Action action)
    {
        yield return GetWaitForSeconds(duration);
        action?.Invoke();
    }
    
    public static bool IsDestroyed(this GameObject gameObject)
    {
        // UnityEngine overloads the == operator for the GameObject type
        // and returns null when the object has been destroyed.
        // This method checks both conditions to determine if the object is destroyed.
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }
}