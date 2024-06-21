

import jade.core.Agent;
import jade.domain.mobility.MobilityOntology;
import jade.core.behaviours.*;
import jade.core.AID;
import jade.lang.acl.ACLMessage;
import jade.core.PlatformID;

public class BookBuyerAgent extends Agent{
private String targetBookTitle;
private AID[] sellerAgents={new AID("SellerNegar1",AID.ISLOCALNAME),
							new AID("SellerNegar2",AID.ISLOCALNAME)};


protected void setup(){
	System.out.println("Hallo! Agent on the pacemaker "+getAID().getName()+" is ready.");
	Object[] args=getArguments();
	if (args !=null && args.length>0){
		targetBookTitle=(String)args[0];
		System.out.println("Trying to buy"+targetBookTitle);
		addBehaviour(new TickerBehaviour(this,10000){
			protected void onTick() {
				myAgent.addBehaviour(new RequestPerformer());
			}
		});
	}
	else{
		System.out.println("No book title specified");
		doDelete();
	}
}
protected void takeDowm(){
	System.out.println("Agent on the pacemake "+ getAID().getName()+" terminating.");
	
}
private class RequestPerformer extends Behaviour {
	private int step = 0;
	
	public void action() {
		System.out.println("Action Behaviour Start");
		step++;
		//ACLMessage msg=new ACLMessage(ACLMessage.INFORM);
		//msg.addReceiver(new AID ("N",AID.ISLOCALNAME));
		//msg.setLanguage("English");
		//msg.setOntology("weather");
		//msg.setContent("raining");
		//send(msg);
		
		//Run OK
		//ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
		//cfp.addReceiver(new AID ("N",AID.ISLOCALNAME));
		//cfp.setContent(targetBookTitle+"eee");
		//myAgent.send(cfp);
		
			
		
		//Learning learning = new Learning();
		//String Out=learning.ReadFromInput();
		
		//if (Out=="False1")
		//{
			ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
		AID r=new AID("N@192.168.1.2:1099/JADE",AID.ISGUID);
		r.addAddresses("http://WIN-2:7778/acc");
		cfp.addReceiver(r);
		cfp.setContent("Hello agent1!All Sensors is out of work!"  +targetBookTitle);
		send(cfp);
			
		//}
	//	AID remoteAMS = new AID("ams@192.168.1.2", AID.ISGUID);
     //   remoteAMS.addAddresses("http://19.168.1.1:7778/acc");
     //   PlatformID destination = new PlatformID(remoteAMS);
      //  agent.doMove(destination);
        
		
		
	}

	public boolean done() {
		
		if (step!=4)
		{
			System.out.println("Try again");
		    return false;}
		else
		{System.out.println("actiondown");
		 return true;}
			}
}  // End of inner class RequestPerformer

}
