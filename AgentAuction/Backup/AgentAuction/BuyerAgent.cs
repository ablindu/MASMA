using System;
using System.Threading;
using jade.core;

namespace AgentAuction
{
    class BuyerAgent : Agent
    {
        public FormAgent windowsForm;
        public AID providerAID = null;
        public double probabilityToBid = 1.0;
        public double probabilityDecreaseFactor = 0.8;
        public int maxOfferIncrease = 50;

        public override void setup()
        {
            object[] args = this.getArguments();
            if (args != null)
            {
                windowsForm = (FormAgent)args[0];
            }

            windowsForm.Text = this.getName();
            windowsForm.Show();

            providerAID = YellowPages.FindService("AuctionService", this, 10);

            if (providerAID == null)
            {
                windowsForm.AddTextLine("No auction provider found.");
            }
            else
            {
                windowsForm.AddTextLine("Found auction provider: " + providerAID.getLocalName());

                addBehaviour(new BuyerRegister(this));
                addBehaviour(new BuyerReceive(this));
            }
        }
    }
}
   
