using Backend.DTO.Requests;
using Backend.Models;
using Google.Cloud.Firestore;

namespace Backend.DataAccess
{
    public class FirebaseRepository
    {
        string projectId;
        FirestoreDb fireStoreDb;

        public FirebaseRepository()
        {
            var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            string filePath = "energyproduction-36021-firebase-adminsdk-cjdi2-27a9ef242f.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            projectId = "energyproduction-36021";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task AddEnergyProduction(FirebaseEnergyProduction energyProduction, CancellationToken cancellationToken)
        {
            CollectionReference colRef = fireStoreDb.Collection(energyProduction.SerialNumber);
            await colRef.AddAsync(energyProduction, cancellationToken);
        }

        public async Task AddEnergyProductions(AddEnergyProductionsRequest addEnergyProductionsRequest, CancellationToken cancellationToken)
        {
            CollectionReference colRef = fireStoreDb.Collection(addEnergyProductionsRequest.SerialNumber);
            foreach (FirebaseEnergyProduction energyProduction in addEnergyProductionsRequest.EnergyProductions)
            {
                await colRef.AddAsync(energyProduction, cancellationToken);
            }
        }
        public async Task DeleteCollection(string serialNumber, int batchSize)
        {
            CollectionReference colRef = fireStoreDb.Collection(serialNumber);
            QuerySnapshot snapshot = await colRef.Limit(batchSize).GetSnapshotAsync();
            IReadOnlyList<DocumentSnapshot> documents = snapshot.Documents;
            while (documents.Count > 0)
            {
                foreach (DocumentSnapshot document in documents)
                {
                    Console.WriteLine("Deleting document {0}", document.Id);
                    await document.Reference.DeleteAsync();
                }
                snapshot = await colRef.Limit(batchSize).GetSnapshotAsync();
                documents = snapshot.Documents;
            }
            Console.WriteLine("Finished deleting all documents from the collection.");
        }

        public async Task<List<FirebaseEnergyProduction>> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = new List<FirebaseEnergyProduction>();
            CollectionReference colRef = fireStoreDb.Collection(serialNumber);
            QuerySnapshot querySnapshot = await colRef.GetSnapshotAsync(cancellationToken);
            foreach (DocumentSnapshot document in querySnapshot.Documents)
            {
                FirebaseEnergyProduction energyProduction = document.ConvertTo<FirebaseEnergyProduction>();
                energyProductions.Add(energyProduction);
            }
            return energyProductions;
        }

        public async Task<List<FirebaseEnergyProduction>> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = new List<FirebaseEnergyProduction>();
            var date = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
            var timestamp = Timestamp.FromDateTime(date);
            CollectionReference colRef = fireStoreDb.Collection(serialNumber);
            Query query = colRef.WhereGreaterThanOrEqualTo("CurrentTime", timestamp).OrderBy("CurrentTime");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync(cancellationToken);
            foreach (DocumentSnapshot document in querySnapshot.Documents)
            {
                FirebaseEnergyProduction energyProduction = document.ConvertTo<FirebaseEnergyProduction>();
                energyProductions.Add(energyProduction);
            }
            return energyProductions;
        }
    }
}
