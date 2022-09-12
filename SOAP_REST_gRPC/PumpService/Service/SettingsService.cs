using PumpService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumpService.Service
{
    public class SettingsService : ISettingsService
    {
        public string FileName { get; set; }
    }
}