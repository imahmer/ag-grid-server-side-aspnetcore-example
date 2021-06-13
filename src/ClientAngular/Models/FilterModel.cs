using Newtonsoft.Json;
using System;

namespace ClientAngular.Models
{
    public class FilterModel
    {
        public FilterModel condition1 { get; set; }
        public FilterModel condition2 { get; set; }
        [JsonProperty("operator")]
        public string logicOperator { get; set; }
        public string type { get; set; }
        public string filter { get; set; }
        public string filterTo { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string filterType { get; set; }
    }
}
