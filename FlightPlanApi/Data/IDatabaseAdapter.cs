using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public interface IDatabaseAdapter
    {
        Task<List<FlightPlan>> GetAllFlightPlans();
        Task<FlightPlan> GetFlightPlanById(string flightPlanId);
        Task<bool> AddFileFlightPlan(FlightPlan flightPlan);
        Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan);
        Task<bool> DeleteFlightPlanById(string flightPlanId);
    }
}