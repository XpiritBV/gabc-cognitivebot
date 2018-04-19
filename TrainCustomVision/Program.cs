using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Training;
using Microsoft.Cognitive.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TrainCustomVision
{
    class Program
    {
        private static List<string> hammerImages;
     
        static void Main(string[] args)
        {
            // Add your training key from the settings page of the portal
            string trainingKey = "<insert code>";

            // Create the Api, passing in the training key
            TrainingApi trainingApi = new TrainingApi() { ApiKey = trainingKey };

            // Getting Project or create project
            Console.WriteLine("Getting project");
            //var project = trainingApi.GetProject(new Guid("<insert project guid>"));
            //var project = trainingApi.CreateProject("MurdersWeapons");

            Console.WriteLine("\tUploading images");
            LoadImagesFromDisk();


            var imageFiles = hammerImages.Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img))).ToList();
            trainingApi.CreateImagesFromFiles(project.Id, new ImageFileCreateBatch(imageFiles, null));


            // Now there are images start training the project
            Console.WriteLine("\tTraining");
            var iteration = trainingApi.TrainProject(project.Id);

            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get it's updated status
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }

            // The iteration is now trained. Make it the default project endpoint
            iteration.IsDefault = true;
            trainingApi.UpdateIteration(project.Id, iteration.Id, iteration);
            Console.WriteLine("Done!\n");
           
            Console.ReadKey();
        }

        private static void LoadImagesFromDisk()
        {         
            hammerImages = Directory.GetFiles(@"..\..\..\Images\Hammer").ToList();
        }
    }
}
