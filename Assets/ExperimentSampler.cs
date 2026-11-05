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
        for (int i = 0; i < entityCounts.Length; i++) {
            stringValues[i] = entityCounts[i].ToString();
        }

        sampleColumns.Add(new sampleColumn("Entities", stringValues));

        samples = new double[expectedSamplesPerAverage];
    }

    public void StartColumn(string columnName) {
        sampleColumns.Add(new sampleColumn(columnName, sampleColumns[0].ValueCount));
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
        Debug.Log($".csv file located at:{Application.persistentDataPath}");
        using(StreamWriter streamWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, filename))) {
            for (int i = 0; i < sampleColumns.Count - 1; i++) {
                streamWriter.Write($"{sampleColumns[i].ColumnName}, ");
            }
            streamWriter.Write($"{sampleColumns[sampleColumns.Count - 1].ColumnName}{streamWriter.NewLine}");
            for (int i = 0; i < sampleColumns[0].ValueCount; i++) {
                for (int j = 0; j < sampleColumns.Count - 1; j++) {
                    streamWriter.Write($"{sampleColumns[j].GetValue(i)}, ");
                }
                streamWriter.Write($"{sampleColumns[sampleColumns.Count - 1].GetValue(i)}{streamWriter.NewLine}");
            }
        }
    }
}

public class sampleColumn {
    private string columnName;
    private string[] values;

    private int index;

    public string ColumnName => columnName;
    public int ValueCount => values.Length;

    public sampleColumn(string name, string[] values) {
        columnName = name;
        this.values = values;
        index = values.Length;
    }

    public sampleColumn(string name, int valueCount) {
        columnName = name;
        values = new string[valueCount];
    }

    public void AddToValues(string value) {
        if(index > values.Length) {
            return;
        }

        values[index] = value;
        index++;
    }

    public string GetValue(int index) {
        if (values[index] == null) {
            return "NA";
        }
        return values[index];
    }
}