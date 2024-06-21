using System;

using System.Windows.Forms;

namespace TheProject
{
    public partial class Form1 : Form
    {
        public double[,] alphas;
        public double[,] betas;
        public double[,] B; // αB1,ßB1 αB2,ßB2 αB3,ßB3 αB4,ßB4 αB5,ßB5

        public double[,] uA1B;
        public double[,] uA2B;
        public double[,] uA3B;
        public double[,] uA4B;
        public double[,] uA5B;

        public double[,] b;

        public Form1()
        {
            InitializeComponent();

            alphas = new double[5,5];
            betas = new double[5,5];
            B = new double[5, 2];

            uA1B = new double[5, 5];
            uA2B = new double[5, 5];
            uA3B = new double[5, 5];
            uA4B = new double[5, 5];
            uA5B = new double[5, 5];
            b = new double[5, 5];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseAlpha_Beta chooseAlphaBetaForm = new ChooseAlpha_Beta(this);
            chooseAlphaBetaForm.Show(this);
        }

        private void calculateA_array(double[,] array, int aNumber)
        {
            aNumber--;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    array[i, j] = Math.Min(alphas[aNumber, i], B[j, 0]);
                }
            }


            for (int i=0; i<5 ; i++)
            {
                b[aNumber,i] = Math.Max(Math.Min(alphas[i,0], array[i,0]) , Math.Max( Math.Min(alphas[i, 1], array[1, i]),
                    Math.Max(Math.Min(alphas[i, 2], array[2, i]) , Math.Max(Math.Min(alphas[i, 3], array[3, i]),
                    Math.Min(alphas[i, 4], array[4, i])))));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            calculateA_array(uA1B, 1);
            calculateA_array(uA2B, 2);
            calculateA_array(uA3B, 3);
            calculateA_array(uA4B, 4);
            calculateA_array(uA5B, 5);


            double[] ys = new double[5];
            double minY = Double.MaxValue;

            for (int i = 0; i < 5; i++)
            {
                ys[i] = b[i, 0] * B[0, 1] + b[i, 1] * B[1, 1] + b[i, 2] * B[2, 1] + b[i, 3] * B[3, 1] + b[i, 4] * B[4, 1];

                if (ys[i] < minY)
                    minY = ys[i];
            }


            MessageBox.Show("Optimum Y is: " + minY);
            
        }
    }
}
