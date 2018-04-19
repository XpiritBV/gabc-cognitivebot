using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Linq;
using System.Net;
using System.IO;
using Microsoft.Bot.Connector.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;

namespace cognitivebot.Services
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        FaceServiceClient faceClient;
        public const string PersonGroup = "gabc";
        
        public FaceRecognitionService()
        {
            //use this face client or roll your own by adding a face api in Azure (This api key will be reset after GABC saturday);
            faceClient = new FaceServiceClient("c52bfd294723483d82a4609f422ea6a1", "https://westeurope.api.cognitive.microsoft.com/face/v1.0");
        }

        public async Task<Face> GetFaceAttributes(string url)
        {
            throw new NotImplementedException("this is lab 01");
        }

        public async Task<bool> AddPhotoToExistingPerson(Guid id, string url)
        {
            throw new NotImplementedException("this is lab 04");
        }

        public async Task<bool> AddNewPerson(string name, string url)
        {
            throw new NotImplementedException("this is lab 04");
        }

        public async Task<Person> IdentifyPerson(string url)
        {
            throw new NotImplementedException("this is lab 02");
        }

        public async Task TrainModel()
        {
            throw new NotImplementedException("this is lab 04");
        }
    }
}
