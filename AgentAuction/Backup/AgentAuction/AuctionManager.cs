using System;
using System.Threading;
using System.Collections.Generic;
using jade.core;

namespace AgentAuction
{
    class AuctionManager : Agent
    {
        public FormAgent windowsForm;

        public List<AID> participantList;
        public int currentPrice = 0;
        public AID currentWinner = null;
        int maxStartingPrice = 100;
        int auctionCallInterval = 5000;

        public override void setup()
        {
            object[] args = this.getArguments();
            if (args != null)
                windowsForm = (FormAgent)args[0];

            participantList = new List<AID>();
            currentPrice = (new Random()).Next(maxStartingPrice);

            windowsForm.Text = this.getName();
            windowsForm.Show();

            YellowPages.RegisterService("AuctionService", this);
            windowsForm.AddTextLine("Registered AuctionService");

            addBehaviour(new ManagerReceive(this));
            addBehaviour(new ManagerSend(this, auctionCallInterval));
        }
    }
}
