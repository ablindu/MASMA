using System;
using System.Collections.Generic;
using System.Text;
using jade.core;
using jade.domain;
using jade.domain.FIPAAgentManagement;
using jade.wrapper;
using AgentContainer = jade.wrapper.AgentContainer;
using System.Windows.Forms;

namespace AgentAuction
{
    
    public class YellowPages
    {
        // Returns the AID of the first found service provider
        public static AID FindService(string serviceName, Agent myAgent, int timeOut)
        {
            AID providerAID = null;
            bool found = false;

            double t1 = PerformanceCounter.GetValue();
            while (!found)
            {
                if (PerformanceCounter.GetValue() - t1 > timeOut)
                    break;

                Application.DoEvents();

                // search for a provider
                DFAgentDescription template = new DFAgentDescription();
                ServiceDescription sd = new ServiceDescription();
                sd.setType(serviceName);
                template.addServices(sd);

                DFAgentDescription[] result = DFService.search(myAgent, template);
                if (result != null && result.Length > 0)
                {
                    providerAID = result[0].getName(); 
                    found = true;
                }
            }
            
            return providerAID;
        }

        
        // Registers a service on behalf of an agent
        public static void RegisterService(string serviceName, Agent myAgent)
        {
            DFAgentDescription dfd = new DFAgentDescription();
            dfd.setName(myAgent.getAID());
            ServiceDescription sd = new ServiceDescription();
            sd.setType(serviceName);
            sd.setName(serviceName);
            dfd.addServices(sd);
            DFService.register(myAgent, dfd);
        }

        // Deregisters a service on behalf of an agent
        public static void DeregisterService(Agent myAgent)
        {
            DFService.deregister(myAgent);
        }

        // Handles the messages associated with the take down of an agent, and deregisters the service on its behalf
        public static void DeregisterServiceOnTakeDown(Agent myAgent)
        {
            DFService.deregister(myAgent);
            MessageBox.Show(
                "Agent " + myAgent.getLocalName() +
                " was taken down.\r\nAn unhandled exception probably occured.");
        }
    }
}
