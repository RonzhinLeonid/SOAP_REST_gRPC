using ClinicService.Context;
using ClinicService.Data;

namespace ClinicService.Services.Impl
{
    public class ConsultationRepository : IConsultationRepository
    {

        #region Serives

        private readonly ClinicServiceDbContext _dbContext;
        private readonly ILogger<ConsultationRepository> _logger;

        #endregion

        #region Constructors

        public ConsultationRepository(ClinicServiceDbContext dbContext,
            ILogger<ConsultationRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #endregion

        public int Add(Consultation item)
        {
            _dbContext.Consultations.Add(item);
            _dbContext.SaveChanges();
            return item.ConsultationId;
        }

        public void Delete(Consultation item)
        {
            if (item == null)
                throw new NullReferenceException();
            Delete(item.ConsultationId);
        }

        public void Delete(int id)
        {
            var consultation = GetById(id);
            if (consultation == null)
                throw new KeyNotFoundException();
            _dbContext.Remove(consultation);
            _dbContext.SaveChanges();
        }

        public IList<Consultation> GetAll()
        {
            return _dbContext.Consultations.ToList();
        }

        public Consultation? GetById(int id)
        {
            return _dbContext.Consultations.FirstOrDefault(cons => cons.ConsultationId == id);
        }

        public void Update(Consultation item)
        {
            if (item == null)
                throw new NullReferenceException();

            var consultation = GetById(item.PetId);
            if (consultation == null)
                throw new KeyNotFoundException();

            consultation.ClientId = item.ClientId;
            consultation.PetId = item.PetId;
            consultation.ConsultationDate = item.ConsultationDate;
            consultation.Description = item.Description;

            _dbContext.Update(consultation);
            _dbContext.SaveChanges();
        }
    }
}
