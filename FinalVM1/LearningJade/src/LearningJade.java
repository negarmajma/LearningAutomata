

import java.io.BufferedReader;
import java.io.Console;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.Scanner;

import jade.core.Agent;
import jade.domain.mobility.MobilityOntology;
import jade.core.behaviours.*;
import jade.core.AID;
import jade.lang.acl.ACLMessage;
import jade.core.PlatformID;


public class LearningJade extends Agent{
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
		ClearData();

		File f_input = new File("c:/1/input.txt");
		File f_executeKdb = new File("c:/1/executeKDB.txt");

		FileInputStream fis_input, fis_executeKdb;
		try {
			fis_input = new FileInputStream(f_input);
			fis_executeKdb = new FileInputStream(f_executeKdb);

			BufferedReader br_input = new BufferedReader(new InputStreamReader(fis_input));
			BufferedReader br_executeKdb = new BufferedReader(new InputStreamReader(fis_executeKdb));

			String line_input = null;
			String line_executeKdb = null;

			/// one time read reward file and dont read from start
			fis_reward = new FileInputStream(f_reward);
			br_reward = new BufferedReader(new InputStreamReader(fis_reward));
			///
			
			while ((line_input = br_input.readLine()) != null) {
				System.out.println("Input: " + line_input);
				
				line_executeKdb = br_executeKdb.readLine();
				String[] input = line_input.split(",");
				double age = Double.parseDouble(input[0]);
				double bmi = Double.parseDouble(input[1]);
				double ac = Double.parseDouble(input[2]);
				double emo = Double.parseDouble(input[3]);
				double bp = Double.parseDouble(input[4]);
				double executeKdb = Double.parseDouble(line_executeKdb);
				 if ( age==0 && bmi==0 && ac==0 && emo==0 && bp==0 )
				 { 
					
						ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
						AID r=new AID("N@192.168.1.2:1099/JADE",AID.ISGUID);
						r.addAddresses("http://WIN-2:7778/acc");
						cfp.addReceiver(r);
						cfp.setContent("Hello superviser agent!All Sensors is out of work!" );
						send(cfp);
						
				 }
				 else
				 {
					 step++;
				RunMatlab(age, bmi, ac, emo, bp, executeKdb);
				writeFile(outputFile, "\r\n");
				//startCalculateReward = true; //start from begining of the reward file
				CalculateReward();
				
				ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
				AID r=new AID("N@192.168.1.2:1099/JADE",AID.ISGUID);
				r.addAddresses("http://WIN-2:7778/acc");
				cfp.addReceiver(r);
				cfp.setContent("Hello superviser agent!All Sensors is work!" );
				send(cfp);
				
				writeFile(outputFile, "\r\n <<<<<<<<<<<<<<>>>>>>>>>>>>>>>>> \r\n");
				WaitForContinue("Press Enter to Continue Read From Input ...");
				 }
			}

			br_input.close();
			br_executeKdb.close();
			
		} catch (Exception e) {
			e.printStackTrace();
			
		}

		WaitForContinue("\r\n End Of Read of Input \r\n Press Enter to Exit ...");
		
		
		
		
		//	ACLMessage cfp=new ACLMessage(ACLMessage.CFP);
		//AID r=new AID("N@192.168.1.2:1099/JADE",AID.ISGUID);
		//r.addAddresses("http://WIN-2:7778/acc");
		//cfp.addReceiver(r);
		//cfp.setContent("Hello agent1!All Sensors is out of work!"  +targetBookTitle);
		//send(cfp);
			
	
		
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

//}







//***********************

//public class LearningJade {

	public String ReadFromInput() {
		ClearData();

		File f_input = new File("c:/1/input.txt");
		File f_executeKdb = new File("c:/1/executeKDB.txt");

		FileInputStream fis_input, fis_executeKdb;
		try {
			fis_input = new FileInputStream(f_input);
			fis_executeKdb = new FileInputStream(f_executeKdb);

			BufferedReader br_input = new BufferedReader(new InputStreamReader(fis_input));
			BufferedReader br_executeKdb = new BufferedReader(new InputStreamReader(fis_executeKdb));

			String line_input = null;
			String line_executeKdb = null;

			/// one time read reward file and dont read from start
			fis_reward = new FileInputStream(f_reward);
			br_reward = new BufferedReader(new InputStreamReader(fis_reward));
			///
			
			while ((line_input = br_input.readLine()) != null) {
				System.out.println("Input: " + line_input);
				
				line_executeKdb = br_executeKdb.readLine();
				String[] input = line_input.split(",");
				double age = Double.parseDouble(input[0]);
				double bmi = Double.parseDouble(input[1]);
				double ac = Double.parseDouble(input[2]);
				double emo = Double.parseDouble(input[3]);
				double bp = Double.parseDouble(input[4]);
				double executeKdb = Double.parseDouble(line_executeKdb);
				 if ( age==0 && bmi==0 && ac==0 && emo==0 && bp==0 )
				 { 
					return "False1";
				 }
				 else
				 {
				RunMatlab(age, bmi, ac, emo, bp, executeKdb);
				writeFile(outputFile, "\r\n");
				//startCalculateReward = true; //start from begining of the reward file
				CalculateReward();
			
				writeFile(outputFile, "\r\n <<<<<<<<<<<<<<>>>>>>>>>>>>>>>>> \r\n");
				WaitForContinue("Press Enter to Continue Read From Input ...");
				 }
			}

			br_input.close();
			br_executeKdb.close();
			
		} catch (Exception e) {
			e.printStackTrace();
			return "False";
		}

		WaitForContinue("\r\n End Of Read of Input \r\n Press Enter to Exit ...");
		return "True";
	}

	private void ClearData() {
		File file = new File("c:/1/output.txt");

		if (file.delete()) {
			System.out.println(file.getName() + " is deleted!");
		} else {
			System.out.println("Delete operation is failed.");
		}
	}

	private void writeFile(String fileName, String text) {
		File fout = new File("c:/1/" + fileName);
		FileOutputStream fos;
		try {
			fos = new FileOutputStream(fout, true);

			OutputStreamWriter osw = new OutputStreamWriter(fos);
			osw.write(text + "\r\n");
			

			osw.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private void WaitForContinue(String message) {
		Scanner in = new Scanner(System.in);
	    System.out.print(message);
	    in.nextLine();
	}

	// public double Reward=0;
	// public double RP1 = 0.3;
	// public double RP2 = 0.6;
	// public double R3 = 0.0;
	// public double RN1 = -0.3;
	// public double RN2 = -0.6;
	// public int t = 1;

	String[] Rules = null;
	String[] RulesConclution = null;
	int maxCounter = 0;
	String outputFile = "output.txt";
	String Input = "";
	Boolean SA1, SA2, SA3, SA4;
	String Find = "False";
	String Counclution = "";
	double PS1, PS2, PS3, PS4 = 0;
	int SNo1, SNo2, SNo3, SNo4 = 0;
	String SelectedStateNo = "0";
	double SelectedStateValue = 0;
	double OutputPulse=0;
	String lblResult = "";
	

	public void RunMatlab(double age, double bmi, double ac, double emo, double bp, double executeKdb) {

		writeFile(outputFile, "System start.......");

		SA1 = true;
		SA2 = true;
		SA3 = true;
		SA4 = true;

		CodingInput(age, bmi, ac, emo, bp);
		ReadRules();
		String ResultFind = SearchRules();
		if (ResultFind != "False") {
			ExecuteKDB(age, bmi, ac, emo, bp); // اتصال به متلب شما یک خروجی ثابت مثل عدد 75 برای آن
							// در نظر بگیرید
			FindNearState(executeKdb, ResultFind);
			writeFile(outputFile, "The rule find and execute");
			writeFile(outputFile, "Waiting for reward....");
			lblResult = "T";
			//button2.Enabled = true;
			//lblLevelNo.Text = "1";

		} else {
			FindNearState(executeKdb, ResultFind);
			SetPulse(OutputPulse);
			//txtOut.Text = OutputPulse.ToString();
			writeFile(outputFile, "The output pulse apply, wait for reward.");
			//button2.Enabled = true;
			lblResult = "F";
			//lblLevelNo.Text = "1";
		}
		
	}

	private void CodingInput(double age, double bmi, double ac, double emo, double bp) {

		// Age
		if (age < 8)
			Input = "1 ";
		else if (age >= 8 && age < 18)
			Input = "2 ";
		else if (age >= 18 && age < 30)
			Input = "3 ";
		else if (age >= 30 && age < 60)
			Input = "4 ";
		else if (age >= 60 && age < 100)
			Input = "5 ";
		else
			Input = "6 ";

		// BMI
		if (bmi < 20)
			Input = Input + "1 ";
		else if (bmi >= 20 && bmi < 27)
			Input = Input + "2 ";
		else if (bmi >= 27 && bmi < 32)
			Input = Input + "3 ";
		else if (bmi >= 32 && bmi < 35)
			Input = Input + "4 ";
		else
			Input = Input + "5 ";

		// AC
		if (ac < 15)
			Input = Input + "1 ";
		else if (ac >= 15 && ac < 25)
			Input = Input + "2 ";
		else if (ac >= 55 && ac < 50)
			Input = Input + "3 ";
		else if (ac >= 50 && ac < 75)
			Input = Input + "4 ";
		else if (ac >= 75 && ac < 95)
			Input = Input + "5 ";
		else if (ac >= 95 && ac < 100)
			Input = Input + "6 ";
		else
			Input = Input + "7 ";

		// Emotion
		if (emo < 25)
			Input = Input + "1 ";
		else if (emo >= 25 && emo < 58)
			Input = Input + "2 ";
		else if (emo >= 58 && emo < 85)
			Input = Input + "3 ";
		else if (emo >= 85 && emo < 100)
			Input = Input + "4 ";
		else
			Input = Input + "5 ";

		// BP
		if (bp < 11.5)
			Input = Input + "1";
		else if (bp >= 11.5 && bp < 14.5)
			Input = Input + "2";
		else if (bp >= 14.5 && bp < 18)
			Input = Input + "3";
		else
			Input = Input + "4";

		// lblRule.Text = lblRule.Text+":"+Input;
		writeFile(outputFile, "Input parameter change to code.");

	}

	private void ReadRules() {
		int counter = 0;
		Rules = new String[300];
		RulesConclution = new String[300];

		// Read the file and display it line by line
		File f = new File("c:/1/Rules.txt");

		FileInputStream fis;
		try {
			fis = new FileInputStream(f);

			BufferedReader br = new BufferedReader(new InputStreamReader(fis));

			String line = null;

			while ((line = br.readLine()) != null) {

				int position = line.lastIndexOf(',');
				String temp = line.substring(0, position);
				Rules[counter] = temp;
				temp = line.substring(position + 2, position + 3);
				RulesConclution[counter] = temp;
				counter++;
			}

			maxCounter = counter;
			br.close();
			writeFile(outputFile, "Rules read successfully.");
			// lblRule.Text = Rules[5];

		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	private String SearchRules() {
		int counter = 0;
		String InputTemp = Input;
		for (counter = 0; counter <= maxCounter - 1; counter++) {
			if (Rules[counter].equals(Input)) {
				Find = "True";
				Counclution = RulesConclution[counter];
			}
		}
		if (Find == "False") {
			InputTemp = Input.substring(0, 8);
			InputTemp = InputTemp + "0";
			// label6.Text = InputTemp;
			for (counter = 0; counter <= maxCounter - 1; counter++) {
				if (Rules[counter].equals(InputTemp)) {
					Find = "True1";
					Counclution = RulesConclution[counter];
				}
			}
		}
		if (Find == "False") {
			InputTemp = Input.substring(0, 6);
			InputTemp = InputTemp + "0 0";
			// label6.Text = InputTemp;
			for (counter = 0; counter <= maxCounter - 1; counter++) {
				if (Rules[counter].equals(InputTemp)) {
					Find = "True2";
					Counclution = RulesConclution[counter];
				}
			}
		}
		if (Find == "False") {
			InputTemp = Input.substring(0, 4);
			InputTemp = InputTemp + "0 0 0";
			// label6.Text = InputTemp;
			for (counter = 0; counter <= maxCounter - 1; counter++) {
				if (Rules[counter].equals(InputTemp)) {
					Find = "True3";
					Counclution = RulesConclution[counter];
				}
			}
		}
		if (Find == "False") {
			InputTemp = Input.substring(0, 2);
			InputTemp = InputTemp + "0 0 0 0";
			// label6.Text = InputTemp;
			for (counter = 0; counter <= maxCounter - 1; counter++) {
				if (Rules[counter].equals(InputTemp)) {
					Find = "True4";
					Counclution = RulesConclution[counter];
				}
			}
		}
		// Counclution = Input.Substring(8, 1);
		writeFile(outputFile, "Rules search for input parameters. the result found:");
		writeFile(outputFile, Find);
		System.out.println("Rules search for input parameters. the result found:" +Find +"-"+ Counclution);
		lblResult = Find;
		return Find;
	}
	
	private void ExecuteKDB(double age, double bmi, double ac, double emo, double bp)
    {

        String InputParam = "" + age + " " + bmi + " " + ac + " " + emo + " " + bp;

         //matlab.Execute("a = readfis('c:\\pacemaker3');");
        //matlab.Execute("b=evalfis([" + InputParam + "], a);");
        //txtOut.Text = matlab.Execute("b");
        //int l = txtOut.Text.LastIndexOf("=");
        //txtOut.Text = txtOut.Text.Substring(l + 6, 5);

        writeFile(outputFile, "Founded rule execute.");
    }
	
	private void FindNearState(double pt, String ResultFind)
    {
        double temp1, temp2, temp3 = 0;
        //double pt = double.Parse(txtOut.Text);

        int S1L = 20;
        int S1U = 35;
        int S2L = 30;
        int S2U = 50;
        int S3L = 55;
        int S3U = 100;
        int S4L = 95;
        int S4U = 150;

        //S1
        temp1 = Math.abs(S1L - pt);
        temp2 = Math.abs(S1U - pt);
        if (temp1 < temp2)
            temp3 = 100 - temp1;
        else
            temp3 = 100 - temp2;
        PS1 = temp3;
        SNo1 = 1;
        //S2
        temp1 = Math.abs(S2L - pt);
        temp2 = Math.abs(S2U - pt);
        if (temp1 < temp2)
            temp3 = 100 - temp1;
        else
            temp3 = 100 - temp2;
        PS2 = temp3;
        SNo2 = 2;
        //S3
        temp1 = Math.abs(S3L - pt);
        temp2 = Math.abs(S3U - pt);
        if (temp1 < temp2)
            temp3 = 100 - temp1;
        else
            temp3 = 100 - temp2;
        PS3 = temp3;
        SNo3 = 3;
        //S4
        temp1 = Math.abs(S4L - pt);
        temp2 = Math.abs(S4U - pt);
        if (temp1 < temp2)
            temp3 = 100 - temp1;
        else
            temp3 = 100 - temp2;
        PS4 = temp3;
        SNo4 = 4;

        //If rule find
        if (lblResult.substring(0, 1) == "T")
        //if (ResultFind.substring(0, 1) == "T")
        {
            if (Counclution == "1")
            {
                PS1 = 100;
            }
            else if (Counclution == "2")
            { PS2 = 100; }
            else if (Counclution == "3")
            { PS3 = 100; }
            else if (Counclution == "4")
            { PS4 = 100; }
        }

        //sort
        if (PS2 > PS1)
        {
            temp3 = PS1;
            PS1 = PS2;
            PS2 = temp3;
            SNo1 = 2;
            SNo2 = 1;
        }
        if (PS3 > PS1)
        {
            temp3 = PS1;
            PS1 = PS3;
            PS3 = temp3;
            SNo1 = 3;
            SNo3 = 1;
        }
        if (PS4 > PS1)
        {
            temp3 = PS1;
            PS1 = PS4;
            PS4 = temp3;
            SNo1 = 4;
            SNo4 = 1;
        }
        if (PS3 > PS2)
        {
            temp3 = PS2;
            PS2 = PS3;
            PS3 = temp3;
            SNo2 = 3;
            SNo3 = 2;
        }
        if (PS4 > PS2)
        {
            temp3 = PS2;
            PS2 = PS4;
            PS4 = temp3;
            SNo2 = 4;
            SNo4 = 2;
        }
        if (PS4 > PS3)
        {
            temp3 = PS3;
            PS3 = PS4;
            PS4 = temp3;
            SNo3 = 4;
            SNo4 = 3;
        }

        SelectedStateValue = PS1;
        SelectedStateNo = "" + SNo1;
        SA1 = false;

        //lblNearestState.Text = SelectedStateValue + "-" + SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;
        writeFile(outputFile, "Near state is:");
        writeFile(outputFile, SelectedStateNo + "//" + SelectedStateValue);

        switch (SelectedStateNo)
        {
            case "1":

                OutputPulse = 27.5;
                break;
            case "2":

                OutputPulse = 40;
                break;
            case "3":

                OutputPulse = 77.5;
                break;
            case "4":

                OutputPulse = 122.5;
                break;
        }

        //lblOutputPulse.Text = OutputPulse.ToString();
        writeFile(outputFile, "Output pulse set to:");
        writeFile(outputFile, "" + OutputPulse);
    }
	
	private void SetPulse(double OutputPulse)
    {
        //run pacemaker with outputpulse 
        for (int i = 0; i <= 1000000; i++) ;

    }
	
	double ox_, temp_, carb_;
	File f_reward = new File("c:/1/reward.txt");
	FileInputStream fis_reward = null;
	BufferedReader br_reward = null;
	private void ReadNextReward() {

		try {
			if(startCalculateReward){				
				fis_reward = new FileInputStream(f_reward);
				br_reward = new BufferedReader(new InputStreamReader(fis_reward));
				
				startCalculateReward = false;
			}
			

			WaitForContinue("Press Enter to Continue Read From Reward ...");

			String line = null;
			if((line = br_reward.readLine()) == null) {				
				System.exit(0);
			}
			else {
				System.out.println("Reward: " + line);
				
				String[] input = line.split(",");
				ox_ = Double.parseDouble(input[0]);
				temp_ = Double.parseDouble(input[1]);
				carb_ = Double.parseDouble(input[2]);
			}
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	Boolean startCalculateReward = true;
	int tryNumber = 0;
	boolean try1 = false, try2 = false, try3 = false;
	double q1, q2, q3, q4 = 0;
	double reward1, reward2, reward3;
	
	private double CalculateReward()
    {
		tryNumber = 0;
		try1 = false;
		try2 = false;
		try3 = false;
		//try1
		double RewardAmount =-0.1;
		while (RewardAmount<0)
		{
		
         RewardAmount = CalculateRewardNew();}
		reward1=RewardAmount;
		System.out.println("Reward calculated:"+reward1);
        writeFile(outputFile, "Reward calculated");
        writeFile(outputFile, "Reward is:" +reward1);
        writeFile(outputFile, "Reward is possitive then try it for 2 times.");
        System.out.println("Reward is possitive then try it for 2 times.");
		
		//try2
		 RewardAmount =-0.1;
		while (RewardAmount<0)
		{
			
         RewardAmount = CalculateRewardNew();
         
		reward2=RewardAmount;
		System.out.println("Reward calculated:"+reward2);
        writeFile(outputFile, "Reward calculated");
        writeFile(outputFile, "Reward is:" + reward2);}
        writeFile(outputFile, "Reward is possitive then try it for 3 times.");
        System.out.println("Reward is possitive then try it for 3 times.");
		
       //try3
		 RewardAmount =-0.1;
		while (RewardAmount<0)
		{
			
         RewardAmount = CalculateRewardNew();}
		reward3=RewardAmount;
        System.out.println("Reward calculated:"+reward3);
        writeFile(outputFile, "Reward calculated");
        writeFile(outputFile, "Reward is:" + reward3);

        AddNewRule();
        
        switch (SelectedStateNo)
        {
            case "1":
                q1 = q1 + reward1+reward2+reward3;
                break;
            case "2":
                q2 = q2 + reward1+reward2+reward3;
                break;
            case "3":
                q3 = q3 + reward1+reward2+reward3;
                break;
            case "4":
                q4 = q4 + reward1+reward2+reward3;
                break;
        }
        
        return RewardAmount;
    }
	
	double CalculateRewardNew()
    {
		ReadNextReward();
		
        String OX = "";
        String TE = "";
        String CA = "";
        double RewardAmount = 0;
        //Oxigen in the blood
        if (ox_ < 40)
            OX = "H";
        else if (ox_ >=40 && ox_ < 60)
            OX = "M";
        else if (ox_ >=60 && ox_ < 80)
            OX = "L";
        else if (ox_ >=80)
            OX = "N";
        
        //Body tempreture
        if (temp_ > 37.2)
            TE = "H";
        else if (temp_ >= 36.5 && temp_ <= 37.2)
            TE = "N";
        else if (temp_ < 36.5)
            TE = "L";
        
        //blood carbon dioxide
        if (carb_ > 45)
            CA = "H";
        else if (carb_ >= 35 && carb_ <= 45)
            CA = "N";
        else if (carb_ < 35)
            CA = "L";

        System.out.println(OX +"--"+TE+"--"+CA);
        //reward set
        if (OX == "N" && TE == "N" && CA == "N")
            RewardAmount = 1;
        else
        {
        switch (OX)
            {
            case "H":
            {
            	RewardAmount = -1; break;
            }
            case "N":
                    {
                        switch (TE)
                        {
                            case "L":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = 0.1; break; }
                                        case "N":
                                            { RewardAmount = 0.64; break; }
                                        case "H":
                                            { RewardAmount = 0.19; break; }

                                    }
                                    break;
                                }
                            case "N":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = 0.46; break; }
                                        case "N":
                                            { RewardAmount = 1; break; }
                                        case "H":
                                            { RewardAmount = 0.55; break; }
                                    }
                                    break;
                                }
                            case "H":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = 0.64; break; }
                                        case "N":
                                            { RewardAmount = 0.7; break; }
                                        case "H":
                                            { RewardAmount = 0.25; break; }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "L":
                    {
                        switch (CA)
                        {
                            case "L":
                                { RewardAmount = -0.39; break; }
                            case "N":
                                { RewardAmount = 0.15; break; }
                            case "H":
                                { RewardAmount = -0.3; break; }
                        }
                        break;
                    }

                case "M":
                    {
                        switch (TE)
                        {
                            case "L":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = -0.65; break; }
                                        case "N":
                                            { RewardAmount = -0.11; break; }
                                        case "H":
                                            { RewardAmount = -0.56; break; }
                                    }
                                    break;
                                }
                            case "N":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = -0.29; break; }
                                        case "N":
                                            { RewardAmount = 0.25; break; }
                                        case "H":
                                            { RewardAmount = -0.2; break; }
                                    }
                                    break;
                                }
                            case "H":
                                {
                                    switch (CA)
                                    {
                                        case "L":
                                            { RewardAmount = -0.59; break; }
                                        case "N":
                                            { RewardAmount = 0.05; break; }
                                        case "H":
                                            { RewardAmount = -0.5; break; }
                                    }
                                    break;
                                }
                        }
                        break;
                    }


            }
        }
        return RewardAmount;

    }
	
	void FindOtherState()
    {
        if (SA2 == true)
        {
            SelectedStateValue = PS2;
            SelectedStateNo = "" + SNo2;
            SA2 = false;
        }
        else if (SA3 == true)
        {
            SelectedStateValue = PS3;
            SelectedStateNo = "" + SNo3;
            SA3 = false;
        }
        else if (SA4 == true)
        {
            SelectedStateValue = PS4;
            SelectedStateNo = "" + SNo4;
            SA4 = false;
        }
        else
        {
            //end of state
        }

        ////if (SA1==false)
        ////{      
        ////        SelectedStateValue = PS2;
        ////        SelectedStateNo = "2";
        ////        if (PS3 > SelectedStateValue)
        ////        {
        ////            SelectedStateValue = PS3;
        ////            SelectedStateNo = "3";
        ////        }
        ////        if (PS4 > SelectedStateValue)
        ////        {
        ////            SelectedStateValue = PS4;
        ////            SelectedStateNo = "4";
        ////        }
        ////}
        ////if (SA2 == false)
        ////{
        ////    SelectedStateValue = PS1;
        ////    SelectedStateNo = "1";
        ////    if (PS3 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS3;
        ////        SelectedStateNo = "3";
        ////    }
        ////    if (PS4 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS4;
        ////        SelectedStateNo = "4";
        ////    }
        ////}
        ////if (SA3 == false)
        ////{
        ////    SelectedStateValue = PS1;
        ////    SelectedStateNo = "1";
        ////    if (PS2 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS2;
        ////        SelectedStateNo = "2";
        ////    }
        ////    if (PS4 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS4;
        ////        SelectedStateNo = "4";
        ////    }
        ////}
        ////if (SA4 == false)
        ////{
        ////    SelectedStateValue = PS1;
        ////    SelectedStateNo = "1";
        ////    if (PS2 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS2;
        ////        SelectedStateNo = "2";
        ////    }
        ////    if (PS3 > SelectedStateValue)
        ////    {
        ////        SelectedStateValue = PS3;
        ////        SelectedStateNo = "3";
        ////    }
        ////}


        //lblNearestState.Text = SelectedStateValue + "-" + SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;
        writeFile(outputFile, "Next near state is:");
        writeFile(outputFile, SelectedStateNo + "//" + SelectedStateValue);

        switch (SelectedStateNo)
        {
            case "1":

                OutputPulse = 27.5;
                break;
            case "2":

                OutputPulse = 40;
                break;
            case "3":

                OutputPulse = 77.5;
                break;
            case "4":

                OutputPulse = 122.5;
                break;
        }


        //lblOutputPulse.Text = OutputPulse.ToString();
        writeFile(outputFile, "Next output pulse set to:");
        writeFile(outputFile, "" + OutputPulse);
        //txtOut.Text = OutputPulse.ToString();
    }
	
	private void Try1()
    {
        double RewardAmount = CalculateRewardNew();  //CalculateReward();

        writeFile(outputFile, "Reward calculated for second time");
        writeFile(outputFile, "Reward is:" + RewardAmount);

        reward2 = RewardAmount;
        if (RewardAmount >= 0)
        {
            if (tryNumber == 1)
            {
            	writeFile(outputFile, "Reward is possitive for the second time then try it for one time more.");
            	writeFile(outputFile, "Wait for try 2 Reward");
                try2 = true;
                tryNumber += 1;
                
                Try2();
            }
        }
        else
        {
        	writeFile(outputFile, "Reward is negative select another state.");
            //FindOtherState();
        	Try1();
        }

    }
	
	private void Try2()
    {
        double RewardAmount = CalculateRewardNew(); //CalculateReward();

        writeFile(outputFile, "Reward calculated for third time");
        writeFile(outputFile, "Reward is:" + RewardAmount);

        reward3 = RewardAmount;
        if (RewardAmount >= 0)
        {
            if (tryNumber == 2)
            {
                if (lblResult == "F")
                {
                	writeFile(outputFile, "Reward is possitive for the third time then add it to the new rules KDB.");
                	writeFile(outputFile, "Rule adds to KDB");
                    //Add rule                       
                    AddNewRule();
                    try2 = true;
                }
                else
                {
                	writeFile(outputFile, "Reward is possitive for the third time then change the CF of the rule in KDB.");
                	writeFile(outputFile, "Rule changes in KDB");
                    //Add rule                       
                    AddNewRule();
                    try2 = true;
                }

            }
        }
        else
        {
        	writeFile(outputFile, "Reward is negative select another state.");
            //FindOtherState();
        	Try2();
        }

    }
	
	void AddNewRule()
    {
        double sumReward = reward1 + reward2 + reward3;
        sumReward = Math.round(sumReward);
        double CF = 0;
        if (sumReward >= 1.8)
            CF = 1;
        else if (sumReward >= 1.5)
            CF = 0.8;
        else if (sumReward >= 1.2)
            CF = 0.5;
        else if (sumReward >= 0.9)
            CF = 0.3;
        else if (sumReward >= 0.5)
            CF = 0.2;

        String NewRule = Input + ", " + SelectedStateNo + " (" + CF + ") : 1";
        //  System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\1\\Rules2.txt");
        //  file.WriteLine(NewRule);
        writeFile("RulesLearn.txt", NewRule);
        writeFile(outputFile, "Rule is:" + NewRule);
        System.out.println("Rule is:" + NewRule);
        //    file.Close();


    }
}

