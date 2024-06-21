using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;


namespace LearningAlgorithm
{
  
    public partial class Form1 : Form
    {
        public string[] Rules= new string[300];
        public string[] RulesConclution = new string[300];

       public string Input = "";
       public string Find = "False";
       public string SelectedStateNo = "0";
       public double SelectedStateValue = 0;
        public double OutputPulse=0;
       public double q1, q2, q3, q4 = 0;
        public  double PS1, PS2, PS3, PS4 = 0;
        public Boolean SA1, SA2, SA3, SA4 ;
        public int SNo1, SNo2, SNo3, SNo4 = 0;
        public double Reward=0;
        public double RP1 = 0.3;
        public double RP2 = 0.6;
        public double R3 = 0.0;
        public double RN1 = -0.3;
        public double RN2 = -0.6;
        public int t = 1;
        public string Counclution = "";

        MLApp.MLApp matlab = new MLApp.MLApp();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListOutput.Items.Add("System start.......");
          
            SA1 = true;
            SA2 = true;
            SA3 = true;
            SA4 = true;

           CodingInput();
           ReadRules();
          string ResultFind= SearchRules();
          if (ResultFind != "False")
           {
                ExecuteKDB();  //اتصال به متلب شما یک خروجی ثابت مثل عدد 75 برای آن در نظر بگیرید 
                FindNearState();
                ListOutput.Items.Add( "The rule find and execute");
                ListOutput.Items.Add("Waiting for reward....");
                lblResult.Text = "T";
                button2.Enabled = true;
                lblLevelNo.Text = "1";

           }
           else
           {
               FindNearState();
               SetPulse(OutputPulse);
               txtOut.Text = OutputPulse.ToString();
               ListOutput.Items.Add("The output pulse apply, wait for reward.");
               button2.Enabled = true;
               lblResult.Text = "F";              
              lblLevelNo.Text = "1";

       

           }
        }

        void AddNewRule()
        {
            double sumReward = double.Parse(txtReward1.Text) + double.Parse(txtReward2.Text) + double.Parse(txtReward3.Text);
           sumReward= Math.Round(sumReward, 1);
            double CF=0;
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

            string NewRule = Input + ", " + SelectedStateNo + " (" + CF + ") : 1";
          //  System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\1\\Rules2.txt");
          //  file.WriteLine(NewRule);
            using (StreamWriter w = File.AppendText("c:\\1\\RulesLearn.txt"))
            {
                w.WriteLine(NewRule);
            }
            ListOutput.Items.Add("This rule added to KDB:" + NewRule);
        //    file.Close();
            
          
        }
        void FindNextNearState()
        {
            if (SA1 == false && SA2==true && SA3==true && SA4==true)//state1 ازدور خارح می شود
                 {   
              
                    SelectedStateValue = PS2;
                    SelectedStateNo = "2";
                    if (PS3 > SelectedStateValue)
                    {
                        SelectedStateValue = PS3;
                        SelectedStateNo = "3";
                    }
                    if (PS4 > SelectedStateValue)
                    {
                        SelectedStateValue = PS4;
                        SelectedStateNo = "4";
                    }

            }
            else if (SA1 == true && SA2 == false && SA3 == true && SA4 == true) //state2 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS3 > SelectedStateValue)
                {
                    SelectedStateValue = PS3;
                    SelectedStateNo = "3";
                }
                if (PS4 > SelectedStateValue)
                {
                    SelectedStateValue = PS4;
                    SelectedStateNo = "4";
                }

            }
            else if (SA1 == true && SA2 == true && SA3 == false && SA4 == true) //state3 ازدور خارح می شود
            {
                    SelectedStateValue = PS1;
                    SelectedStateNo = "1";
                    if (PS2 > SelectedStateValue)
                    {
                        SelectedStateValue = PS2;
                        SelectedStateNo = "2";
                    }
                    if (PS4 > SelectedStateValue)
                    {
                        SelectedStateValue = PS4;
                        SelectedStateNo = "4";
                    }

            }
            else if (SA1 == true && SA2 == true && SA3 == true && SA4 == false) //state4 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS2 > SelectedStateValue)
                {
                    SelectedStateValue = PS2;
                    SelectedStateNo = "2";
                }
                if (PS3 > SelectedStateValue)
                {
                    SelectedStateValue = PS3;
                    SelectedStateNo = "3";
                }
            }
            else if (SA1 == false && SA2 == false && SA3 == true && SA4 == true) //state1 , 2 ازدور خارح می شود
            {
                SelectedStateValue = PS3;
                SelectedStateNo = "3";
                if (PS4 > SelectedStateValue)
                {
                    SelectedStateValue = PS4;
                    SelectedStateNo = "4";
                }
            }
            else if (SA1 == false && SA2 == true && SA3 == false && SA4 == true) //state1 , 3 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS4 > SelectedStateValue)
                {
                    SelectedStateValue = PS4;
                    SelectedStateNo = "4";
                }
            }
            else if (SA1 == false && SA2 == true && SA3 == true && SA4 == false) //state1 , 4 ازدور خارح می شود
            {
                SelectedStateValue = PS2;
                SelectedStateNo = "2";
                if (PS3 > SelectedStateValue)
                {
                    SelectedStateValue = PS3;
                    SelectedStateNo = "3";
                }
            }
            else if (SA1 == true && SA2 == false && SA3 == false && SA4 == true) //state 2 , 3 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS4 > SelectedStateValue)
                {
                    SelectedStateValue = PS4;
                    SelectedStateNo = "4";
                }
            }
            else if (SA1 == true && SA2 == false && SA3 == true && SA4 == false) //state2 , 4 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS3 > SelectedStateValue)
                {
                    SelectedStateValue = PS3;
                    SelectedStateNo = "3";
                }
            }
            else if (SA1 == true && SA2 == true && SA3 == false && SA4 == false) //state1 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
                if (PS2 > SelectedStateValue)
                {
                    SelectedStateValue = PS2;
                    SelectedStateNo = "2";
                }
            }

            else if (SA1 == false && SA2 == false && SA3 == false && SA4 == true) //state1,2,3 ازدور خارح می شود
            {
                SelectedStateValue = PS4;
                SelectedStateNo = "4";
            }
            else if (SA1 == false && SA2 == false && SA3 == true && SA4 == false) //state1,2,4 ازدور خارح می شود
            {
                SelectedStateValue = PS3;
                SelectedStateNo = "3";
            }
            else if (SA1 == false && SA2 == true && SA3 == false && SA4 == false) //state1,3,4 ازدور خارح می شود
            {
                SelectedStateValue = PS2;
                SelectedStateNo = "2";
            }
            else if (SA1 == true && SA2 == false && SA3 == false && SA4 == false) //state1,2,3 ازدور خارح می شود
            {
                SelectedStateValue = PS1;
                SelectedStateNo = "1";
            }

        lblNearestState.Text = SelectedStateValue + "-" + SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;

            switch (SelectedStateNo)
            {
                case "1":
                    SA1 = false;
                    OutputPulse = 27.5;
                    break;
                case "2":
                    SA2 = false;
                    OutputPulse = 40;
                    break;
                case "3":
                    SA3 = false;
                    OutputPulse = 77.5;
                    break;
                case "4":
                    SA4 = false;
                    OutputPulse = 122.5;
                    break;
            }

        }

          void FindNearStateFCPN()
        {
            double temp1, temp2, temp3 = 0;
            double pt=int.Parse(txtOut.Text);

            int S1L=20;
            int S1U=35;
            int S2L = 30;
            int S2U = 50;
            int S3L = 55;
            int S3U = 100;
            int S4L = 95;
            int S4U = 150;
 
           

            //S1
            temp1 =Math.Abs(S1L - pt);
            temp2 = Math.Abs(S1U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS1 = temp3;
            SNo1 = 1;
            //S2
            temp1 = Math.Abs(S2L - pt);
            temp2 = Math.Abs(S2U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS2 = temp3;
            SNo2 = 2;
            //S3
            temp1 = Math.Abs(S3L - pt);
            temp2 = Math.Abs(S3U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS3 = temp3;
            SNo3 = 3;
            //S4
            temp1 = Math.Abs(S4L - pt);
            temp2 = Math.Abs(S4U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS4 = temp3;
            SNo4 = 4;
            
            //If rule find
               if (OutFCPN.Text=="Low")
                 { PS1 = 100; }
            else if (OutFCPN.Text=="Medium")
                { PS2 = 100; }
            else if (OutFCPN.Text=="Normal")
                { PS3 = 100; }
            else if (OutFCPN.Text=="High")
               { PS4 = 100; }

                


            //sort
            
            if (PS2>PS1)
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

            SelectedStateValue=PS1;
            SelectedStateNo=SNo1.ToString();
            SA1 = false;

            lblNearestState.Text = SelectedStateValue +"-"+ SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;
            ListOutput.Items.Add("Near state is:");
            ListOutput.Items.Add(SelectedStateNo + "//" + SelectedStateValue);

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

           lblOutputPulse.Text = OutputPulse.ToString();
           ListOutput.Items.Add("Output pulse set to:");
           ListOutput.Items.Add(OutputPulse.ToString());


        }

        void FindNearState()
        {
            double temp1, temp2, temp3 = 0;
            double pt=double.Parse(txtOut.Text);

            int S1L=20;
            int S1U=35;
            int S2L = 30;
            int S2U = 50;
            int S3L = 55;
            int S3U = 100;
            int S4L = 95;
            int S4U = 150;
 
           

            //S1
            temp1 =Math.Abs(S1L - pt);
            temp2 = Math.Abs(S1U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS1 = temp3;
            SNo1 = 1;
            //S2
            temp1 = Math.Abs(S2L - pt);
            temp2 = Math.Abs(S2U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS2 = temp3;
            SNo2 = 2;
            //S3
            temp1 = Math.Abs(S3L - pt);
            temp2 = Math.Abs(S3U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS3 = temp3;
            SNo3 = 3;
            //S4
            temp1 = Math.Abs(S4L - pt);
            temp2 = Math.Abs(S4U - pt);
            if (temp1 < temp2)
                temp3 = 100 - temp1;
            else
                temp3 = 100 - temp2;
            PS4 = temp3;
            SNo4 = 4;
            
            //If rule find
            if (lblResult.Text.Substring(0,1)=="T")
            {


                if (Counclution=="1")
                { PS1 = 100; 
                }
                else  if (Counclution=="2")
                { PS2 = 100; }
                else if (Counclution == "3")
                { PS3 = 100; }
                else if (Counclution == "4")
                { PS4 = 100; }
                           
                
                }


            //sort
            
            if (PS2>PS1)
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

            SelectedStateValue=PS1;
            SelectedStateNo=SNo1.ToString();
            SA1 = false;

            lblNearestState.Text = SelectedStateValue +"-"+ SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;
            ListOutput.Items.Add("Near state is:");
            ListOutput.Items.Add(SelectedStateNo + "//" + SelectedStateValue);

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

           lblOutputPulse.Text = OutputPulse.ToString();
           ListOutput.Items.Add("Output pulse set to:");
           ListOutput.Items.Add(OutputPulse.ToString());


        }
        void CodingInput()
        {
          
//Age
            if (double.Parse(txtAge.Text) <8)
               Input = "1 ";
            else if (double.Parse(txtAge.Text)>=8  && double.Parse(txtAge.Text) < 18)
               Input = "2 ";
            else if (double.Parse(txtAge.Text) >= 18 && double.Parse(txtAge.Text) < 30)
                Input = "3 ";
            else if (double.Parse(txtAge.Text) >= 30 && double.Parse(txtAge.Text) < 60)
                Input = "4 ";
            else if (double.Parse(txtAge.Text) >= 60 && double.Parse(txtAge.Text) < 100)
                Input = "5 ";
            else
                Input = "6 ";
          
            //BMI
            if ( double.Parse(txtBMI.Text) < 20)
                Input = Input + "1 ";
            else if (double.Parse(txtBMI.Text) >= 20 && double.Parse(txtBMI.Text) < 27)
                Input = Input + "2 ";
            else if (double.Parse(txtBMI.Text) >= 27 && double.Parse(txtBMI.Text) < 32)
                Input = Input + "3 ";
            else if (double.Parse(txtBMI.Text) >= 32 && double.Parse(txtBMI.Text) < 35)
                Input = Input + "4 ";
            else
                Input = Input + "5 ";

            //AC
             if ( double.Parse(txtAC.Text) < 15)
                 Input = Input + "1 ";
            else if (double.Parse(txtAC.Text) >= 15 && double.Parse(txtAC.Text) < 25)
                 Input = Input + "2 ";
             else if (double.Parse(txtAC.Text) >= 55 && double.Parse(txtAC.Text) < 50)
                 Input = Input + "3 ";
             else if (double.Parse(txtAC.Text) >= 50 && double.Parse(txtAC.Text) < 75)
                 Input = Input + "4 ";
             else if (double.Parse(txtAC.Text) >= 75 && double.Parse(txtAC.Text) < 95)
                 Input = Input + "5 ";
             else if (double.Parse(txtAC.Text) >= 95 && double.Parse(txtAC.Text) < 100)
                 Input = Input + "6 ";
             else
                 Input = Input + "7 ";

            //Emotion
             if (double.Parse(txtEmo.Text) < 25)
                 Input = Input + "1 ";
             else if (double.Parse(txtEmo.Text) >= 25 && double.Parse(txtEmo.Text) < 58)
                 Input = Input + "2 ";
             else if (double.Parse(txtEmo.Text) >= 58 && double.Parse(txtEmo.Text) < 85)
                 Input = Input + "3 ";
             else if (double.Parse(txtEmo.Text) >= 85 && double.Parse(txtEmo.Text) < 100)
                 Input = Input + "4 ";
             else
                 Input = Input + "5 ";


            //BP
             if (double.Parse(txtBP.Text) < 11.5)
                 Input = Input + "1";
             else if (double.Parse(txtBP.Text) >= 11.5 && double.Parse(txtBP.Text) < 14.5)
                 Input = Input + "2";
             else if (double.Parse(txtBP.Text) >= 14.5 && double.Parse(txtBP.Text) < 18)
                 Input = Input + "3";
             else
                 Input = Input + "4";


             lblRule.Text = lblRule.Text+":"+Input;
             ListOutput.Items.Add("Input parameter change to code.");

        }
        void ReadRules()
        {
            int counter = 0;
            string line;
            

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("c:\\1\\Rules.txt");
            while ((line = file.ReadLine()) != null)
            {
                int position = line.LastIndexOf(',');
                Rules[counter] = line.Substring(0,position);
                RulesConclution[counter] = line.Substring(position+2,1);
               counter++;
            }
            file.Close();
            ListOutput.Items.Add("Rules read successfully.");
         //   lblRule.Text = Rules[5];

        }
        string SearchRules()
        {
            int counter = 0;
            string InputTemp = Input;
            for (counter = 0; counter <= Rules.Count() - 1; counter++)
            {
                if (Rules[counter] == Input)
                {
                    Find = "True";
                    Counclution = RulesConclution[counter];
                }
            }
            if (Find=="False")
            {
                 InputTemp = Input.Substring(0, 8);
                 InputTemp = InputTemp + "0";
                 label6.Text = InputTemp;
                for (counter = 0; counter <= Rules.Count() - 1; counter++)
                {
                    if (Rules[counter] == InputTemp)
                    {
                        Find = "True1";
                        Counclution = RulesConclution[counter];
                    }
                }
            }
            if (Find == "False")
            {
                InputTemp = Input.Substring(0, 6);
                InputTemp = InputTemp + "0 0";
                label6.Text = InputTemp;
                for (counter = 0; counter <= Rules.Count() - 1; counter++)
                {
                    if (Rules[counter] == InputTemp)
                    {
                        Find = "True2";
                        Counclution = RulesConclution[counter];
                    }
                }
            }
            if (Find == "False")
            {
                InputTemp = Input.Substring(0, 4);
                InputTemp = InputTemp + "0 0 0";
                label6.Text = InputTemp;
                for (counter = 0; counter <= Rules.Count() - 1; counter++)
                {
                    if (Rules[counter] == InputTemp)
                    {
                        Find = "True3";
                        Counclution = RulesConclution[counter];
                    }
                }
            }
            if (Find == "False")
            {
                InputTemp = Input.Substring(0, 2);
                InputTemp = InputTemp + "0 0 0 0";
                label6.Text = InputTemp;
                for (counter = 0; counter <= Rules.Count() - 1; counter++)
                {
                    if (Rules[counter] == InputTemp)
                    {
                        Find = "True4";
                        Counclution = RulesConclution[counter];
                    }
                }
            }
           //Counclution = Input.Substring(8, 1);
            ListOutput.Items.Add("Rules search for input parameters. the result found:");
            ListOutput.Items.Add(Find);
            lblResult.Text =  Find;
            return Find;
        }
      void  ExecuteKDB()
        {

  string InputParam = txtAge.Text + " " + txtBMI.Text + " " + txtAC.Text + " " + txtEmo.Text + " " + txtBP.Text;

             matlab.Execute("a = readfis('c:\\pacemaker3');");
            matlab.Execute("b=evalfis(["+ InputParam +"], a);");
            txtOut.Text = matlab.Execute("b");
            int l = txtOut.Text.LastIndexOf("=");
            txtOut.Text = txtOut.Text.Substring(l+6,5);
            
          ListOutput.Items.Add("Founded rule execute.");
        }
        void  SetPulse(double OutputPulse)
    {
                        //run pacemaker with outputpulse 
        for (int i = 0; i <= 1000000; i++) ;

    }
      

        private void btnReward_Click(object sender, EventArgs e)
        {
          

            }

        private void btnReward2_Click(object sender, EventArgs e)
        {
         
            SetPulse(OutputPulse);
            txtOut.Text = OutputPulse.ToString();
            lblStep.Text = "the near state execute wait for reward 3";  
        }

        private void btnReward3_Click(object sender, EventArgs e)
        {
          
            SetPulse(OutputPulse);
            txtOut.Text = OutputPulse.ToString();
            lblStep.Text = "the near state execute add rule";  
           
            AddNewRule(); //12
           
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        double CalculateReward ()
        {
            string OX = "";
            string TE = "";
            string CA = "";
            double RewardAmount = 0;
            //Oxigen in the blood
            if (double.Parse(txtOX.Text) > 97)
                OX = "H";
            else if (double.Parse(txtOX.Text) >= 95 && double.Parse(txtOX.Text) <= 97)
                OX = "N";
            else if (double.Parse(txtOX.Text) < 95)
                OX = "L";
            //Body tempreture
            if (double.Parse(txtTemp.Text) > 37.2)
                TE = "H";
            else if (double.Parse(txtTemp.Text) >= 36.5 && double.Parse(txtTemp.Text) <= 37.2)
                TE = "N";
            else if (double.Parse(txtTemp.Text) < 36.5)
                TE = "L";
            //blood carbon dioxide
            if (double.Parse(txtCarb.Text) > 30)
                CA = "H";
            else if (double.Parse(txtCarb.Text) >= 23 && double.Parse(txtCarb.Text) <= 30)
                CA = "N";
            else if (double.Parse(txtCarb.Text) < 23)
                CA = "L";

            //reward set
            if (OX == "N" && TE == "N" && CA == "N")
                RewardAmount = RP2;
            else
            {
                switch (SelectedStateNo)
                {
                    case "1":
                        if (OX == "H")
                            RewardAmount = RN2;
                        else if (OX == "L")
                            RewardAmount = RN1;
                        break;
                    case "2":
                        if (OX == "H" && TE == "H")
                            RewardAmount = RN2;
                        else if (OX == "H" && TE == "N")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "L")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "H")
                            RewardAmount = RP1;
                        else if (OX == "L" && TE == "N")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "L")
                            RewardAmount = RN2;
                        break;
                    case "3":
                        if (OX == "H" && TE == "H")
                            RewardAmount = RN1;
                        else if (OX == "H" && TE == "N")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "L")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "H")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "N")
                            RewardAmount = RP1;
                        else if (OX == "L" && TE == "L")
                            RewardAmount = RN1;
                        break;
                    case "4":
                        if (OX == "H" && TE == "H" && CA == "H")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "H" && CA == "N")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "H" && CA == "L")
                            RewardAmount = RN1;
                        else if (OX == "H" && TE == "N" && CA == "H")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "N" && CA == "N")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "N" && CA == "L")
                            RewardAmount = RN1;
                        else if (OX == "H" && TE == "L" && CA == "H")
                            RewardAmount = RP1;
                        else if (OX == "H" && TE == "L" && CA == "N")
                            RewardAmount = RN1;
                        else if (OX == "H" && TE == "L" && CA == "L")
                            RewardAmount = RN2;
                        else if (OX == "L" && TE == "H" && CA == "H")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "H" && CA == "N")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "H" && CA == "L")
                            RewardAmount = RN2;
                        else if (OX == "L" && TE == "N" && CA == "H")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "N" && CA == "N")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "N" && CA == "L")
                            RewardAmount = RN2;
                        else if (OX == "L" && TE == "L" && CA == "H")
                            RewardAmount = RN1;
                        else if (OX == "L" && TE == "L" && CA == "N")
                            RewardAmount = RN2;
                        else if (OX == "L" && TE == "L" && CA == "L")
                            RewardAmount = RN2;
                        break;
                }
            }
            return RewardAmount;

        }
        double CalculateRewardNew()
        {
            string OX = "";
            string TE = "";
            string CA = "";
            double RewardAmount = 0;
            //Oxigen in the blood
            if (double.Parse(txtOX.Text) > 97)
                OX = "H";
            else if (double.Parse(txtOX.Text) >= 95 && double.Parse(txtOX.Text) <= 97)
                OX = "N";
            else if (double.Parse(txtOX.Text) < 95)
                OX = "L";
            //Body tempreture
            if (double.Parse(txtTemp.Text) > 37.2)
                TE = "H";
            else if (double.Parse(txtTemp.Text) >= 36.5 && double.Parse(txtTemp.Text) <= 37.2)
                TE = "N";
            else if (double.Parse(txtTemp.Text) < 36.5)
                TE = "L";
            //blood carbon dioxide
            if (double.Parse(txtCarb.Text) > 30)
                CA = "H";
            else if (double.Parse(txtCarb.Text) >= 23 && double.Parse(txtCarb.Text) <= 30)
                CA = "N";
            else if (double.Parse(txtCarb.Text) < 23)
                CA = "L";

            //reward set
            if (OX == "N" && TE == "N" && CA == "N")
                RewardAmount = 0.9;
            else
            {
                switch (OX)
                {
                    case "N":
                        {
                            switch (TE)
                            {
                                case "L":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = 0.5; break; }
                                            case "N":
                                                { RewardAmount = 0.9; break; }
                                            case "H":
                                                { RewardAmount = 0.6; break; }

                                        }
                                        break;
                                    }
                                case "N":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = 0.7; break; }
                                            case "N":
                                                { RewardAmount = 0.9; break; }
                                            case "H":
                                                { RewardAmount = 0.7; break; }
                                        }
                                        break;
                                    }
                                case "H":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = 0.6; break; }
                                            case "N":
                                                { RewardAmount = 0.9; break; }
                                            case "H":
                                                { RewardAmount = 0.5; break; }
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
                                                 { RewardAmount = -0.9; break; }
                                            case "N":
                                                 { RewardAmount = -0.8; break; }
                                            case "H":
                                                    { RewardAmount = -0.8; break; }
                                        }
                            break;
                                    }
                            
                 case "H":
                        {
                            switch (TE)
                            {
                                case "L":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = -0.3; break; }
                                            case "N":
                                                { RewardAmount = -0.1; break; }
                                            case "H":
                                                { RewardAmount = -0.6; break; }
                                        }
                                        break;
                                    }
                                case "N":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = -0.3; break; }
                                            case "N":
                                                { RewardAmount = 0; break; }
                                            case "H":
                                                { RewardAmount = -0.6; break; }
                                        }
                                        break;
                                    }
                                case "H":
                                    {
                                        switch (CA)
                                        {
                                            case "L":
                                                { RewardAmount = -0.3; break; }
                                            case "N":
                                                { RewardAmount = 0; break; }
                                            case "H":
                                                { RewardAmount = -0.6; break; }
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
            if (SA2==true)
            {
                SelectedStateValue = PS2;
                SelectedStateNo = SNo2.ToString();
                SA2 = false;
            }
            else if (SA3 == true)
            {
                SelectedStateValue = PS3;
                SelectedStateNo = SNo3.ToString();
                SA3 = false;
            }
            else if (SA4== true)
            {
                SelectedStateValue = PS4;
                SelectedStateNo = SNo4.ToString();
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
           

            lblNearestState.Text = SelectedStateValue + "-" + SelectedStateNo + "//" + PS1 + "-" + PS2 + "-" + PS3 + "-" + PS4;
            ListOutput.Items.Add("Next near state is:");
            ListOutput.Items.Add(SelectedStateNo + "//" + SelectedStateValue);

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
           

            lblOutputPulse.Text = OutputPulse.ToString();
            ListOutput.Items.Add("Next output pulse set to:");
            ListOutput.Items.Add(OutputPulse.ToString());
            txtOut.Text = OutputPulse.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            double RewardAmount = CalculateRewardNew(); //CalculateReward();
          //  double dis = Math.Abs(OutputPulse - txtCurrentPulse);

            ListOutput.Items.Add("Reward calculated");
            ListOutput.Items.Add("Reward is:"+RewardAmount.ToString());


            //if (lblLevelNo.Text == "1")
            //{
                txtReward1.Text = RewardAmount.ToString();
                if (double.Parse(txtReward1.Text) >= 0)
                {
                    if (lblTryNo.Text == "0")
                    {
                        ListOutput.Items.Add("Reward is possitive then try it for 2 times.");
                        ListOutput.Items.Add("Wait for try 1 Reward");
                        btnTry1.Visible = true;
                        btnTry1.Enabled = true;
                        button2.Enabled = false;
                        lblTryNo.Text = "1";
                    }
                }
                else
                {
                    ListOutput.Items.Add("Reward is negative select another state.");
                    FindOtherState();
                }
         //   }

            //else if (lblLevelNo.Text == "2")
            //    txtReward2.Text = RewardAmount.ToString();
            //else if (lblLevelNo.Text == "3")
            //    txtReward3.Text = RewardAmount.ToString();

         switch (SelectedStateNo)
           {
               case "1":
                   q1 = q1 + RewardAmount;
                   break;
               case "2":
                   q2 = q2 + RewardAmount;
                   break;
               case "3":
                   q3 = q3 + RewardAmount;
                   break;
               case "4":
                   q4 = q4 + RewardAmount;
                   break;
           }

        
   
            
        }

        private void btnTry1_Click(object sender, EventArgs e)
        {
            double RewardAmount = CalculateRewardNew();  //CalculateReward();
           
            ListOutput.Items.Add("Reward calculated for second time");
            ListOutput.Items.Add("Reward is:" + RewardAmount.ToString());

               txtReward2.Text = RewardAmount.ToString();
                if (double.Parse(txtReward2.Text) >= 0)
                {
                    if (lblTryNo.Text == "1")
                    {
                        ListOutput.Items.Add("Reward is possitive for the second time then try it for one time more.");
                        ListOutput.Items.Add("Wait for try 2 Reward");
                        btnTry2.Visible = true;
                        btnTry2.Enabled = true;
                        btnTry1.Enabled = false;
                        lblTryNo.Text = "2";
                    }
                }
                else
                { ListOutput.Items.Add("Reward is negative select another state.");
                FindOtherState();
                }
            
        }

        private void btnTry2_Click(object sender, EventArgs e)
        {
            double RewardAmount = CalculateRewardNew(); //CalculateReward();

            ListOutput.Items.Add("Reward calculated for third time");
            ListOutput.Items.Add("Reward is:" + RewardAmount.ToString());

            txtReward3.Text = RewardAmount.ToString();
            if (double.Parse(txtReward3.Text) >= 0)
            {
                if (lblTryNo.Text == "2")
                {
                  if (lblResult.Text=="F")
                  { 
                    ListOutput.Items.Add("Reward is possitive for the third time then add it to the new rules KDB.");
                    ListOutput.Items.Add("Rule adds to KDB");
                    //Add rule                       
                    AddNewRule();
                    btnTry2.Visible = true;
                  }
                  else
                  {
                      ListOutput.Items.Add("Reward is possitive for the third time then change the CF of the rule in KDB.");
                      ListOutput.Items.Add("Rule changes in KDB");
                      //Add rule                       
                      AddNewRule();
                      btnTry2.Visible = true;
                  }

                }
            }
            else
            {
                ListOutput.Items.Add("Reward is negative select another state.");
                FindOtherState();

            }
            
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            btnTry1.Visible = false;
            btnTry1.Enabled = true;
           btnTry2.Visible = false;
           btnTry2.Enabled = true;
            ListOutput.Text = "";
            ListOutput.Items.Clear();
            txtReward1.Text = "";
            txtReward2.Text = "";
            txtReward3.Text = "";
            lblTryNo.Text = "0";

        }

        private void btnHFCPN_Click(object sender, EventArgs e)
        {
            ////var places = new PlaceInformation();
            ////places = XMLParsersPacemaker.ParseByXMLDocument();

            ////string[] Initvalue = new string[6];
            ////Initvalue[0] = places.Placelist[0].name;
            ////Initvalue[1] = places.Placelist[1].name;

            ////lblOutFCPN.Text = Initvalue[1];

            //XMLParsersPacemaker.ParseByXMLDocumentWrite(txtInputHFCPN.Text);
        }

        private void txtInputHFCPN_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReadHFCPN_Click(object sender, EventArgs e)
        {
            

           // string text = System.IO.File.ReadAllText("../../result.txt");
            string[] lines = System.IO.File.ReadAllLines("../../result.txt");

            lblOutFCPN.Text = lines[0];

            if (lblOutFCPN.Text.Contains("Low"))
                txtOut.Text = "30";
            if (lblOutFCPN.Text.Contains("Medium"))
                txtOut.Text = "40";
            if (lblOutFCPN.Text.Contains("Normal"))
                txtOut.Text = "70";
            if (lblOutFCPN.Text.Contains("High"))
                txtOut.Text = "90";
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            ListOutput.Items.Add("System start.......");
            if (OutFCPN.Text=="Low")
                txtOut.Text="27";
            else if (OutFCPN.Text=="Medium")
                txtOut.Text="42";
            else if (OutFCPN.Text=="Normal")
                txtOut.Text="77";
            else if (OutFCPN.Text=="High")
                txtOut.Text="122";


            SA1 = true;
            SA2 = true;
            SA3 = true;
            SA4 = true;

                FindNearStateFCPN();
                ListOutput.Items.Add("The rule find and execute");
                ListOutput.Items.Add("Waiting for reward....");
                lblResult.Text = "T";
                button2.Enabled = true;
                lblLevelNo.Text = "1";

          



            }
        }

           
        }

        
   

