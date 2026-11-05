using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExperimentSampler {

    private List<sampleColumn> sampleColumns;

    private double sampleStartTimeStamp;

    private int CurrentColumnIndex => sampleColumns.Count - 1;

    private double[] samples;
    private int currentSample;

    public bool FinishedSamples => currentSample >= samples.Length - 1;

    public ExperimentSampler(int[] entityCounts, int expectedSamplesPerAverage) {
        sampleColumns = new List<sampleColumn>();

        string[] stringValues = new string[entityCounts.Length];
        for (int i = 1; i < entityCounts.Length; i++) {
            stringValues[i] = entityCounts[i].ToString();
        }

        sampleColumns.Add(new sampleColumn("Entities", stringValues));

        samples = new double[expectedSamplesPerAverage];
    }

    public void StartColumn(string columnName) {
        sampleColumns.Add(new sampleColumn(columnName, sampleColumns[0].Values.Length));
    }

    public void StartAverage() {
        currentSample = 0;
    }

    public void StartSample() {
        sampleStartTimeStamp = Time.realtimeSinceStartupAsDouble;
    }

    public void EndSample() {
        double end = Time.realtimeSinceStartupAsDouble;
        double durationMilliseconds = (end - sampleStartTimeStamp) * 1000.0;

        if(currentSample > samples.Length - 1) {
            return;
        }

        samples[currentSample] = durationMilliseconds;
        currentSample++;
    }

    public double FinishSampleAverage() {
        double average = 0;
        for (int i = 0; i < samples.Length; i++) {
            average += samples[i];
        }
        average /= samples.Length;

        sampleColumns[CurrentColumnIndex].AddToValues(average.ToString());
        return average;
    }

    public void ExportSampleCSV(string filename) {
        using(StreamWriter streamWriter = new StreamWriter(filename)) {
            for (int i = 0; i < sampleColumns.Count - 1; i++) {
                streamWriter.Write($"{sampleColumns[i].ColumnName}, ");
            }
            streamWriter.Write($"{sampleColumns[sampleColumns.Count - 1].ColumnName}");
        }
    }
}

public struct sampleColumn {
    private string columnName;
    private string[] values;

    private int valueCounter;

    public string ColumnName => columnName;
    public string[] Values => values;

    public sampleColumn(string name, string[] values) {
        columnName = name;
        this.values = values;
        valueCounter = values.Length;
    }

    public sampleColumn(string name, int valueCount) {
        columnName = name;
        values = new string[valueCount];
        valueCounter = 0;
    }

    public void AddToValues(string value) {
        if(valueCounter > values.Length) {
            return;
        }

        values[valueCounter] = value;
        valueCounter++;
    }
}