using System;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace cognitivebot.Services
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        FaceServiceClient faceClient;
        
        public FaceRecognitionService()
        {
            faceClient = new FaceServiceClient("", "https://westeurope.api.cognitive.microsoft.com/face/v1.0");
        }

        public async Task<Face> GetFaceAttributes(string url)
        {
            var requiredFaceAttributes = new FaceAttributeType[] {
                        FaceAttributeType.Age,
                        FaceAttributeType.Gender,
                        FaceAttributeType.FacialHair,
                        FaceAttributeType.Hair,
                    };

            var face = await faceClient.DetectAsync(url, true, false, requiredFaceAttributes);
            if (face != null && face.Length > 0)
            {
                return face[0];
            }
            else
            {
                return null;
            }
        }
    }
}
