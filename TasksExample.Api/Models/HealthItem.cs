using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksExample.Api.Models
{
    /// <summary>
    /// Health Item
    /// </summary>
    public class HealthItem
    {
        /// <summary>
        /// The key for the health item
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value of the health item
        /// </summary>
        public string Value { get; set; }
    }
}