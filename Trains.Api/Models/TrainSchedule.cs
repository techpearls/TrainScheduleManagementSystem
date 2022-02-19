using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trains.Api.Models
{
    public class TrainSchedule
    {
        [RegularExpression(@"^[a-zA-Z0-9]{1,4}$")]
        public string TrainId { get; set; }
        public List<string> Schedules { get; set; }
    }
}
