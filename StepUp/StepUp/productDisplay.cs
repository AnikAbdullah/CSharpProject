using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepUp
{
    public partial class productDisplay : Component
    {
        public productDisplay()
        {
            InitializeComponent();
        }

        public productDisplay(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
