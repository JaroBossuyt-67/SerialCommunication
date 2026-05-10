using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconnected";
                }
                else
                {
                    serialPortArduino.PortName = (string) comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string) comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int) numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;


                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";

                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";

                    }
                }
            }
            catch (Exception exception)
            { 
             labelStatus.Text= "Error: " + exception.Message;
             serialPortArduino.Close() ;
             radioButtonVerbonden.Checked= false;
                buttonConnect.Text = "Connect";
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl.SelectedTab == tabPageOefening3)
                {
                    timerOefening3.Enabled = true;
                }
                else
                {
                    timerOefening3.Enabled = false;
                }

                if (tabControl.SelectedTab == tabPageOefening4)
                {
                    timerOefening4.Enabled = true;
                }
                else
                {
                    timerOefening4.Enabled = false;
                }

                if (tabControl.SelectedTab == tabPageOefening5)
                {
                    timerOefening5.Enabled = true;
                }
                else
                {
                    timerOefening5.Enabled = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino != null && serialPortArduino.IsOpen)
                {
                    // Clear any previous data
                    try { serialPortArduino.ReadExisting(); } catch { }

                    // digital 5
                    serialPortArduino.WriteLine("digital 5");
                    string antwoord5 = serialPortArduino.ReadLine().Trim();
                    radioButtonDigital5.Checked = (antwoord5 == "1");

                    // digital 6
                    try { serialPortArduino.ReadExisting(); } catch { }
                    serialPortArduino.WriteLine("digital 6");
                    string antwoord6 = serialPortArduino.ReadLine().Trim();
                    radioButtonDigital6.Checked = (antwoord6 == "1");

                    // digital 7
                    try { serialPortArduino.ReadExisting(); } catch { }
                    serialPortArduino.WriteLine("digital 7");
                    string antwoord7 = serialPortArduino.ReadLine().Trim();
                    radioButtonDigital7.Checked = (antwoord7 == "1");
                }
            }
            catch (Exception ex)
            {
                try { labelStatus.Text = "Error: " + ex.Message; } catch { }
            }
        }

        private void timerOefening4_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino != null && serialPortArduino.IsOpen)
                {
                    // Clear previous data from Arduino
                    try { serialPortArduino.ReadExisting(); } catch { }

                    // Request analog 0 value
                    serialPortArduino.WriteLine("analog 0");

                    string antwoord = serialPortArduino.ReadLine().Trim();

                    // extract numeric value if the reply contains extra text
                    var match = System.Text.RegularExpressions.Regex.Match(antwoord, "\\d+");
                    string value = match.Success ? match.Value : antwoord;

                    labelAnalog0.Text = value;
                }
            }
            catch (Exception ex)
            {
                try { labelStatus.Text = "Error: " + ex.Message; } catch { }
            }
        }

        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino != null && serialPortArduino.IsOpen)
                {
                    // Read desired temperature from analog pin 0
                    try { serialPortArduino.ReadExisting(); } catch { }
                    serialPortArduino.WriteLine("analog 0");
                    string antwoord0 = serialPortArduino.ReadLine().Trim();
                    var match0 = System.Text.RegularExpressions.Regex.Match(antwoord0, "\\d+");
                    int raw0 = 0;
                    if (match0.Success) Int32.TryParse(match0.Value, out raw0);

                    // scale 0..1023 -> 5..45 °C
                    double slopeDesired = 40.0 / 1023.0; // (45-5)/1023
                    double offsetDesired = 5.0;
                    double desiredTemp = slopeDesired * raw0 + offsetDesired;
                    string desiredText = Math.Round(desiredTemp, 1).ToString("0.0") + " °C";
                    labelGewensteTemp.Text = desiredText;

                    // Read current temperature from analog pin 1
                    try { serialPortArduino.ReadExisting(); } catch { }
                    serialPortArduino.WriteLine("analog 1");
                    string antwoord1 = serialPortArduino.ReadLine().Trim();
                    var match1 = System.Text.RegularExpressions.Regex.Match(antwoord1, "\\d+");
                    int raw1 = 0;
                    if (match1.Success) Int32.TryParse(match1.Value, out raw1);

                    // scale 0..1023 -> 0..500 °C
                    double slopeCurrent = 500.0 / 1023.0;
                    double offsetCurrent = 0.0;
                    double currentTemp = slopeCurrent * raw1 + offsetCurrent;
                    string currentText = Math.Round(currentTemp, 1).ToString("0.0") + " °C";
                    labelHuidigeTemp.Text = currentText;

                    // Control LED on digital pin 2: ON when current < desired
                    try { serialPortArduino.ReadExisting(); } catch { }
                    string cmd = (currentTemp < desiredTemp) ? "set d2 high" : "set d2 low";
                    serialPortArduino.WriteLine(cmd);
                }
            }
            catch (Exception ex)
            {
                try { labelStatus.Text = "Error: " + ex.Message; } catch { }
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino != null && serialPortArduino.IsOpen)
                {
                    string commando = $"set pwm9 {trackBarPWM9.Value}";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception ex)
            {
                try { labelStatus.Text = "Error: " + ex.Message; } catch { }
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino != null && serialPortArduino.IsOpen)
                {
                    string commando = checkBoxDigital2.Checked ? "set d2 high" : "set d2 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception ex)
            {
                try { labelStatus.Text = "Error: " + ex.Message; } catch { }
            }
        }
    }
}
