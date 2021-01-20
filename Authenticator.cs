using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterGasTool
{
    public partial class Authenticator : Form
    {

        Form1 F1 = new Form1();
        string passwordReference = string.Empty;
        public bool Flag_AuthenticationSucccess = false;
        public Authenticator(string passwordRef)
        {
            InitializeComponent();
            this.passwordReference = passwordRef;
            label2.Text = "Hint: "+ passwordReference.Length + " Chars";
        }
        
        //public bool ChecktheUserPass()
        //{

        //    if (string.Equals(F1.CSPassword.ToUpper(), textBox_P_pin.Text.ToUpper()))
        //    {
        //        if (string.Equals(textBox_P_pin.Text, "1234"))
        //        {
        //            return true;
        //        }
        //    }
        //    else { return false; }
        //    return false;
        //}

        private void textBox_P_pin_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if ((string.Equals(passwordReference.ToUpper(), textBox_P_pin.Text.ToUpper())) && char.IsControl(ch))
            {
                this.Flag_AuthenticationSucccess = true;
                Close();

            }
            else if(char.IsControl(ch))
                textBox_P_pin.Clear();
            //if (string.Equals(passwordReference.ToUpper(), textBox_P_pin.Text.ToUpper()))
            //{
            //    Close();
            //}
        }
    }
}
