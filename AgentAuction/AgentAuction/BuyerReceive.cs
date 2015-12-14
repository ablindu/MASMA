using System;
using System.Collections.Generic;
using System.Text;
using jade.core;
using jade.core.behaviours;
using jade.lang.acl;

namespace AgentAuction
{
    class BuyerReceive : CyclicBehaviour
    {
        private new BuyerAgent myAgent;

        public BuyerReceive(BuyerAgent a)
            :base(a)
        {
            myAgent = a;
        }

        public override void action()
        {
            ACLMessage m = myAgent.receive();

            if (m != null)
            {
                string received = m.getContent();

                if (received.Contains("registered"))
                {
                    //manager registered me for the auction
                    myAgent.windowsForm.AddTextLine("Successfully registered for auction");
                }

                if(received.Contains("current_price"))
                {
                    // received new call from managerAgent

                    //the highest current price is:
                    int currentPrice = Convert.ToInt32(received.Remove(0, 13));
                    myAgent.windowsForm.AddTextLine("Received current price: " + currentPrice);

                    // probability to make offer decreases with each price increase
                    myAgent.probabilityToBid *= myAgent.probabilityDecreaseFactor;

                    ACLMessage toSend = new ACLMessage(ACLMessage.INFORM);
                    toSend.addReceiver(m.getSender());
                    //use agent name to generate unique seed for random numbers
                    int seed = 1234 * Convert.ToInt32(myAgent.getLocalName().Remove(0, 5));
                    //decide if to send new offer or retreat
                    if (myAgent.probabilityToBid > new Random(seed).NextDouble())
                    {
                        //make an offer which is higher than the current price
                        seed = 4321 * Convert.ToInt32(myAgent.getLocalName().Remove(0, 5));
                        int newPrice = currentPrice + new Random(seed).Next(1, myAgent.maxOfferIncrease);
                        toSend.setContent("offer" + newPrice);
                        myAgent.windowsForm.AddTextLine("Sent new offer: " + newPrice);
                    }
                    else
                    {
                        //price is too high, retreat from auction
                        toSend.setContent("retreat");
                        myAgent.windowsForm.AddTextLine("Retreated from auction.");
                    }
                    myAgent.send(toSend);
                }
            }
            else
                block();
        }
    }
}
