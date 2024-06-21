
import jade.core.AID;
import jade.core.Agent;
import jade.core.behaviours.Behaviour;
import jade.core.behaviours.TickerBehaviour;
import jade.lang.acl.ACLMessage;
import jade.core.behaviours.*;


public class agent1 extends Agent{
	
	
	protected void setup(){
		System.out.println("N "+getAID().getName()+" is ready.");
		addBehaviour(new NBehaviour());
			
	}
	
	private class NBehaviour extends CyclicBehaviour {
		public void action() {
		ACLMessage msg=receive();
		if (msg !=null)
		{
		System.out.println("message is:"+ msg.getContent());
		}
		}
		
		
		}
	}
	

