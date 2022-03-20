using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandomizer<T>
{
    private List<Element<T>> elements = new List<Element<T>>();
    private float max_weight = 0f;

    public class Element<T>
    {
        public T value;
        public float weight;

        public Element(T value, float weight)
        {
            this.value = value;
            this.weight = weight;
        }
    }

    public WeightedRandomizer(List<Element<T>> elements = null)
    {
        elements = new List<Element<T>>();

        foreach(var element in elements)
        {
            AddElement(element);
        }
    }

    public void AddElement(Element<T> element)
    {
        elements.Add(element);
        max_weight += element.weight;
    }

    public void AddElement(T value, float weight)
    {
        AddElement(new Element<T>(value, weight));
    }

    public T Random()
    {
        float r_weight = UnityEngine.Random.Range(0f, max_weight);
        float temp_weight = 0f;
        foreach(var element in elements)
        {
            temp_weight += element.weight;
            if (r_weight < temp_weight) return element.value;
        }

        return elements[elements.Count - 1].value;
    }
}
