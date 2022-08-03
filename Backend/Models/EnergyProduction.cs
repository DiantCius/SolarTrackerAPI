using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace Backend.Models
{
    [FirestoreData]
    public class EnergyProduction
    {
        [FirestoreProperty]
        public string CurrentProduction { get; set; }
        [FirestoreProperty]
        public string DailyProduction { get; set; }
        [FirestoreProperty]
        public DateTime CurrentTime { get; set; }
        public string SerialNumber { get; set; }

    }
}
