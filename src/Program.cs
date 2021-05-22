using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace DataMiningNormalization
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            List<List<float>> DataSet = NormalizationManager.GetDataSet();
            //List<List<float>> DataSet = Utils.GenerateDummyData(1000, 1000);
            watcher.Stop();
            Console.WriteLine("Geting DataSet -> Done ( "+watcher.ElapsedMilliseconds + "ms )");

            watcher.Restart();
            List<List<float>> NormalizedDataSet = DataSet.NormalizeDataSet();
            watcher.Stop();
            Console.WriteLine("NormalizeDataSet -> Done ( " + watcher.ElapsedMilliseconds + "ms )");
            
            DataTable normalizedTable = NormalizedDataSet.ToDataTable();
            watcher.Restart();
            List<List<float>> ZScoreDataSet = DataSet.GetZScore();
            watcher.Stop();
            Console.WriteLine("Calc ZScore -> Done ( " + watcher.ElapsedMilliseconds + "ms )");
            DataTable zscoreTable = ZScoreDataSet.ToDataTable();

        }
    }
}
