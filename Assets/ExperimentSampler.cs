using System.Collections.Generic;
using UnityEngine;

public class ExperimentSampler {

    private Dictionary<string, string> sampleColumns;

    private string currentColumn;

    private double sampleStartTimeStamp;

    public void ConfigureSampleColumns(int[] entityCounts) {
        sampleColumns = new Dictionary<string, string>();

        string value = entityCounts[0].ToString();
        for (int i = 1; i < entityCounts.Length; i++) {
            value += $",{entityCounts[i]}";
        }

        sampleColumns.Add("Entities", value);
    }

    public void StartColumn(string columnName) {
        currentColumn = columnName;
    }

    public void StartSample() {
        sampleStartTimeStamp = Time.realtimeSinceStartupAsDouble;
    }

    public void EndSample() {
        double end = Time.realtimeSinceStartupAsDouble;
        double durationMilliseconds = (end - sampleStartTimeStamp) * 1000.0;

        if (sampleColumns.ContainsKey(currentColumn)) {
            sampleColumns[currentColumn] = sampleColumns[currentColumn] + $",{durationMilliseconds}";
        } else {
            sampleColumns.Add(currentColumn, durationMilliseconds.ToString());
        }
    }

    public void ExportSampleCSV() {

    }
}