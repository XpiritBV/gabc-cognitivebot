using Microsoft.Cognitive.CustomVision.Prediction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cognitivebot.Services
{
    public interface ICustomVisionService
    {
        Task<bool> AddNewWeapon(string name, string url);

        Task<ImagePredictionResultModel> IdentifyWeapon(string contentUrl);
    }
}
