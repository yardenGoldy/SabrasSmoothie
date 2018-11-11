using Microsoft.ML;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.Learners;
using Microsoft.ML.Transforms.Conversions;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class PredictedSmoothieController : Controller
    {
        float fMilk = 0.0f;
        float fOrange = 0.0f;
        float fStrawberry = 0.0f;
        float fNana = 0.0f;
        float fBanana = 0.0f;
        float fApple = 0.0f;
        float fNuts = 0.0f;
        float fSweets = 0.0f;
        float fHoney = 0.0f;
        float fGinger = 0.0f;

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

        // GET: PredictedSmoothie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Submit(string cbMilk, string cbOrange, 
                                   string cbStrawberry, string cbNana,
                                   string cbBananas, string cbApples,
                                   string cbNuts, string cbSweets,
                                   string cbHoney, string cbGinger)
        {
            if (cbMilk == "on")
            {
                fMilk = 1.0f;
            }
            if (cbOrange == "on")
            {
                fOrange = 1.0f;
            }
            if (cbStrawberry == "on")
            {
                fStrawberry = 1.0f;
            }
            if (cbNana == "on")
            {
                fNana = 1.0f;
            }
            if (cbBananas == "on")
            {
                fBanana = 1.0f;
            }
            if (cbApples == "on")
            {
                fApple = 1.0f;
            }
            if (cbNuts == "on")
            {
                fNuts = 1.0f;
            }
            if (cbSweets == "on")
            {
                fSweets = 1.0f;
            }
            if (cbHoney == "on")
            {
                fHoney = 1.0f;
            }
            if (cbGinger == "on")
            {
                fGinger = 1.0f;
            }

            var mlContext = new MLContext();

            string dataPath = @"C:\Users\yuval\Source\Repos\yardenGoldy\SabrasSmoothie\SabrasSmoothie\Controllers\Prediction.txt";
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
                    new TextLoader.Column("Honey", DataKind.R4, 7),
                    new TextLoader.Column("Ginger", DataKind.R4, 8),
                    new TextLoader.Column("Label", DataKind.Text, 9)
                }
            });


            IDataView trainingDataView = reader.Read(new MultiFileSource(dataPath));

            var pipeline = mlContext.Transforms.Categorical.MapValueToKey("Label")
                .Append(mlContext.Transforms.Concatenate("Features", "Milk", 
                                                         "Orange", "Strawberry", 
                                                         "Nana", "Banana", 
                                                         "Apple", "Nuts", 
                                                         "Honey", "Ginger"))
                .Append(mlContext.MulticlassClassification.Trainers
                        .StochasticDualCoordinateAscent(label: "Label", features: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            /*var model = pipeline.Fit(trainingDataView);

            var prediction = model.MakePredictionFunction<SmoothieData, SmoothiePrediction>(mlContext)
                .Predict
            (
                new SmoothieData()
                {
                    Milk = fMilk,
                    Orange = fOrange,
                    Strawberry = fStrawberry,
                    Nana = fNana,
                    Banana = fBanana,
                    Apple = fApple,
                    Nuts = fNuts,
                    Honey = fHoney,
                    Ginger = fGinger
                }
            );*/

            return RedirectToAction("Results", "PredictedSmoothie", "Gods Smoothie");//prediction.PredictedLabels);

            //Console.WriteLine($"Predicted Smoothie type is: {prediction.PredictedLabels}");

            string filePath = @"C:\Users\yuval\Source\Repos\yardenGoldy\SabrasSmoothie\SabrasSmoothie\Controllers\Prediction.txt";

            string strComponents = fMilk + ".0, " +
                        fOrange + ".0, " +
                        fStrawberry + ".0, " +
                        fNana + ".0, " +
                        fBanana + ".0, " +
                        fApple + ".0, " +
                        fNuts + ".0, " +
                        fSweets + ".0, " +
                        fHoney + ".0, " +
                        fGinger + ".0";

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
            if (!bFound)
            {

                Console.WriteLine("Does It The Best Fit Smoothie? Y/n");
                char Answer = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (Answer == 'Y')
                {
                    //File.AppendAllText(filePath,
                    //    Environment.NewLine +
                    //    strComponents + ", " +
                    //    prediction.PredictedLabels
                    //    );
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
                            //File.AppendAllText(filePath,
                           // Environment.NewLine +
                           // strComponents + ", " +
                           // smoothieFullName);
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

            return View();
        }

        public ActionResult Results(string predict, string lPrediction) {

            string filePath = @"C:\Users\yuval\Source\Repos\yardenGoldy\SabrasSmoothie\SabrasSmoothie\Controllers\Prediction.txt";

            string strComponents = fMilk + ".0, " +
                        fOrange + ".0, " +
                        fStrawberry + ".0, " +
                        fNana + ".0, " +
                        fBanana + ".0, " +
                        fApple + ".0, " +
                        fNuts + ".0, " +
                        fSweets + ".0, " +
                        fHoney + ".0, " +
                        fGinger + ".0";

            string strComponentToSearch = strComponents.Trim();
            string[] fileContent = System.IO.File.ReadAllLines(filePath);
            bool bFound = false;
            foreach (string line in fileContent)
            {
                if (line.Contains(strComponentToSearch))
                {
                    bFound = true;
                    break;
                }

            }
            if (!bFound)
            {

            } 

                return View();
        }
    }
}