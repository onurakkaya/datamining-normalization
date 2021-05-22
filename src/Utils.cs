using System;
using System.Collections.Generic;
using System.Data;

namespace DataMiningNormalization
{
    public static class Utils
    {
        public static DataTable ToDataTable(this IList<List<float>> data)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < data.Count; i++)
            {
                _ = table.Columns.Add("Column" + i.ToString());
            }

            object[] values = new object[data.Count];
            foreach (IList<float> item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = item[i];
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static List<List<float>> GenerateDummyData(int col, int row)
        {
            Random rnd = new Random();
            List<List<float>> resultSet = new List<List<float>>();
            for (int i = 0; i < col; i++)
            {
                List<float> subList = new List<float>();
                for (int j = 0; j < row; j++)
                {
                    subList.Add(rnd.Next(0, 9999));
                }
                resultSet.Add(subList);
            }
            return resultSet;
        }
    }
}