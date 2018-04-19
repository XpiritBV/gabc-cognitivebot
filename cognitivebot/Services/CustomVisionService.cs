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

        //public const string PersonGroup = "gabc";

        public CustomVisionService()
        {
            _customVisionTrainingAPI = new TrainingApi() { ApiKey = "d34ef84918894544889d7136f32d4e67" };
            _predictionEndpoint = new PredictionEndpoint() { ApiKey = "0b3b87df030c4c27adfc763895e7c7f2" };
        }

        public async Task<bool> AddNewWeapon(string name, string contentUrl)
        {
            var token = await new MicrosoftAppCredentials("ce6d5c93-aef8-4b2a-abf9-0dc55ef67d27", "qksWUH1886{()nctuMYYF8@").GetTokenAsync();
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var memoryStream = await httpClient.GetStreamAsync(contentUrl);

            var project = await _customVisionTrainingAPI.GetProjectAsync(new Guid("06d4aeff-fb9d-453f-8912-5b2e89d1f1d4"));


            //var webClient = new WebClient();
            //var memoryStream = new MemoryStream(webClient.DownloadData(contentUrl));


            var images = new List<ImageUrlCreateEntry>();
            images.Add(new ImageUrlCreateEntry(contentUrl + "/" + name));            

            return false;
        }

        public async Task<ImagePredictionResultModel> IdentifyWeapon(string contentUrl)
        {
            var project = await _customVisionTrainingAPI.GetProjectAsync(new Guid("06d4aeff-fb9d-453f-8912-5b2e89d1f1d4"));

            var token = await new MicrosoftAppCredentials("ce6d5c93-aef8-4b2a-abf9-0dc55ef67d27", "qksWUH1886{()nctuMYYF8@").GetTokenAsync();
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var memoryStream = await httpClient.GetStreamAsync(contentUrl);            

            var respon = await this._predictionEndpoint.PredictImageAsync(project.Id, memoryStream);
            
            return respon;
        }
    }
}
