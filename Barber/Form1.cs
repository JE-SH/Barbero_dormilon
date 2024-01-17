using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;


namespace Barber
{
    public partial class Form1 : Form
    {
        int clientes;
        Queue<PictureBox> sillas;
        int cortando;
        bool entrando;
        bool run;
        Semaphore sem;

        Random rand;
        System.Timers.Timer myTimerSilla;
        System.Timers.Timer myTimerCorte;
        public Form1()
        {
            InitializeComponent();
            sem = new Semaphore(1, 2);
            sillas = new Queue<PictureBox>();
            //sillas = 4;                         //semáforo de sillas
            clientes = 0;                       //semáforo de clientes (no hay)
            cortando = 0;                   //barbero no cortando aun
            entrando = false;                          //no hay nadie
            //rand = new Random();                //numero aleatorio 

            reset();
        }
//----------------------------------------------------------------------------------------------------
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar.ToString()== "\u001b")
            {
                //this.Close();
                MessageBox.Show("La barbería ha cerrado. Da clic al botón para abrir", "CERRANDO");
                buttonINICIO.Enabled = true;
                reset();
            }

        }
        private void buttonINICIO_Click(object sender, EventArgs e)
        {
            run = true;
            buttonINICIO.Enabled = false;
            var seed = Environment.TickCount;
            rand = new Random(seed);
            myTimerSilla.Interval = rand.Next(0, 7000);
            myTimerSilla.Enabled = true;
            myTimerSilla.Elapsed += OnTimedEvent;
            muestraClienteEspera();

            hilo();
        }
        void hilo()
        {
            new Thread(barberia).Start();
        }

        //Se muestran los clientes según la cantidad de gente en las sillas
        void muestraClienteEspera()
        {
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            if (sillas.Count > 0)
            {
                foreach (PictureBox silla in sillas)
                {
                    silla.Show();
                }
            }
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
        }
        //Devuelve la una silla disponible
        public PictureBox seekLastPicture()
        {
            if (pictureBox1.Visible == false)
                return pictureBox1;
            else if (pictureBox2.Visible == false)
                return pictureBox2;
            else if (pictureBox3.Visible == false)
                return pictureBox3;
            else if (pictureBox4.Visible == false)
                return pictureBox4;
            else
                return null;
        }
        //Wait & signal
        public void waitClientes()
        {
            clientes++;
        }
        public void signalClietes()
        {
            clientes = clientes - 1;
        }
        public void waitSillas(PictureBox pic)
        {
            sillas.Enqueue(pic);
        }
        public void waitSillas()
        {
            PictureBox pic = seekLastPicture();
            if (pic != null)
            {
                sillas.Enqueue(pic);
            }
        }
        public void signalSillas()
        {
            sillas.Dequeue();
        }
        public bool isBarberReady()
        {
            return Convert.ToBoolean(cortando);
        }
        public void changeBarberReady()
        {
            cortando = Convert.ToInt32(!Convert.ToBoolean(cortando));
        }

        //-----ENTRA NUEVO CLIENTE
        void muestraEntrando()
        {
            var seed = Environment.TickCount;
            rand = new Random(seed);

            pictureBoxllegando.Show();
            pictureBoxllegando.Refresh();
            Thread.Sleep(2500);
            pictureBoxllegando.Hide();
            pictureBoxllegando.Refresh();

            if (clientes == 0) //AUN NO HAY CLIENTES
            {
                //waitSillas();   no es necesaria ya que no se sienta en las sillas
                waitClientes();
                cortando=1;     //pasa a ser a "cortando"
                //tiempo aleatorio para hacer el corte
                myTimerCorte.Interval = rand.Next(2000, 15000);
                myTimerCorte.Enabled = true;
                myTimerCorte.Elapsed += OnTimedCut;
            }
            else if (clientes > 0)//YA HAY CLIENTES (cortando o esperando)
            {
                waitSillas();   //ingresa un cliente a las sillas
                waitClientes(); //
            }
            
            //tiempo aleatorio para un nuevo ingreso
            myTimerSilla.Interval = rand.Next(1000, 7000);
            myTimerSilla.Enabled = true;

            entrando = false;
            //muestraClienteEspera(sillas);
            muestraCortando();
        }
        //---- ACCIONES DEL BARBERO
        void MuestraDurmiendo()
        {
            pictureBoxdurmiendo.Show();
            pictureBoxcortando.Hide();
            pictureBoxcortando.Refresh();
            pictureBoxdurmiendo.Refresh();

            //muestraClienteEspera(0);
        }
        void muestraCortando()
        {
            pictureBoxdurmiendo.Hide();
            pictureBoxcortando.Show();
            pictureBoxcortando.Refresh();
            pictureBoxdurmiendo.Refresh();
        }

        //*********************ON TIME EVENT*********************
        private void OnTimedEvent(Object source, EventArgs e)
        {
            if (!entrando)
            {
                myTimerSilla.Stop();
                entrando = true;
            }
        }
        private void OnTimedCut(Object source, EventArgs e)
        {
            cortando = 0;
            myTimerCorte.Stop();

        }
        //*********************BARBERÍA*********************
        public void barberia()
        {
            sem.WaitOne();
            while(run)
            {
                var seed = Environment.TickCount;
                rand = new Random(seed);

                pictureBoxsillas.Refresh();

                if (entrando)
                {
                    muestraEntrando();
                    muestraClienteEspera();
                }

                if (clientes > 0)
                {
                    if (cortando == 0) //DEJÓ DE CORTAR
                    {
                        if (sillas.Count == 0)//Solo había un cliente en corte
                        {
                            signalClietes();
                            cortando = 0;
                        }
                        else
                        {
                            signalClietes();
                            signalSillas();
                            cortando = 1;
                            myTimerCorte.Interval = rand.Next(5000, 15000);
                            myTimerCorte.Enabled = true;
                            cortando = 1;
                            
                            pictureBoxdurmiendo.Show();
                            pictureBoxcortando.Hide();
                            Thread.Sleep(300);
                            pictureBoxdurmiendo.Hide();
                            pictureBoxcortando.Show();
                            pictureBoxcortando.Refresh();
                            pictureBoxdurmiendo.Refresh();

                            muestraClienteEspera();

                        }
                    }
                }
                else //no tiene clientes
                {
                    MuestraDurmiendo();
                    Thread.Sleep(500);
                }
            }
            sem.Release();
        }
        void reset()
        {

            //ocultar imagenes no necesarias
            pictureBoxsillas.Show();
            pictureBoxdurmiendo.Show();
            pictureBoxllegando.Hide();
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBoxcortando.Hide();
            //temporizadores para entrar a la barberia
            myTimerSilla = new System.Timers.Timer();//temporizador
            //temporizador para cortar el pelo
            myTimerCorte = new System.Timers.Timer();//temporizador
        }

        private void button1_Click(object sender, EventArgs e)
        {
            run = false;
            //this.Close();
        }
    }
}
