using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XbrlGenerator.Models
{
    public class ReportInfo
    {
        public ReportNamespace Namespace { get; set; } = new();
        public List<ReportUnit> Units { get; set; } = new();
        public List<ReportContext> Contexts { get; set; } = new();
    }

    public class ReportNamespace
    {
        public string Prefix { get; set; } = "";
        public string Uri { get; set; } = "";
    }

    public class ReportUnit
    {
        public string Id { get; set; } = "";
        public string Measure { get; set; } = "";
    }

    public class ReportContext
    {
        public string Id { get; set; } = "";
        public string EntityScheme { get; set; } = "";
        public string EntityIdentifier { get; set; } = "";
        public DateTime Instant { get; set; }
    }
}