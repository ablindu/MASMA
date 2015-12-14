using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using jade.core;


namespace AgentAuction
{
    public class MASInit
    {
        public static void DoInitialization()
        {
            //create main container
            jade.wrapper.AgentContainer mainContainer = CreateContainer("Container1M", true, "localhost", null, "1090");
            mainContainer.start();

            FormAgent managerAgentForm = new FormAgent();
            managerAgentForm.Location = new Point(50, 50);

            FormAgent agent1Form = new FormAgent();
            agent1Form.Location = new Point(500, 50);

            FormAgent agent2Form = new FormAgent();
            agent2Form.Location = new Point(50, 350);

            FormAgent agent3Form = new FormAgent();
            agent3Form.Location = new Point(500, 350);

            //create and start auction manager and participants
            jade.wrapper.AgentController managerAg =
                CreateAgent(mainContainer, "managerAgent", "AgentAuction.AuctionManager", new object[] { managerAgentForm });

            jade.wrapper.AgentController ag1 =
                CreateAgent(mainContainer, "Buyer1", "AgentAuction.BuyerAgent", new object[] { agent1Form });
            
            jade.wrapper.AgentController ag2 =
                CreateAgent(mainContainer, "Buyer2", "AgentAuction.BuyerAgent", new object[] { agent2Form });

            jade.wrapper.AgentController ag3 =
                CreateAgent(mainContainer, "Buyer3", "AgentAuction.BuyerAgent", new object[] { agent3Form });

            managerAg.start();
            ag1.start();
            ag2.start();
            ag3.start();

        }


        /*
        Create a container:
        
        hostAddress = the IP address of the host
        hostPort = the port through which the host communicates
        localPort = the local port through which agents communicate
         */ 
        private static jade.wrapper.AgentContainer CreateContainer(string containerName, bool isMainContainer, string hostAddress, string hostPort, string localPort)
        {
            ProfileImpl p = new ProfileImpl();

            if (containerName != String.Empty) 
                p.setParameter(Profile.CONTAINER_NAME, containerName);

            p.setParameter(Profile.MAIN, isMainContainer.ToString());

            if (localPort != null)
                p.setParameter(Profile.LOCAL_PORT, localPort);

            if (hostAddress != String.Empty)
                p.setParameter(Profile.MAIN_HOST, hostAddress);

            if (hostPort != String.Empty)
                p.setParameter(Profile.MAIN_PORT, hostPort);

            if (isMainContainer == true)
            {
                return Runtime.instance().createMainContainer(p);
            }
            else
            {
                return Runtime.instance().createAgentContainer(p);
            }
        }

        private static jade.wrapper.AgentController CreateAgent(jade.wrapper.AgentContainer container, string agentName, string agentClass, object[] args)
        {
            return container.createNewAgent(agentName, agentClass, args);
        }
    }
}
