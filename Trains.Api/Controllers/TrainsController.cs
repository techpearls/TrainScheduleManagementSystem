using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Trains.Api.Models;
using Trains.Api.Services;

namespace Trains.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrainsController : ControllerBase
    {
        private readonly IScheduleService _service;

        public TrainsController(IScheduleService service = null)
        {
            _service = service ?? new ScheduleService();
        }

        /// <summary>
        /// Gets schedule for a given train Id.
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{trainId}", Name = nameof(GetSchedule))]
        public IActionResult GetSchedule(string trainId)
        {
            if (string.IsNullOrEmpty(trainId) || string.IsNullOrWhiteSpace(trainId))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Invalid train id provided");
            }

            var schedule = _service.GetSchedule(trainId);
            if(schedule == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, $"Train id {trainId} not found");
            }

            return Ok(schedule);
        }

        /// <summary>
        /// Adds schedule for a given train. If a schedule already exists for a given train, this method returns BadRequest
        /// Time needs to be an array of strings in the HH:MM pattern where HH represents hours (between 0 and 23 inclusive)
        /// and MM represents minutes (between 0 and 59 inclusive)
        /// </summary>
        /// <param name="trainSchedule"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(AddSchedule))]        
        public IActionResult AddSchedule([FromBody] TrainSchedule trainSchedule)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    return StatusCode((int)HttpStatusCode.BadRequest, errors);
                }

                _service.CreateSchedule(trainSchedule.TrainId, trainSchedule.Schedules);
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("next/{time}", Name = nameof(GetNext))]
        public IActionResult GetNext(string time)
        {
            if (string.IsNullOrEmpty(time) || string.IsNullOrWhiteSpace(time))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Invalid time provided");
            }

            if(!new Regex(@"\d{1,2}:\d{1,2}").IsMatch(time))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Please ensure time is in the HH:MM format. Exxample 9:23 or 12:03");
            }

            try
            {
                return StatusCode((int)HttpStatusCode.OK, _service.GetNext(time));
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("all", Name = nameof(GetAll))]
        public IActionResult GetAll()
        {
            return StatusCode((int)HttpStatusCode.OK, _service.GetAll());
        }
    }
}
