using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Continuous.Test.BasicService;

namespace Continuous.CompabilityTests.BasicService
{
    public partial class ContinuousBasicService : ServiceBase
    {
        private readonly FileWriter _writer;

        public ContinuousBasicService()
        {
            InitializeComponent();
            
            _writer = new FileWriter();
        }

        protected override void OnStart(string[] args)
        {
            _writer.LogStart();
        }
        
        protected override void OnStop()
        {
            _writer.LogEnd();
        }
    }
}
