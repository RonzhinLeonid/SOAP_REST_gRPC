using ClinicService.Context;
using ClinicService.Data;
using Grpc.Core;
using PetServiceProtos;
using static PetServiceProtos.PetClientService;

namespace ClinicService.Services.ClientService
{
    public class PetClientService : PetClientServiceBase
    {
        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<PetClientService> _logger;

        #endregion

        #region Constructors

        public PetClientService(ClinicServiceDbContext dbContext,
            ILogger<PetClientService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #endregion

        public override Task<CreatePetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
        {
            var pet = new Pet
            {
                Name = request.Name,
                ClientId = request.ClientId,
                Birthday = request.Birthday.ToDateTime()
        };

            _dbContext.Pets.Add(pet);

            _dbContext.SaveChanges();

            var response = new CreatePetResponse
            {
                PetId = pet.PetId
            };

            return Task.FromResult(response);
        }

        public override Task<GetPetsResponse> GetPets(GetPetsRequest request, ServerCallContext context)
        {
            var response = new GetPetsResponse();

            response.Pets.AddRange(_dbContext.Pets.Select(pet => new PetResponse
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Birthday = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(pet.Birthday),
                ClientId = pet.ClientId
            }).ToList());

            return Task.FromResult(response);
        }
    }
}
