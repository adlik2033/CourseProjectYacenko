using CourseProjectYacenko.Interfaces;
using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Repository
{
    public interface ISubscriberRepository : IRepository<Subscriber>
    {
        Task<Subscriber> GetByPhoneAsync(string phoneNumber);
        Task<Subscriber> GetByEmailAsync(string email);
        Task<IEnumerable<Subscriber>> GetSubscribersWithTariffsAsync();
        Task<IEnumerable<Subscriber>> GetSubscribersWithBalanceBelowAsync(decimal threshold);
        Task<decimal> GetTotalBalanceAsync();
        Task<int> GetActiveSubscribersCountAsync();
    }
}
