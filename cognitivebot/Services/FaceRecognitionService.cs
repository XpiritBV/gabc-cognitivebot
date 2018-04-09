using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Linq;

namespace cognitivebot.Services
{
    public class FaceRecognitionService : IFaceRecognitionService
    {
        FaceServiceClient faceClient;
        public const string PersonGroup = "gabc";
        
        public FaceRecognitionService()
        {
            faceClient = new FaceServiceClient("c52bfd294723483d82a4609f422ea6a1", "https://westeurope.api.cognitive.microsoft.com/face/v1.0");
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

        public async Task<IList<Person>> GetKnownPersons()
        {
            return await faceClient.ListPersonsAsync(PersonGroup);
        }

        public async Task<bool> AddPhotoToExistingPerson(Guid id, string url)
        {
            var result = await faceClient.AddPersonFaceAsync(PersonGroup, id, url);

            if(result != null && result.PersistedFaceId != Guid.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddNewPerson(string name, string url)
        {
            var person = await faceClient.CreatePersonAsync(PersonGroup, name);
            var result = await faceClient.AddPersonFaceAsync(PersonGroup, person.PersonId, url);

            if (result != null && result.PersistedFaceId != Guid.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Person> IdentifyPerson(string url)
        {
            var result = await faceClient.DetectAsync(url, true);
            if(result != null && result.Length > 0)
            {
                try
                {
                    var person = await faceClient.GetPersonAsync(PersonGroup, result[0].FaceId);
                    if (person != null)
                    {
                        return person;
                    }
                }
                catch(FaceAPIException faceException)
                {
                    //if face is not found the api throws an exception :(
                    return null;
                }
            }
            return null;
        }

        public async Task TrainModel()
        {
            await faceClient.TrainPersonGroupAsync(PersonGroup);
        }
    }
}
