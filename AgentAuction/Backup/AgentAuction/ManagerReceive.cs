using System;
using System.Collections.Generic;
using System.Text;
using jade.core;
using jade.core.behaviours;
using jade.lang.acl;

namespace AgentAuction 
{
    class ManagerReceive : CyclicBehaviour
    {
        private new AuctionManager myAgent;

        public ManagerReceive(AuctionManager a)
            : base(a)
        {
            myAgent = a;
        }

        public override void action()
        {
            ACLMessage m = myAgent.receive();

            if (m != null)
            {
                string received = m.getContent();
                AID sender = m.getSender();
                
                if(received.Contains("registering"))
                {
                    //sender wants to register for auction
                    myAgent.participantList.Add(sender);
                    
                    //reply to sender that registration was successful
                    ACLMessage reply = new ACLMessage(ACLMessage.INFORM);
                    reply.setContent("registered");
                    reply.addReceiver(sender);
                    myAgent.send(reply);

                    myAgent.windowsForm.AddTextLine("New participant: " + sender.getLocalName());
                }

                if (received.Contains("offer"))
                { 
                    //received new offer from agent
                    int receivedPrice = Convert.ToInt32(received.Remove(0, 5));

                    myAgent.windowsForm.AddTextLine("Received from " + sender.getLocalName() + " offer: " + receivedPrice);

                    if (receivedPrice > myAgent.currentPrice)
                    {
                        myAgent.currentPrice = receivedPrice;
                        myAgent.currentWinner = sender;
                    }
                }

                if (received.Contains("retreat"))
                {
                    //agent retreated
                    myAgent.participantList.Remove(sender);
                    myAgent.windowsForm.AddTextLine(sender.getLocalName() + " retreated.");
                }
            }
            else
                block();

        }
    }
}
