using Hangfire;
using System.Linq.Expressions;

namespace HangFire.Configuration.Services
{

    public interface IHangfireBackgroundService
    {
        void Enqueue(Expression<Action> methodCall);
        void Enqueue<T>(Expression<Action<T>> methodCall);  //For strongly-typed
        void Schedule(Expression<Action> methodCall, TimeSpan delay);
        void Schedule(Expression<Action> methodCall, DateTime enqueueAt);
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
        string Schedule<T>(Expression<Action<T>> methodCall, DateTime enqueueAt);
        void AddRecurring(string recurringJobId, Expression<Action> methodCall, string cronExpression);
        void AddRecurring<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression);
        void Delete(string jobId);
        void DeleteRecurring(string recurringJobId);
    }

    public class HangfireBackgroundService : IHangfireBackgroundService
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public HangfireBackgroundService(IBackgroundJobClient jobClient, IRecurringJobManager recurringJobManager)
        {
            _jobClient = jobClient;
            _recurringJobManager = recurringJobManager;
        }

        public void Enqueue(Expression<Action> methodCall) => _jobClient.Enqueue(methodCall);
        public void Enqueue<T>(Expression<Action<T>> methodCall) => _jobClient.Enqueue<T>(methodCall);

        public void Schedule(Expression<Action> methodCall, TimeSpan delay) => _jobClient.Schedule(methodCall, delay);
        public void Schedule(Expression<Action> methodCall, DateTime enqueueAt) => _jobClient.Schedule(methodCall, enqueueAt);
        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) => _jobClient.Schedule<T>(methodCall, delay);
        public string Schedule<T>(Expression<Action<T>> methodCall, DateTime enqueueAt) => _jobClient.Schedule<T>(methodCall, enqueueAt);

        public void AddRecurring(string recurringJobId, Expression<Action> methodCall, string cronExpression) => _recurringJobManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        public void AddRecurring<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression) => _recurringJobManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        public void Delete(string jobId) => _jobClient.Delete(jobId);
        public void DeleteRecurring(string recurringJobId) => _recurringJobManager.RemoveIfExists(recurringJobId);
    }
}
