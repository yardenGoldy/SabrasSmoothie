using System;
using Microsoft.ML;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;
using Microsoft.ML.Transforms.Conversions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        public class SmoothieData
        {
            public float Milk;
            public float Orange;
            public float Strawberry;
            public float Nana;
            public float Banana;
            public float Apple;
            public float Nuts;
            public float Sweets;
            public float Honey;
            public float Ginger;
            public string Label;
        }

        public class SmoothiePrediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabels;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Milk:");
            float MilkFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Orange:");
            float OrangeFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Strawberry:");
            float StrawberryFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Nana:");
            float NanaFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Banana:");
            float BananaFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Apple:");
            float AppleFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Nuts:");
            float NutsFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Sweets:");
            float SweetsFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Honey:");
            float HoneyFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            Console.WriteLine("Ginger:");
            float GingerFlag = float.Parse(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            var mlContext = new MLContext();

            string dataPath = "Prediction.txt";
            var reader = mlContext.Data.TextReader(new TextLoader.Arguments()
            {
                Separator = ",",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("Milk", DataKind.R4, 0),
                    new TextLoader.Column("Orange", DataKind.R4, 1),
                    new TextLoader.Column("Strawberry", DataKind.R4, 2),
                    new TextLoader.Column("Nana", DataKind.R4, 3),
                    new TextLoader.Column("Banana", DataKind.R4, 4),
                    new TextLoader.Column("Apple", DataKind.R4, 5),
                    new TextLoader.Column("Nuts", DataKind.R4, 6),
                    new TextLoader.Column("Sweets", DataKind.R4, 7),
                    new TextLoader.Column("Honey", DataKind.R4, 8),
                    new TextLoader.Column("Ginger", DataKind.R4, 9),
                    new TextLoader.Column("Label", DataKind.Text, 10)
                }
            });


            IDataView trainingDataView = reader.Read(new MultiFileSource(dataPath));

            var pipeline = mlContext.Transforms.Categorical.MapValueToKey("Label")
                .Append(mlContext.Transforms.Concatenate("Features", "Milk", "Orange", "Strawberry", "Nana", "Banana", "Apple", "Nuts", "Sweets", "Honey", "Ginger"))
                .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(label: "Label", features: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
  
            var model = pipeline.Fit(trainingDataView);

            var prediction = model.MakePredictionFunction<SmoothieData, SmoothiePrediction>(mlContext).Predict
            (
                new SmoothieData()
                {
                    Milk = MilkFlag,
                    Orange = OrangeFlag,
                    Strawberry = StrawberryFlag,
                    Nana = NanaFlag,
                    Banana = BananaFlag,
                    Apple = AppleFlag,
                    Nuts = NutsFlag,
                    Sweets = SweetsFlag,
                    Honey = HoneyFlag,
                    Ginger = GingerFlag
                }
            );

            Console.WriteLine($"Predicted Smoothie type is: {prediction.PredictedLabels}");

            string filePath = @"C:\Users\yuval\source\repos\ConsoleApp1\ConsoleApp1\Prediction.txt";

            string strComponents = MilkFlag + ".0, " +
                        OrangeFlag + ".0, " +
                        StrawberryFlag + ".0, " +
                        NanaFlag + ".0, " +
                        BananaFlag + ".0, " +
                        AppleFlag + ".0, " +
                        NutsFlag + ".0, " +
                        SweetsFlag + ".0, " +
                        HoneyFlag + ".0, " +
                        GingerFlag + ".0";

            string strComponentToSearch = strComponents.Trim();
            string[] fileContent = System.IO.File.ReadAllLines(filePath);
            bool bFound = false;
            foreach (string line in fileContent)
            {
                if (line.Contains(strComponentToSearch))
                {
                    Console.WriteLine("100% Match!");
                    bFound = true;
                    break;
                }

            }
            if (!bFound) {

                Console.WriteLine("Does It The Best Fit Smoothie? Y/n");
                char Answer = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (Answer == 'Y')
                {
                    File.AppendAllText(filePath, 
                        Environment.NewLine +
                        strComponents + ", " +
                        prediction.PredictedLabels
                        );
                    Console.WriteLine("Thanks!");
                }
                else if (Answer == 'n')
                {
                    Console.WriteLine("Insert Wanted Smoothie Name And You'll Learn For Next Time :)");
                    string smoothieName = Console.ReadLine().Trim();
                    bFound = false;
                    foreach (string line in fileContent)
                    {
                        if (line.Contains(smoothieName))
                        {
                            string[] strFound = line.Split(',');
                            string smoothieFullName = strFound[strFound.Length - 1].Remove(0, 1);
                            File.AppendAllText(filePath,
                            Environment.NewLine +
                            strComponents + ", " +
                            smoothieFullName);
                            Console.WriteLine("Noted, Thanks!");
                            bFound = true;
                            break;
                        }

                    }
                    if (!bFound)
                    {
                        Console.WriteLine("Unfortunately, Not Found, See You Next Time!");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press 'Enter' For Exit");
            Console.ReadLine();

        }
    }
}
