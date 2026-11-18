// Lab 10
// Student name: Tingwei Zhu
// Student number: 13860812


using System;
using System.Collections.Generic;

Console.WriteLine("CPEN223 Lab 10");

//Testing: Write some test cases to test well all methods you are to implement
//         This is to demonstrates what test cases you have considered
//TODO
void Test(
    string testName,
    IReadOnlyList<Sample> samples,
    string sensor,
    int startInclusive,
    int endExclusive,
    params Sample[] expected)
{
    var original = new List<Sample>(samples);
    var actual = WindowSelector.SelectWindow(samples, sensor, startInclusive, endExclusive);

    bool passed = actual is List<Sample> &&
                  !ReferenceEquals(actual, samples) &&
                  actual.Count == expected.Length &&
                  samples.Count == original.Count;

    for (int i = 0; i < actual.Count && i < expected.Length; i++)
    {
        if (actual[i] != expected[i])
            passed = false;
    }

    for (int i = 0; i < samples.Count && i < original.Count; i++)
    {
        if (samples[i] != original[i])
            passed = false;
    }

    Console.WriteLine($"{testName}: {(passed ? "PASS" : "FAIL")}");
}

Test("Input order and exclusive end",
    new List<Sample>
    {
        new Sample("A", 4, 8),
        new Sample("B", 2, 4),
        new Sample("A", 0, 6),
        new Sample("A", 5, 9)
    },
    "A", 0, 5,
    new Sample("A", 4, 8),
    new Sample("A", 0, 6));

Test("Duplicate timestamps and inclusive start",
    new List<Sample>
    {
        new Sample("A", 2, 1),
        new Sample("A", 2, 3)
    },
    "A", 2, 3,
    new Sample("A", 2, 1),
    new Sample("A", 2, 3));

Test("Empty input",
    new List<Sample>(), "A", 2, 3);

Test("Case-sensitive sensor matching",
    new List<Sample>
    {
        new Sample("a", 2, 1),
        new Sample("A", 3, 2)
    },
    "A", 2, 5,
    new Sample("A", 3, 2));

Test("No matching sensor",
    new List<Sample>
    {
        new Sample("B", 3, 9)
    },
    "A", 2, 5);

Test("Empty time window",
    new List<Sample>
    {
        new Sample("A", 2, 4)
    },
    "A", 2, 2);
//end Testing code

//Do not change the program skeleton: keep the record structs, the class name,
//method names, parameters, and return types exactly as given.
//You may add private helper methods.

public readonly record struct Sample(
    string Sensor,
    int Minute,
    double Value);

public static class WindowSelector
{
    /// <summary>
    /// Selects records for one sensor in a half-open time window.
    /// </summary>
    /// <param name="samples">
    /// Non-null. Each Sample.Sensor is non-null and each Value is
    /// finite. Minute is measured in integer minutes since the
    /// beginning of the fictional session.
    /// </param>
    /// <param name="sensor">Non-null, nonempty sensor name.</param>
    /// <param name="startInclusive">Inclusive start (minutes).</param>
    /// <param name="endExclusive">
    /// Exclusive end (minutes); at least startInclusive.
    /// </param>
    /// <returns>
    /// A new list of Sample records with matching Sensor (ordinal,
    /// case-sensitive equality) and Minute in [startInclusive,
    /// endExclusive). Preserve input order and duplicate times.
    /// Return an empty list when none match.
    /// </returns>
    /// <remarks>
    /// Requires: all input conditions above must hold.
    /// Effects: neither the input nor its elements are modified.
    /// No outcome is specified for invalid calls.
    /// </remarks>
    public static IReadOnlyList<Sample> SelectWindow(
        IReadOnlyList<Sample> samples,
        string sensor,
        int startInclusive,
        int endExclusive)
    {
        var selected = new List<Sample>();

        foreach (var sample in samples)
        {
            if (string.Equals(sample.Sensor, sensor, StringComparison.Ordinal) &&
                sample.Minute >= startInclusive &&
                sample.Minute < endExclusive)
            {
                selected.Add(sample);
            }
        }

        return selected;
    }
}

