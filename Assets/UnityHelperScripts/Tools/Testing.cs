using System;
using UnityEngine;

namespace SUHScripts.Testing
{
    public delegate bool TestValidator<T>(T result, T expected);
    public static class Testing
    {
        public static T Validate<T>(this T result, T expected, TestValidator<T> validation, string test)
        {
            var testResult = validation(result, expected);
            var resultString = testResult ? "SUCCESS" : "FAIL";
            var full = $"{test} {resultString}. Result: {result}, Expected: {expected}";

            if (testResult)
                Debug.Log(full);
            else
                Debug.LogError(full);

            return result;
        }

    }
}



