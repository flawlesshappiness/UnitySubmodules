using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomCharExtensions
{
    public static bool IsConsonant(this char c)
    {
        string consonants = "bcdfghjklmnpqrstvwxz";
        foreach (var cons in consonants)
            if (cons == c)
                return true;
        return false;
    }

    public static bool IsVowel(this char c)
    {
        string vowels = "aeiouy";
        foreach (var vowel in vowels)
            if (vowel == c)
                return true;
        return false;
    }
}
