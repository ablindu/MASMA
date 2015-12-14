using System;
using System.Collections.Generic;
using System.Text;
using jade.core;
using jade.core.behaviours;
using jade.lang.acl;

namespace AgentAuction
{
    class ManagerSend : TickerBehaviour
    {
        private new AuctionManager myAgent;

        public ManagerSend(AuctionManager a, long period)
            : base(a, period)
        {
            myAgent = a;
        }

        public override void onTick()
        {
            int nParticipants = myAgent.participantList.Count;

            if (nParticipants > 1) // still more than 1 agent making offers
            {
                /* New auction round:
                 * send calls to the participants
                   inform them on current auction price
                   ask them to send new offers
                 */
                ACLMessage toSend = new ACLMessage(ACLMessage.INFORM);
                toSend.setContent("current_price" + myAgent.currentPrice);

                foreach (AID a in myAgent.participantList)
                    toSend.addReceiver(a);
                
                myAgent.send(toSend);

                myAgent.windowsForm.AddTextLine(Environment.NewLine + "New Round: current price is " + myAgent.currentPrice);
            }
            else if (nParticipants == 1) //only one agent left, the winner
            {   
                AID winner = myAgent.currentWinner;
                myAgent.windowsForm.AddTextLine(Environment.NewLine + "Auction over. Winner is " + winner.getLocalName() + " with price: " + myAgent.currentPrice);
                this.stop();
            }
            else // no participants left
            {
                myAgent.windowsForm.AddTextLine(Environment.NewLine + "Auction over. All participants retreated.");
                this.stop();
            }
        }
    }
}
