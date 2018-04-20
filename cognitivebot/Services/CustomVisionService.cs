using Microsoft.Bot.Connector.Authentication;
using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Prediction.Models;
using Microsoft.Cognitive.CustomVision.Training;
using Microsoft.Cognitive.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace cognitivebot.Services
{
    public class CustomVisionService : ICustomVisionService
    {
        TrainingApi _customVisionTrainingAPI;
        PredictionEndpoint _predictionEndpoint;

        
        public CustomVisionService()
        {
            _customVisionTrainingAPI = new TrainingApi() { ApiKey = "insert key" };
            _predictionEndpoint = new PredictionEndpoint() { ApiKey = "insert key" };
        }

        public async Task<bool> AddNewWeapon(string name, string contentUrl)
        {
           
        }

        public async Task<ImagePredictionResultModel> IdentifyWeapon(string contentUrl)
        {
           
        }
    }
}
