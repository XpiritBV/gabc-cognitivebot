using System;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;

namespace cognitivebot.Services
{
    public interface IFaceRecognitionService
    {
        Task<Face> GetFaceAttributes(string url);
        
    }
}
