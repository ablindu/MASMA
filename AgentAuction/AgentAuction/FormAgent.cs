using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgentAuction
{
    public partial class FormAgent : Form
    {
        public FormAgent()
        {
            InitializeComponent();
        }

        public void DoEvents()
        {
            Application.DoEvents();
        }

        public void AddTextLine(string s)
        {
            textBoxMessages.AppendText(s + Environment.NewLine);
        }

        private void FormAgent_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0); // closes all open forms and exits the application
        }
    }
}