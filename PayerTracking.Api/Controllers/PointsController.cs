using Microsoft.AspNetCore.Mvc;
using PayerTracking.Api.DTOs.Requests;
using PayerTracking.Api.DTOs.Responses;
using PayerTracking.Library;

namespace PayerTracking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly Orchestrator orchestrator;

        public PointsController(Orchestrator orchestrator)
        {
            this.orchestrator = orchestrator;
        }

        /// <summary>
        /// Adds points to a specific payer
        /// </summary>
        /// <param name="request">Which payer to add, how many points to add, and when were they added (defaults to now if not provided)</param>
        /// <returns>Whether the operation was successful.</returns>
        [HttpPut]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPoints([FromBody] AddPointsRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("No request object sent.");

                var result = orchestrator.AddPoints(request.PayerName, request.Amount, request.TimeStamp);
                if (!result.IsSuccessful)
                {
                    return StatusCode(result.StatusCode, result.ErrorMessage);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Spends a specified number of points according to preset rules.
        /// </summary>
        /// <param name="request">How many points to spend</param>
        /// <returns></returns>
        [HttpPut]
        [Route("spend")]
        [ProducesResponseType(typeof(List<SpendPointsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SpendPoints([FromBody] SpendPointsRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("No request object sent.");

                var result = orchestrator.SpendPoints(request.Points);
                if (!result.IsSuccessful)
                {
                    return StatusCode(result.StatusCode, result.ErrorMessage);
                }

                var response = result.Data.Select(pt => new SpendPointsResponse(pt)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBalances()
        {
            try
            {
                var result = orchestrator.GetBalances();
                if (!result.IsSuccessful)
                {
                    return StatusCode(result.StatusCode, result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
