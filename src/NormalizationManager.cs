using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataMiningNormalization
{
    public static class NormalizationManager
    {
        private static int _lastColumn = 0;
        private static float _minVal = float.MaxValue;
        private static float _maxVal = float.MinValue;
        private static float _avgVal = 0;

        private static float _stdDev = 0;
        private static float MaxMinNormalization(this float x, float xMin, float xMax) => (x - xMin) / (xMax - xMin);
        private static float ZScore(this float x, float avg, float stdDeviation) => (x - avg) / stdDeviation;

        private static float MinValue(this List<List<float>> obj, int columnSelector)
        {
            if (_minVal != float.MaxValue && _lastColumn == columnSelector) return _minVal;

            float min = float.MaxValue;
            for (int i = 0; i < obj.Count; i++)
            {
                min = min > obj[i][columnSelector] ? obj[i][columnSelector] : min;
            }
            _minVal = min;
            return _minVal;
        }
        private static float MaxValue(this List<List<float>> obj, int columnSelector)
        {
            if (_maxVal != float.MinValue && _lastColumn == columnSelector) return _maxVal;

            float max = float.MinValue;
            for (int i = 0; i < obj.Count; i++)
            {
                max = max < obj[i][columnSelector] ? obj[i][columnSelector] : max;
            }
            _maxVal = max;
            return _maxVal;
        }
        private static float AvgValue(this List<List<float>> obj, int columnSelector)
        {
            if (_avgVal != 0 && _lastColumn == columnSelector) return _avgVal;

            float avg = 0;
            float sumVal = 0;
            obj.ForEach(cell => sumVal += cell[columnSelector]);
            avg = sumVal / obj.Count;
            _avgVal = avg;
            return _avgVal;
        }
        private static float StdDevValue(this List<List<float>> obj, int columnSelector)
        {
            if (_stdDev != 0 && _lastColumn == columnSelector) return _stdDev;

            float stdDev = 0;
            double sumVal = 0;
            float avgVal = obj.AvgValue(columnSelector);
            obj.ForEach(cell => sumVal += Math.Pow((cell[columnSelector] - avgVal), 2));
            stdDev = Convert.ToSingle(Math.Sqrt(sumVal / (obj.Count - 1)));
            _stdDev = stdDev;
            return _stdDev;
        }
        public static List<List<float>> GetDataSet()
        {
            List<List<float>> tempData = new List<List<float>>();
            using (StreamReader readerObj = new StreamReader("normalizasyon.csv"))
            {
                while (readerObj.Peek() > 0)
                {
                    string rawRow = readerObj.ReadLine();
                    string[] values = rawRow.Split(';');
                    tempData.Add(Array.ConvertAll(values, val => float.Parse(val)).ToList());
                }
            }
            return tempData;
        }
        public static List<List<float>> NormalizeDataSet(this List<List<float>> rawDataSet)
        {
            List<List<float>> resultSet = new List<List<float>>();
            int colSelector = rawDataSet[0].Count;
            for (int y = 0; y < colSelector; y++)
            {
                for (int x = 0; x < rawDataSet.Count; x++)
                {
                    float dsMin = rawDataSet.MinValue(y);
                    float dsMax = rawDataSet.MaxValue(y);

                    if (resultSet.Count <= x)
                    {
                        resultSet.Add(new List<float>());
                    }

                    resultSet[x].Add(rawDataSet[x][y].MaxMinNormalization(dsMin, dsMax));
                }
            }
            return resultSet;
        }
        public static List<List<float>> GetZScore(this List<List<float>> rawDataSet)
        {
            List<List<float>> resultSet = new List<List<float>>();
            int colSelector = rawDataSet[0].Count;
            for (int y = 0; y < colSelector; y++)
            {
                for (int x = 0; x < rawDataSet.Count; x++)
                {
                    float avg = rawDataSet.AvgValue(y);
                    float stdDev = rawDataSet.StdDevValue(y);

                    if (resultSet.Count <= x)
                    {
                        resultSet.Add(new List<float>());
                    }

                    resultSet[x].Add(rawDataSet[x][y].ZScore(avg, stdDev));
                }
            }
            return resultSet;
        }
    }
}
