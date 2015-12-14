using System;
using System.Collections.Generic;
using System.Text;
using jade.core;
using jade.core.behaviours;
using jade.lang.acl;

namespace AgentAuction
{
    class BuyerRegister : OneShotBehaviour
    {
        private new BuyerAgent myAgent;

        public BuyerRegister(BuyerAgent a)
            :base(a)
        {
            myAgent = a;
        }

        public override void action()
        {
            //inform managerAgent that I want to register for the auction

            ACLMessage m = new ACLMessage(ACLMessage.INFORM);

            AID receiverAID = new AID("managerAgent", AID.ISLOCALNAME);
            m.addReceiver(receiverAID);
            m.setContent("registering");
            myAgent.send(m);

            myAgent.windowsForm.AddTextLine("Registering for auction ...");
        }
    }
}
