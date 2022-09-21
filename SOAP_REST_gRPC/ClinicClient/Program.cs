using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using ClinicServiceProtos;
using static ClinicServiceProtos.AuthenticateService;
using static ClinicServiceProtos.ClinicClientService;
using static ConsultationServiceProtos.ConsultationClientService;
using static PetServiceProtos.PetClientService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //AppContext.SetSwitch(
            //    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            AuthenticateServiceClient authenticateServiceClient = new AuthenticateServiceClient(channel);

            var authenticationResponse = authenticateServiceClient.Login(new AuthenticationRequest
            {
                UserName = "sample@gmail.ru",
                Password = "12345"
            });

            if (authenticationResponse.Status != 0)
            {
                Console.WriteLine("Authentication error.");
                Console.ReadKey();
                return;
            }


            Console.WriteLine($"Session token: {authenticationResponse.SessionContext.SessionToken}");

            var callCredentials = CallCredentials.FromInterceptor((c, m) =>
            {
                m.Add("Authorization",
                    $"Bearer {authenticationResponse.SessionContext.SessionToken}");
                return Task.CompletedTask;
            });

            var protectedChannel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
            });


            ClinicClientServiceClient client = new ClinicClientServiceClient(protectedChannel);

            PetClientServiceClient pet = new PetClientServiceClient(protectedChannel);

            ConsultationClientServiceClient consultation = new ConsultationClientServiceClient(protectedChannel);

            var createClientResponse = client.CreateClient(new ClinicServiceProtos.CreateClientRequest
            {
                Document = "PASS123",
                FirstName = "cтаниcлав",
                Surname = "Байраковcкий",
                Patronymic = "Антонович"
            });

            Console.WriteLine($"Client ({createClientResponse.ClientId}) created successfully.");

            var getClientsResponse = client.GetClients(new ClinicServiceProtos.GetClientsRequest());

            Console.WriteLine("Clients:");
            Console.WriteLine("========\n");
            foreach (var clientObj in getClientsResponse.Clients)
            {
                Console.WriteLine($"{clientObj.Document} >> {clientObj.Surname} {clientObj.FirstName}");
            }

            var createPetResponse = pet.CreatePet(new PetServiceProtos.CreatePetRequest
            {
                Name = "Питомец",
                ClientId = createClientResponse.ClientId,
                Birthday = Timestamp.FromDateTime(DateTime.UtcNow)
            });

            Console.WriteLine($"Pet ({createPetResponse.PetId}) created successfully.");

            var getPetsResponse = pet.GetPets(new PetServiceProtos.GetPetsRequest());

            Console.WriteLine("Pets:");
            Console.WriteLine("========\n");
            foreach (var petObj in getPetsResponse.Pets)
            {
                Console.WriteLine($"{petObj.Name} >> {petObj.Birthday} {petObj.ClientId}");
            }

            //var createConsultationResponse = consultation.CreateConsultation(new ConsultationServiceProtos.CreateConsultationRequest
            //{
            //    ClientId = 1,
            //    PetId = 1,
            //    ConsultationDate = Timestamp.FromDateTime(DateTime.UtcNow),
            //    Description = "Консультация"
            //});

            //Console.WriteLine($"Consultation ({createConsultationResponse.ConsultationId}) created successfully.");

            //var getConsultationsResponse = consultation.GetConsultations(new ConsultationServiceProtos.GetConsultationsRequest());

            //Console.WriteLine("Consultations:");
            //Console.WriteLine("========\n");
            //foreach (var consObj in getConsultationsResponse.Consultations)
            //{
            //    Console.WriteLine($"{consObj.ConsultationId} >> {consObj.ClientId} {consObj.PetId} {consObj.ConsultationDate} {consObj.Description} ");
            //}

            Console.ReadKey();
        }
    }
}