using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;

namespace cognitivebot.Services
{
    public interface IFaceRecognitionService
    {
        Task<bool> AddNewPerson(string name, string url);
        Task<bool> AddPhotoToExistingPerson(Guid id, string url);
        Task<Face> GetFaceAttributes(string url);
        Task<IList<Person>> GetKnownPersons();
        Task<Person> IdentifyPerson(string url);     
    }
}
