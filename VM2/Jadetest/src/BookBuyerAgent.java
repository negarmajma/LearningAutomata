
import jade.core.Agent;
import jade.core.behaviours.*;
import jade.core.AID;
import jade.lang.acl.ACLMessage;

public class BookBuyerAgent extends Agent{
private String targetBookTitle;
private AID[] sellerAgents={new AID("SellerNegar1",AID.ISLOCALNAME),
							new AID("SellerNegar2",AID.ISLOCALNAME)};


protected void setup(){
	System.out.println("Hallo! Buyer-agent "+getAID().getName()+" is ready.");
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
	System.out.println("Buyer-agent "+ getAID().getName()+" terminating.");
	
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
		ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
		cfp.addReceiver(new AID ("N@192.168.1.2:1099/JADE",AID.ISGUID));
		cfp.setContent(targetBookTitle);
		myAgent.send(cfp);
		
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
