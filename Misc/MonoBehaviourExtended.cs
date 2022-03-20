using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourExtended : MonoBehaviour
{
    public enum ComponentSearchType { THIS, CHILDREN, PARENT }

    private Dictionary<System.Type, Component> _component_dictionary = new Dictionary<System.Type, Component>();
    private Dictionary<System.Type, Component[]> _components_dictionary = new Dictionary<System.Type, Component[]>();

    #region GET_COMPONENT_ONCE
    /// <summary>
    /// Gets a component, and caches the result
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <param name="type_search">Where to search for the component</param>
    /// <param name="include_inactive">True if inactive gameObjects are included in the search</param>
    /// <returns>The component if found, else null</returns>
    public T GetComponentOnce<T>(ComponentSearchType type_search = ComponentSearchType.THIS, bool include_inactive = false) where T : Component
    {
        System.Type type = typeof(T);
        if (_component_dictionary.ContainsKey(type))
            return (T)_component_dictionary[type];

        T component = GetComponent<T>(type_search, include_inactive);
        _component_dictionary.Add(type, component);
        return component;
    }

    /// <summary>
    /// Gets an array of components, and caches the result
    /// </summary>
    /// <typeparam name="T">The component type</typeparam>
    /// <param name="type_search">Where to search for the component</param>
    /// <param name="include_inactive">True if inactive gameObjects are included in the search</param>
    /// <returns>The component array if found, else null</returns>
    public T[] GetComponentsOnce<T>(ComponentSearchType type_search = ComponentSearchType.THIS, bool include_inactive = false) where T : Component
    {
        System.Type type = typeof(T);
        if (_components_dictionary.ContainsKey(type))
            return (T[])_components_dictionary[type];

        T[] components = GetComponents<T>(type_search, include_inactive);
        _components_dictionary.Add(type, components);
        return components;
    }

    T GetComponent<T>(ComponentSearchType type_search, bool include_inactive = false) where T : Component
    {
        switch (type_search)
        {
            case ComponentSearchType.THIS:
                return GetComponent<T>();
            case ComponentSearchType.CHILDREN:
                return GetComponentInChildren<T>(include_inactive);
            case ComponentSearchType.PARENT:
                return GetComponentInParent<T>();
            default: return default(T);
        }
    }

    T[] GetComponents<T>(ComponentSearchType type_search, bool include_inactive = false) where T : Component
    {
        switch (type_search)
        {
            case ComponentSearchType.THIS:
                var component = GetComponent<T>();
                if (component != null)
                    return new T[] { component };
                return null;
            case ComponentSearchType.CHILDREN:
                return GetComponentsInChildren<T>(include_inactive);
            case ComponentSearchType.PARENT:
                return GetComponentsInParent<T>(include_inactive);
            default: return null;
        }
    }
    #endregion
}
