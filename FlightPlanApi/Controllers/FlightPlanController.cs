using Amazon.Runtime.Internal.Transform;
using FlightPlanApi.Data;
using FlightPlanApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanApi.Controllers
{
    [Route("api/v1/flightplan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IDatabaseAdapter _database;

        public FlightPlanController(IDatabaseAdapter database)
        {
            _database = database;
        }
        [HttpGet]
        public async Task<IActionResult> FlightPlanList()
        {
            var flightPlanList = await _database.GetAllFlightPlans();

            if (flightPlanList.Count.Equals(0))
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(flightPlanList);
        }
        [HttpGet]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);

            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return NotFound();
            }

            return Ok(flightPlan);
        }
        [HttpPost]
        [Route("file")]
        public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            var newFlightPlan = await _database.AddFileFlightPlan(flightPlan);

            switch (newFlightPlan)
            {
                case TransactionResult.Success:
                    return Ok(newFlightPlan);
                case TransactionResult.BadRequest:
                    return BadRequest();
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
        {
            var updateResult = await _database.UpdateFlightPlan(flightPlan.FlightPlanId, flightPlan);

            switch (updateResult)
            {
                case TransactionResult.Success:
                    return Ok(updateResult);
                case TransactionResult.NotFound:
                    return NotFound();
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
        {
            var result = await _database.DeleteFlightPlanById(flightPlanId);

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
        [HttpGet]
        [Route("airport/departure/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanDepartureAirtport(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return NotFound();
            }

            return Ok(flightPlan.DepartureAirport);
        }
        [HttpGet]
        [Route("route/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanRoute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return NotFound();
            }

            return Ok(flightPlan.Route);
        }
        [HttpGet]
        [Route("time/enroute/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return NotFound();
            }

            var estimatedTimeEnroute = flightPlan.ArrivalTime - flightPlan.DepartureTime;

            return Ok(estimatedTimeEnroute);
        }
    }
}